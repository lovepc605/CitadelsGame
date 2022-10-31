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
using CitadelsSystem.Interface;
using CitadelsSystem.Players;

namespace CitadelsGame.PlayerWindow
{
    /// <summary>
    /// Interaction logic for TargetWindow.xaml
    /// </summary>
    public partial class TargetWindow : Window
    {
        public string Name = "";
        public Image Result;
        public TargetWindow()
        {
            InitializeComponent();
        }

        public void SetData(List<AbstractPlayer> input,string mode)
        {
            int row, col;
            //access the resource.xaml for the button style
            ResourceDictionary mystyles = new ResourceDictionary();;
            mystyles.Source = new Uri("Resources.xaml",
                UriKind.RelativeOrAbsolute);


            for (int i = 0; i < input.Count; i++ )
            {

                row = i / 3;
                col = i % 3;

                //add name into the grid , via the textblock

                Binding binding = new Binding();
                binding.Mode = BindingMode.OneWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                
                //Binding player name
                if (mode == "Player")
                {//Style="{DynamicResource OptionsButton}"
                    Button btn = new Button();
                    //btn.Style = mystyles["OptionsButton"] as Style;
                    binding.Source = input[i];
                    binding.Path = new PropertyPath("Name");
                    btn.Click += new RoutedEventHandler(btn_Click);
                    btn.SetBinding(ContentProperty, binding);
                    grid1.Children.Add(btn);
                    
                    Grid.SetColumn(btn, col);
                    Grid.SetRow(btn, row);

                    
                }
                else //binding character image
                {

                    Image img = new Image();
                    binding.Source = input[i].CurrentChar;
                    binding.Path = new PropertyPath("HeadImg");
                    img.SetBinding(Image.SourceProperty, binding);
                    img.Margin = new Thickness(2);
                    img.SetBinding(Image.SourceProperty, binding);
                    img.MouseLeftButtonDown += img_MouseLeftButtonDown;
                    grid1.Children.Add(img);
                    Grid.SetColumn(img, col);
                    Grid.SetRow(img, row);
                    
                }


            }
        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            Name = (sender as Button).Content.ToString();
            DialogResult = true;
        }

        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Result = (sender as Image);
            DialogResult = true;
        }

        
    }

    public class TargetUser:IChooseTarget
    {
        
        #region IChooseTarget Members
        
        public AbstractPlayer Choose(List<AbstractPlayer> input)
        {
            string mode = (GameSystem.CurrentCharacter.Id <= 2) ? "Char" : "Player";

            TargetWindow win = new TargetWindow();

            win.SetData(input,mode);
            win.ShowDialog();

            AbstractPlayer p;
            if (mode == "Char")
            {
                
                p = (from i in input where i.CurrentChar.HeadImg == win.Result.Source select i).ToArray()[0];
                //MessageBox.Show(c.ToString());
                
            }
            else// player
            {
            
                p = (from i in input where i.Name == win.Name select i).ToArray()[0];
            }
            return p;
        }

        #endregion
    }
}
