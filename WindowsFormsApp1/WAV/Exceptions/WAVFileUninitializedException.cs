using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.WAV.Exceptions
{
    class WAVFileUninitializedException : Exception
    {
        public WAVFileUninitializedException()
        {

        }
        public WAVFileUninitializedException(string message) : base(message)
        {

        }
    }
}
