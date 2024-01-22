using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Alpha
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Fenêtre.
        Accueil fenetreAccueil = new Accueil();

        // Timer.
        private DispatcherTimer minuterieJeu = new DispatcherTimer();
        private DispatcherTimer robotTimer = new DispatcherTimer();

        // Sons.
        private MediaPlayer musiqueJeu = new MediaPlayer();

        // Skin.
        private ImageBrush joueurRougeSkin = new ImageBrush();
        private ImageBrush joueurBleuSkin = new ImageBrush();

        // Constantes.
        private const int RECTANGLE_LARGEUR = 75;
        private const int RECTANGLE_HAUTEUR = 75;
        private const int RECTANGLE_ESPACEMENT = 5;

        private const int VITESSE_TICK_FACILE = 350;
        private const int VITESSE_TICK_NORMALE = 250;
        private const int VITESSE_TICK_DIFFICILE = 200;

        private const int SET_LEFT_J1_5X5 = 728;
        private const int SET_TOP_J1_5X5 = 320;
        private const int SET_LEFT_J2_5X5 = 1048;
        private const int SET_TOP_J2_5X5 = 640;
        private const int SET_LEFT_J1_7X7 = 648;
        private const int SET_TOP_J1_7X7 = 240;
        private const int SET_LEFT_J2_7X7 = 1128;
        private const int SET_TOP_J2_7X7 = 720;

        public const int CONVERTION_VOLUME_DECIMALE = 100;

        // Booléens.
        private bool statOuvert = false;

        // Variables.
        private static int nbLigne;
        private static int nbColonne;

        private int scoreRouge = 0;
        private int scoreBleu = 0;

        private int nbTourRouge = 0;
        private int nbTourBleu = 0;

        private int nbPartieGagneRouge = 0;
        private int nbPartieGagneBleu = 0;

        private int pasJoueur = 80;

        private int x1;
        private int y1;
        private int x2;
        private int y2;

        private int difficulte;
        public static double volume;

        private static double tempsInitial;
        private double tempsJeu;

        // Keys.
        private Key ValKeyHautJ1;
        private Key ValKeyGaucheJ1;
        private Key ValKeyBasJ1;
        private Key ValKeyDroiteJ1;
        private Key ValKeyHautJ2;
        private Key ValKeyGaucheJ2;
        private Key ValKeyBasJ2;
        private Key ValKeyDroiteJ2;

        // Limites grille.
        private double maxX;
        private double minX;
        private double maxY;
        private double minY;

        // Tableau (grille).
        private System.Windows.Shapes.Rectangle[,] grille = null;
        public MainWindow()
        {
            // Début du jeu.
            InitializeComponent();
            fenetreAccueil.ShowDialog();
            Console.WriteLine("Démarrage du jeu.");

            // Minuterie - Moteur du jeu.
            minuterieJeu.Tick += MoteurJeu;
            minuterieJeu.Interval = TimeSpan.FromMilliseconds(16);

            // On actualise et on change le jeu en fonction des paramètres.
            NouveauxParametres();

            // Chemin musique.
            musiqueJeu.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Ultrasyd-Who_Cares.mp3"));

            // Chemin des skins.
            joueurRougeSkin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/SlimeRouge.png"));
            joueurBleuSkin.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/SlimeBleu.png"));
            Console.WriteLine("Jeu - Chargement des skins joueurs...");

            // On rempli les rectangles avec les skins.
            joueur1.Fill = joueurRougeSkin;
            joueur2.Fill = joueurBleuSkin;
            Console.WriteLine("Jeu - Skins chargés.");
        }
        private void MoteurJeu(object sender, EventArgs e)
        {
            tempsJeu = Math.Round(tempsJeu - 0.016, 2);
            labTemps.Content = tempsJeu.ToString();

            TestGagnant();

            // Création de Rect joueurs pour la détection de collision.
            Rect rectJoueur1 = new Rect(Canvas.GetLeft(joueur1), Canvas.GetTop(joueur1), joueur1.Width, joueur1.Height);
            Rect rectJoueur2 = new Rect(Canvas.GetLeft(joueur2), Canvas.GetTop(joueur2), joueur2.Width, joueur2.Height);
            //Console.WriteLine("Jeu - Rect joueurs créés."); Je désactive celui là pour éviter de polluer la console.

            foreach (System.Windows.Shapes.Rectangle x in monCanvas.Children.OfType<System.Windows.Shapes.Rectangle>())
            {
                TestCollisionJoueur(x, rectJoueur1, true);
                TestCollisionJoueur(x, rectJoueur2, false);
            }
        }

        private void AdversaireTimer(object sender, EventArgs e)
        {
            //Console.WriteLine("1 joueur."); Je désactive celui là pour éviter de polluer la console.
            AdversaireIntelligent();
        }

        private void CreationRectangle()
        {
            // Création de la grille + propriétés des rectangles.
            grille = new System.Windows.Shapes.Rectangle[nbLigne, nbColonne];
            for (int i = 0; i < nbLigne; i++)
            {
                for (int j = 0; j < nbColonne; j++)
                {
                    grille[i, j] = new System.Windows.Shapes.Rectangle
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
            double largeurTotale = nbColonne * (RECTANGLE_LARGEUR + RECTANGLE_ESPACEMENT) - RECTANGLE_ESPACEMENT;
            double hauteurTotale = nbLigne * (RECTANGLE_HAUTEUR + RECTANGLE_ESPACEMENT) - RECTANGLE_ESPACEMENT;

            // Coordonnées pour centrer la grille.
            double coordonneesX = (rectFond.ActualWidth - largeurTotale) / 2;
            double coordonneesY = (rectFond.ActualHeight - hauteurTotale) / 2;

            for (int i = 0; i < nbLigne; i++)
            {
                for (int j = 0; j < nbColonne; j++)
                {
                    double x = coordonneesX + j * (RECTANGLE_LARGEUR + RECTANGLE_ESPACEMENT);
                    double y = coordonneesY + i * (RECTANGLE_HAUTEUR + RECTANGLE_ESPACEMENT);

                    Canvas.SetTop(grille[i, j], y);
                    Canvas.SetLeft(grille[i, j], x);
                    monCanvas.Children.Add(grille[i, j]);
                }
            }
            Console.WriteLine("Jeu - Grille créée.");
        }

        private void MouvementsJoueursEtVerifications(object sender, KeyEventArgs e)
        {
            // Joueur 1 - Z, Q, S, D. 
            if (e.Key == ValKeyHautJ1)
            {
                if (y1 > minY && (y1 - 1 != y2 || x1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetTop(joueur1, Canvas.GetTop(joueur1) - pasJoueur);
                    y1 -= 1;
                    nbTourRouge++;
                }
            }
            if (e.Key == ValKeyGaucheJ1)
            {
                if (x1 > minX && (y1 != y2 || x1 - 1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetLeft(joueur1, Canvas.GetLeft(joueur1) - pasJoueur);
                    x1 -= 1;
                    nbTourRouge++;
                }
            }
            if (e.Key == ValKeyBasJ1)
            {
                if (y1 < maxY && (y1 + 1 != y2 || x1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetTop(joueur1, Canvas.GetTop(joueur1) + pasJoueur);
                    y1 += 1;
                    nbTourRouge++;
                }
            }
            if (e.Key == ValKeyDroiteJ1)
            {
                if (x1 < maxX && (y1 != y2 || x1 + 1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetLeft(joueur1, Canvas.GetLeft(joueur1) + pasJoueur);
                    x1 += 1;
                    nbTourRouge++;
                }
            }
            labCooRouge.Content = "x,y : " + x1 + "," + y1;
            labTourRouge.Content = "Nb tours joués : " + nbTourRouge;

            // Joueur 2 - Up, Left, Down, Right.
            if (e.Key == ValKeyHautJ2 && fenetreAccueil.deuxJoueurs == true)
            {
                if (y2 > minY && (y1 != y2 - 1 || x1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) - pasJoueur);
                    y2 -= 1;
                    nbTourBleu++;
                }
            }
            if (e.Key == ValKeyGaucheJ2 && fenetreAccueil.deuxJoueurs == true)
            {
                if (x2 > minX && (y1 != y2 || x1 != x2 - 1) && tempsJeu < tempsInitial)
                {
                    Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) - pasJoueur);
                    x2 -= 1;
                    nbTourBleu++;
                }
            }
            if (e.Key == ValKeyBasJ2 && fenetreAccueil.deuxJoueurs == true)
            {
                if (y2 < maxY && (y1 != y2 + 1 || x1 != x2) && tempsJeu < tempsInitial)
                {
                    Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) + pasJoueur);
                    y2 += 1;
                    nbTourBleu++;
                }
            }
            if (e.Key == ValKeyDroiteJ2 && fenetreAccueil.deuxJoueurs == true)
            {
                if (x2 < maxX && (y1 != y2 || x1 != x2 + 1) && tempsJeu < tempsInitial)
                {
                    Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) + pasJoueur);
                    x2 += 1;
                    nbTourBleu++;
                }
            }
            labCooBleu.Content = "x,y : " + x2 + "," + y2;
            labTourBleu.Content = "Nb tours joués : " + nbTourBleu;

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
                    Console.WriteLine("Jeu - Statistiques fermées.");
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
                    Console.WriteLine("Jeu - Statistiques ouvertes.");
                }
            }
            if (e.Key == Key.Space)
            {
                labCommencer.Visibility = Visibility.Hidden;

                minuterieJeu.Start();
                Console.WriteLine("Démarrage du timer 'MoteurJeu'.");

                if (fenetreAccueil.unJoueur == true)
                {
                    robotTimer.Start();
                    Console.WriteLine("Démarrage du timer 'robotTimer'.");
                }
                
                musiqueJeu.Play();
                musiqueJeu.MediaEnded += (sender, e) => musiqueJeu.Position = TimeSpan.Zero;
                Console.WriteLine("Lancement de la musique 'Ultrasyd-Who_Cares.mp3'.");
            }
        }

        private void TestCollisionJoueur(System.Windows.Shapes.Rectangle x, Rect joueur, bool estJoueur1)
        {
            // Vérification de la collision avec le joueur.
            Rect carreau = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
            if (joueur.IntersectsWith(carreau) && (string)x.Tag != "joueur")
            {
                if (estJoueur1 && (string)x.Tag != "rouge")
                {
                    x.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 25, 25));
                    scoreRouge++;
                    labScoreRouge.Content = scoreRouge.ToString();
                    if (joueur.IntersectsWith(carreau) && (string)x.Tag == "bleu")
                    {
                        scoreBleu = scoreBleu - 1;
                        labScoreBleu.Content = scoreBleu.ToString();
                    }
                    x.Tag = "rouge";
                    Console.WriteLine("Jeu - Collision du joueur rouge.");
                }
                else if (!estJoueur1 && (string)x.Tag != "bleu")
                {
                    x.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(25, 25, 255));
                    scoreBleu++;
                    labScoreBleu.Content = scoreBleu.ToString();
                    if (joueur.IntersectsWith(carreau) && (string)x.Tag == "rouge")
                    {
                        scoreRouge = scoreRouge - 1;
                        labScoreRouge.Content = scoreRouge.ToString();
                    }
                    x.Tag = "bleu";
                    Console.WriteLine("Jeu - Collision du joueur bleu.");
                }    
            }
        }

        private void TestGagnant()
        {
            // On vérifie que le temps arrive à 0 et on fait des vérifications pour savoir qui est le gagnant.
            if (tempsJeu <= 0)
            {
                Console.WriteLine("Jeu - Vérification du score...");
                if (scoreBleu > scoreRouge)
                {
                    labBleuGagne.Visibility = Visibility.Visible;
                    nbPartieGagneBleu++;
                    Console.WriteLine("Jeu - Le joueur bleu gagne la partie.");
                }
                else if (scoreBleu < scoreRouge)
                {
                    labRougeGagne.Visibility = Visibility.Visible;
                    nbPartieGagneRouge++;
                    Console.WriteLine("Jeu - Le joueur rouge gagne la partie.");
                }
                else
                {
                    labPersonneGagne.Visibility = Visibility.Visible;
                    Console.WriteLine("Jeu - Match nul.");
                }
                Canvas.SetZIndex(rectFond, 2);
                butRejouer.Visibility = Visibility.Visible;
                butMenu.Visibility = Visibility.Visible;

                minuterieJeu.Stop();
                robotTimer.Stop();
                Console.WriteLine("Arrêt du minuteur 'minuterieJeu' et 'robotTimer'.");
                tempsJeu = 0;
            }
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

            for (int i = 0; i < nbLigne; i++)
            {
                for (int j = 0; j < nbColonne; j++)
                {
                    if ((string)grille[i, j].Tag == "blanc")
                    {
                        double distance = Math.Sqrt(Math.Pow(j - x2, 2) + Math.Pow(i - y2, 2));
                        if (distance < distanceBlanche)
                        {
                            blancDeltaX = j - x2;
                            blancDeltaY = i - y2;
                            distanceBlanche = distance;
                        }
                    }
                    else if ((string)grille[i, j].Tag == "rouge" && (i != y1 || j != x1))
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
                (string)grille[newPosY, newPosX].Tag != "bleu")
            {
                Canvas.SetLeft(joueur2, Canvas.GetLeft(joueur2) + pasJoueur * Math.Sign(deltaX));
                Canvas.SetTop(joueur2, Canvas.GetTop(joueur2) + pasJoueur * Math.Sign(deltaY));
                x2 = newPosX;
                y2 = newPosY;
            }
        }

        private void Reinitialisation()
        {
            // On arrête la musique.
            musiqueJeu.Stop();

            // On réinitialise le score.
            scoreBleu = 0;
            scoreRouge = 0;
            labScoreBleu.Content = scoreBleu.ToString();
            labScoreRouge.Content = scoreRouge.ToString();
            Console.WriteLine("Jeu - Scores réinitialisés.");

            // On réinitialise les stats.
            nbTourBleu = 0;
            nbTourRouge = 0;
            labTourRouge.Content = "Nb tours joués : " + nbTourRouge;
            labTourBleu.Content = "Nb tours joués : " + nbTourBleu;
            Console.WriteLine("Jeu - Statistiques réinitialisées.");

            // On actualise la stat du nombre de partie gagné.
            labNbPartiesGagneRouge.Content = "Parties gagnées : " + nbPartieGagneRouge;
            labNbPartiesGagneBleu.Content = "Parties gagnées : " + nbPartieGagneBleu;
            Console.WriteLine("Jeu - Nombre de parties gagnées actualisées.");

            // On réinitialise le temps.
            tempsJeu = tempsInitial;
            labTemps.Content = tempsJeu.ToString();
            Console.WriteLine("Jeu - Temps réinitialisé.");

            // On réinitialise les carreaux et on remet les joueurs à leur place.
            foreach (System.Windows.Shapes.Rectangle x in monCanvas.Children.OfType<System.Windows.Shapes.Rectangle>())
            {
                if ((string)x.Tag == "joueur")
                {
                    if (fenetreAccueil.grille5x5 == true)
                    {
                        Canvas.SetLeft(joueur1, SET_LEFT_J1_5X5);
                        Canvas.SetTop(joueur1, SET_TOP_J1_5X5);

                        Canvas.SetLeft(joueur2, SET_LEFT_J2_5X5);
                        Canvas.SetTop(joueur2, SET_TOP_J2_5X5);
                    }
                    else
                    {
                        Canvas.SetLeft(joueur1, SET_LEFT_J1_7X7);
                        Canvas.SetTop(joueur1, SET_TOP_J1_7X7);

                        Canvas.SetLeft(joueur2, SET_LEFT_J2_7X7);
                        Canvas.SetTop(joueur2, SET_TOP_J2_7X7);
                    }
                }
                if ((string)x.Tag == "bleu" || (string)x.Tag == "rouge")
                {
                    x.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                    x.Tag = "blanc";
                }
            }
            Console.WriteLine("Jeu - Grille et joueurs réinitialisés.");

            // On remet le fond à sa place.
            Canvas.SetZIndex(rectFond, 0);

            // On réinitialise les coordonnées.
            x1 = nbLigne - nbLigne;
            y1 = nbColonne - nbColonne;
            x2 = nbColonne - 1;
            y2 = nbLigne - 1;
            labCooRouge.Content = "x,y : " + x1 + "," + y1;
            labCooBleu.Content = "x,y : " + x2 + "," + y2;
            Console.WriteLine("Jeu - Coordonnées réinitialisées.");

            // On met en Hidden toutes les fenêtres pour restart le jeu.
            labBleuGagne.Visibility = Visibility.Hidden;
            labRougeGagne.Visibility = Visibility.Hidden;
            labPersonneGagne.Visibility = Visibility.Hidden;
            butRejouer.Visibility = Visibility.Hidden;
            butMenu.Visibility = Visibility.Hidden;

            // Et c'est reparti !
            labCommencer.Visibility = Visibility.Visible;
        }

        private void Rejouer(object sender, RoutedEventArgs e)
        {
            Reinitialisation();
            Console.WriteLine("Jeu - Rejouer.");
        }

        private void NouveauxParametres()
        {
            // Difficulté (affecte la rapidité de l'adversaire).
            if (fenetreAccueil.unJoueur == true)
            {
                if (fenetreAccueil.modeFacile == true)
                {
                    difficulte = VITESSE_TICK_FACILE;
                }      
                else if (fenetreAccueil.modeNormal == true)
                {
                    difficulte = VITESSE_TICK_NORMALE;
                }
                else
                {
                    difficulte = VITESSE_TICK_DIFFICILE;
                } 
                robotTimer.Tick += AdversaireTimer;
                robotTimer.Interval = TimeSpan.FromMilliseconds(difficulte);
            }

            // Taille de la grille.
            if (fenetreAccueil.grille5x5 == true)
            {
                Canvas.SetLeft(joueur1, SET_LEFT_J1_5X5);
                Canvas.SetTop(joueur1, SET_TOP_J1_5X5);

                Canvas.SetLeft(joueur2, SET_LEFT_J2_5X5);
                Canvas.SetTop(joueur2, SET_TOP_J2_5X5);
                nbLigne = 5;
                nbColonne = 5;
            }
            else
            {
                Canvas.SetLeft(joueur1, SET_LEFT_J1_7X7);
                Canvas.SetTop(joueur1, SET_TOP_J1_7X7);

                Canvas.SetLeft(joueur2, SET_LEFT_J2_7X7);
                Canvas.SetTop(joueur2, SET_TOP_J2_7X7);
                nbLigne = 7;
                nbColonne = 7;
            }

            // Limites grille.
            maxX = nbColonne - 1;
            minX = 0;
            maxY = nbLigne - 1;
            minY = 0;

            // Coordonnées.
            x1 = nbLigne - nbLigne;
            y1 = nbColonne - nbColonne;
            x2 = nbColonne - 1;
            y2 = nbLigne - 1;

            // Volume.
            volume = Accueil.valeurSon;
            musiqueJeu.Volume = volume / CONVERTION_VOLUME_DECIMALE;

            // Fonds.
            if (fenetreAccueil.fond1 == true)
            {
                rectFond.Fill = fenetreAccueil.espaceFond;
            }
            else if (fenetreAccueil.fond2 == true)
            {
                rectFond.Fill = fenetreAccueil.auroreBorealeFond;
            }
            else if (fenetreAccueil.fond3 == true)
            {
                rectFond.Fill = fenetreAccueil.cielBleuFond;
            }
            else if (fenetreAccueil.fond4 == true)
            {
                rectFond.Fill = fenetreAccueil.nebuleuseFond;
            }
            else if (fenetreAccueil.fond5 == true)
            {
                rectFond.Fill = fenetreAccueil.eauFond;
            }
            else if (fenetreAccueil.fond6 == true)
            {
                rectFond.Fill = fenetreAccueil.herbeFond;
            }
            Console.WriteLine("Jeu - Remplissage du fond.");

            // Création des rectangles et on charge le Canvas pour que les coordonnées de la grille soient correcte.
            CreationRectangle();
            monCanvas.Loaded += (sender, e) => CreationGrille();
            Console.WriteLine("Jeu - Création de la grille.");

            // On actualise les stats.
            labCooRouge.Content = "x,y : " + x1 + "," + y1;
            labCooBleu.Content = "x,y : " + x2 + "," + y2;

            labTourRouge.Content = "Nb tours joués : " + nbTourRouge;
            labTourBleu.Content = "Nb tours joués : " + nbTourBleu;

            labNbPartiesGagneRouge.Content = "Parties gagnées : " + nbPartieGagneRouge;
            labNbPartiesGagneBleu.Content = "Parties gagnées : " + nbPartieGagneBleu;
            Console.WriteLine("Statistiques actualisées.");

            // Temps.
            tempsInitial = 10 * Math.Round(Accueil.valeurTemps);
            tempsJeu = tempsInitial;
            labTemps.Content = tempsJeu.ToString();

            // On attribue les nouvelles keys (s'il y en a).
            ValKeyHautJ1 = Accueil.KeyHautJ1;
            ValKeyGaucheJ1 = Accueil.KeyGaucheJ1;
            ValKeyBasJ1 = Accueil.KeyBasJ1;
            ValKeyDroiteJ1 = Accueil.KeyDroiteJ1;
            ValKeyHautJ2 = Accueil.KeyHautJ2;
            ValKeyGaucheJ2 = Accueil.KeyGaucheJ2;
            ValKeyBasJ2 = Accueil.KeyBasJ2;
            ValKeyDroiteJ2 = Accueil.KeyDroiteJ2;
            Console.WriteLine("Jeu - Touches redéfinient.");
        }

        private void SupprimerGrille()
        {
            // On créer une liste temporaire pour stocker les rectangles à supprimer.
            List<System.Windows.Shapes.Rectangle> rectanglesASupprimer = new List<System.Windows.Shapes.Rectangle>();

            // Parcours de tous les enfants du Canvas et ajout des rectangles à la liste temporaire
            foreach (System.Windows.Shapes.Rectangle x in monCanvas.Children.OfType<System.Windows.Shapes.Rectangle>())
            {
                if ((string)x.Tag == "rouge" || (string)x.Tag == "bleu" || (string)x.Tag == "blanc")
                {
                    rectanglesASupprimer.Add(x);
                }
            }
            // Suppression des rectangles du Canvas en utilisant la liste temporaire
            foreach (System.Windows.Shapes.Rectangle x in rectanglesASupprimer)
            {
                monCanvas.Children.Remove(x);
            }
            // On réinitialise la grille.
            grille = null;
        }

        private void Menu(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Jeu - Retour au menu.");
            musiqueJeu.Stop();
            Accueil.musiqueAccueil.Play();
            Accueil.musiqueAccueil.MediaEnded += (sender, e) => Accueil.musiqueAccueil.Position = TimeSpan.Zero;
            fenetreAccueil.ShowDialog();
            if (fenetreAccueil.nouvellePartie == true)
            {
                SupprimerGrille();
                NouveauxParametres();
                CreationGrille();
                Reinitialisation();
                Console.WriteLine("Jeu - Nouvelle partie.");
                fenetreAccueil.nouvellePartie = false;    
            }
        }
    }
}