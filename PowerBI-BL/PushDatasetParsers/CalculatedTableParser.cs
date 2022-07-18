using BusinessLayer.DatasetTemplates;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PushDatasetParsers
{
    public class CalculatedTableParser : TableParser
    {
        public static string CalculatedTablesName = "CalculatedTables";

        private static Column[] Columns = new Column[]
        {
            new Column("TableName", ColumnTypes.String.ToString()),
            new Column("Expression", ColumnTypes.String.ToString()),
        };

        public Table TableForCalculatedTables;

        public CalculatedTableParser() 
        {
            TableForCalculatedTables = new Table(CalculatedTablesName, Columns);
            TableForCalculatedTables.Rows = new List<Row>();
        }
        public override CreateDatasetRequest CreateTableStructure(TableTemplate TableTemplate, CreateDatasetRequest Dataset)
        {
            var calcTable = new Table();
            calcTable.Name = TableTemplate.Name;

            calcTable.Columns = new List<Column>();
            Dataset.Tables.Add(calcTable);

            var calculatedTable = new CalculatedTableRow(TableTemplate.Name, TableTemplate.Expression);
            TableForCalculatedTables.Rows.Add(calculatedTable);

            foreach (var colTemplate in TableTemplate.Columns)
            {
                var newCol = new Column(colTemplate.Name, colTemplate.Type.ToString());
                calcTable.Columns.Add(newCol);
            }

            calcTable.Measures = ParseMeasures(TableTemplate.Measures);

            return Dataset;
        }
        
        protected override void ParseArtefact(string DatasetId, TableTemplate TableTemplate, DataTable CsvTable, string Date)
        {
            try
            {
                var response = api.ExecuteQuery(TableTemplate.Expression, DatasetId);
                var dataRows = response.Results[0].Tables[0].Rows;

                var data = new List<Object>();

                foreach (var row in dataRows)
                {
                    var jsonObj = row as JObject;
                    var dictObj = jsonObj.ToObject<Dictionary<string, string>>();
                    var newObj = new JObject();
                    foreach (var key in dictObj.Keys)
                    {
                        var newKey = key.Split('[')[1].Split(']')[0];
                        newObj.Add(newKey, dictObj[key]);
                    }
                    AddMissingColumn(newObj, TableTemplate.Columns);
                    data.Add(newObj);
                }

                api.UpdateRowsInTable(DatasetId, TableTemplate.Name, data);
            }
            catch(Exception ex)
            {
                throw new Exception($"Cannot update calculated table {TableTemplate.Name} - Unspecified error");
            }
        }
        private void AddMissingColumn(JObject row, List<ColumnTemplate> columns)
        {
            foreach (var col in columns)
            {
                if (!row.ContainsKey(col.Name))
                    row.Add(col.Name, "0");
            }
        }
        
    }
}
