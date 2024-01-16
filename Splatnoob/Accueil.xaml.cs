using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace Splatnoob
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        public bool unJoueur = false;
        public bool deuxJoueurs = false;
        private MediaPlayer musiqueAccueil = new MediaPlayer();
        private double volume;
        public Accueil()
        {
            InitializeComponent();
            musiqueAccueil.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Apocalypse.mp3"));
            musiqueAccueil.Play();
        }
        
        public void Button_parametre_Click(object sender, RoutedEventArgs e)
        {
            musiqueAccueil.Stop();
            parametre fenetreParametre = new parametre();
            fenetreParametre.ShowDialog();
            volume = parametre.valeursons;
            musiqueAccueil.Volume = volume / MainWindow.CONVERTION_VOLUME_DECIMALE;
            musiqueAccueil.Play();
        }

        public void Button_start_Click(object sender, RoutedEventArgs e)
        {
            musiqueAccueil.Stop();
            this.Hide();
        }

        private void Canvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            musiqueAccueil.Stop();
            Application.Current.Shutdown();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (comChoixJoueurs.SelectedIndex)
            {
                case 0:
                    unJoueur = true;
                    break;
                case 1:
                    deuxJoueurs = true;
                    break;
            }
        }
    }
}
