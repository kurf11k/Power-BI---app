using BusinessLayer.PushDatasetParsers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DatasetTemplates
{
    public class TableTemplate
    {
        public string PrimaryKey { get; set; }
        public string Name { get; set; }
        public TableTypes Type { get; set; }
        public List<ColumnTemplate> Columns { get; set; }
        public string Expression { get; set; }
        public List<MeasureTemplate> Measures { get; set; }
        public bool IsDefault { get; set; }
        [JsonConstructor]
        public TableTemplate()
        {
            Measures = new List<MeasureTemplate>();
            Columns = new List<ColumnTemplate>();
        }
        public TableTemplate(string name): this()
        {
            Name = name;
            Type = TableTypes.Static;
        }
        public TableTemplate(string name, string expression) : this(name)
        {
            Expression = expression;
            Type = TableTypes.Calculated;
        }
        public void AddColumn(ColumnTemplate columnToAdd)
        {
            foreach (var column in Columns)
            {
                if (column.Name.ToLower() == columnToAdd.Name.ToLower())
                    throw new Exception($"The column name already exists in the table.");
            }

            foreach (var measure in Measures)
            {
                if (measure.Name.ToLower() == columnToAdd.Name.ToLower())
                    throw new Exception("The column name is already in the table as a measure.");
            }

            if(columnToAdd.Name.ToLower() == DynamicTableParser.MeasurementsColumnNames[0].ToLower() || columnToAdd.Name.ToLower() == DynamicTableParser.MeasurementsColumnNames[1].ToLower())
                throw new Exception("The column name is key word, you must use some other.");

            if (columnToAdd.IsDynamic)
                Type = TableTypes.Dynamic;

            Columns.Add(columnToAdd);
        }

        public void RemoveColumn(ColumnTemplate columnToRemove)
        {
            foreach (var column in Columns)
            {
                if (columnToRemove.Name == column.Name)
                {
                    columnToRemove = column;
                    break;
                }
            }
            Columns.Remove(columnToRemove);
        }

        public void AddMeasure(MeasureTemplate measureToAdd)
        {
            foreach (var column in Columns)
            {
                if (column.Name.ToLower() == measureToAdd.Name.ToLower())
                    throw new Exception($"The measure name is already in the table as a column.");
            }

            foreach (var measure in Measures)
            {
                if (measure.Name.ToLower() == measureToAdd.Name.ToLower())
                    throw new Exception($"The measure name already exists in the table.");
            }

            if (measureToAdd.Name.ToLower() == DynamicTableParser.MeasurementsColumnNames[0].ToLower() || measureToAdd.Name.ToLower() == DynamicTableParser.MeasurementsColumnNames[1].ToLower())
                throw new Exception("The measure name is key word, you must use some other.");

            Measures.Add(measureToAdd);
        }

        public void RemoveMeasure(MeasureTemplate measureToRemove)
        {
            foreach (var measure in Measures)
            {
                if (measureToRemove.Name == measure.Name)
                {
                    measureToRemove = measure;
                    break;
                }
            }
            Measures.Remove(measureToRemove);
        }
        public void ChangeTypeFromCalculated()
        {
            foreach (var column in Columns)
            {
                if (column.IsDynamic)
                {
                    Type = TableTypes.Dynamic;
                    return;
                }
            }
            Type = TableTypes.Static;
        }
        public void SetAllColumnsStatic()
        {
            foreach (var col in Columns)
            {
                col.IsDynamic = false;
            }
        }

        public bool NeedID()
        {
            var IsDynamicTbale = Type == TableTypes.Dynamic;
            var HasID = Columns[0].Name == "ID";
            if (IsDynamicTbale && !HasID)
            {
                var col = new ColumnTemplate("ID", ColumnTypes.String, false);
                Columns.Insert(0, col);
                return true;
            }
            return false;
        }

        public bool RemoveIdColumn()
        {
            foreach(var col in Columns)
            {
                if (col.IsDynamic)
                {
                    return false;
                }
            }
            if(Columns.Count > 0 && Columns[0].Name == "ID" && !Columns[0].IsDefault)
            {
                Columns.RemoveAt(0);
                Type = TableTypes.Static;
                return true;
            }
            return false;
        }
    }
}
