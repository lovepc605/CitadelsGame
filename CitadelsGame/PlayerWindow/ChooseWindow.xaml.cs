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

namespace CitadelsGame
{
    /// <summary>
    /// Interaction logic for ChooseWindow.xaml
    /// </summary>
    public partial class ChooseWindow : Window
    {
        public ChooseWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Media.Brush brush = Background;

            System.Windows.Media.SolidColorBrush scb = (System.Windows.Media.SolidColorBrush)brush;
            System.Drawing.Color color = System.Drawing.Color.White;
            if (scb != null)
                color = System.Drawing.Color.FromArgb(scb.Color.A, scb.Color.R, scb.Color.G, scb.Color.B);

            System.Drawing.Brush solidBrush = new System.Drawing.SolidBrush(color); 

            GifAnimationControl animatedImageControl =
                new GifAnimationControl(this, Properties.Resources.msn0, solidBrush);
            grid1.Children.Add(animatedImageControl);
            grid1.HorizontalAlignment = HorizontalAlignment.Left;
            grid1.VerticalAlignment = VerticalAlignment.Top;
            animatedImageControl.Margin = new Thickness(30, 10, 30, 10);
        }

    }
}
