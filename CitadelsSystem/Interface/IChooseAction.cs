using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CitadelsSystem.Card;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Interface
{
    public interface IChooseAction
    {


        /// <summary>
        /// 接Character傳回來的選項, 0:End ,1:TakeMoney ,2:TakeCard ,3:InvokeFunction ,4:Build
        /// </summary>
        /// <returns></returns>
        int ChooseActionList();



        void Reset();
    }
}
