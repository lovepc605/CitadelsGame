using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using IrrKlang;

namespace CitadelsSystem.Utils
{
    public class VoiceHelper
    {
        public static readonly string DirPath = AppDomain.CurrentDomain.BaseDirectory + @"Sounds\";
        private static ISoundEngine engine = new ISoundEngine();
        private static ISoundEngine engine2 = new ISoundEngine();


        /*
             Applaud.wav                                                                                     114 KB      Wave    音訊                             2009  /7 /2 下午     04 :20

    Background.wma                                                                                3,621 KB      Window Media Audio                     2009  /7 /2 下午     05 :00
    Build.wav                                                                                         10 KB     Wave    音訊                             2009  /7 /2 下午     04 :00
    Bye_Boy.wav                                                                                       17 KB     Wave    音訊                             2009  /7 /2 下午     04 :02

    Bye_Girl.wav                                                                                       9 KB     Wave    音訊                             2009  /7 /2 下午     04 :02
    Coins.wav                                                                                         77 KB     Wave    音訊                             2009  /7 /6 上午     10 :07

    DealCard.wav                                                                                       5 KB     Wave    音訊                             2009  /7 /2 下午     04 :00
    Destruction.wav                                                                                   28 KB     Wave    音訊                             2009  /7 /2 下午     03 :54

    Ding.wav                                                                                          25 KB     Wave    音訊                             2009  /7 /2 下午     04 :13
    Dock.wav                                                                                          12 KB     Wave    音訊                             2009  /7 /2 下午     04 :04
    GameBegin.wav                                                                                   130 KB      Wave    音訊                             2009  /7 /2 下午     04 :21

    Gun.wav                                                                                         442 KB      Wave    音訊                             2009  /7 /6 上午     10 :09
    HA.WAV                                                                                            15 KB     Wave    音訊                             1996  /1 /2 下午     11 :54

    Loading_1.wav                                                                                     95 KB     Wave    音訊                             2009  /7 /2 下午     04 :11
    Loading_2.wav                                                                                     19 KB     Wave    音訊                             2009  /7 /2 下午     04 :10
    lookgood.wav                                                                                      30 KB     Wave    音訊                             2009  /7 /2 下午     04 :27

    MouseClick.wav                                                                                    84 KB     Wave    音訊                             2008  /10 /10  下午     12  43
    MouseOver.wav                                                                                     23 KB     Wave    音訊                             2008  /10 /10  下午     12  43

    No_1.wav                                                                                          16 KB     Wave    音訊                             2009  /7 /2 下午     04 :03
    No_2.wav                                                                                           8 KB     Wave    音訊                             2009  /7 /2 下午     04 :03

    No_3.wav                                                                                           4 KB     Wave    音訊                             2009  /7 /2 下午     04 :04
    Play.wav                                                                                           9 KB     Wave    音訊                             2009  /7 /2 下午     04 :04
    Problem.wav                                                                                       38 KB     Wave    音訊                             2009  /7 /2 下午     04 :25

    Start.wav                                                                                       203 KB      Wave    音訊                             2006  /10 /19  下午     08  11
    Thank.wav                                                                                         11 KB     Wave    音訊                             2009  /7 /2 下午     04 :06

    Warning.wav                                                                                        9 KB     Wave    音訊                             2009  /7 /2 下午     04 :07
    YA.wav     
         */
        

        public static void AllPlayerSound(string name ,int time)
        {
            string filePath = GetFile(name);
            engine.Play2D(filePath);

        }


        public static string GetFile(string name)
        {
            string path = DirPath + name + @".wav";
            if(File.Exists(path))
            {
                return path;
            }
            path = DirPath + name + @".wma";
            if (File.Exists(path))
            {
                return path;
            }
            return "";
        }

    }
}
