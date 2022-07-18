using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.MyExceptions
{
    public class UnknownColumnException : Exception
    {
        public UnknownColumnException(string columnName) : base($"File has reduntant column: {columnName}")
        {
            
        }
    }
}
