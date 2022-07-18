using DataLayer;
using DataLayer.Exceptions;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Extensions;
using Microsoft.PowerBI.Api.Models;
using Microsoft.PowerBI.Api.Models.Credentials;
using Microsoft.PowerShell;
using Microsoft.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PowerBIGateway
    {

        private static PowerBIGateway api = new PowerBIGateway();


        const string urlPowerBiRestApiRoot = "https://api.powerbi.com/";

        const string GetTableRowsQuery = "EVALUATE VALUES(@TableName)";

        //powershell commands
        const string pathToModule = @"..\..\..\..\Data\MicrosoftPowerBIMgmt.Profile\1.2.1077\MicrosoftPowerBIMgmt.Profile.psd1";
        static string importModule = $"Import-Module {pathToModule}";

        const string loginPowerBI = "Login-PowerBI";
        const string getToken = "Get-PowerBIAccessToken";

        const string getColumnsQuery = "Evaluate summarize(ColumnStatistics(), [Table Name], [Column Name])";
        PowerBIClient client { get; set; }

        FileGateway fileGateway = FileGateway.GetInstance();

        bool shouldWaitForResponseImport = true;
        int waitingStepSeconds = 1;
        int timeout = 30;

        object shouldWaitForResponseImportLock = new object();


        private PowerBIGateway() { }
           
        public bool Login()
        {
            var loginResult = ShowLoginForm();
            if (loginResult != null && loginResult.BaseObject is Hashtable)
            {
                var authToken = GetAuthToken(loginResult);
                Console.WriteLine(authToken);
                var tokenCredentials = new TokenCredentials(authToken, "Bearer");
                client = new PowerBIClient(new Uri(urlPowerBiRestApiRoot), tokenCredentials);
                return true;
            }
            return false;
        }
        public bool IsLogged()
        {
            if (client == null)
                return false;
            return true;
        }
        private PSObject ShowLoginForm()
        {
            using (var ps = PowerShell.Create())
            {
                ps.AddCommand("Set-ExecutionPolicy").AddParameter("ExecutionPolicy", ExecutionPolicy.RemoteSigned).AddParameter("Scope", ExecutionPolicyScope.CurrentUser);

                ps.AddScript(importModule);
                ps.AddScript(loginPowerBI);
                ps.AddScript(getToken);

                Collection<PSObject> results = ps.Invoke();

                if(results.Count > 0)
                    return results[results.Count - 1];
            }
            return null;
        }
        private string GetAuthToken(PSObject result)
        {
            var hashTable = (Hashtable)result.BaseObject;
            string authToken = hashTable["Authorization"].ToString().Replace("Bearer ", "");
            return authToken;

        }
        public Import ImportPBIX(string name, Guid? groupId = null)
        {
            var stream = fileGateway.GetPBIXTemplate();
            Import importResult;
            try
            {
                if (groupId == null)
                    importResult = client.Imports.PostImportWithFile(stream, name);
                else
                    importResult = client.Imports.PostImportWithFile(groupId.Value, stream, name);
            }
            catch
            {
                throw new ImportPBIXException();
            }
            return WaitForResult(importResult);         
        }
        private Import WaitForResult(Import importResult)
        {
            int actualTime = 0;
            shouldWaitForResponseImport = true;
            while (true)
            {
                Wait();
                actualTime += waitingStepSeconds;
                lock (shouldWaitForResponseImportLock)
                {
                    if (!shouldWaitForResponseImport)
                        break;
                }
                
                importResult = client.Imports.GetImport(importResult.Id);
           
                if (importResult.ImportState == "Succeeded")
                    return importResult;
      
                if(actualTime > timeout)
                    throw new Exception("The project template import timed out.");

            }
            return importResult;
            
        }
        private void Wait()
        {
            Thread th = new Thread(() =>
            {
                Thread.Sleep(waitingStepSeconds * 1000);
            });
            th.Start();
            th.Join();
        }

        public string CreateDataset(CreateDatasetRequest datasetTemplate, Guid? groupId = null)
        {
            try
            {
                var importResult = client.Datasets.PostDataset(datasetTemplate);
                return importResult.Id;
            }
            catch (Exception ex)
            {
                var message = GetMessage(ex);
                throw new Exception("Failed to create dataset. Check the structure of the created template." + message);
            }
        }

        public void RemoveDataset(string datasetId, Guid? groupId = null)
        {
            try
            {
                if (groupId == null)
                    client.Datasets.DeleteDataset(datasetId);
                else
                    client.Datasets.DeleteDataset(groupId.Value, datasetId);
            }
            catch(Exception ex)
            {
                var message = GetMessage(ex);
                throw new Exception("Failed to delete dataset." + message);
            }
        }

        public void BindDatasetToReport(Guid reportId, string datasetId)
        {
            try
            {
                client.Reports.RebindReport(reportId, new RebindReportRequest(datasetId));
            }
            catch(Exception ex)
            {
                var message = GetMessage(ex);
                throw new Exception("Failed to bind dataset to report." + message);
            }
        }

        public List<Report> GetReports(Guid? groupId = null)
        {
            if(groupId == null)
                return client.Reports.GetReports().Value.ToList();
            return client.Reports.GetReports(groupId.Value).Value.ToList();
        }

        public void UpdateRowsInTable(string datasetId, string tableName, List<object> data)
        {
            DeleteRowsInTable(datasetId, tableName);
            AddRowsToTable(datasetId, tableName, data);
        }
        public void AddRowsToTable(string datasetId, string tableName, List<object> data)
        {
            try
            {
                client.Datasets.PostRows(datasetId, tableName, new PostRowsRequest(data));
            }
            catch(Exception ex)
            {
                throw new Exception($"Failed to add data to table {tableName}.");
            }
        }

        public List<Object> GetDatasetColumns(string datasetId)
        {
            return ExecuteQuery(getColumnsQuery, datasetId).Results[0].Tables[0].Rows.ToList();
        }
        public List<Object> GetTableRows(string datasetId, string tableName)
        {
            var query = GetTableRowsQuery.Replace("@TableName", tableName);
            return ExecuteQuery(query, datasetId).Results[0].Tables[0].Rows.ToList();
        }
        public List<Object> GetTableRowsWithParsedColumnNames(string datasetId, string tableName)
        {
            var rows = GetTableRows(datasetId, tableName);
            var data = new List<Object>();
            foreach (var row in rows)
            {
                var jsonObj = row as JObject;
                var dictObj = jsonObj.ToObject<Dictionary<string, string>>();
                var newObj = new JObject();
                foreach (var key in dictObj.Keys)
                {
                    var newKey = key.Split('[')[1].Split(']')[0];
                    newObj.Add(newKey, dictObj[key]);
                }
                data.Add(newObj);
            }

            return data;
        }

        public DatasetExecuteQueriesResponse ExecuteQuery(string query, string datasetId)
        {
            var objQuery = new DatasetExecuteQueriesQuery(query);
            var objQueries = new DatasetExecuteQueriesRequest(new List<DatasetExecuteQueriesQuery>());
            objQueries.Queries.Add(objQuery);
            
            return client.Datasets.ExecuteQueries(datasetId, objQueries);
        
        }
        public void ExportTest()
        {
            var groupId = new Guid("515165ba-a80a-44ac-86e5-dd5f19e053a6");
            var reportId = new Guid("90ace1e0-e078-45ba-8ef5-f76919334a38");

            var stream = client.Reports.ExportReport(reportId);
            Console.WriteLine(stream);
        }

        public void DeleteRowsInTable(string datasetId, string tableName)
        {
            try
            {
                client.Datasets.DeleteRows(datasetId, tableName);
            }
            catch(Exception ex)
            {
                var message = GetMessage(ex);
                throw new Exception($"Failed to delete data in table {tableName}" + message);
            }
        }

        public IList<Datasource> GetDatasources(string datasetId){
            var datasources = client.Datasets.GetDatasources(datasetId);

            foreach (var item in datasources.Value)
            {
                Console.WriteLine(item.ConnectionDetails);
            }

            return datasources.Value;
        }

        public void SetDatasource(string path, string datasetId)
        {
            var dataSourceParam = new UpdateMashupParameterDetails("DataSource", path);
            var updateRequest = new UpdateMashupParametersRequest(dataSourceParam);
            client.Datasets.UpdateParameters(datasetId, updateRequest);
        }

        public static PowerBIGateway GetInstance()
        {
            return api;
        }

        private string GetMessage(Exception ex)
        {
            var httpEx = ex as HttpOperationException;
            string message = "\n";
            if (httpEx != null)
            {
                var response = JObject.Parse(httpEx.Response.Content);
                message += Regex.Replace(response["error"]["message"].ToString(), "<.*?>", String.Empty);
            }
            return message;
        }
        
    }
}
