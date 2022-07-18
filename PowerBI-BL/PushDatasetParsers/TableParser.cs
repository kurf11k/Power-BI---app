using BusinessLayer.DatasetTemplates;
using BusinessLayer.MyExceptions;
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
    public abstract class TableParser
    {
        protected PowerBIGateway api = PowerBIGateway.GetInstance();
        public abstract CreateDatasetRequest CreateTableStructure(TableTemplate TableTemplate, CreateDatasetRequest Dataset);
        protected abstract void ParseArtefact(string DatasetId, TableTemplate TableTemplate, DataTable CsvTable, string Date);
        protected List<Measure> ParseMeasures(List<MeasureTemplate> MeasureTemplates)
        {
            var Measures = new List<Measure>();
            foreach (var measure in MeasureTemplates)
            {
                if(measure.Format == null)
                    Measures.Add(new Measure(measure.Name, measure.Expression));
                else
                    Measures.Add(new Measure(measure.Name, measure.Expression, formatString: measure.Format));
            }
            
            return Measures;
        }
        public void UpdateArtefact(string DatasetId, TableTemplate TableTemplate, DataTable CsvTable, string Date)
        {
            if(CsvTable != null)
                CheckFileStructure(CsvTable, TableTemplate);

            ParseArtefact(DatasetId, TableTemplate, CsvTable, Date);
        }

        private void CheckFileStructure(DataTable CsvTable, TableTemplate TableTemplate)
        {
            foreach (var col in TableTemplate.Columns)
            {
                if (!CsvTable.Columns.Contains(col.Name)) throw new MissingColumnException(col.Name);
            }
        }
        
    }
}
