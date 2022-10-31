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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CitadelsSystem.Players;
using CitadelsSystem.Utils;
using CitadelsSystem.Card;
using CitadelsSystem;
using CitadelsGame.PlayerWindow;

namespace CitadelsGame
{
    /// <summary>
    /// Interaction logic for PlayerArea.xaml
    /// </summary>
    public partial class PlayerArea : UserControl
    {
        Image[] imgBuidling = new Image[8];

        public PlayerArea()
        {
            
            InitializeComponent();

            InitializeUI();
        }

        private void InitializeUI()
        {
            
            //set the pics for player board


            BitmapSource coin = ResourceHelper.LoadBitmap("Coin");
            BitmapSource handCard = ResourceHelper.LoadBitmap("HandCard");
            imgCoin.Source = coin;
            imgHandCard.Source = handCard;

            //設定&初始 PlayerArea UI 連結 
            #region set Img
            {
                int row = 0, col = 0;
                for(int i =0 ; i < 8 ; i++)
                {
                    row = i / 4 ;
                    col = i % 4;
                    imgBuidling[i] = new Image();
                    //add the row to the specific row and col
                    gridBuilding.Children.Add(imgBuidling[i]);
                    Grid.SetColumn(imgBuidling[i], col);
                    Grid.SetRow(imgBuidling[i], row);

                }
            }
            #endregion

            btnHandCard.Click += new RoutedEventHandler(btnHandCard_Click);
        }

        //show the user's handcard , when button click
        void btnHandCard_Click(object sender, RoutedEventArgs e)
        {
            

            CardWindow win = new CardWindow();
            win.SetData(0, GameSystem.TheRealPlayer.HandCards, null);
            win.ShowDialog();

        }

        public void SetBinding(AbstractPlayer input)
        {
            #region Binding Name,Money,Handcard ,crown , color 
            //1.Name
            Binding b1 = new Binding();
            b1.Source = input;
            b1.Path = new PropertyPath("Name");
            b1.Mode = BindingMode.OneWay;
            b1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.tbPlayerName.SetBinding(TextBlock.TextProperty, b1);

            //2.Money
            Binding b2 = new Binding();
            b2.Source = input;
            b2.Path = new PropertyPath("Money");
            b2.Mode = BindingMode.OneWay;
            b2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.tbMoneyCount.SetBinding(TextBlock.TextProperty, b2);

            //3.Handcard Count
            Binding b3 = new Binding();
            b3.Source = input;
            b3.Path = new PropertyPath("HandCardCount");
            b3.Mode = BindingMode.OneWay;
            b3.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.tbHandCardCount.SetBinding(TextBlock.TextProperty, b3);

            //4.HasCrown
            Binding b4 = new Binding();
            b4.Source = input;
            b4.Path = new PropertyPath("ImgCrown");
            b4.Mode = BindingMode.OneWay;
            b4.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.imgCrown.SetBinding(Image.SourceProperty, b4);
            
            //5.HasFiveColor
            Binding b5 = new Binding();
            b5.Source = input;
            b5.Path = new PropertyPath("ImgFiveColor");
            b5.Mode = BindingMode.OneWay;
            b5.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.imgRainbow.SetBinding(Image.SourceProperty, b5);
            #endregion

            #region binding Building Image
            {
                Binding bI0 = new Binding();
                bI0.Source = input;
                bI0.Path = new PropertyPath("ImgBuilding0");
                bI0.Mode = BindingMode.OneWay;
                bI0.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                imgBuidling[0].SetBinding(Image.SourceProperty, bI0);
                imgBuidling[0].MouseDown += new MouseButtonEventHandler(PlayerArea_MouseDown);

                Binding bI1 = new Binding();
                bI1.Source = input;
                bI1.Path = new PropertyPath("ImgBuilding1");
                bI1.Mode = BindingMode.OneWay;
                bI1.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                imgBuidling[1].SetBinding(Image.SourceProperty, bI1);
                imgBuidling[1].MouseDown += new MouseButtonEventHandler(PlayerArea_MouseDown);


                Binding bI2 = new Binding();
                bI2.Source = input;
                bI2.Path = new PropertyPath("ImgBuilding2");
                bI2.Mode = BindingMode.OneWay;
                bI2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                imgBuidling[2].SetBinding(Image.SourceProperty, bI2);
                imgBuidling[2].MouseDown += new MouseButtonEventHandler(PlayerArea_MouseDown);


                Binding bI3 = new Binding();
                bI3.Source = input;
                bI3.Path = new PropertyPath("ImgBuilding3");
                bI3.Mode = BindingMode.OneWay;
                bI3.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                imgBuidling[3].SetBinding(Image.SourceProperty, bI3);
                imgBuidling[3].MouseDown += new MouseButtonEventHandler(PlayerArea_MouseDown);


                Binding bI4 = new Binding();
                bI4.Source = input;
                bI4.Path = new PropertyPath("ImgBuilding4");
                bI4.Mode = BindingMode.OneWay;
                bI4.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                imgBuidling[4].SetBinding(Image.SourceProperty, bI4);
                imgBuidling[4].MouseDown += new MouseButtonEventHandler(PlayerArea_MouseDown);


                Binding bI5 = new Binding();
                bI5.Source = input;
                bI5.Path = new PropertyPath("ImgBuilding5");
                bI5.Mode = BindingMode.OneWay;
                bI5.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                imgBuidling[5].SetBinding(Image.SourceProperty, bI5);
                imgBuidling[5].MouseDown += new MouseButtonEventHandler(PlayerArea_MouseDown);

                Binding bI6 = new Binding();
                bI6.Source = input;
                bI6.Path = new PropertyPath("ImgBuilding6");
                bI6.Mode = BindingMode.OneWay;
                bI6.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                imgBuidling[6].SetBinding(Image.SourceProperty, bI6);
                imgBuidling[6].MouseDown += new MouseButtonEventHandler(PlayerArea_MouseDown);


                Binding bI7 = new Binding();
                bI7.Source = input;
                bI7.Path = new PropertyPath("ImgBuilding7");
                bI7.Mode = BindingMode.OneWay;
                bI7.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                imgBuidling[7].SetBinding(Image.SourceProperty, bI7);
                imgBuidling[7].MouseDown += new MouseButtonEventHandler(PlayerArea_MouseDown);


                
            }

            #endregion

        }

        void PlayerArea_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Image img = (sender as Image);
            if(img.Source!=null)
            {
                //find the AbstractCard from GameSystem.AllCard
                AbstractCard card = (from c in GameSystem.AllCards where c.FrontImg == img.Source select c).ToArray()[0];

                ShowCardWindow win = new ShowCardWindow();
                win.SetData(card);
                win.ShowDialog();
            }
        }


    }
}
