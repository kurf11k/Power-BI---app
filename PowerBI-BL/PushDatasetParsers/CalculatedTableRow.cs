using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CalculatedTableRow : Row
    {
        public string TableName { get; set; }
        public string Expression { get; set; }

        public CalculatedTableRow(string tableName, string expression)
        {
            TableName = tableName;
            Expression = expression;
        }
    }
}
