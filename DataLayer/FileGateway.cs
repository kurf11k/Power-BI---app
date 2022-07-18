using LumenWorks.Framework.IO.Csv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class FileGateway
    {
        static FileGateway gateway = new FileGateway();

        //private static string pathToUserDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //private static string pathToDatasourcesFiles = $"{pathToUserDocuments}/PowerBIApp/@ProjectName";
        
        private const string pathToTemplatesFolder = "../../../../Data/Templates/";

        private const string templatePBIXFile = "template.pbix";
        private const string pathToTemplatePBIXFile = pathToTemplatesFolder + templatePBIXFile;

        private const string datasetTemplateFile = "dataset_template.json";
        private const string pathToDatasetTemplateFile = pathToTemplatesFolder + datasetTemplateFile;

        private const string dynamicColumnsFile = "default_dynamic_columns.json";
        private const string pathToDynamicColumnsFile = pathToTemplatesFolder + dynamicColumnsFile;
        private FileGateway() { }
        
        public FileStream GetPBIXTemplate()
        {
            return new FileStream(pathToTemplatePBIXFile, FileMode.Open);
        }
        public string GetDatasetTemplate()
        {
            return ReadJsonFile(pathToDatasetTemplateFile);
        }

        public string GetDynamicColumns()
        {
            return ReadJsonFile(pathToDynamicColumnsFile);
        }

        public DataTable ReadDataFromSpecificationFile(string path)
        {
            var csvTable = new DataTable();
            using (var csvReader = new CsvReader(new StreamReader(File.OpenRead(path)), true, ';'))
            {
                csvTable.Load(csvReader);
            }
            return csvTable;
        }
        public static FileGateway GetInstance()
        {
            return gateway;
        }

        private string ReadJsonFile(string path)
        {
            if (File.Exists(path))
                return File.ReadAllText(path);
            else
                return "{}";
        }
    }
}
