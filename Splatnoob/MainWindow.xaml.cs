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

        private const int LIGNE = 5;
        private const int COLONNE = 5;
        private const int RECTANGLE_LARGEUR = 75;
        private const int RECTANGLE_HAUTEUR = 75;
        private const int RECTANGLE_ESPACEMENT = 5;
        private const int POSITION_JOUEUR_Z = 1;
        private int joueurVitesse = 80;

        private Rectangle[,] grille;

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += MoteurJeu;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16);
            dispatcherTimer.Start();

            fondSkin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/fond.jpeg"));

            rectangleFond.Fill = fondSkin;

            CreationRectangle();
            monCanvas.Loaded += (sender, e) => CreationGrille();
        }

        private void CreationRectangle()
        {
            grille = new Rectangle[LIGNE, COLONNE];
            for (int i = 0; i < LIGNE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    grille[i, j] = new Rectangle
                    {
                        Width = RECTANGLE_LARGEUR,
                        Height = RECTANGLE_HAUTEUR,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1
                    };
                }
            }
        }

        private void CreationGrille()
        {
            double largeurTotale = COLONNE * (RECTANGLE_LARGEUR + RECTANGLE_ESPACEMENT) - RECTANGLE_ESPACEMENT;
            double hauteurTotale = LIGNE * (RECTANGLE_HAUTEUR + RECTANGLE_ESPACEMENT) - RECTANGLE_ESPACEMENT;

            double coordonneesX = (rectangleFond.ActualWidth - largeurTotale) /2;
            double coordonneesY = (rectangleFond.ActualHeight - hauteurTotale) /2;

            for (int i = 0; i < LIGNE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    double x = coordonneesX + j * (RECTANGLE_LARGEUR + RECTANGLE_ESPACEMENT);
                    double y = coordonneesY + i * (RECTANGLE_HAUTEUR + RECTANGLE_ESPACEMENT);

                    Canvas.SetTop(grille[i, j], y);
                    Canvas.SetLeft(grille[i, j], x);
                    monCanvas.Children.Add(grille[i, j]);
                }
            }
            Canvas.SetZIndex(joueur1, POSITION_JOUEUR_Z);
            Canvas.SetZIndex(joueur2, POSITION_JOUEUR_Z);
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
