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

namespace Alpha
{
    /// <summary>
    /// Interaction logic for Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        // Timer.
        private DispatcherTimer timer = new DispatcherTimer();

        // Musiques.
        private MediaPlayer musiqueParametres = new MediaPlayer();
        public static MediaPlayer musiqueAccueil = new MediaPlayer();

        // Fonds.
        public ImageBrush espaceFond = new ImageBrush();
        public ImageBrush auroreBorealeFond = new ImageBrush();
        public ImageBrush cielBleuFond = new ImageBrush();
        public ImageBrush nebuleuseFond = new ImageBrush();
        public ImageBrush eauFond = new ImageBrush();
        public ImageBrush herbeFond = new ImageBrush();

        // Booléens.
        public bool unJoueur, deuxJoueurs, modeFacile, modeNormal, modeDifficile = false;
        private bool boutonHautJ1, boutonGaucheJ1, boutonBasJ1, boutonDroiteJ1, boutonHautJ2, boutonGaucheJ2, boutonBasJ2, boutonDroiteJ2 = false;
        
        public bool fond1 = true;
        public bool fond2, fond3, fond4, fond5, fond6 = false;

        public bool grille5x5 = true;
        public bool grille7x7 = false;

        public bool nouvellePartie = false;

        // Constantes.
        private const int TEMPS_PAR_DEPLACAGE = 10;
        private const int NBR_SEC_MIN = 60;

        // Variables.

        // Keys.
        public static Key keyHautJ1 = Key.Z;
        public static Key keyGaucheJ1 = Key.Q;
        public static Key keyBasJ1 = Key.S;
        public static Key keyDroiteJ1 = Key.D;

        public static Key keyHautJ2 = Key.Up;
        public static Key keyGaucheJ2 = Key.Left;
        public static Key keyBasJ2 = Key.Down;
        public static Key keyDroiteJ2 = Key.Right;

        public static Key KeyHautJ1
        {
            get { return keyHautJ1; }
            set { keyHautJ1 = value; }
        }

        public static Key KeyGaucheJ1
        {
            get { return keyGaucheJ1; }
            set { keyGaucheJ1 = value; }
        }
        
        public static Key KeyBasJ1
        {
            get { return keyBasJ1; }
            set { keyBasJ1 = value; }
        }
        
        public static Key KeyDroiteJ1
        {
            get { return keyDroiteJ1; }
            set { keyDroiteJ1 = value; }
        }
        

        public static Key KeyHautJ2
        {
            get { return keyHautJ2; }
            set { keyHautJ2 = value; }
        }
        
        public static Key KeyGaucheJ2
        {
            get { return keyGaucheJ2; }
            set { keyGaucheJ2 = value; }
        }
        
        public static Key KeyBasJ2
        {
            get { return keyBasJ2; }
            set { keyBasJ2 = value; }
        }
        
        public static Key KeyDroiteJ2
        {
            get { return keyDroiteJ2; }
            set { keyDroiteJ2 = value; }
        }

        // Sliders.
        public static double valeurTemps = 1;
        public static double valeurSon = 100;

        public static double ValeurTemps
        {
            get { return valeurTemps; }
            set { valeurTemps = value; }
        }

        public static double ValeurSon
        {
            get { return valeurSon; }
            set { valeurSon = value; }
        }

        public Accueil()
        {
            // Lancement page d'accueil & paramètres.
            InitializeComponent();
            Console.WriteLine("Démarrage de la fenêtre 'Accueil'.");

            // On attribue la valeur 'temps' et 'son' aux sliders.
            slidChoisirTps.Value = valeurTemps;
            slidChoisirSon.Value = valeurSon;

            // Chemin musiques.
            musiqueParametres.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Wrong_Place.mp3"));
            musiqueAccueil.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Apocalypse.mp3"));

            // Chemin des fonds.
            espaceFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Espace.jpg"));
            auroreBorealeFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Aurore_Boreale.jpg"));
            cielBleuFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Ciel_Bleu.jpg"));
            nebuleuseFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Nebuleuse.jpg"));
            eauFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Eau.jpg"));
            herbeFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Herbe.jpg"));
            Console.WriteLine("Paramètres - Chargement des fonds...");

            // On rempli nos boutons avec les fonds.
            butFond1.Background = espaceFond;
            butFond2.Background = auroreBorealeFond;
            butFond3.Background = cielBleuFond;
            butFond4.Background = nebuleuseFond;
            butFond5.Background = eauFond;
            butFond6.Background = herbeFond;
            Console.WriteLine("Paramètres - Fonds chargés.");

            // Lancement de la musique.
            musiqueAccueil.Play();
            musiqueAccueil.MediaEnded += (sender, e) => musiqueAccueil.Position = TimeSpan.Zero;
            Console.WriteLine("Accueil - Lancement de la musique 'Apocalypse.mp3'.");
        }

