using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            

            Analyzer analyzer = new Analyzer();

            analyzer.Enable = true;
            Console.WriteLine($"Listening device :{analyzer.GetDevice()}\nPress enter to listen");
            Console.Read();
            while (true)
            {
                List<double> tempdatas = analyzer.GetSpectrumData();
                analyzer.UpdateSpectrumData();
                List<double> datas = analyzer.GetSpectrumData();    
                double a = analyzer.GetPeek();
                
                for(int i = 0; i < datas.Count; i++)
                {
                    if(a != 0)
                    {
                Console.Write(a);
                Console.WriteLine("\n-------------------------------------");

                    }
                }

            }
        }
    }
}
