using BusinessLayer.DatasetTemplates;
using BusinessLayer.MyExceptions;
using BusinessLayer.PushDatasetParsers;
using DataLayer;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.PushDatasetParsers

{
    public class PushDatasetParser
    {
        public static string ParameterTableName = "Parameters";
        public static string RefreshHistoryTableName = "RefreshHistory";

        private StaticTableParser StaticTableParser;
        private DynamicTableParser DynamicTableParser;
        private CalculatedTableParser CalculatedTableParser;

        private CreateDatasetRequest Dataset { get; set; }
        private List<Object> CalculatedTablesRows { get; set; }
        private PowerBIGateway api = PowerBIGateway.GetInstance();
        
        public PushDatasetParser()
        {
            StaticTableParser = new StaticTableParser();
            DynamicTableParser = new DynamicTableParser();
            CalculatedTableParser = new CalculatedTableParser();
        }
        public string CreatePushDataset(DatasetTemplate DatasetTemplate, Dictionary<string, string> Parameters)
        {
            Dataset = new CreateDatasetRequest();
            CalculatedTablesRows = new List<Object>();

            Dataset.Name = DatasetTemplate.Name;
            Dataset.Tables = new List<Table>();
            Dataset.Relationships = new List<Relationship>();
            Dataset.Tables.Add(CalculatedTableParser.TableForCalculatedTables);


            foreach (var tableTemplate in DatasetTemplate.Tables)
            {
                TableParser parser = GetParser(tableTemplate.Type);
                if (parser == null) continue;

                Dataset = parser.CreateTableStructure(tableTemplate, Dataset);
            }

            foreach (var relation in DatasetTemplate.Relationships)
            {
                var newRel = new Relationship(relation.Name, relation.FromTable, relation.FromColumn, relation.ToTable, relation.ToColumn, relation.Type.ToString());
                Dataset.Relationships.Add(newRel);
            }

            CalculatedTablesRows = Dataset.Tables[0].Rows.ToList<Object>();
            Dataset.Tables[0].Rows = null;

            var datasetId = api.CreateDataset(Dataset);
            CalculatedTableRow calcTable = null;
            try 
            {
                foreach (var calculatedTable in CalculatedTablesRows)
                {
                    var tab = calculatedTable as CalculatedTableRow;
                    calcTable = tab;
                    api.ExecuteQuery(tab.Expression, datasetId);
                }
            }
            catch(Exception ex)
            {
                api.RemoveDataset(datasetId);
                throw new Exception($"Calculated table {calcTable.TableName} has incorrect syntax.");
            }
            api.AddRowsToTable(datasetId, CalculatedTableParser.CalculatedTablesName, CalculatedTablesRows);
            
            var parameters = new List<Object>();
            foreach(var obj in Parameters)
            {
                var parameter = new JObject();
                parameter.Add("Key", obj.Key);
                parameter.Add("Value", obj.Value);
                parameters.Add(parameter);
            }
            api.AddRowsToTable(datasetId, ParameterTableName, parameters);

            var refreshHistory = new List<Object>();
            foreach (var table in DatasetTemplate.Tables)
            {
                if (table.Type == TableTypes.Calculated || table.Name == PushDatasetParser.RefreshHistoryTableName || table.Name == PushDatasetParser.ParameterTableName) continue;

                var tableHistory = new JObject();
                tableHistory.Add("TableName", table.Name);
                tableHistory.Add("Date", "Not updated yet");
                tableHistory.Add("FileName", "Not updated yet");
                refreshHistory.Add(tableHistory);
            }
            api.AddRowsToTable(datasetId, PushDatasetParser.RefreshHistoryTableName, refreshHistory);

            return datasetId;
        }
        private TableParser GetParser(TableTypes tableType)
        {
            TableParser parser = null;
            switch (tableType)
            {
                case TableTypes.Static:
                    parser = StaticTableParser;
                    break;
                case TableTypes.Dynamic:
                    parser = DynamicTableParser;
                    break;
                case TableTypes.Calculated:
                    parser = CalculatedTableParser;
                    break;
            }
            return parser;
        }
        public void UpdateArtefact(string datasetId, TableTemplate tableTemplate, DataTable csvTable = null, string date = null)
        {
            var parser = GetParser(tableTemplate.Type);
            parser.UpdateArtefact(datasetId, tableTemplate, csvTable, date);
        }

    }
}
