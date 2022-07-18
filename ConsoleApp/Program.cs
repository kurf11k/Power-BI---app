using BusinessLayer;
using BusinessLayer.DatasetTemplates;
using BusinessLayer.PushDatasetParsers;
using BusinessLayer.PushDatasetParsers;
using DataLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            PowerBIGateway api = PowerBIGateway.GetInstance();
            App app = App.GetInstance();
            
            var isLogged = api.Login();
            if (!isLogged)
                return;

            /*
            var datasetId = "0a981ee9-ff5c-4f82-88e8-2fd8aaeee3d9";
            var tableName = "CRS_Dates";

            api.AddRowsToTable(datasetId, tableName, DataToImport());
            //api.DeleteRowsInTable(datasetId, tableName);

            //api.AddRowsToTable(datasetId, "Parameters", ImportParameters());
            var reader = new PushDatasetReader(datasetId);
            //api.GetDatasetProperties(datasetId, CalculatedTableParser.calculatedTablesName);
            */
            /*
            var datasetId = "680515eb-7f97-478c-8735-0eb59523b48a";
            
            var query = "DEFINE var result = AddColumns(SUMMARIZECOLUMNS(CRS[Category], \"CompletedUnfinished\", Calculate(COUNT(CRS[Progress]), FILTER(CRS, [Progress]=\"Completed\")), \"Remaining\", Calculate(COUNT(CRS[Progress]), FILTER(CRS, ([Progress]=\"In Progress\" || [Progress]=\"Not Started\")))), \"Completed\", (([CompletedUnfinished]+ 0))) EVALUATE result";
            //var query = "DEFINE var result = ADDCOlumns(SUMMARIZECOLUMNS(SYRS[CRS_ID], \"Completed\", CALCULATE(COUNT(SYRS[CRS_ID]), FILTER(SYRS, SYRS[Progress] = \"Completed\")), \"Remaining\", CALCULATE(COUNT(SYRS[CRS_ID]), FILTER(SYRS, (SYRS[Progress] = \"Not Started\" || SYRS[Progress] = \"In Progress\")))),\"Ratio\", [Completed]/([Completed]+[Remaining])) EVALUATE result";
            //var query = "DEFINE var result = ADDCOlumns(SUMMARIZECOLUMNS(SWRS[SYRS_ID], \"Completed\", CALCULATE(COUNT(SWRS[SYRS_ID]), FILTER(SWRS, SWRS[Status] = \"Passed Test\")), \"Remaining\", CALCULATE(COUNT(SWRS[SYRS_ID]), FILTER(SWRS, (SWRS[Status] <> \"Passed Test\" )))),\"Ratio\", [Completed]/([Completed]+[Remaining])) EVALUATE result EVALUATE result";
            //var query = "EVALUATE SUMMARIZECOLUMNS(Team[Name], Attendance[Activity], \"Count\", COUNT(Attendance[Activity]))";
            List<Object> data = new List<object>();
            var response = api.ExecuteQuery(query, datasetId).Results[0].Tables[0].Rows;
            foreach(var row in response)
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
            */
            
            
            
            var next = "y";
            while(next == "y")
            {
                try
                {
                    var datasetTemplate = JsonConvert.DeserializeObject<DatasetTemplate>(FileGateway.GetInstance().GetDatasetTemplate());
                    var pushDatasetParser = new PushDatasetParser();

                    datasetTemplate.Name = "Testovaci dataset";
                    var datasetId = pushDatasetParser.CreatePushDataset(datasetTemplate, new Dictionary<string, string>());

                    api.RemoveDataset(datasetId);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("y - pro pokračování");
                next = Console.ReadLine();
            }


            Console.ReadLine();
            
        }
        public static void ExecuteQuery()
        {
            PowerBIGateway api = PowerBIGateway.GetInstance();
            var datasetId = "c23ef6a1-4889-4107-9fe1-f76d5ed14ea2";
            //var query = "DEFINE var latestDateOrder = MAX(CRS_Dates[Order]) var latestDate = LOOKUPVALUE(CRS_Dates[Date], CRS_Dates[Order], latestDateOrder) var result = AddColumns(SUMMARIZECOLUMNS(CRS[Category], \"CompletedUnfinished\", Calculate(COUNT(CRS_Measurements[Progress]), FILTER(CRS_Measurements, [Progress] = \"Completed\" && [Date] = latestDate)), \"Remaining\", Calculate(COUNT(CRS_Measurements[Progress]), FILTER(CRS_Measurements, ([Progress] = \"In Progress\" || [Progress] = \"Not Started\") && [Date] = latestDate))), \"Completed\", (([CompletedUnfinished] + 0))) EVALUATE result";
            //var query = "DEFINE var result = SUMMARIZECOLUMNS(Product[Name], \"CompletedUnfinished\", Calculate(COUNT(Product[Name]))) EVALUATE result ";
            //var query = "DEFINE var latestDateOrder = MAX(SYRS_Dates[Order]) var latestDate = LOOKUPVALUE(SYRS_Dates[Date], SYRS_Dates[Order], latestDateOrder) var result = ADDCOlumns(SUMMARIZECOLUMNS(SYRS[CRS_ID], \"Completed\", CALCULATE(COUNT(SYRS[CRS_ID]), FILTER(SYRS_Measurements, SYRS_Measurements[Progress] = \"Completed\" && [Date] = latestDate)), \"Remaining\", CALCULATE(COUNT(SYRS[CRS_ID]), FILTER(SYRS_Measurements, (SYRS_Measurements[Progress] = \"Not Started\" || SYRS_Measurements[Progress] = \"In Progress\") && [Date] = latestDate))),\"Ratio\", [Completed]/([Completed]+[Remaining])) EVALUATE result";
            var query = "DEFINE var latestDateOrder = MAX(SWRS_Dates[Order]) var latestDate = LOOKUPVALUE(SWRS_Dates[Date], SWRS_Dates[Order], latestDateOrder) var result = ADDCOlumns(SUMMARIZECOLUMNS(SWRS[SYRS_ID], \"Completed\", CALCULATE(COUNT(SWRS[SYRS_ID]), FILTER(SWRS_Measurements,SWRS_Measurements[Status] = \"Passed Test\" && [Date] = latestDate)), \"Remaining\", CALCULATE(COUNT(SWRS[SYRS_ID]), FILTER(SWRS_Measurements, (SWRS_Measurements[Status] <> \"Passed Test\" ) && [Date] = latestDate))),\"Ratio\", [Completed]/([Completed]+[Remaining])) EVALUATE result";


            var result = api.ExecuteQuery(query, datasetId);
            Console.WriteLine(result);
        }
        public static List<Object> ImportParameters()
        {
            string dataJson = @"
                            [
                                { ""Key"": ""ProjectStart"", ""Value"": ""01.01.2022""},                                
                                { ""Key"": ""Budget"", ""Value"": ""950000""},
                                { ""Key"": ""ProjectRelease"", ""Value"": ""01.01.2023""},
                                { ""Key"": ""RiskBuffer"", ""Value"": ""1,3""}
                            ]";
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Object>>(dataJson.ToString());
            return data;
        }
        public static List<Object> DataToImport()
        {
            var dateObj = new JObject();
            dateObj.Add("Date", "2020/03/05");
            dateObj.Add("Order", "20200305");

            var dateObj1 = new JObject();
            dateObj1.Add("Date", "05.03.2022");
            dateObj1.Add("Order", "20200305");

            var dateObj2 = new JObject();
            dateObj2.Add("Date", "05/03/2022");
            dateObj2.Add("Order", "20200305");


            var dateData = new List<Object>();
            dateData.Add(dateObj);
            dateData.Add(dateObj1);
            dateData.Add(dateObj2);

            return dateData;
        }
        public static void Import()
        {

            string data = @"
                            [
                                { ""CRS_ID"": ""1"", ""DateUpdate"": ""9/6/2021"", ""Status"": ""High"", ""SatisfiedByCustomer"": ""No""},                                
                                { ""CRS_ID"": ""2"", ""DateUpdate"": ""9/6/2021"", ""Status"": ""High"", ""SatisfiedByCustomer"": ""No""}
                            ]";
            var dataJson = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Object>>(data.ToString());

            //api.UpdateTable(datasetId, "CRS_Measurements", dataJson);


            data = @"
                                [
                                    { ""ID"": ""1"", ""Description"": ""Tom"", ""Priority"": ""High""},                                
                                    { ""ID"": ""2"", ""Description"": ""Jerry"", ""Priority"": ""High""}
                                ]
                            ";
            dataJson = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Object>>(data.ToString());

        }
    }
}
