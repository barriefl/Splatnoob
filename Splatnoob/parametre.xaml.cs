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
        private int TEMPSPARDEPLACAGE = 15;
        private int NBRSECMIN = 60;

        public parametre()
        {
            InitializeComponent();
            dispatcherTimer.Tick += actualisation;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(16);
            dispatcherTimer.Start();
        }
        private void actualisation(object sender, EventArgs e)
        {
            double tpsTotal = TEMPSPARDEPLACAGE * Math.Round(choisirTps.Value);
            double tpsMin = Math.Truncate(tpsTotal / NBRSECMIN);
            double tpsSec = (tpsTotal - (tpsMin * NBRSECMIN));
            if (tpsSec != 0)
                affichTps.Content = (tpsMin + ":" + tpsSec);
            else
                affichTps.Content = (tpsMin + ":00");
                    
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Canvas_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
