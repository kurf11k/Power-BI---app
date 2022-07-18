using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.MyExceptions
{
    public class MissingRowDataException : Exception
    {
        public MissingRowDataException() : base($"File contains some empty values.")
        {

        }
    }
}
