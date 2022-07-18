using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Exceptions
{
    public class ImportPBIXException : Exception
    {
        public ImportPBIXException() : base($"An unspecified error occurred while importing the project template.")
        {

        }
    }
}
