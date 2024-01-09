using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
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
        // Timer.
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private double temps = 10;

        // Skin.
        private ImageBrush fondSkin = new ImageBrush();

        // Points.
        private Point J1;
        private Point J2;

        // Constantes.
        private const int LIGNE = 5;
        private const int COLONNE = 5;
        private const int RECTANGLE_LARGEUR = 75;
        private const int RECTANGLE_HAUTEUR = 75;
        private const int RECTANGLE_ESPACEMENT = 5;
        private const int POSITION_JOUEUR_Z = 1;

        // Variables.
        private int scoreRouge = 0;
        private int scoreBleu = 0;

        private int pasJoueur = 80;
        private int x1 = 0;
        private int y1 = 0;
        private int x2 = COLONNE - 1;
        private int y2 = LIGNE - 1;

        // Variables rectOriginal.
        private int xO = COLONNE / 2;
        private int yO = LIGNE / 2;

        // Limites grille.
        private double maxX = COLONNE - 1;
        private double minX = 0;
        private double maxY = LIGNE - 1;
        private double minY = 0; 

        // Tableau (grille).
        private Rectangle[,] grille;

        public MainWindow()
        {
            // Début du jeu.
            InitializeComponent();
            dispatcherTimer.Tick += MoteurJeu;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16);
            dispatcherTimer.Start();

            // Chemin du skin.
            fondSkin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/fond.jpeg"));

            // On rempli le rectangle avec le skin.
            rectFond.Fill = fondSkin;

            // Création des rectangles et on charge le Canvas pour que les coordonnées de la grille soient correcte.
            CreationRectangle();
            monCanvas.Loaded += (sender, e) => CreationGrille();

            Point J1 = new Point(x1, y1);
            Point J2 = new Point(x2, y2);
            cooLabelJ1.Content = J1;
            cooLabelJ2.Content = J2;
        }

        private void CreationRectangle()
        {
            // Création de la grille + propriétés des rectangles.
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
                        StrokeThickness = 1,
                        Tag = "blanc"
                    };
                }
            }
        }

        private void CreationGrille()
        {
            // Variables pour les coordonnées.
            double largeurTotale = COLONNE * (RECTANGLE_LARGEUR + RECTANGLE_ESPACEMENT) - RECTANGLE_ESPACEMENT;
            double hauteurTotale = LIGNE * (RECTANGLE_HAUTEUR + RECTANGLE_ESPACEMENT) - RECTANGLE_ESPACEMENT;

            // Coordonnées pour centrer la grille.
            double coordonneesX = (rectFond.ActualWidth - largeurTotale) /2;
            double coordonneesY = (rectFond.ActualHeight - hauteurTotale) /2;

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
            // Placement des joueurs dans l'espace.
            Canvas.SetZIndex(joueur1, POSITION_JOUEUR_Z);
            Canvas.SetZIndex(joueur2, POSITION_JOUEUR_Z);
        }

        private void MouvementsJoueursEtVerifications(object sender, KeyEventArgs e)
        {
            // Joueur 1 - Z, Q, S, D. 
            Point J1 = new Point(x1, y1);

            if (e.Key == Key.Z)
            {
                if (y1 > minY)
                {
                    Canvas.SetTop(joueur1, Canvas.GetTop(joueur1) - pasJoueur);
                    y1 -= 1;
                    cooLabelJ1.Content = x1 + "," + y1;
                }
            }
            if (e.Key == Key.Q)
            {
                if (x1 > minX)
                {
                    Canvas.SetLeft(joueur1, Canvas.GetLeft(joueur1) - pasJoueur);
                    x1 -= 1;
                    cooLabelJ1.Content = x1 + "," + y1;
                }
            }
            if (e.Key == Key.S)
            {
                if (y1 < maxY)
                {
                    Canvas.SetTop(joueur1, Canvas.GetTop(joueur1) + pasJoueur);
                    y1 += 1;
                    cooLabelJ1.Content = x1 + "," + y1;
                }
            }
            if (e.Key == Key.D)
            {
                if (x1 < maxX)
                {
                    Canvas.SetLeft(joueur1, Canvas.GetLeft(joueur1) + pasJoueur);
                    x1 += 1;
                    cooLabelJ1.Content = x1 + "," + y1;
                }
            }

            // Joueur 2 - Up, Left, Down, Right.
            Point J2 = new Point(x2, y2);

            if (e.Key == Key.Up)
            {
                if (y2 > minY)
                {
                    Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) - pasJoueur);
                    y2 -= 1;
                    cooLabelJ2.Content = x2 + "," + y2;
                }
            }
            if (e.Key == Key.Left)
            {
                if (x2 > minX)
                {
                    Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) - pasJoueur);
                    x2 -= 1;
                    cooLabelJ2.Content = x2 + "," + y2;
                }
            }
            if (e.Key == Key.Down)
            {
                if (y2 < maxY)
                {
                    Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) + pasJoueur);
                    y2 += 1;
                    cooLabelJ2.Content = x2 + "," + y2;
                }
            }
            if (e.Key == Key.Right)
            {
                if (x2 < maxX)
                {
                    Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) + pasJoueur);
                    x2 += 1;
                    cooLabelJ2.Content = x2 + "," + y2;
                }
            }
            // Création d’un rectangle joueur pour la détection de collision.
            Rect rectJoueur1 = new Rect(Canvas.GetLeft(joueur1), Canvas.GetTop(joueur1), joueur1.Width, joueur1.Height);
            Rect rectJoueur2 = new Rect(Canvas.GetLeft(joueur2), Canvas.GetTop(joueur2), joueur2.Width, joueur2.Height);
            foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
            {
                TestCollisionJoueur1(x, rectJoueur1);
                TestCollisionJoueur2(x, rectJoueur2);
            }
        }

        private void TestCollisionJoueur1(Rectangle x, Rect joueur)
        {
            // Vérification de la collision avec le joueur.
            Rect carreau = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
            if (joueur.IntersectsWith(carreau) && (string)x.Tag != "joueur" && (string)x.Tag != "rouge")
            {
                x.Fill = new SolidColorBrush(Color.FromRgb(255, 25, 25));
                scoreRouge++;
                labScoreRouge.Content = scoreRouge.ToString();
                if (joueur.IntersectsWith(carreau) && (string)x.Tag == "bleu")
                {
                    scoreBleu = scoreBleu - 1;
                    labScoreBleu.Content = scoreBleu.ToString();
                }
                x.Tag = "rouge";
            }
        }

        private void TestCollisionJoueur2(Rectangle x, Rect joueur)
        {
            // Vérification de la collision avec le joueur.
            Rect carreau = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
            if (joueur.IntersectsWith(carreau) && (string)x.Tag != "joueur" && (string)x.Tag != "bleu")
            {
                x.Fill = new SolidColorBrush(Color.FromRgb(25, 25, 255));
                scoreBleu++;
                labScoreBleu.Content = scoreBleu.ToString();
                if (joueur.IntersectsWith(carreau) && (string)x.Tag == "rouge")
                {
                    scoreRouge = scoreRouge - 1;
                    labScoreRouge.Content = scoreRouge.ToString();
                }
                x.Tag = "bleu";
            }
        }

        private void TestGagnant()
        {
            if (temps <= 0)
            {
                dispatcherTimer.Stop();
                temps = 0;
            }
        }

        private void MoteurJeu(object sender, EventArgs e)
        {
            temps = Math.Round(temps - 0.016, 2);
            labTemps.Content = temps.ToString();

            TestGagnant();
        }
    }
}
