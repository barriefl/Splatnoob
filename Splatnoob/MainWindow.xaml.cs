using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
        private DispatcherTimer minuteurJeu = new DispatcherTimer();
        private DispatcherTimer timer = new DispatcherTimer();
        private DispatcherTimer robotTimer = new DispatcherTimer();
        private static double tempsInitial;
        private double tempsJeu;

        // Sons.
        private MediaPlayer musiqueFond = new MediaPlayer();

        // Skin.
        private ImageBrush fondSkin = new ImageBrush();
        private ImageBrush joueurRougeSkin = new ImageBrush();
        private ImageBrush joueurBleuSkin = new ImageBrush();

        // Points.
        private Point PO;
        private Point J1;
        private Point J2;

        // Constantes.
        private const int RESET = 0;

        private const int LIGNE = 5;
        private const int COLONNE = 5;
        private const int RECTANGLE_LARGEUR = 75;
        private const int RECTANGLE_HAUTEUR = 75;
        private const int RECTANGLE_ESPACEMENT = 5;
        private const int POSITION_JOUEUR_Z = 1;
        private const int VITESSE_TICK_FACILE = 400;
        private const int VITESSE_TICK_NORMALE = 200;
        private const int VITESSE_TICK_DIFFICILE = 100;

        public const int CONVERTION_VOLUME_DECIMALE = 100;

        // Booléens.
        private bool statOuvert = false;

        // Variables.
        private int scoreRouge = 0;
        private int scoreBleu = 0;

        private int nbTourRouge = 0;
        private int nbTourBleu = 0;

        private int nbPartieGagneRouge = 0;
        private int nbPartieGagneBleu = 0;

        private int pasJoueur = 80;
        private int x1 = LIGNE - LIGNE;
        private int y1 = COLONNE - COLONNE;
        private int x2 = COLONNE - 1;
        private int y2 = LIGNE - 1;
        private int xO = COLONNE / 2;
        private int yO = LIGNE / 2;

        private int difficulté;

        private Key ValKeyHautJ1;
        private Key ValKeyGaucheJ1;
        private Key ValKeyBasJ1;
        private Key ValKeyDroiteJ1;
        private Key ValKeyHautJ2;
        private Key ValKeyGaucheJ2;
        private Key ValKeyBasJ2;
        private Key ValKeyDroiteJ2;

        Accueil fenetreAccueil = new Accueil();

        private double volume;

        // Limites grille.
        private double maxX = COLONNE - 1;
        private double minX = 0;
        private double maxY = LIGNE - 1;
        private double minY = 0;

        // Tableau (grille).
        private Rectangle[,] grille5x5;

        public MainWindow()
        {
            // Début du jeu.
            InitializeComponent();
            Console.WriteLine("Démarrage du jeu.");

            minuteurJeu.Tick += MoteurJeu;
            minuteurJeu.Interval = TimeSpan.FromMilliseconds(16);

            timer.Tick += Timer;
            timer.Interval = TimeSpan.FromSeconds(1);

            // Chemin des skins.
            fondSkin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/fond.jpeg"));
            joueurRougeSkin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/SlimeRouge.png"));
            joueurBleuSkin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/SlimeBleu.png"));
            Console.WriteLine("Chargement des skins.");

            // Chemin des musiques.
            musiqueFond.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Ultrasyd-Who_Cares.mp3"));

            // Volume des musiques.
            Console.WriteLine("Chargement de la musique.");

            // On rempli les rectangles avec les skins.
            rectFond.Fill = fondSkin;
            joueur1.Fill = joueurRougeSkin;
            joueur2.Fill = joueurBleuSkin;
            Console.WriteLine("Skins chargés.");

            // Fenêtre de dialogue.
            fenetreAccueil.ShowDialog();

            //création du robot
            if (fenetreAccueil.unJoueur == true)
            {
                if (fenetreAccueil.modeFacile == true)
                    difficulté = VITESSE_TICK_FACILE;
                else if (fenetreAccueil.modeNormal == true)
                    difficulté = VITESSE_TICK_NORMALE;
                else
                    difficulté = VITESSE_TICK_DIFFICILE;
                robotTimer.Tick += AdversaireTimer;
                robotTimer.Interval = TimeSpan.FromMilliseconds(difficulté);
            }
            // Création des rectangles et on charge le Canvas pour que les coordonnées de la grille soient correcte.
            CreationRectangle();
            monCanvas.Loaded += (sender, e) => CreationGrille();
            Console.WriteLine("Création de la grille.");

            // On actualise les stats.
            labCooRouge.Content = "x,y : " + x1 + "," + y1;
            labCooBleu.Content = "x,y : " + x2 + "," + y2;

            labTourRouge.Content = "Nb tours joués : " + nbTourRouge;
            labTourBleu.Content = "Nb tours joués : " + nbTourBleu;

            labNbPartiesGagneRouge.Content = "Parties gagnées : " + nbPartieGagneRouge;
            labNbPartiesGagneBleu.Content = "Parties gagnées : " + nbPartieGagneBleu;
            Console.WriteLine("Actualisation des statistiques.");

            tempsInitial = 10 * Math.Round(Parametre.Valeurtemps);
            tempsJeu = tempsInitial;

            ValKeyHautJ1 = Parametre.KeyHautJ1;
            ValKeyGaucheJ1 = Parametre.KeyGaucheJ1;
            ValKeyBasJ1 = Parametre.KeyBasJ1;
            ValKeyDroiteJ1 = Parametre.KeyDroiteJ1;
            ValKeyHautJ2 = Parametre.KeyHautJ2;
            ValKeyGaucheJ2 = Parametre.KeyGaucheJ2;
            ValKeyBasJ2 = Parametre.KeyBasJ2;
            ValKeyDroiteJ2 = Parametre.KeyDroiteJ2;
            volume = Parametre.valeursons;
            musiqueFond.Volume = volume/CONVERTION_VOLUME_DECIMALE;
        }

        private void CreationRectangle()
        {
            // Création de la grille + propriétés des rectangles.
            grille5x5 = new Rectangle[LIGNE, COLONNE];
            for (int i = 0; i < LIGNE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    grille5x5[i, j] = new Rectangle
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
            double coordonneesX = (rectFond.ActualWidth - largeurTotale) / 2;
            double coordonneesY = (rectFond.ActualHeight - hauteurTotale) / 2;

            for (int i = 0; i < LIGNE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    double x = coordonneesX + j * (RECTANGLE_LARGEUR + RECTANGLE_ESPACEMENT);
                    double y = coordonneesY + i * (RECTANGLE_HAUTEUR + RECTANGLE_ESPACEMENT);

                    Canvas.SetTop(grille5x5[i, j], y);
                    Canvas.SetLeft(grille5x5[i, j], x);
                    monCanvas.Children.Add(grille5x5[i, j]);
                }
            }
            Console.WriteLine("Grille créée.");

            // Placement des joueurs dans l'espace.
            Canvas.SetZIndex(joueur1, POSITION_JOUEUR_Z);
            Canvas.SetZIndex(joueur2, POSITION_JOUEUR_Z);
            Console.WriteLine("Placement des joueurs dans l'espace.");
        }

        private void MouvementsJoueursEtVerifications(object sender, KeyEventArgs e)
        {
            Point PO = new Point(xO, yO);

            // Joueur 1 - Z, Q, S, D. 
            Point J1 = new Point(x1, y1);

            if (e.Key == ValKeyHautJ1)
            {
                if (y1 > minY && (y1 - 1 != y2 || x1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetTop(joueur1, Canvas.GetTop(joueur1) - pasJoueur);
                    y1 -= 1;
                    labCooRouge.Content = "x,y : " + x1 + "," + y1;
                    nbTourRouge++;
                    labTourRouge.Content = "Nb tours joués : " + nbTourRouge;
                }
            }
            if (e.Key == ValKeyGaucheJ1)
            {
                if (x1 > minX && (y1 != y2 || x1 - 1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetLeft(joueur1, Canvas.GetLeft(joueur1) - pasJoueur);
                    x1 -= 1;
                    labCooRouge.Content = "x,y : " + x1 + "," + y1;
                    nbTourRouge++;
                    labTourRouge.Content = "Nb tours joués : " + nbTourRouge;
                }
            }
            if (e.Key == ValKeyBasJ1)
            {
                if (y1 < maxY && (y1 + 1 != y2 || x1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetTop(joueur1, Canvas.GetTop(joueur1) + pasJoueur);
                    y1 += 1;
                    labCooRouge.Content = "x,y : " + x1 + "," + y1;
                    nbTourRouge++;
                    labTourRouge.Content = "Nb tours joués : " + nbTourRouge;
                }
            }
            if (e.Key == ValKeyDroiteJ1)
            {
                if (x1 < maxX && (y1 != y2 || x1 + 1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetLeft(joueur1, Canvas.GetLeft(joueur1) + pasJoueur);
                    x1 += 1;
                    labCooRouge.Content = "x,y : " + x1 + "," + y1;
                    nbTourRouge++;
                    labTourRouge.Content = "Nb tours joués : " + nbTourRouge;
                }
            }

            // Joueur 2 - Up, Left, Down, Right.
            Point J2 = new Point(x2, y2);

            if (e.Key == ValKeyHautJ2 && fenetreAccueil.deuxJoueurs == true)
            {
                if (y2 > minY && (y1 != y2 - 1 || x1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) - pasJoueur);
                    y2 -= 1;
                    labCooBleu.Content = "x,y : " + x2 + "," + y2;
                    nbTourBleu++;
                    labTourBleu.Content = "Nb tours joués : " + nbTourBleu;
                }
            }
            if (e.Key == ValKeyGaucheJ2 && fenetreAccueil.deuxJoueurs == true)
            {
                if (x2 > minX && (y1 != y2 || x1 != x2 - 1) && tempsJeu < tempsInitial)
                {
                    Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) - pasJoueur);
                    x2 -= 1;
                    labCooBleu.Content = "x,y : " + x2 + "," + y2;
                    nbTourBleu++;
                    labTourBleu.Content = "Nb tours joués : " + nbTourBleu;
                }
            }
            if (e.Key == ValKeyBasJ2 && fenetreAccueil.deuxJoueurs == true)
            {
                if (y2 < maxY && (y1 != y2 + 1 || x1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) + pasJoueur);
                    y2 += 1;
                    labCooBleu.Content = "x,y : " + x2 + "," + y2;
                    nbTourBleu++;
                    labTourBleu.Content = "Nb tours joués : " + nbTourBleu;
                }
            }
            if (e.Key == ValKeyDroiteJ2 && fenetreAccueil.deuxJoueurs == true)
            {
                if (x2 < maxX && (y1 != y2 || x1 != x2 + 1) && tempsJeu < tempsInitial)
                {
                    Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) + pasJoueur);
                    x2 += 1;
                    labCooBleu.Content = "x,y : " + x2 + "," + y2;
                    nbTourBleu++;
                    labTourBleu.Content = "Nb tours joués : " + nbTourBleu;
                }
            }

            // Autre.
            if (e.Key == Key.F1)
            {
                if (statOuvert == true)
                {
                    labCooRouge.Visibility = Visibility.Hidden;
                    labTourRouge.Visibility = Visibility.Hidden;
                    labNbPartiesGagneRouge.Visibility = Visibility.Hidden;

                    labCooBleu.Visibility = Visibility.Hidden;
                    labTourBleu.Visibility = Visibility.Hidden;
                    labNbPartiesGagneBleu.Visibility = Visibility.Hidden;

                    statOuvert = false;
                    Console.WriteLine("Statistiques fermées.");
                }
                else
                {
                    labCooRouge.Visibility = Visibility.Visible;
                    labTourRouge.Visibility = Visibility.Visible;
                    labNbPartiesGagneRouge.Visibility = Visibility.Visible;

                    labCooBleu.Visibility = Visibility.Visible;
                    labTourBleu.Visibility = Visibility.Visible;
                    labNbPartiesGagneBleu.Visibility = Visibility.Visible;

                    statOuvert = true;
                    Console.WriteLine("Statistiques ouvertes.");
                }
            }
            if (e.Key == Key.Space)
            {
                labCommencer.Visibility = Visibility.Hidden;

                minuteurJeu.Start();
                timer.Start();
                Console.WriteLine("Début du minuteur.");

                musiqueFond.Play();
                Console.WriteLine("Début de la musique et des sons.");

                robotTimer.Start();
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
                Console.WriteLine("Collision du joueur rouge.");
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
                Console.WriteLine("Collision du joueur bleu.");
            }
        }

        private void TestGagnant()
        {
            // On vérifie que le temps arrive à 0 et on fait des vérifications pour savoir qui est le gagnant.
            if (tempsJeu <= 0)
            {
                Console.WriteLine("Vérification du score.");
                if (scoreBleu > scoreRouge)
                {
                    labBleuGagne.Visibility = Visibility.Visible;
                    nbPartieGagneBleu++;
                    Console.WriteLine("Le joueur bleu gagne la partie.");
                }
                else if (scoreBleu < scoreRouge)
                {
                    labRougeGagne.Visibility = Visibility.Visible;
                    nbPartieGagneRouge++;
                    Console.WriteLine("Le joueur rouge gagne la partie.");
                }
                else
                {
                    labPersonneGagne.Visibility = Visibility.Visible;
                    Console.WriteLine("Match nul.");
                }
                Canvas.SetZIndex(rectFond, 2);
                butRejouer.Visibility = Visibility.Visible;
                butMenu.Visibility = Visibility.Visible;

                minuteurJeu.Stop();
                timer.Stop();
                robotTimer.Stop();
                Console.WriteLine("Arrêt du minuteur.");
                tempsJeu = 0;
            }
        }

        private void MoteurJeu(object sender, EventArgs e)
        {
            TestGagnant();
            // Création de rectangles joueurs pour la détection de collision.
            Rect rectJoueur1 = new Rect(Canvas.GetLeft(joueur1), Canvas.GetTop(joueur1), joueur1.Width, joueur1.Height);
            Rect rectJoueur2 = new Rect(Canvas.GetLeft(joueur2), Canvas.GetTop(joueur2), joueur2.Width, joueur2.Height);
            Console.WriteLine("Rect joueurs créés.");

            foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
            {
                TestCollisionJoueur1(x, rectJoueur1);
                TestCollisionJoueur2(x, rectJoueur2);
            }
        }

        private void Timer(object sender, EventArgs e)
        {
            tempsJeu = tempsJeu - 1;
            labTemps.Content = tempsJeu.ToString();
        }

        private void butRejouer_Click(object sender, RoutedEventArgs e)
        {
            // On arrête la musique.
            musiqueFond.Stop();

            // On réinitialise le score.
            scoreBleu = RESET;
            scoreRouge = RESET;
            labScoreBleu.Content = scoreBleu.ToString();
            labScoreRouge.Content = scoreRouge.ToString();
            Console.WriteLine("Réinitialisation du score.");

            // On réinitialise les stats.
            nbTourBleu = RESET;
            nbTourRouge = RESET;
            labTourRouge.Content = "Nb tours joués : " + nbTourRouge;
            labTourBleu.Content = "Nb tours joués : " + nbTourBleu;
            Console.WriteLine("Réinitialisation des statistiques.");

            // On actualise la stat du nombre de partie gagné.
            labNbPartiesGagneRouge.Content = "Parties gagnées : " + nbPartieGagneRouge;
            labNbPartiesGagneBleu.Content = "Parties gagnées : " + nbPartieGagneBleu;
            Console.WriteLine("Actualisation du nombre de parties gagnées.");

            // On réinitialise le temps.
            tempsJeu = tempsInitial;
            labTemps.Content = tempsJeu;
            Console.WriteLine("Réinitialisation du temps.");

            // On réinitialise les carreaux et on remet les joueurs à leur place.
            foreach (Rectangle x in monCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "joueur")
                {
                    Canvas.SetLeft(joueur1, 228);
                    Canvas.SetTop(joueur1, 45);

                    Canvas.SetLeft(joueur2, 548);
                    Canvas.SetTop(joueur2, 365);
                }
                if ((string)x.Tag == "bleu" || (string)x.Tag == "rouge")
                {
                    x.Fill = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    x.Tag = "blanc";
                }
            }
            Console.WriteLine("Réinitialisation de la grille et des joueurs.");

            // On remet le fond à sa place.
            Canvas.SetZIndex(rectFond, 0);

            // On réinitialise les coordonnées.
            x1 = LIGNE - LIGNE;
            y1 = COLONNE - COLONNE;
            x2 = COLONNE - 1;
            y2 = LIGNE - 1;
            labCooRouge.Content = "x,y : " + x1 + "," + y1;
            labCooBleu.Content = "x,y : " + x2 + "," + y2;
            Console.WriteLine("Réinitialisation des coordonnées.");

            // On met en Hidden toutes les fenêtres pour restart le jeu.
            labBleuGagne.Visibility = Visibility.Hidden;
            labRougeGagne.Visibility = Visibility.Hidden;
            labPersonneGagne.Visibility = Visibility.Hidden;
            butRejouer.Visibility = Visibility.Hidden;
            butMenu.Visibility = Visibility.Hidden;

            // Et c'est reparti !
            labCommencer.Visibility = Visibility.Visible;
            Console.WriteLine("Nouvelle partie.");
        }

        private void monCanvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AdversaireIntelligent()
        {
            // On cherche la case blanche non occupée la plus proche.
            int blancDeltaX = 0;
            int blancDeltaY = 0;
            double distanceBlanche = double.MaxValue;

            // On cherche la case rouge non occupée la plus proche.
            int rougeDeltaX = 0;
            int rougeDeltaY = 0;
            double distanceRouge = double.MaxValue;

            for (int i = 0; i < LIGNE; i++)
            {
                for (int j = 0; j < COLONNE; j++)
                {
                    if ((string)grille5x5[i, j].Tag == "blanc")
                    {
                        double distance = Math.Sqrt(Math.Pow(j - x2, 2) + Math.Pow(i - y2, 2));
                        if (distance < distanceBlanche)
                        {
                            blancDeltaX = j - x2;
                            blancDeltaY = i - y2;
                            distanceBlanche = distance;
                        }
                    }
                    else if ((string)grille5x5[i, j].Tag == "rouge" && (i != y1 || j != x1))
                    {
                        double distance = Math.Sqrt(Math.Pow(j - x2, 2) + Math.Pow(i - y2, 2));
                        if (distance < distanceRouge)
                        {
                            rougeDeltaX = j - x2;
                            rougeDeltaY = i - y2;
                            distanceRouge = distance;
                        }
                    }
                }
            }

            // On cherche la meilleure solution entre la case blanche et la case rouge.
            int deltaX, deltaY;

            if (distanceBlanche < distanceRouge)
            {
                deltaX = blancDeltaX;
                deltaY = blancDeltaY;
            }
            else
            {
                deltaX = rougeDeltaX;
                deltaY = rougeDeltaY;
            }

            // On déplace le robot en fonction de la nouvelle position calculée.
            int newPosX = x2 + Math.Sign(deltaX);
            int newPosY = y2 + Math.Sign(deltaY);

            if (newPosX >= minX && newPosX <= maxX && newPosY >= minY && newPosY <= maxY &&
                (string)grille5x5[newPosY, newPosX].Tag != "bleu")
            {
                Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) + pasJoueur * Math.Sign(deltaX));
                Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) + pasJoueur * Math.Sign(deltaY));
                x2 = newPosX;
                y2 = newPosY;
            }
        }

        private void butMenu_Click(object sender, RoutedEventArgs e)
        {
            butMenu.Visibility = Visibility.Hidden;
            fenetreAccueil.Show();
        }

        private void AdversaireTimer(object sender, EventArgs e)
        {
            if (fenetreAccueil.unJoueur == true)
            {
                Console.WriteLine("1 joueur.");
                AdversaireIntelligent();
            }
        }
    }
}