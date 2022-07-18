using BusinessLayer.DatasetTemplates;
using BusinessLayer.PushDatasetParsers;
using BusinessLayer.PushDatasetParsers;
using DataLayer;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BusinessLayer
{
    public class Project
    {
        private static string ReportUrlTemplate = "https://app.powerbi.com/reportEmbed?reportId=@reportId&autoAuth=true";
        private static string EditReportUrlTemplate = "https://app.powerbi.com/groups/me/reports/@reportId";
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string DatasetId { get; set; }
        public string ReportUrl { get; set; }
        public string EditReportUrl { get; set; }

        private List<TableTemplate> artefacts;

        public List<TableTemplate> Artefacts { 
            get 
            {
                if (artefacts == null)
                    LoadTables();
                return artefacts;
            }
            set { artefacts = value; }
        }
        private List<TableTemplate> calculatedTables;
        public List<TableTemplate> CalculatedTables { 
            get 
            {
                if (calculatedTables == null)
                    LoadTables();
                return calculatedTables;
            }
            set { calculatedTables = value; } }

        private Dictionary<string, JObject> refreshHistory;
        public Dictionary<string, JObject> RefreshHistory { get
            {
                if(refreshHistory == null)
                {
                    refreshHistory = new Dictionary<string, JObject>();
                    var rows = api.GetTableRowsWithParsedColumnNames(DatasetId, PushDatasetParser.RefreshHistoryTableName);
                    foreach (var row in rows)
                    {
                        var jsonObj = row as JObject;
                        refreshHistory.Add(jsonObj["TableName"].Value<string>(), jsonObj);
                    }
                }
                return refreshHistory;
            } }

        public EventHandler RemoveProject;
        PowerBIGateway api = PowerBIGateway.GetInstance();
        App app = App.GetInstance();
        FileGateway fileGateway = FileGateway.GetInstance();

        public Project(string id, string name, string fullName, string datasetId)
        {
            Id = id;
            Name = name;
            FullName = fullName;
            DatasetId = datasetId;
            RemoveProject += app.RemoveProject;
            ReportUrl = ReportUrlTemplate.Replace("@reportId", Id);
            EditReportUrl = EditReportUrlTemplate.Replace("@reportId", Id);
        }       
        public void UpdateData(string pathToFile, TableTemplate artefact, string date, Action UpdateAccepted, Action<string> UpdateFailed)
        {
            var thread = new Thread(() => {
                var errors = new List<Exception>();
                try
                {
                    var csvTable = fileGateway.ReadDataFromSpecificationFile(pathToFile);
                    var pushDatasetParser = new PushDatasetParser();
                    pushDatasetParser.UpdateArtefact(DatasetId, artefact, csvTable, date);
                    RefreshHistory[artefact.Name]["Date"] = date;
                    var splittedPath = pathToFile.Split('\\');
                    RefreshHistory[artefact.Name]["FileName"] = splittedPath[splittedPath.Length - 1];
                    api.UpdateRowsInTable(DatasetId, PushDatasetParser.RefreshHistoryTableName, new List<Object>(RefreshHistory.Values));
                    
                    errors = UpdateCalculateTables();
                    
                    UpdateAccepted();
                }
                catch(Exception ex)
                {
                    UpdateFailed(ex.Message);
                }
            });
            thread.Start();
        }
        private List<Exception> UpdateCalculateTables()
        {
            var errors = new List<Exception>();
            var pushDatasetParser = new PushDatasetParser();
            foreach (var table in CalculatedTables)
            {
                try
                {
                    pushDatasetParser.UpdateArtefact(DatasetId, table);
                }
                catch(Exception ex)
                {
                    errors.Add(ex);
                }
            }
            return errors;
        }
        private void LoadTables()
        {
            var PushDatasetReader = new PushDatasetReader(DatasetId);
            Artefacts = PushDatasetReader.Artefacts;
            CalculatedTables = PushDatasetReader.CalculatedTables;
        }
        public void Remove()
        {
            api.RemoveDataset(DatasetId);
            RemoveProject.Invoke(this, EventArgs.Empty);
        }
    }
}