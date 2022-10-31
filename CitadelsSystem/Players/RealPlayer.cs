using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CitadelsSystem.Card;
using CitadelsSystem.Interface;
using CitadelsSystem.Utils;

namespace CitadelsSystem.Players
{
    public class RealPlayer:AbstractPlayer
    {
        public RealPlayer(string name)
        {
            this.Name = name;
            //GameSystem.AddSystemMessage(name + " 加入遊戲");
        }

        /*
        public class ChooseCardUser : IChooseCard
        {
            #region IChooseCardUI Members

            public AbstractCard[] Choose(int n, int m, List<AbstractCard> cards)
            {
                string userChoose = "";

                do
                {

                    Console.WriteLine("Choose {0} card(s) from {1} card(s)", m, n);
                    int i = 1;
                    foreach (AbstractCard c in cards)
                    {
                        Console.WriteLine(@"{0}.{1}", i, c);
                        i++;
                    }
                    Console.WriteLine("Input 0 to exit!");
                    userChoose = Console.ReadLine();
                    if (userChoose == "0")
                        return null;

                } while (string.IsNullOrEmpty(userChoose) || userChoose.Split(',').Length != m);


                List<AbstractCard> temp = new List<AbstractCard>();
                foreach (string str in userChoose.Split(','))
                {
                    int index = int.Parse(str);
                    if (index == -1)
                        break;

                    temp.Add(cards[index - 1]);
                }
                return temp.ToArray();
            }


            #endregion
        }

        public class ChooseCharacterUser : IChooseCharacter
        {

            #region IChooseCharacter Members

            public Character.Character Choose(List<CitadelsSystem.Character.Character> input)
            {



                return input[(int)GameSystem.RandomNum.NextDouble()*input.Count];
               

            }

            public Character.Character ChooseTarget(List<CitadelsSystem.Character.Character> input)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public class ChooseDestructionUser : IChooseBuilding
        {

            public Card.AbstractCard ChooseBuilding_WarLord(List<Card.AbstractCard> buildings)
            {
                throw new NotImplementedException();
            }

            public AbstractCard[] ChooseBuilding_Diplomat(List<AbstractCard> selfBuilding, List<AbstractCard> targetBuilding)
            {
                throw new NotImplementedException();
            }
        }

        public class ChooseTargetUser : IChooseTarget
        {

            #region IChooseTarget Members

            public CitadelsSystem.Players.AbstractPlayer Choose(List<CitadelsSystem.Players.AbstractPlayer> input)
            {
                return input[(int)(GameSystem.RandomNum.NextDouble()*123) % input.Count];
            }

            #endregion
        }

        public class ChooseYesOrNoUser : IChooseYesOrNo
        {

            #region IChooseYesOrNo Members
            // 0:means NO , 1 :means Yes ,Need a title for show the message
            public int Choose(string title , TwoChoiceEnum choice)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public class ChooseActionUser : IChooseAction
        {

            // 0:End ,1:TakeMoney ,2:TakeCard ,3:Build ,4:Tax, 5:InvokeFunction

            public int ChooseActionList()
            {
                return 0;
            }
        }
         * */
    }
    

}