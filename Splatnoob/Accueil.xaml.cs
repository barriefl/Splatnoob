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

namespace Splatnoob
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Window
    {
        public Accueil()
        {
            InitializeComponent();
        }
        
            private int bouton;

        public int Bouton
        {
            get { return bouton; }
            set { bouton = value; }
        }

    
        public void Button_parametre_Click(object sender, RoutedEventArgs e)
        {
            bouton = 1;
            this.Close();
            
        }
        public void Button_start_Click(object sender, RoutedEventArgs e)
        {
            bouton = 2;
            this.Close();
        }

        private void Canvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
