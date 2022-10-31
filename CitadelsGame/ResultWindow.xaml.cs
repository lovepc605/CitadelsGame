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
using CitadelsSystem.Players;

namespace CitadelsGame
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow : Window
    {
        public ResultWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
        //add result data into the grid
        internal void SetData(List<AbstractPlayer> input)
        {
            int row = 1, col = 0;
            for(int i=0 ; i< input.Count ; i++)
            {
                AbstractPlayer p = input[i];
                TextBlock tb =null;
                
                //add name
                tb = new TextBlock();
                tb.Text = p.Name;
                grid1.Children.Add(tb);
                Grid.SetColumn(tb, 0);
                Grid.SetRow(tb, i+1);

                //add point 
                tb = new TextBlock();
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Text = p.CalculatePoints().ToString();
                grid1.Children.Add(tb);
                Grid.SetColumn(tb, 1);
                Grid.SetRow(tb, i + 1);

                //add building count

                tb = new TextBlock();
                tb.HorizontalAlignment = HorizontalAlignment.Center;
                tb.Text = p.Buildings.Count.ToString();
                grid1.Children.Add(tb);
                Grid.SetColumn(tb, 2);
                Grid.SetRow(tb, i + 1);
            }
        }
    }
}
