
namespace CitadelsSystem.Utils
{
    public enum ExcuteEnum
    {
        AtBegining,
        ByWill,
        None
    }

    public enum StateEnum
    {
        NotSelected,
        Selected,
        FaceDown,
        FaceUp
    }

    public enum PurpleCardAcviteEnum
    {
        None,
        AtDarwing,
        AfterDarwing,
        AtBuilding,
        AfterBuilding,
        Deconstructing,
        EndOfGame
    }

    public enum TwoChoiceEnum
    {
        Card_Money,
        Yes_No

    }
    //0:End ,1:TakeMoney ,2:TakeCard 
    //,3:Build ,4:Tax, 5:InvokeFunction ,6: purple
    public enum UserActionEnum
    {
        End =0,
        TakeMoney =1,
        TakeCard=2,
        Build =3 ,
        Tax =4 ,
        CharFunc=5,
        PurpleFunc=6,

    }

    public enum GameCharEnum
    {

        Standard,
        Extension,
        Mix

    }

}