using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;
using CitadelsSystem.Utils;
using CitadelsSystem.Properties;

namespace CitadelsSystem.Card
{
    public class SimpleCardFactory
    {

        public static SimpleCard[] Create(string resource)
        {
            List<SimpleCard> cards = new List<SimpleCard>();
            try
            {
                
                //Name,Point,Color,FileName,Number;
                string[] data = resource.Split(',');

                for(int i=0 ; i< int.Parse(data[4]) ; i++)
                {
                    SimpleCard card = new SimpleCard();
                    //1.basic fields
                    card.Name = data[0];
                    card.Point = int.Parse(data[1]);
                    card.CardColor = Color.FromName(data[2]);

                    //2.set image
                    card.FrontImg = ResourceHelper.LoadBitmap(data[3]);
                    cards.Add(card);
                }
                return cards.ToArray();
            }
            catch
            {
                throw new Exception("Load SimpleCard fail");
            }
        }
    }
}
