using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsFormsApp1;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Particles p;
        System.Timers.Timer t1;
       
        double add =1;
        public string[] Files
        { get; set; }
        Analyzer analyzer = new Analyzer();
        public MainWindow()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            InitializeComponent();
            p = new Particles(1920, 1080);
            t1 = new System.Timers.Timer();
            t1.Interval = 5;
            t1.Elapsed += Render;
            t1.Start();
            p.InitResources(myCanvas);
           
            var converter = new System.Windows.Media.BrushConverter();
            Brush brush = (Brush)converter.ConvertFromString("#ffffff");
            
            analyzer.Enable = true;
            analyzer.UpdateSpectrumData();
           
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            
        }
        private void Render(object sender,EventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() => {


                    analyzer.UpdateSpectrumData();
                   
                    double rate = analyzer.GetPeek();
                    rate /= 100;
                    add += rate;
                    if (add < -1)
                        add = -1;
                    p.RenderParticle((float)add);


                });
            }
            catch (Exception ex)
            {

            }
            
        }

        System.Timers.Timer t2 = new System.Timers.Timer();
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {

                add = 10;
                
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                if (t2.Enabled)
                {
                    t2.Stop();
                    add = 10;
                }
                t2.Elapsed += mrb;
                t2.Interval = 100;
                t2.Start();
            }
        }

        private void mrb(object sender,EventArgs e)
        {
            add--;
            if (add == 1)
                t2.Stop();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

       
    }
}
