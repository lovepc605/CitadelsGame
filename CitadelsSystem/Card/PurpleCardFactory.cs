using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Card
{
    public class PurpleCardFactory
    {
        
        public static PurpleCard[] Create(string resource)
        {
            List<PurpleCard> cards = new List<PurpleCard>();
            try
            {

                //Name,Point,Color,FileName,Number,Exec,Func,Desc;
                string[] data = resource.Split(',');

                for (int i = 0; i < int.Parse(data[4]); i++)
                {
                    PurpleCard card = new PurpleCard();
                    //1.basic fields
                    card.Name = data[0];
                    card.Point = int.Parse(data[1]);
                    card.CardColor = Color.FromName(data[2]);
                    card.Excutable = bool.Parse(data[5]);
                    card.Desc = data[7];

                    //2.set image
                    card.FrontImg = ResourceHelper.LoadBitmap(data[3]);

                    //3.
                    card.Function = data[6];

                    cards.Add(card);
                }
                return cards.ToArray();
            }
            catch
            {
                throw new Exception("Load PurpleCard fail");
                
            }

        }
    }
}
