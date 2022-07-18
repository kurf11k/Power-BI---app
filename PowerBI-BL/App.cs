using BusinessLayer.DatasetTemplates;
using BusinessLayer.PushDatasetParsers;
using DataLayer;
using Microsoft.PowerBI.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class App
    {
        private static App app = new App();

        public static string ProjectSuffixId = " - Power BI - App";

        private PowerBIGateway api;
        private FileGateway fileGateway;

        public DatasetTemplate DatasetTemplate { get 
            {
                return JsonConvert.DeserializeObject<DatasetTemplate>(fileGateway.GetDatasetTemplate());
            }}
        
        public List<Project> Projects { get; set; }

        private App()
        {
            Projects = new List<Project>();
            api = PowerBIGateway.GetInstance();
            fileGateway = FileGateway.GetInstance();
                        
        }
        public bool IsUserLogged()
        {
            return api.IsLogged();
        }
        public bool LoginToPowerBI()
        {
            return api.Login();
        }
        public List<Project> LoadProjects()
        {
            var reports = api.GetReports();
            foreach(var report in reports)
            {
                if (IsProjectFromApp(report.Name))
                {
                    var nameWithoutSuffix = report.Name.Replace(ProjectSuffixId, "");
                    var project = new Project(report.Id.ToString(), nameWithoutSuffix, report.Name, report.DatasetId);
                    Projects.Add(project);
                }
                
            }
            return Projects;
        }
        private bool IsProjectFromApp(string projectName)
        {
            return projectName.EndsWith(ProjectSuffixId);
        }

        public bool IsProjectNameAvailable(string projectName)
        {
            foreach (var project in Projects)
            {
                if (project.Name == projectName)
                    return false;
            }
            return true;
        }
        public void CreateProject(Dictionary<string ,string> parameters, DatasetTemplate datasetTemplate, Action<Project> ProjectAdded, Action<Exception> ProjectRejected)
        {
            Thread thread = new Thread(() => {
                try
                {
                    AreProjectParametersValid(parameters);
                    var name = parameters["ProjectName"];
                    var fullName = name + ProjectSuffixId;
                    datasetTemplate.Name = fullName;
    
                    var pushDatasetParser = new PushDatasetParser();
                    var datasetId = pushDatasetParser.CreatePushDataset(datasetTemplate, parameters);

                    try
                    {
                        var importReportResult = api.ImportPBIX(fullName);
                        var reportId = importReportResult.Reports[0].Id;
                        api.BindDatasetToReport(reportId, datasetId);
                        api.RemoveDataset(importReportResult.Datasets[0].Id);
                        var project = new Project(reportId.ToString(), name, fullName, datasetId);
                        Projects.Add(project);
                        ProjectAdded(project);
                    }
                    catch(Exception ex)
                    {
                        api.RemoveDataset(datasetId);
                        throw ex;
                    }     
                }
                catch(Exception ex)
                {
                    ProjectRejected(ex);
                    return;
                }            
            });

            thread.Start();
        }

        private void AreProjectParametersValid(Dictionary<string, string> parameters)
        {
            if (parameters["ProjectName"] == "")
                throw new Exception("You must enter project name.");
            
            if (parameters["Budget"] == "")
                throw new Exception("You must enter budget.");

            double value = 0;
            var isBudgedValid = Double.TryParse(parameters["Budget"], out value);
            if (!isBudgedValid)
                throw new Exception("Budget is not a valid number.");
            
            var isBufferValid = Double.TryParse(parameters["RiskBuffer"], out value);
            if (!isBufferValid)
                throw new Exception("Risk buffer is not a valid number.");

            if (!IsProjectNameAvailable(parameters["ProjectName"]))
                throw new Exception($"Project with name {parameters["ProjectName"]} is already exist.");
        }
        public void RemoveProject(object sender, EventArgs args)
        {
            var project = sender as Project;
            Projects.Remove(project);
        }
        
        public static App GetInstance()
        {
            return app;
        }
    }
}
