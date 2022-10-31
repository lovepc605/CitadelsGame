using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitadelsSystem.Card;

namespace CitadelsSystem.Interface
{
    public interface IChooseCard
    {  
        AbstractCard[] Choose(string title, int m, List<AbstractCard> cards);
        
    }
}