        private void Mode1Joueur (object sender, RoutedEventArgs e)
        {
            unJoueur = true;
            deuxJoueurs = false;
            butFacile.Visibility = Visibility.Visible;
            butNormal.Visibility = Visibility.Visible;
            butDifficile.Visibility = Visibility.Visible;
            labAttentionJoueurs.Visibility = Visibility.Hidden;
            but1Joueur.BorderBrush = Brushes.White;
            but2Joueurs.BorderBrush = Brushes.Black;
            Console.WriteLine("Accueil - Mode 1 joueur choisi.");
        }


        private void Mode2Joueurs (object sender, RoutedEventArgs e)
        {
            unJoueur = false;
            deuxJoueurs = true;
            butFacile.Visibility = Visibility.Hidden;
            butNormal.Visibility = Visibility.Hidden;
            butDifficile.Visibility = Visibility.Hidden;
            labAttentionJoueurs.Visibility = Visibility.Hidden;
            but1Joueur.BorderBrush = Brushes.Black;
            but2Joueurs.BorderBrush = Brushes.White;
            Console.WriteLine("Accueil - Mode 2 joueurs choisi.");
        }

        private void ModeFacile (object sender, RoutedEventArgs e)
        {
            modeFacile = true;
            modeNormal = false;
            modeDifficile = false;
            labAttentionDifficulte.Visibility = Visibility.Hidden;
            butFacile.BorderBrush = Brushes.White;
            butNormal.BorderBrush = Brushes.Black;
            butDifficile.BorderBrush = Brushes.Black;
            Console.WriteLine("Accueil - Mode facile choisi.");
        }

        private void ModeNormal (object sender, RoutedEventArgs e)
        {
            modeFacile = false;
            modeNormal = true;
            modeDifficile = false;
            labAttentionDifficulte.Visibility = Visibility.Hidden;
            butFacile.BorderBrush = Brushes.Black;
            butNormal.BorderBrush = Brushes.White;
            butDifficile.BorderBrush = Brushes.Black;
            Console.WriteLine("Accueil - Mode normal choisi.");
        }

        private void ModeDifficile (object sender, RoutedEventArgs e)
        {
            modeFacile = false;
            modeNormal = false;
            modeDifficile = true;
            labAttentionDifficulte.Visibility = Visibility.Hidden;
            butFacile.BorderBrush = Brushes.Black;
            butNormal.BorderBrush = Brushes.Black;
            butDifficile.BorderBrush = Brushes.White;
            Console.WriteLine("Accueil - Mode difficile choisi.");
        }

        private void NouvellePartie (object sender, RoutedEventArgs e)
        {
            if (unJoueur == false && deuxJoueurs == false)
            {
                labAttentionJoueurs.Visibility = Visibility.Visible;
                Console.WriteLine("Accueil - Erreur, il faut choisir un mode 1/2 joueur(s).");
            }
            else if ((modeFacile == false && modeNormal == false && modeDifficile == false) && unJoueur == true)
            {
                labAttentionDifficulte.Visibility = Visibility.Visible;
                Console.WriteLine("Accueil - Erreur, il faut choisir un niveau de difficulté.");
            }
            else
            {
                nouvellePartie = true;
                musiqueAccueil.Stop();
                timer.Stop();
                Console.WriteLine("Accueil - Arrêt de la musique 'Apocalypse.mp3'." + "\nParamètres - Arrêt du timer 'Actualisation'." + "\nAccueil - Lancement du jeu.");
                this.Hide();
            }
        }      

        private void Parametres (object sender, RoutedEventArgs e)
        {
            timer.Tick += Actualisation;
            timer.Interval = TimeSpan.FromMilliseconds(150);
            timer.Start();
            Console.WriteLine("Démarrage du timer 'Actualisation'.");

            Console.WriteLine("Accueil - Affichage des paramètres.");
            canvasAccueil.Visibility = Visibility.Hidden;
            canvasParametre.Visibility = Visibility.Visible;

            musiqueAccueil.Stop();
            musiqueParametres.Play();
            musiqueParametres.MediaEnded += (sender, e) => musiqueParametres.Position = TimeSpan.Zero;
            Console.WriteLine("Paramètres - Lancement de la musique 'Wrong_Place.mp3'.");
        }

        private void RetourAccueil (object sender, RoutedEventArgs e)
        {
            timer.Stop();
            musiqueParametres.Stop();
            musiqueAccueil.Play();
            Console.WriteLine("Paramètres - Arrêt de la musique 'Wrong_Place.mp3'." + "\nParamètres - Affichage de l'accueil." + "\nParamètres - Arrêt du timer 'Actualisation'." + "\nAccueil - Lancement de la musique 'Apocalypse.mp3'.");
            canvasAccueil.Visibility = Visibility.Visible;
            canvasParametre.Visibility = Visibility.Hidden;
        }

