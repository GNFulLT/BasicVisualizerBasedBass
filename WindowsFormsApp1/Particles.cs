using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Mathematics.Interop;
using Microsoft.Graphics.Canvas.Brushes;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace WindowsFormsApp1
{
    public class Particles
    {
        public static Random random = new Random();

        private int _maxWidth;
        private int _maxHeight;
        public int max_count = 100;
        private List<Particle> _particles;
        public Image image;
        public System.Windows.Controls.UserControl control = new System.Windows.Controls.UserControl();
        private int hDir = 40;
        private int wDir = 40;
        public float radius = 32.0f;
        System.Windows.Controls.Canvas grid1;
        Canvas defaultCanvas;
        public static Brush colorBrush;
        public Particles(int width, int height)
        {
            _maxWidth =width;
            _maxHeight = height;
            _particles = new List<Particle>();

            control.Width = width;
            control.Height = height;
            control.Background = System.Windows.Media.Brushes.Black;
            grid1 = new System.Windows.Controls.Canvas();
            grid1.Width = width;
            grid1.Height = height;
            grid1.Name = "canvas1";
            control.Content = grid1;
           
        }
        public struct Particle
        {
            public Vector2 initialLocation;
            public Vector2 location;
            public Ellipse ellipse;

            public float xVelocity;
            public float yVelocity;

            public Particle(Vector2 location,float xV,float yV)
            {
                initialLocation = location;
                this.location = location;
                xVelocity = xV;
                yVelocity = yV;

                float opac = random.Next(1, 10);
                float opacS = opac / 10;



                ellipse = new Ellipse();
                ellipse.Width = 30.0f;
                ellipse.Height = 30.0f;    
                ellipse.Opacity = opacS;

                var converter = new System.Windows.Media.BrushConverter();
                ImageBrush brsh = new ImageBrush();
                brsh.ImageSource = new BitmapImage(new Uri(@"C:\Users\ugurbey\Desktop\MyAudioVisualizerC#\WindowsFormsApp1\WindowsFormsApp1\Images\particle.png"));
                Brush brush = (Brush)converter.ConvertFromString("#FFFFFF");
                colorBrush = brush;
                ellipse.Fill = brsh;

            }

            public Particle(Vector2 initialLocation,Vector2 location,float xV,float yV)
            {
                this.initialLocation = initialLocation;
                this.location = location;
                xVelocity = xV;
                yVelocity = yV;

                float opac = random.Next(1, 10);
                float opacS = opac / 10;

                ellipse = new Ellipse();
                ellipse.Width = 30.0f;
                ellipse.Height = 10.0f;     
                ellipse.Opacity = opacS;

                ImageBrush brsh = new ImageBrush();
                var converter = new System.Windows.Media.BrushConverter();
                Brush brush = (Brush)converter.ConvertFromString("#FFFFFF");

                brsh.ImageSource = new BitmapImage(new Uri(@"C:\Users\ugurbey\Desktop\MyAudioVisualizerC#\WindowsFormsApp1\WindowsFormsApp1\Images\particle.png"));
                ellipse.Fill = brsh;

            }

            public Particle(Vector2 initialLocation, Vector2 location, float xV, float yV,Ellipse elps)
            {
                this.initialLocation = initialLocation;
                this.location = location;
                xVelocity = xV;
                yVelocity = yV;

                ellipse = elps;
               
            }
        }
       
        public void InitResources(Canvas cnv)
        {               
            defaultCanvas = cnv;
            CreateParticles();

           for(int i = 0; i < max_count; i++)
            {
                Canvas.SetLeft(_particles[i].ellipse, _particles[i].location.X);
                Canvas.SetTop(_particles[i].ellipse, _particles[i].location.Y);
                _particles[i].ellipse.Visibility = System.Windows.Visibility.Visible;
            }

        }
        public void RenderParticle(float additionalVelocity = 1)
        {
            DeleteParticles();
            CreateParticles();
            UpdateParticles(additionalVelocity);
            SetStroke(additionalVelocity);
        }

        private void UpdateParticles(float additionalVelocity)
        {
            additionalVelocity = Clamp(additionalVelocity, 15, 0.25f);
            float additionalVelocity2 = Clamp(additionalVelocity, 10, 0.25f);

            for (int i = 0; i < max_count; i++)
            {
               
            Vector2 newVector = new Vector2(_particles[i].location.X + (_particles[i].xVelocity * additionalVelocity),
                                            _particles[i].location.Y + (_particles[i].yVelocity * additionalVelocity));

                Particle p = new Particle(_particles[i].initialLocation, newVector, _particles[i].xVelocity, _particles[i].yVelocity,_particles[i].ellipse);
               
                _particles[i] = p;
                
                
         
                Canvas.SetLeft(_particles[i].ellipse, _particles[i].location.X);
                Canvas.SetTop(_particles[i].ellipse, _particles[i].location.Y);


            }
        }

        private void CreateParticles()
        {
            while(_particles.Count < max_count)
            {
                Vector2 newPosition = new Vector2(random.Next(0,_maxWidth),random.Next(0,_maxHeight));

                float x = (float)random.Next(-3, 3);
                float y = (float)random.Next(-3, 3);
                if (x == 0)
                    x = 1;
                if (y == 0)
                    y = 1;
                Particle P = new Particle(newPosition,x,y);
                defaultCanvas.Children.Add(P.ellipse);
                _particles.Add(P);
            }
        }

        private void DeleteParticles()
        {
            for(int i= 0; i < _particles.Count; i++)
            {
                if (_particles[i].location.X < 0 || _particles[i].location.Y < 0 || _particles[i].location.X > _maxWidth || _particles[i].location.Y > _maxHeight)
                {

                      defaultCanvas.Children.Remove(_particles[i].ellipse);

                    _particles.RemoveAt(i);
                }
            }
        }

        private float Clamp(float value, float Max, float Min)
        {
            if (value > Max)
            {
                return Max;
            }

            if (value < Min)
            {
                return Min;
            }

            return value;
        }

        private void SetStroke(float additionalVelocity)
        {
            additionalVelocity = Clamp(additionalVelocity, 10, 0.25f);

            HashSet<int> indexes = new HashSet<int>();
            for(int i = 0; i < additionalVelocity * 10;i++)
            {
            indexes.Add(random.Next(0,max_count));
            }

        }
    }
}
