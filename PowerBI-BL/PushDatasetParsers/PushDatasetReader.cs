using BusinessLayer.DatasetTemplates;
using DataLayer;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PushDatasetParsers
{
    public class PushDatasetReader
    {
        PowerBIGateway api = PowerBIGateway.GetInstance();
        public List<TableTemplate> Artefacts { get; set; }
        public List<TableTemplate> CalculatedTables { get; set; }
        public PushDatasetReader(string datasetId)
        {
            Artefacts = new List<TableTemplate>();
            CalculatedTables = new List<TableTemplate>();
            var tables = ParseTables(datasetId);
            tables.ForEach(table => {
                if (table.Type == TableTypes.Calculated)
                    CalculatedTables.Add(table);
                else
                    Artefacts.Add(table);
            });
        }
        private List<TableTemplate> ParseTables(string datasetId)
        {
            var tablesDict = new Dictionary<string, TableTemplate>();
            var columns = api.GetDatasetColumns(datasetId);
            var calculatedTables = api.GetTableRows(datasetId, CalculatedTableParser.CalculatedTablesName);

            foreach(var calculatedTable in calculatedTables)
            {
                var obj = (JObject)calculatedTable;
                var tableName = obj[CalculatedTableParser.CalculatedTablesName + "[TableName]"].Value<string>();
                var expression = obj[CalculatedTableParser.CalculatedTablesName + "[Expression]"].Value<string>();
                var newTable = new TableTemplate(tableName, expression);
                tablesDict.Add(tableName, newTable); 
            }
                
            foreach (var col in columns)
            {
                var objCol = (JObject)col;
                var tableName = objCol["[Table Name]"].Value<string>();
                var columnName = objCol["[Column Name]"].Value<string>();

                if (columnName.StartsWith("RowNumber"))
                    continue;

                bool isDynamic = false;

                if (tableName.EndsWith(DynamicTableParser.MeasurementsTableSuffix))
                {   
                    tableName = tableName.Replace(DynamicTableParser.MeasurementsTableSuffix, "");
                    isDynamic = true;

                    if (IsColumnFromAdditionalTables(columnName, tableName))
                    {
                        continue;
                    }
                }

                if (!tablesDict.ContainsKey(tableName))
                    tablesDict.Add(tableName, new TableTemplate(tableName));

                if (isDynamic)
                    tablesDict[tableName].Type = TableTypes.Dynamic;

                var column = new ColumnTemplate(columnName, isDynamic);

                if(!IsColumnInTable(column, tablesDict[tableName].Columns))
                    tablesDict[tableName].Columns.Add(column);
            }

            tablesDict.Remove(PushDatasetParser.ParameterTableName);
            tablesDict.Remove(PushDatasetParser.RefreshHistoryTableName);
            tablesDict.Remove(CalculatedTableParser.CalculatedTablesName);
            return tablesDict.Values.ToList();
        }
        private bool IsColumnInTable(ColumnTemplate searchedColumn, List<ColumnTemplate> columns)
        {
            foreach (var col in columns)
            {
                if(col.Name == searchedColumn.Name)
                {
                    if (searchedColumn.IsDynamic) col.IsDynamic = true;
                    return true;
                }
            }
            return false;
        }
        private bool IsColumnFromAdditionalTables(string columnName, string tableName)
        {
            foreach (var column in DynamicTableParser.MeasurementsColumnNames)
            {
                var changedColumn = column.Replace("@TableName", tableName);
                if (changedColumn == columnName)
                    return true;
            }
            return false;
        }
    }
}
