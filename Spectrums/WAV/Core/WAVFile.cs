using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.WAV.Exceptions;

namespace WindowsFormsApp1.WAV.Core
{
    public class WAVFile
    {
        private bool _isReadyToUse = false;

        public WAVFile()
        {
        
        }

        public void OpenMP4File(string path)
        {
            throw new WAVFileConvertingErrorException();
        }


        private bool CheckUsability()
        {
            throw new WAVFileUninitializedException();
        }

        private bool CheckIsMp4File()
        {
            throw new WAVFileConvertingErrorException();
        }
    }
}
