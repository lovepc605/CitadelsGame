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
using CitadelsSystem.Card;
using CitadelsSystem.Character;

namespace CitadelsGame
{
    /// <summary>
    /// Interaction logic for ShowCardWindow.xaml
    /// </summary>
    public partial class ShowCardWindow : Window
    {
        public ShowCardWindow()
        {
            InitializeComponent();
        }

        //add the building into the grid 
        //setup the timer ?
        public void SetData(AbstractCard card)
        {
            this.Topmost = true;


            imgCard.Source = card.FrontImg;
            if ( string.IsNullOrEmpty(card.Desc))
                txbDoc.Text = "此卡片無說明";
            else
                txbDoc.Text = card.Desc;

            imgCard.MouseLeftButtonDown += new MouseButtonEventHandler(buildImg_MouseLeftButtonDown);
        }

        public void SetData(Character theChar)
        {
            this.Topmost = true;

            imgCard.Source = theChar.HeadImg;
            txbDoc.Text = theChar.Desc;

            imgCard.MouseLeftButtonDown += new MouseButtonEventHandler(buildImg_MouseLeftButtonDown);

        }

        void buildImg_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        

    }
}
