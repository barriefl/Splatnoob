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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Splatnoob
{
    /// <summary>
    /// Logique d'interaction pour parametre.xaml
    /// </summary>
    public partial class parametre : Window
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        
        private int TEMPSPARDEPLACAGE = 10;
        private int NBRSECMIN = 60;

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
        public parametre()
        {
            InitializeComponent();
            choisirTps.Value = valeurtemps;
            choisirSons.Value = valeursons;
            dispatcherTimer.Tick += actualisation;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16);
            dispatcherTimer.Start();
        }

        private void actualisation(object sender, EventArgs e)
        {
            valeurtemps = choisirTps.Value;
            double tpsTotal = TEMPSPARDEPLACAGE * Math.Round(choisirTps.Value);
            double tpsMin = Math.Truncate(tpsTotal / NBRSECMIN);
            double tpsSec = (tpsTotal - (tpsMin * NBRSECMIN));
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Canvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
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
    }
}
