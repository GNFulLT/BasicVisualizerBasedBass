using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.BassWasapi;

namespace WindowsFormsApp1
{
    public class Analyzer
    {
        private List<double> _spectrumData;
        private List<double> _tempData;
        private bool _enabled;
        public float[] _fft;
        private WASAPIPROC _process;
        private int devindex = 0;
        public ComboBox _deviceList;//Device List

        private bool _initialized;

        private int _lines = 10; // number of spectrum lines 
        public Analyzer()
        {
            _fft = new float[8192];
            _spectrumData = new List<double>();
            _deviceList = new ComboBox();
            _process = new WASAPIPROC(Process);
            _enabled = false;

            Init(0);
        }

        public bool DisplayEnable { get; set; }

        public bool Enable
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (value)
                {
                    if (!_initialized)
                    {
                        var array = (_deviceList.Items[_deviceList.SelectedIndex] as string).Split(' ');
                        devindex = Convert.ToInt32(array[0]);
                        bool result = BassWasapi.BASS_WASAPI_Init(devindex, 0, 0, BASSWASAPIInit.BASS_WASAPI_BUFFER, 1f, 0.05f, _process, IntPtr.Zero);
                        if (!result)
                        {
                            var error = Bass.BASS_ErrorGetCode();
                            MessageBox.Show(error.ToString());
                        }
                        else
                        {
                            _initialized = true;
                        }
                    }
                    BassWasapi.BASS_WASAPI_Start();
                }
                else BassWasapi.BASS_WASAPI_Stop(true);
                System.Threading.Thread.Sleep(500);
            }
        }
        private void Init(int Channel)
        {
            bool result = false;
            for (int i = 0; i < BassWasapi.BASS_WASAPI_GetDeviceCount(); i++)
            {
                var device = BassWasapi.BASS_WASAPI_GetDeviceInfo(i);
                if (device.IsEnabled && device.IsLoopback)
                {
                    _deviceList.Items.Add(string.Format("{0} - {1}", i, device.name));
                }
            }
            _deviceList.SelectedIndex = Channel;
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, false);
            result = Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            if (!result) throw new Exception("Init Error");
        }

        public void UpdateSpectrumData()
        {
            _tempData = new List<double>(_spectrumData);
            _spectrumData.Clear();

            int ret = BassWasapi.BASS_WASAPI_GetData(_fft, (int)BASSData.BASS_DATA_FFT8192);
            if (ret < 0) return;
            int x, y, b0 = 0;

            for(x = 0; x < _lines; x++)
            {
                double peak = 0;
                int b1 = (int)Math.Pow(2, x * 10.0 / (_lines - 1));
                if (b1 <= b0) b1 = b0 + 1;
                for (; b0 < b1; b0++)
                {
                    if (peak < _fft[1 + b0]) peak = _fft[1 + b0];
                }

                y = (int)(Math.Sqrt(peak) * 3 * 255 - 4);
                if (y > 255) y = 255;
                if (y < 0) y = 0;
                _spectrumData.Add(y);
            }
        }

        public List<double> GetSpectrumData()
        {
            return _spectrumData;
        }

        public string GetDevice()
        {
            return _deviceList.Items[0].ToString();
        }
        private int Process(IntPtr buffer, int length, IntPtr user)
        {
            return length;
             
        }


        public double GetPeek()
        {
            double rate = 0;
            for(int i = 0; i < _tempData.Count; i++)
            {
                double dif = 0;
                dif = _spectrumData[i] - _tempData[i];
                rate += dif;
            }
            return rate;
        }
    }
}
