using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DatasetTemplates
{
    public class DatasetTemplate
    {
        public string Name { get; set; }
        public List<TableTemplate> Tables { get; set; }
        public List<RelationshipTemplate> Relationships { get; set; }
        [JsonConstructor]
        public DatasetTemplate()
        {
            Tables = new List<TableTemplate>();
            Relationships = new List<RelationshipTemplate>();
        }
        public void AddRelationship(RelationshipTemplate relation)
        {
            foreach(var rel in Relationships)
            {
                if (rel.FromTable == relation.FromTable && rel.ToTable == relation.ToTable || rel.FromTable == relation.ToTable && rel.ToTable == relation.FromTable)
                    throw new Exception("Relationship with that name is already exist.");
            }
            Relationships.Add(relation);
        }
        public void RemoveRelationship(RelationshipTemplate relation)
        {
            RelationshipTemplate toRemove = null;
            foreach (var rel in Relationships)
            {
                if (rel.FromTable == relation.FromTable && rel.FromColumn == relation.FromColumn && rel.ToTable == relation.ToTable && rel.ToColumn == relation.ToColumn)
                {
                    toRemove = rel;
                    break;
                }
            }
            Relationships.Remove(toRemove);
        }
        public void AddTable(TableTemplate tableToAdd)
        {
            if (tableToAdd.Name == "")
            {
                throw new Exception("You cannot add artefact without name.");
            }
            foreach (var table in Tables)
            {
                if (table.Name == tableToAdd.Name)
                    throw new Exception("Table with that name is already exist");
            }
            Tables.Add(tableToAdd);
        }

        public void RemoveTable(TableTemplate tableToRemove)
        {
            foreach (var table in Tables)
            {
                if(tableToRemove.Name == table.Name)
                {
                    tableToRemove = table;
                    break;
                }
            }
            Tables.Remove(tableToRemove);
        }
        public void Check()
        {
            foreach (var tab in Tables)
            {
                if (tab.Columns.Count == 0)
                    throw new Exception($"Table {tab.Name} has no columns. The table must have at least one column. ");
            }
        }

    }
}
