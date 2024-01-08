using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;

namespace Splatnoob
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private ImageBrush fondSkin = new ImageBrush();

        string[,] grille5x5 = new string[5, 5] {
                                            { "00", "01", "02", "03", "04" },
                                            { "10", "11", "12", "13", "14" },
                                            { "20", "21", "22", "23", "24" },
                                            { "30", "31", "32", "33", "34" },
                                            { "40", "41", "42", "43", "44" }
                                           };
        private int joueurVitesse = 80;

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += MoteurJeu;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16);
            dispatcherTimer.Start();

            fondSkin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/fond.jpeg"));

            rectangleFond.Fill = fondSkin;
        }
        private void ToucheCanvasEnBas(object sender, KeyEventArgs e)
        {
            // Joueur 1
            if (e.Key == Key.Z)
            {
                if (Canvas.GetTop(joueur1) > 0)
                {
                    Canvas.SetTop(joueur1, Canvas.GetTop(joueur1) - joueurVitesse);
                }
            }
            if (e.Key == Key.Q)
            {
                if (Canvas.GetLeft(joueur1) > 0)
                {
                    Canvas.SetLeft(joueur1, Canvas.GetLeft(joueur1) - joueurVitesse);
                }
            }
            if (e.Key == Key.S)
            {
                if (Canvas.GetTop(joueur1) + joueur1.Width < Application.Current.MainWindow.Width)
                {
                    Canvas.SetTop(joueur1, Canvas.GetTop(joueur1) + joueurVitesse);
                }
            }
            if (e.Key == Key.D)
            {
                if (Canvas.GetLeft(joueur1) + joueur1.Width < Application.Current.MainWindow.Width)
                {
                    Canvas.SetLeft(joueur1, Canvas.GetLeft(joueur1) + joueurVitesse);
                }
            }
            // Joueur 2
            if (e.Key == Key.Up)
            {
                if (Canvas.GetTop(joueur2) > 0)
                {
                    Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) - joueurVitesse);
                }
            }
            if (e.Key == Key.Left)
            {
                if (Canvas.GetLeft(joueur2) > 0)
                {
                    Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) - joueurVitesse);
                }
            }
            if (e.Key == Key.Down)
            {
                if (Canvas.GetTop(joueur2) + joueur2.Width < Application.Current.MainWindow.Width)
                {
                    Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) + joueurVitesse);
                }
            }
            if (e.Key == Key.Right)
            {
                if (Canvas.GetLeft(joueur2) + joueur2.Width < Application.Current.MainWindow.Width)
                {
                    Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) + joueurVitesse);
                }
            }
        }
     
        private void MoteurJeu(object sender, EventArgs e)
        {
            
        }
    }
}
