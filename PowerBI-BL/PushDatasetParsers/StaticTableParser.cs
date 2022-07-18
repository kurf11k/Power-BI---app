using BusinessLayer.DatasetTemplates;
using BusinessLayer.MyExceptions;
using DataLayer;
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
    public class StaticTableParser : TableParser
    {
        public override CreateDatasetRequest CreateTableStructure(TableTemplate TableTemplate, CreateDatasetRequest Dataset)
        {
            var staticTable = new Table();
            staticTable.Name = TableTemplate.Name;
            staticTable.Columns = new List<Column>();
            Dataset.Tables.Add(staticTable);            

            foreach (var colTemplate in TableTemplate.Columns)
            {
                var newCol = new Column(colTemplate.Name, colTemplate.Type.ToString());
                staticTable.Columns.Add(newCol);
            }

            staticTable.Measures = ParseMeasures(TableTemplate.Measures);

            return Dataset;
        }

        protected override void ParseArtefact(string DatasetId, TableTemplate TableTemplate, DataTable CsvTable, string Date)
        {
            var data = new List<Object>();
            foreach(DataRow row in CsvTable.Rows)
            {
                int countEmptyColumns = 0;
                var obj = new JObject();
                for(var i = 0; i < TableTemplate.Columns.Count; i++)
                {
                    var value = row[i] as string;
                    var column = CsvTable.Columns[i];
                    if (value == null || value == "")
                        countEmptyColumns += 1;
                    else
                    obj.Add(column.ColumnName, value.ToString());
                }
                if (countEmptyColumns != TableTemplate.Columns.Count && countEmptyColumns > 0)
                    throw new MissingRowDataException();
                else if (countEmptyColumns == 0)
                {
                    data.Add(obj);
                }
            }


            api.UpdateRowsInTable(DatasetId, TableTemplate.Name, data);
        }
    }
}
