using BusinessLayer.DatasetTemplates;
using BusinessLayer.MyExceptions;
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
    public class DynamicTableParser : TableParser
    {
        public static string RelationshipSuffix = "_Relationship";
        public static string MeasurementsTableSuffix = "_Measurements";

        public static string[] MeasurementsColumnNames = new string[] { "ID", "Date" };
        public static ColumnTypes[] MeasurementsColumnTypes = new ColumnTypes[] { ColumnTypes.String, ColumnTypes.String };
        public override CreateDatasetRequest CreateTableStructure(TableTemplate TableTemplate, CreateDatasetRequest Dataset)
        {
            var normalTable = new Table();
            normalTable.Name = TableTemplate.Name;
            normalTable.Columns = new List<Column>();
            Dataset.Tables.Add(normalTable);
            var measurementTable = GetDynamicTable(TableTemplate.Name);


            Dataset.Tables.Add(measurementTable);

            var relationToMeasurement = new Relationship(measurementTable.Name + RelationshipSuffix, measurementTable.Name, measurementTable.Columns[0].Name, TableTemplate.Name, TableTemplate.Columns[0].Name, RelationshipTypes.BothDirections.ToString());
            //var relationToDates = new Relationship(datesTable.Name + relationshipSuffix, datesTable.Name, datesTable.Columns[0].Name, measurementTable.Name, measurementTable.Columns[0].Name, RelationshipTypes.BothDirections.ToString());

            Dataset.Relationships.Add(relationToMeasurement);
            //Dataset.Relationships.Add(relationToDates);

            foreach (var colTemplate in TableTemplate.Columns)
            {
                var newCol = new Column(colTemplate.Name, colTemplate.Type.ToString());
                normalTable.Columns.Add(newCol);
                if (colTemplate.IsDynamic)
                    measurementTable.Columns.Add(newCol);
            }

            normalTable.Measures = ParseMeasures(TableTemplate.Measures);

            return Dataset;
        }

        private Table GetDynamicTable(string name)
        {
            var measuredTableColumns = CreateColumns(MeasurementsColumnNames, MeasurementsColumnTypes);
            return new Table(name + MeasurementsTableSuffix, measuredTableColumns.ToList());
        }
        private List<Column> CreateColumns(string[] columnNames, ColumnTypes[] types)
        {
            List<Column> columns = new List<Column>();
            for(var i = 0; i < columnNames.Length; i++)
            {
                columns.Add(new Column(columnNames[i], types[i].ToString()));
            }
            return columns;
        }

        protected override void ParseArtefact(string DatasetId, TableTemplate TableTemplate, DataTable CsvTable, string Date)
        {       
            var staticData = new List<Object>();
            var dynamicData = new List<Object>();           

            foreach (DataRow row in CsvTable.Rows)
            {
                int countEmptyColumns = 0;
                var staticObj = new JObject();
                var dynamicObj = new JObject();
                dynamicObj.Add("Date", Date);

                for (var i = 0; i < TableTemplate.Columns.Count; i++)
                {
                    var colIndex = GetColumnIndex(TableTemplate.Columns[i].Name, CsvTable);
                    var value = row[colIndex] as string;
                    var column = CsvTable.Columns[colIndex];
                    if (value == null || value == "")
                        countEmptyColumns += 1;
                    else
                    {
                        staticObj.Add(column.ColumnName, value.ToString());
                        if (IsColumnDynamic(column.ColumnName, TableTemplate.Columns) || column.ColumnName == "ID")
                            dynamicObj.Add(column.ColumnName, value.ToString());
                    }
                    
                }
                if (countEmptyColumns != TableTemplate.Columns.Count && countEmptyColumns > 0)
                    throw new MissingRowDataException();
                else if (countEmptyColumns == 0)
                {
                    staticData.Add(staticObj);
                    dynamicData.Add(dynamicObj);
                }
            }

            //Static Table

            api.UpdateRowsInTable(DatasetId, TableTemplate.Name, staticData);

            //Measurements Tabled
            api.AddRowsToTable(DatasetId, TableTemplate.Name + MeasurementsTableSuffix, dynamicData);     
        }
        private bool IsColumnDynamic(string searchedColumn,List<ColumnTemplate> columns)
        {
            foreach (var col in columns)
            {
                if (col.Name == searchedColumn && col.IsDynamic) return true;
            }
            return false;
        }

        private int GetColumnIndex(string name, DataTable csvTable)
        {
            int i = 0;
            foreach(DataColumn column in csvTable.Columns)
            {
                if (column.ColumnName == name)
                    return i;
                i++;
            }
            return i;
        }
    }
}
