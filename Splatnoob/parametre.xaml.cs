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
            double tps_total = TEMPSPARDEPLACAGE * Math.Round(choisirtps.Value);
            double tps_min = Math.Truncate(tps_total / NBRSECMIN);
            double tps_sec = (tps_total - (tps_min * NBRSECMIN));
            affichtps.Content = (tps_min + ":" + tps_sec);
        }
    }
}