        private void Actualisation(object sender, EventArgs e)
        {
            valeurTemps = slidChoisirTps.Value;
            double tpsTotal = TEMPS_PAR_DEPLACAGE * Math.Round(slidChoisirTps.Value);
            double tpsMin = Math.Truncate(tpsTotal / NBR_SEC_MIN);
            double tpsSec = (tpsTotal - (tpsMin * NBR_SEC_MIN));

            if (tpsSec != 0)
            {
                labAfficheTps.Content = (tpsMin + ":" + tpsSec);
            }
            else
            {
                labAfficheTps.Content = (tpsMin + ":00");
            }

            valeurSon = Math.Round(slidChoisirSon.Value);
            labAfficheSon.Content = (valeurSon + "%");
            MainWindow.volume = valeurSon;
            musiqueAccueil.Volume = MainWindow.volume / MainWindow.CONVERTION_VOLUME_DECIMALE;
            musiqueParametres.Volume = MainWindow.volume / MainWindow.CONVERTION_VOLUME_DECIMALE;

            butHautJ1.Content = keyHautJ1;
            butGaucheJ1.Content = keyGaucheJ1;
            butBasJ1.Content = keyBasJ1;
            butDroiteJ1.Content = keyDroiteJ1;
            butHautJ2.Content = keyHautJ2;
            butGaucheJ2.Content = keyGaucheJ2;
            butBasJ2.Content = keyBasJ2;
            butDroiteJ2.Content = keyDroiteJ2;
        }

        private void BoutonHautJoueur1 (object sender, RoutedEventArgs e)
        {
            boutonHautJ1 = true;
            boutonGaucheJ1 = false;
            boutonBasJ1 = false;
            boutonDroiteJ1 = false;
            boutonHautJ2 = false;
            boutonGaucheJ2 = false;
            boutonBasJ2 = false;
            boutonDroiteJ2 = false;
        }

        private void BoutonGaucheJoueur1 (object sender, RoutedEventArgs e)
        {
            boutonHautJ1 = false;
            boutonGaucheJ1 = true;
            boutonBasJ1 = false;
            boutonDroiteJ1 = false;
            boutonHautJ2 = false;
            boutonGaucheJ2 = false;
            boutonBasJ2 = false;
            boutonDroiteJ2 = false;
        }

        private void BoutonBasJoueur1 (object sender, RoutedEventArgs e)
        {
            boutonHautJ1 = false;
            boutonGaucheJ1 = false;
            boutonBasJ1 = true;
            boutonDroiteJ1 = false;
            boutonHautJ2 = false;
            boutonGaucheJ2 = false;
            boutonBasJ2 = false;
            boutonDroiteJ2 = false;
        }

        private void BoutonDroiteJoueur1 (object sender, RoutedEventArgs e)
        {
            boutonHautJ1 = false;
            boutonGaucheJ1 = false;
            boutonBasJ1 = false;
            boutonDroiteJ1 = true;
            boutonHautJ2 = false;
            boutonGaucheJ2 = false;
            boutonBasJ2 = false;
            boutonDroiteJ2 = false;
        }

        private void BoutonHautJoueur2 (object sender, RoutedEventArgs e)
        {
            boutonHautJ1 = false;
            boutonGaucheJ1 = false;
            boutonBasJ1 = false;
            boutonDroiteJ1 = false;
            boutonHautJ2 = true;
            boutonGaucheJ2 = false;
            boutonBasJ2 = false;
            boutonDroiteJ2 = false;
        }

        private void BoutonGaucheJoueur2 (object sender, RoutedEventArgs e)
        {
            boutonHautJ1 = false;
            boutonGaucheJ1 = false;
            boutonBasJ1 = false;
            boutonDroiteJ1 = false;
            boutonHautJ2 = false;
            boutonGaucheJ2 = true;
            boutonBasJ2 = false;
            boutonDroiteJ2 = false;
        }

        private void BoutonBasJoueur2 (object sender, RoutedEventArgs e)
        {
            boutonHautJ1 = false;
            boutonGaucheJ1 = false;
            boutonBasJ1 = false;
            boutonDroiteJ1 = false;
            boutonHautJ2 = false;
            boutonGaucheJ2 = false;
            boutonBasJ2 = true;
            boutonDroiteJ2 = false;
        }

        private void BoutonDroiteJoueur2 (object sender, RoutedEventArgs e)
        {
            boutonHautJ1 = false;
            boutonGaucheJ1 = false;
            boutonBasJ1 = false;
            boutonDroiteJ1 = false;
            boutonHautJ2 = false;
            boutonGaucheJ2 = false;
            boutonBasJ2 = false;
            boutonDroiteJ2 = true;
        }

