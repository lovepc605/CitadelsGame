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
using System.Windows.Threading;
using CitadelsSystem.Interface;
using System.Threading;
using CitadelsSystem.Character;
using Windows;

namespace CitadelsGame.PlayerWindow
{

    public partial class CharacterWindow :Window
    {
        public Image Result = null;
        public ToolTipWindow tooltip = new ToolTipWindow();
        public CharacterWindow()
        {
            InitializeComponent();
        }

        public void SetData(List<CitadelsSystem.Character.Character> input )
        {
            
            //create options for user
            int row = 0, col = 0;
            for (int i = 0; i < input.Count; i++)
            {
                row = i/3;
                col = i%3;
                Image img = new Image();

                //binding image
                Binding binding = new Binding();
                binding.Source = input[i];
                binding.Path = new PropertyPath("HeadImg");
                binding.Mode = BindingMode.OneWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                img.SetBinding(Image.SourceProperty, binding);
                img.Margin = new Thickness(2);
                img.Tag = input[i];

                //add the row to the specific row and col
                grid1.Children.Add(img);
                Grid.SetColumn(img, col);
                Grid.SetRow(img, row);
                img.MouseLeftButtonDown += img_MouseLeftButtonDown;
                img.MouseEnter += new MouseEventHandler(img_MouseEnter);
                img.MouseLeave += new MouseEventHandler(img_MouseLeave);
            }
        }

        void img_MouseLeave(object sender, MouseEventArgs e)
        {
            tooltip.Visibility = Visibility.Hidden;
        }

        void img_MouseEnter(object sender, MouseEventArgs e)
        {
            //tooltip = new ToolTipWindow();

            tooltip.Visibility = Visibility.Visible;
            tooltip.WindowStartupLocation = WindowStartupLocation.Manual;
            tooltip.tbMessage.Text = ((sender as Image).Tag as Character).Desc;

            //Point p = (sender as Image).PointFromScreen(new Point(100, 100));
            tooltip.Height = 180;
            tooltip.Left = this.Left + this.Width - 200;
            tooltip.Top = this.Top + this.Height - tooltip.Height; 
            

        }

        void img_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowCardWindow win = new ShowCardWindow();
            
            win.SetData( ((sender as Image).Tag as Character));
            win.ShowDialog();
        }

        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Result = (sender as Image);
            this.DialogResult = true;
        }

    }

    public class CharacterUser :IChooseCharacter
    {
        #region IChooseCharacter Members

        public CitadelsSystem.Character.Character Choose(List<CitadelsSystem.Character.Character> input)
        {
            CharacterWindow win = new CharacterWindow();
            win.SetData(input );
            //win.Topmost = true;

            if (win.ShowDialog() == true)
            {

                CitadelsSystem.Character.Character c;
                c = (from img in input where img.HeadImg == win.Result.Source select img).ToArray()[0];
                //MessageBox.Show(c.ToString());
                return c;
            }
            return null;
        }

        #endregion
    }

}
