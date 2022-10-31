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
using CitadelsSystem.Card;
using Windows;

namespace CitadelsGame.PlayerWindow
{
    /// <summary>
    /// Interaction logic for CardWindow.xaml
    /// </summary>
    public partial class CardWindow : Window
    {

        public ToolTipWindow tooltip = new ToolTipWindow();

        CardUser user = null;
        private int quota = 0;

        public CardWindow()
        {
            InitializeComponent();
        }

        public void SetData(int m, List<AbstractCard> cards, CardUser user)
        {
            quota = m;
            this.user = user;
            int row = 0, col = 0;


            for (int i = 0; i < cards.Count; i++)
            {
                row = i / 3;
                col = i % 3;
                Image img = new Image();

                //binding image
                Binding binding = new Binding();
                binding.Source = cards[i];
                binding.Path = new PropertyPath("FrontImg");
                binding.Mode = BindingMode.OneWay;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                img.SetBinding(Image.SourceProperty, binding);
                img.Margin = new Thickness(2);
                img.Tag = cards[i];

                //add the row to the specific row and col
                grid1.Children.Add(img);
                Grid.SetColumn(img, col);
                Grid.SetRow(img, row);
                img.Tag = cards[i];
                img.MouseLeftButtonDown +=  img_MouseLeftButtonDown;
                img.MouseEnter+=new MouseEventHandler(img_MouseEnter);
                img.MouseLeave+=new MouseEventHandler(img_MouseLeave);
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
            tooltip.tbMessage.Text = ((sender as Image).Tag as AbstractCard).Desc;
            if (string.IsNullOrEmpty(tooltip.tbMessage.Text))
                tooltip.tbMessage.Text = "此卡片無說明";
            //Point p = (sender as Image).PointFromScreen(new Point(100, 100));
            tooltip.Height = 180;
            tooltip.Left = this.Left + this.Width - 200;
            tooltip.Top = this.Top + this.Height - tooltip.Height;

        }

        void img_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (user != null)
            {
                user.images.Add((sender as Image));
                (sender as Image).IsEnabled = false;
                (sender as Image).Visibility = Visibility.Hidden;
                //check
                if (user.images.Count >= quota)
                {
                    btnOK_Click(null, null);
                }
            }
            else
                btnOK_Click(null, null);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            
        }
    }

    public class CardUser: IChooseCard ,IChooseBuilding
    {
        public List<Image> images = new List<Image>();
        private List<AbstractCard> SelectCards = new List<AbstractCard>();
        private AbstractCard SelectCard = null;

        // n choose m card
        public AbstractCard[] Choose(string title, int m, List<AbstractCard> cards)
        {

            CardWindow win = new CardWindow();
            win.textBlock1.Text = title;// "請選擇想要的建築牌卡";
            win.SetData(m, cards, this);

            if (win.ShowDialog() == true)
            {
                //image to card
                SelectCards = (from c in cards from i in images where c.FrontImg == i.Source select c).ToList();
                
                return SelectCards.ToArray();

            }

            return null;
            
        }

        public AbstractCard ChooseBuilding_WarLord(List<CitadelsSystem.Card.AbstractCard> buildings)
        {
            CardWindow win = new CardWindow();
            win.textBlock1.Text = "請選擇想要拆除的建築";
            win.SetData(1, buildings, this);
            if (win.ShowDialog() == true)
            {
                SelectCard = (from b in buildings from i in images where b.FrontImg == i.Source select b).ToList()[0];
                return SelectCard;
            }
            return null;
        }

        public AbstractCard[] ChooseBuilding_Diplomat(List<CitadelsSystem.Card.AbstractCard> selfBuilding, List<CitadelsSystem.Card.AbstractCard> targetBuilding)
        {

            

            List<AbstractCard> result = new List<AbstractCard>();

            CardWindow win = new CardWindow();

            try
            {

                //choose building 1 
                win.textBlock1.Text = "請選擇自己的建築";
                win.SetData(1, selfBuilding, this);
                // select self building 
                if (win.ShowDialog() == true)
                {

                    SelectCard =
                        (from b in selfBuilding from i in images where b.FrontImg == i.Source select b).ToList()[0];

                    result.Add(SelectCard);
                }


                // select target building 
                // choose building2 
                win = new CardWindow();
                win.textBlock1.Text = "請選擇對方的建築";
                win.SetData(1, targetBuilding, this);
                // select self building 
                if (win.ShowDialog() == true)
                {
                    SelectCard =
                        (from b in targetBuilding from i in images where b.FrontImg == i.Source select b).ToList()[0];

                    result.Add(SelectCard);
                }
            }
            catch
            {
                return null;
            }
            return result.ToArray();

        }




    }
}
