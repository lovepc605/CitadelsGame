using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CitadelsSystem.Players;
using CitadelsSystem.Character;
using Timer=System.Windows.Forms.Timer;


namespace CitadelsGame
{
    /// <summary>
    /// Interaction logic for ShowMessageWindow.xaml
    /// </summary>
    public partial class ShowMessageWindow : Window
    {
        private Timer timer;

        public ShowMessageWindow()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 2000;
            timer.Enabled = true;
            timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Dispose();
            this.DialogResult = true;
        }

        public void SetData(AbstractPlayer p ,string state)
        {
            Character c = p.CurrentChar;
            textBlock1.Text += p.Name;
            image1.Source = c.HeadImg;
            tbState.Text = state;
            timer.Start();
            this.ShowDialog();
        }

    }
}
