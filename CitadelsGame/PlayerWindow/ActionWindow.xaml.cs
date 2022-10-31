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
using CitadelsSystem.Character;
using CitadelsSystem.Interface;

namespace CitadelsGame.PlayerWindow
{
    /// <summary>
    /// Interaction logic for ActionWindow.xaml
    /// </summary>
    public partial class ActionWindow : Window
    {
        public int playerAction =-1;
        
        public ActionWindow()
        {
            InitializeComponent();
        }



        private void btnTakeMoney_Click(object sender, RoutedEventArgs e)
        {

                playerAction = 1;
                DialogResult = true;
            
        }

        private void btnTakeCard_Click(object sender, RoutedEventArgs e)
        {

                playerAction = 2;
                DialogResult = true;
            
        }

        private void btnBuild_Click(object sender, RoutedEventArgs e)
        {

                playerAction = 3;
                DialogResult = true;
            
        }

        private void btnTax_Click(object sender, RoutedEventArgs e)
        {   

                playerAction = 4;
                DialogResult = true;
            
        }

        private void btnbtnCharFunc_Click(object sender, RoutedEventArgs e)
        {

                playerAction = 5;
                DialogResult = true;
            
        }

        private void btnPurple_Click(object sender, RoutedEventArgs e)
        {

                playerAction = 6;
                DialogResult = true;
            
        }

        private void btnEnd_Click(object sender, RoutedEventArgs e)
        {

                playerAction = 0;
                DialogResult = true;
            
        }

        public bool IsValid()
        {
            LinearGradientBrush brush = new LinearGradientBrush((Color)ColorConverter.ConvertFromString("Gray"),
                                                                (Color)ColorConverter.ConvertFromString("White"),
                                                                0.5
                );

            if (GameSystem.TheRealPlayer == GameSystem.CurrentPlayer)
            {
                Character c = GameSystem.CurrentCharacter;
                if (c.IsCardOrMoneyTaken)
                {
                    btnTakeMoney.IsEnabled = false;
                    btnTakeCard.IsEnabled = false;

                }

                if(c.IsBuilt)
                {
                    btnBuild.IsEnabled = false;
                }

                if(c.IsTaxed)
                {
                    btnTax.IsEnabled = false;
                }

                if(c.IsSpecialFuncUsed)
                {
                    btnCharFunc.IsEnabled = false;
                }
                


                return true;
            }
            return false;
        }

        private void EnableButtons(bool value)
        {
            btnTakeCard.IsEnabled = value;
            btnTakeMoney.IsEnabled = value;
            btnTax.IsEnabled = value;
            btnBuild.IsEnabled = value;
            btnCharFunc.IsEnabled = value;
            btnPurple.IsEnabled = value;
            btnEnd.IsEnabled = value;
        }
    }

    public class ActionUser:IChooseAction
    {

        #region IChooseAction Members

        private ActionWindow win;
        public int ChooseActionList()
        {

            win = new ActionWindow();
            if (win.IsValid())
            {
                win.ShowDialog();
                return win.playerAction;
            }
            return -1;
        }

        public void Reset()
        {
            win = new ActionWindow();
        }


        #endregion
    }
}
