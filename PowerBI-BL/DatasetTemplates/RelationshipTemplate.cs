using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DatasetTemplates
{
    public class RelationshipTemplate
    {
        public string Name { get; set; } 
        public string FromTable { get; set; }
        public string FromColumn { get; set; }
        public string ToTable { get; set; }
        public string ToColumn { get; set; }
        public RelationshipTypes Type { get; set; }

    }
}
