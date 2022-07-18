using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.MyExceptions
{
    public class MissingColumnException : Exception
    {
        public MissingColumnException(string columnName) : base($"Column {columnName} wasn´t find, wrong input file.")
        {

        }
    }
}
