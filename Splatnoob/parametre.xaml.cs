using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Splatnoob
{
    /// <summary>
    /// Logique d'interaction pour Parametre.xaml
    /// </summary>
    public partial class Parametre : Window
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public ImageBrush[] Brushfond = new ImageBrush[6];
        public string[] SourceImageBrush = new string[6] { "Images/Espace.jpg", "Images/Aurore_Boreale.jpg", "Images/Ciel_Bleu.jpg", "Images/Nebuleuse.jpg", "Images/Eau.jpg", "Images/Herbe.jpg" };

        /*
        public ImageBrush espaceFond = new ImageBrush();
        public ImageBrush auroreBorealeFond = new ImageBrush();
        public ImageBrush cielBleuFond = new ImageBrush();
        public ImageBrush nebuleuseFond = new ImageBrush();
        public ImageBrush eauFond = new ImageBrush();
        public ImageBrush herbeFond = new ImageBrush();
        */

        public bool[] fonds = new bool[6];

        private const int TEMPS_PAR_DEPLACAGE = 10;
        private const int NBR_SEC_MIN = 60;

        public static Key keyHautJ1 = Key.Z;
        public static Key KeyHautJ1
        {
            get { return keyHautJ1; }
            set { keyHautJ1 = value; }
        }
        public static Key keyGaucheJ1 = Key.Q;
        public static Key KeyGaucheJ1
        {
            get { return keyGaucheJ1; }
            set { keyGaucheJ1 = value; }
        }
        public static Key keyBasJ1 = Key.S;
        public static Key KeyBasJ1
        {
            get { return keyBasJ1; }
            set { keyBasJ1 = value; }
        }
        public static Key keyDroiteJ1 = Key.D;
        public static Key KeyDroiteJ1
        {
            get { return keyDroiteJ1; }
            set { keyDroiteJ1 = value; }
        }
        public static Key keyHautJ2 = Key.Up;
        public static Key KeyHautJ2
        {
            get { return keyHautJ2; }
            set { keyHautJ2 = value; }
        }
        public static Key keyGaucheJ2 = Key.Left;
        public static Key KeyGaucheJ2
        {
            get { return keyGaucheJ2; }
            set { keyGaucheJ2 = value; }
        }
        public static Key keyBasJ2 = Key.Down;
        public static Key KeyBasJ2
        {
            get { return keyBasJ2; }
            set { keyBasJ2 = value; }
        }
        public static Key keyDroiteJ2 = Key.Right;
        public static Key KeyDroiteJ2
        {
            get { return keyDroiteJ2; }
            set { keyDroiteJ2 = value; }
        }

        private bool boutonHautJ1 = false;
        private bool boutonGaucheJ1 = false;
        private bool boutonBasJ1 = false;
        private bool boutonDroiteJ1 = false;
        private bool boutonHautJ2 = false;
        private bool boutonGaucheJ2 = false;
        private bool boutonBasJ2 = false;
        private bool boutonDroiteJ2 = false;

        public static double valeurtemps = 1;

        public static double Valeurtemps
        {
            get { return valeurtemps; }
            set { valeurtemps = value; }
        }
        public static double valeursons = 100;
        public static double Valeursons
        {
            get { return valeursons; }
            set { valeursons = value; }
        }

        private MediaPlayer musiqueParametres = new MediaPlayer();

        public Parametre()
        {
            InitializeComponent();
            Console.WriteLine("Démarrage fenêtre paramètre.");
            for (int i = 0; i < 6; i++)
            {
                fonds[i] = false;

                //Console.WriteLine(SourceImageBrush[i]);
                //Console.WriteLine(fonds[i]);

                Brushfond[i] = new ImageBrush();
                Brushfond[i].ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + SourceImageBrush[i]));
            }
            fonds[0] = true;
            /*
            espaceFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Espace.jpg"));
            auroreBorealeFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Aurore_Boreale.jpg"));
            cielBleuFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Ciel_Bleu.jpg"));
            nebuleuseFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Nebuleuse.jpg"));
            eauFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Eau.jpg"));
            herbeFond.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Images/Herbe.jpg"));
            */

            butFond1.Background = Brushfond[0]; //espaceFond;
            butFond2.Background = Brushfond[1]; //auroreBorealeFond;
            butFond3.Background = Brushfond[2]; //cielBleuFond;
            butFond4.Background = Brushfond[3]; //nebuleuseFond;
            butFond5.Background = Brushfond[4]; //eauFond;
            butFond6.Background = Brushfond[5]; //herbeFond;
            

            choisirTps.Value = valeurtemps;
            choisirSons.Value = valeursons;
            dispatcherTimer.Tick += actualisation;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(50);
            dispatcherTimer.Start();
            musiqueParametres.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Wrong_Place.mp3"));
            musiqueParametres.Play();

        }

        private void actualisation(object sender, EventArgs e)
        {
            valeurtemps = choisirTps.Value;
            double tpsTotal = TEMPS_PAR_DEPLACAGE * Math.Round(choisirTps.Value);
            double tpsMin = Math.Truncate(tpsTotal / NBR_SEC_MIN);
            double tpsSec = (tpsTotal - (tpsMin * NBR_SEC_MIN));

            valeursons = Math.Round(choisirSons.Value);
            affichSons.Content = (valeursons + "%");

            if (tpsSec != 0)
                affichTps.Content = (tpsMin + ":" + tpsSec);
            else
                affichTps.Content = (tpsMin + ":00");

            bHautJ1.Content = keyHautJ1;
            bGaucheJ1.Content = keyGaucheJ1;
            bBasJ1.Content = keyBasJ1;
            bDroiteJ1.Content = keyDroiteJ1;
            bHautJ2.Content = keyHautJ2;
            bGaucheJ2.Content = keyGaucheJ2;
            bBasJ2.Content = keyBasJ2;
            bDroiteJ2.Content = keyDroiteJ2;

            musiqueParametres.Volume = valeursons/ MainWindow.CONVERTION_VOLUME_DECIMALE;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            musiqueParametres.Stop();
            this.Close();
        }

        private void Canvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            musiqueParametres.Stop();
            Application.Current.Shutdown();
        }

        private void Click_HautJ1(object sender, RoutedEventArgs e)
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
        private void Click_GaucheJ1(object sender, RoutedEventArgs e)
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
        private void Click_BasJ1(object sender, RoutedEventArgs e)
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
        private void Click_DroiteJ1(object sender, RoutedEventArgs e)
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
        private void Click_HautJ2(object sender, RoutedEventArgs e)
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
        private void Click_GaucheJ2(object sender, RoutedEventArgs e)
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
        private void Click_BasJ2(object sender, RoutedEventArgs e)
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
        private void Click_DroiteJ2(object sender, RoutedEventArgs e)
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

        private void TouchePresser(object sender, KeyEventArgs e)
        {
            if (boutonHautJ1)
            {
                KeyHautJ1 = e.Key;
                boutonHautJ1 = false;
            }
            if (boutonGaucheJ1)
            {
                KeyGaucheJ1 = e.Key;
                boutonGaucheJ1 = false;
            }
            if (boutonBasJ1)
            {
                KeyBasJ1 = e.Key;
                boutonBasJ1 = false;
            }
            if (boutonDroiteJ1)
            {
                KeyDroiteJ1 = e.Key;
                boutonDroiteJ1 = false;
            }
            if (boutonHautJ2)
            {
                KeyHautJ2 = e.Key;
                boutonHautJ2 = false;
            }
            if (boutonGaucheJ2)
            {
                KeyGaucheJ2 = e.Key;
                boutonGaucheJ2 = false;
            }
            if (boutonBasJ2)
            {
                KeyBasJ2 = e.Key;
                boutonBasJ2 = false;
            }
            if (boutonDroiteJ2)
            {
                KeyDroiteJ2 = e.Key;
                boutonDroiteJ2 = false;
            }
        }

        private void butFond1_Click(object sender, RoutedEventArgs e)
        {
            fonds[0] = true;
            int[] tab = new int[]{ 1, 2, 3, 4, 5};
            foreach (int i in tab)
            {
                fonds[i] = false;
            }
            butFond1.BorderBrush = Brushes.White;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.Black;
            
        }

        private void butFond2_Click(object sender, RoutedEventArgs e)
        {
            fonds[1] = true;
            int[] tab = new int[] {0, 2, 3, 4, 5 };
            foreach (int i in tab)
            {
                fonds[i] = false;
            }
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.White;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.Black;
            
        }

        private void butFond3_Click(object sender, RoutedEventArgs e)
        {
            fonds[2] = true;
            int[] tab = new int[] { 0, 1, 3, 4, 5 };
            foreach (int i in tab)
            {
                fonds[i] = false;
            }
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.White;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.Black;
            
        }

        private void butFond4_Click(object sender, RoutedEventArgs e)
        {
            fonds[3] = true;
            int[] tab = new int[] { 0, 1, 2, 4, 5 };
            foreach (int i in tab)
            {
                fonds[i] = false;
            }
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.White; 
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.Black;
            Console.WriteLine(fonds[3]);
        }

        private void butFond5_Click(object sender, RoutedEventArgs e)
        {
            fonds[4] = true;
            int[] tab = new int[] { 0, 1, 2, 3, 5 };
            foreach (int i in tab)
            {
                fonds[i] = false;
            }
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.White;
            butFond6.BorderBrush = Brushes.Black;
            
        }

        private void butFond6_Click(object sender, RoutedEventArgs e)
        {
            fonds[5] = true;
            int[] tab = new int[] { 0, 1, 2, 3, 4 };
            foreach (int i in tab)
            {
                fonds[i] = false;
            }
            butFond1.BorderBrush = Brushes.Black;
            butFond2.BorderBrush = Brushes.Black;
            butFond3.BorderBrush = Brushes.Black;
            butFond4.BorderBrush = Brushes.Black;
            butFond5.BorderBrush = Brushes.Black;
            butFond6.BorderBrush = Brushes.White;
        }
    }
}
