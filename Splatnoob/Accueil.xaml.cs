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
        public bool modeFacile = false;
        public bool modeNormal = false;
        public bool modeDifficile = false;
        public Accueil()
        {
            InitializeComponent();
        }
        
        public void Button_parametre_Click(object sender, RoutedEventArgs e)
        {
            parametre fenetreParametre = new parametre();
            fenetreParametre.ShowDialog();
        }

        public void Button_start_Click(object sender, RoutedEventArgs e)
        {
            if (unJoueur == false && deuxJoueurs == false)
            {
                labAttentionJoueurs.Visibility = Visibility.Visible;
            }
            else if ((modeFacile == false && modeNormal == false && modeNormal == false) && unJoueur == true)
            {
                labAttentionDifficulte.Visibility = Visibility.Visible;
            }
            else
            {
                this.Hide();
            }   
        }

        private void Canvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void but1Joueur_Click(object sender, RoutedEventArgs e)
        {
            butFacile.Visibility = Visibility.Visible;
            butNormal.Visibility = Visibility.Visible;
            butDifficile.Visibility = Visibility.Visible;
            labAttentionJoueurs.Visibility = Visibility.Hidden;
            unJoueur = true;
            deuxJoueurs = false;
        }

        private void butFacile_Click(object sender, RoutedEventArgs e)
        {
            modeFacile = true;
            modeNormal = false;
            modeDifficile = false;
            labAttentionDifficulte.Visibility = Visibility.Hidden;
        }

        private void butNormal_Click(object sender, RoutedEventArgs e)
        {
            modeFacile = false;
            modeNormal = true;
            modeDifficile = false;
            labAttentionDifficulte.Visibility = Visibility.Hidden;
        }

        private void butDifficile_Click(object sender, RoutedEventArgs e)
        {
            modeFacile = false;
            modeNormal = false; 
            modeDifficile = true;
            labAttentionDifficulte.Visibility = Visibility.Hidden;
        }

        private void but2Joueurs_Click(object sender, RoutedEventArgs e)
        {
            unJoueur = false;
            deuxJoueurs = true;
            butFacile.Visibility = Visibility.Hidden;
            butNormal.Visibility = Visibility.Hidden;
            butDifficile.Visibility = Visibility.Hidden;
            labAttentionJoueurs.Visibility = Visibility.Hidden;
        }
    }
}
