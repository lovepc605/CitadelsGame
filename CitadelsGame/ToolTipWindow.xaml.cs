using System;
using System.Windows;
using System.Windows.Input;

namespace Windows {

    public partial class ToolTipWindow : Window
    {

        public ToolTipWindow()
        {
            InitializeComponent();
        }

        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}