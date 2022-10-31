using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Character
{
    public class CharacterFactory
    {


        public static Character Create(string resource)
        {
            //ID	Name	Color	Excutable	Func	FileName	Desc;

            try
            {
                string[] data = resource.Split(',');

                Character ch = new Character();
                //1.basic fields
                ch.Id = int.Parse(data[0]);
                ch.Name = data[1];
                ch.TaxColor = Color.FromName(data[2]);
                ch.ExcuteTiming = (ExcuteEnum)Enum.Parse(typeof(ExcuteEnum), data[3]);
                string[] temp = data[4].Split('_');
                ch.Function = temp[0];
                ch.SelectType = temp[1];
                ch.Desc = data[6];
                //2.set image
                ch.HeadImg = ResourceHelper.LoadBitmap(data[5]);

                
                return ch;
            }
            catch
            {
                throw new Exception("Load character fail");
                
            }
        }
    }
}
