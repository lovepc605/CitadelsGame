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
using CitadelsSystem.Interface;

namespace CitadelsGame.PlayerWindow
{
    /// <summary>
    /// Interaction logic for YesOrNoWindow.xaml
    /// </summary>
    public partial class YesOrNoWindow : Window
    {
        public int Result = -1;

        public YesOrNoWindow()
        {
            InitializeComponent();
        }

        #region IChooseYesOrNo Members

        public int Choose(string title, CitadelsSystem.Utils.TwoChoiceEnum choice)
        {
            return 0;
        }

        #endregion

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            Result = 0;
            this.DialogResult = true;
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            Result = 1;
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = -1;
            this.DialogResult = true;
        }
    }
    public class YesOrNoUser:IChooseYesOrNo
    {
        #region IChooseYesOrNo Members

        public int Choose(string title, CitadelsSystem.Utils.TwoChoiceEnum choice)
        {
            YesOrNoWindow win = new YesOrNoWindow();
            win.tbTitle.Text = title;
            if(choice == CitadelsSystem.Utils.TwoChoiceEnum.Card_Money)
            {
                win.btnYes.Content = "卡片";
                win.btnNo.Content = "金幣";
            }
            else
            {
                win.btnYes.Content = "Yes";
                win.btnNo.Content = "No";
            }

            win.ShowDialog();
            return win.Result;
        }

        #endregion
    }

}