        private void ToucheBas(object sender, KeyEventArgs e)
        {
            if (boutonHautJ1)
            {
                KeyHautJ1 = e.Key;
                boutonHautJ1 = false;
                Console.WriteLine("Paramètres - Touche haut du joueur 1 changée par: " + e.Key);
            }
            if (boutonGaucheJ1)
            {
                KeyGaucheJ1 = e.Key;
                boutonGaucheJ1 = false;
                Console.WriteLine("Paramètres - Touche gauche du joueur 1 changée par: " + e.Key);
            }
            if (boutonBasJ1)
            {
                KeyBasJ1 = e.Key;
                boutonBasJ1 = false;
                Console.WriteLine("Paramètres - Touche bas du joueur 1 changée par: " + e.Key);
            }
            if (boutonDroiteJ1)
            {
                KeyDroiteJ1 = e.Key;
                boutonDroiteJ1 = false;
                Console.WriteLine("Paramètres - Touche droite du joueur 1 changée par: " + e.Key);
            }
            if (boutonHautJ2)
            {
                KeyHautJ2 = e.Key;
                boutonHautJ2 = false;
                Console.WriteLine("Paramètres - Touche haut du joueur 2 changée par: " + e.Key);
            }
            if (boutonGaucheJ2)
            {
                KeyGaucheJ2 = e.Key;
                boutonGaucheJ2 = false;
                Console.WriteLine("Paramètres - Touche gauche du joueur 2 changée par: " + e.Key);
            }
            if (boutonBasJ2)
            {
                KeyBasJ2 = e.Key;
                boutonBasJ2 = false;
                Console.WriteLine("Paramètres - Touche bas du joueur 2 changée par: " + e.Key);
            }
            if (boutonDroiteJ2)
            {
                KeyDroiteJ2 = e.Key;
                boutonDroiteJ2 = false;
                Console.WriteLine("Paramètres - Touche droite du joueur 2 changée par: " + e.Key);
            }
        }

        private void FondEspace (object sender, RoutedEventArgs e)
        {
            fond1 = true;
            fond2 = false;
            fond3 = false;
            fond4 = false;
            fond5 = false;
            fond6 = false;
            butFond1.BorderBrush = Brushes.White;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.Black;
            Console.WriteLine("Paramètres - Fond choisi: Espace.jpg");
        }

        private void FondAuroreBoreale (object sender, RoutedEventArgs e)
        {
            fond1 = false;
            fond2 = true;
            fond3 = false;
            fond4 = false;
            fond5 = false;
            fond6 = false;
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.White;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.Black;
            Console.WriteLine("Paramètres - Fond choisi: Aurore_Boreale.jpg");
        }

        private void FondCielBleu (object sender, RoutedEventArgs e)
        {
            fond1 = false;
            fond2 = false;
            fond3 = true;
            fond4 = false;
            fond5 = false;
            fond6 = false;
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.White;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.Black;
            Console.WriteLine("Paramètres - Fond choisi: Ciel_Bleu.jpg");
        }

        private void FondNebuleuse (object sender, RoutedEventArgs e)
        {
            fond1 = false;
            fond2 = false;
            fond3 = false;
            fond4 = true;
            fond5 = false;
            fond6 = false;
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.White;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.Black;
            Console.WriteLine("Paramètres - Fond choisi: Nebuleuse.jpg");
        }

        private void FondEau (object sender, RoutedEventArgs e)
        {
            fond1 = false;
            fond2 = false;
            fond3 = false;
            fond4 = false;
            fond5 = true;
            fond6 = false;
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.White;
            butFond6.BorderBrush = Brushes.Black;
            Console.WriteLine("Paramètres - Fond choisi: Eau.jpg");
        }

        private void FondHerbe (object sender, RoutedEventArgs e)
        {
            fond1 = false;
            fond2 = false;
            fond3 = false;
            fond4 = false;
            fond5 = false;
            fond6 = true;
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.White;
            Console.WriteLine("Paramètres - Fond choisi: Herbe.jpg");
        }

        private void Grille5x5 (object sender, RoutedEventArgs e)
        {
            butGrille5x5.BorderBrush = Brushes.White;
            butGrille7x7.BorderBrush = Brushes.Black;
            grille5x5 = true;
            grille7x7 = false;
            Console.WriteLine("Paramètres - Grille 5x5 choisi.");
        }

        private void Grille7x7 (object sender, RoutedEventArgs e)
        {
            butGrille5x5.BorderBrush = Brushes.Black;
            butGrille7x7.BorderBrush = Brushes.White;
            grille5x5 = false;
            grille7x7 = true;
            Console.WriteLine("Paramètres - Grille 7x7 choisi.");
        }
    }
}