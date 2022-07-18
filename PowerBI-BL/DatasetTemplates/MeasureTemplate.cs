using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DatasetTemplates
{
    public class MeasureTemplate
    {
        public string Name { get; set; }
        public string Expression { get; set; }
        public string Format { get; set; }
        public bool IsDefault { get; set; }
    }
}
