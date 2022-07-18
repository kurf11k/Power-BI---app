using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DatasetTemplates
{
    public class ColumnTemplate
    {
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ColumnTypes Type { get; set; }
        public bool IsDynamic { get; set; }
        public bool IsDefault { get; set; }

        [JsonConstructor]
        public ColumnTemplate() { }
        public ColumnTemplate(string name, bool isDynamic)
        {
            Name = name;
            IsDynamic = isDynamic;
        }
        public ColumnTemplate(string name, ColumnTypes type, bool isDynamic) :this(name, isDynamic)
        {
            Type = type;
        }
    }
}
