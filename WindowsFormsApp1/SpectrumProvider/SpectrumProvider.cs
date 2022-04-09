using CSCore.DSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.SpectrumProvider
{
    public class SpectrumProvider : FftProvider
    {
        public SpectrumProvider(int channels, FftSize fftSize) : base(channels, fftSize)
        {
            
        }
    }
}
