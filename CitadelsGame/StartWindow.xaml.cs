using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CitadelsSystem;
using CitadelsSystem.Utils;

namespace CitadelsGame
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if(rdbStd.IsChecked == true)
            {
                GameSystem.SelectCharEnum = GameCharEnum.Standard;
            }
            if (rdbExt.IsChecked == true)
            {
                GameSystem.SelectCharEnum = GameCharEnum.Extension;
            }
            if (rdbMix.IsChecked == true)
            {
                GameSystem.SelectCharEnum = GameCharEnum.Mix;
            }
            this.Visibility = Visibility.Hidden;

            MainWindow win =new MainWindow(this.tbName.Text);
            
            win.ShowDialog();

        }

        private void btnCredit_Click(object sender, RoutedEventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }
}
