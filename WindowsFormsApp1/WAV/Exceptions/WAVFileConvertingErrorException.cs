using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.WAV.Exceptions
{
    class WAVFileConvertingErrorException : Exception
    {
        public WAVFileConvertingErrorException()
        {

        }
        public WAVFileConvertingErrorException(string message) : base(message)
        {

        }
    }
}
