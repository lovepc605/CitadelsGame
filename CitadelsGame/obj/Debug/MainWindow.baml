   M S B A M L   `   `   `  ????  CitadelsGame    CitadelsGame.MainWindow   5   	   ^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5        ? Title$  Citadels Test??5       ? 
ResizeMode$ NoResize=?6    ? WindowStyle$
 None=?61    ? WindowState$ 	Maximized=?6D    ? Topmost .  $??714.373??6l   $??824.362??6}   
???   ? ? WindowStartupLocation$ CenterScreen=?6?    +? IsFocusScope . |?. .??5      ?? -   5        $??windowFrame??5   O   
???????$??4q?6    $
??Auto??64   $
??Auto??6A    ?? CornerRadius$ 10??5      ??5   
   ?? 5	      ??5      ? ? 5       ? Color$ Black??6'   	 ? Offset$		 0.0Q?65   ? 5      $ #CEE3FF??6'   $		 0.5Q?67   ? 5      $
 Gray??6'   $		 0.9Q?64   5      5      ._?5   
   ? -   5        $??gridPlayerArea??5      
 ? RowDefinitions
 5   
   ?? 5      $??35 ?6   ?? 5      $??* ?6   ?? 5      $??* ?6   ?? 5      $??* ?6   ?? 5      $??* ?6    ? ColumnDefinitions 5   
   ?? 5      $
??0.3* ?6   ?? 5      $??1* ?6   ?? 5      $??1* ?6   ?? 5       $??0.42* ?6   .+?5$   
   ?? -   5        $??	gpxAction??5$      
?????? $??24(?6>   $??3??6L   $??5??6\   |?. $??3q?6~   
???????.??5%      ? -   5        $??
girdAction??5%      |?. 
 5&      ?? 5'      $??* ?6$   ?? 5(      $
??0.5* ?6$   ?? 5)      $
??0.5* ?6$   ?? 5*      $
??0.5* ?6$   ?? 5+      $
??0.5* ?6$   ?? 5,       5.      ?? 5/      $??* ?6'   ?? 50      $??* ?6'   .+?52      ?? -   5        $??imgMain??52   ,   $??2??6    ?? StretchDirection$
 Both=?6;   $
??Fill=?6S   $
??Auto??6b   $??Bottom=?6p   |?. 6?   ?? 53      -   5         ??Name$ busyAnimation??54         BusyAnimationStyle#??[  55      $??2??69   $??0,0,0,1q?6M   ?? 56      -   5        $??btnTakeMoney??56   :   $??0??6   $??1??6-     OptionsButton#???  6N   $??Stretch=?6v   .??6?   Money6?   ?? 57      -   5        $??btnTakeCard??57   :   $??0??6   $??2??6-   #???  6M   .??6?   Card6?   ?? 58      -	   5        $??btnTax??58   :   $??0??6   $??3??6-   #???  6H   $??Stretch=?6q   .??6?   Tax6?   ?? 59      -
   5        $??btnBuild??59   :   $??1??6   $??1??6-   #???  6J   .??6?   Build6?   ?? 5:      -   5        $??	btnPurple??5:   :   $??1??6   $??2??6-   #???  6K   $??Stretch=?6s   .??6?   Purple6?   ?? 5;      -   5        $??btnCharFunc??5;   :   $??1??6   $??3??6-   #???  6M   .??6?   Func6?   ?? 5<      -   5        $??btnEnd??5<   ?   $??2??6   $??4??62   #???  6M   .??6?   End6?   5=      5>      ?? 5@   
   -   5        $??txbRound??5@   $   $??2??6   $??20(?64   $??Right=?6B   $	??128??6^    J? BitmapEffect 5A      O? 5B       O? 	GlowColor$	 Red??6(   6:   5D      ?? 5E   
   -   5        $??tbPlayerWord??5E   '   
???????$?? ??6;   $??2??6C   $??1??6W   $??20(?6g    ?? TextAlignment$
 Left=?6u   $
??Wrap=?6?    5F      O? 5G      $ 	LightBlue??6(   6@   5J      ?? 5N      -   5        $??textBox1??5N   .   $??4??6   $??4q?6#   $ ?? HorizontalScrollBarVisibility$
 Auto=?6>   " ?? VerticalScrollBarVisibility$
 Auto=?6c   $??2??6?   $??2??6?   
???? ??$??1q?6?   
?????? $??20(?6?   6?   ? 5O      -   5        $??grid1??5O   !   $??1??6   $??3??6.   
 5P      ?? 5Q      $??35 ?6$   ?? 5R      $??35 ?6$   ?? 5S      $??35 ?6$   ?? 5T      $??35 ?6$   ?? 5U      $??35 ?6$   ?? 5V      $??35 ?6$   ?? 5W      $??35 ?6$   ?? 5X      $??35 ?6$   ?? 5Y      $??178.03* ?6$    5[      ?? 5\      $	??38* ?6'   .+?5^      ?? -   5        $??
textBlock1??5^      $??目前角色名稱：??6/   
???????6>   ?? 5_      -   5        $??
tbCharName??5_      $?? ??6/   
??? ???$??1??6I   $??20(?6V   $ Center=?6d   $
??Wrap=?6{   ?? 5`      -   5        $??
textBlock2??5`   /   
???????$??目前角色ID：??6A   $??2??6P   $
??Wrap=?6]   ?? 5a      -   5        $??tbCharID??5a   .   
??? ???$?? ??6>   $??3??6F   $??20(?6S   $ Center=?6a   $
??Wrap=?6x   ?? 5b      -   5        $??
textBlock3??5b   /   
???????$??目前玩家：??6A   $??4??6N   $
??Wrap=?6[   $??35??6o   $	??Top=?6{   ?? 5c      -   5        $??tbPlayerName??5c   .   
??? ???$?? ??6B   $??5??6J   $??20(?6W   $ Center=?6e   $
??Wrap=?6|   ?? 5d      -   5        $??
textBlock4??5d   1   
???????$??剩餘牌堆：??6C   $
??Wrap=?6P   $??6??6d   ?? 5e      -   5        $??
tbCardLeft??5e   .   
??? ???$?? ??6@   $??7??6H   $??20(?6U   $ Center=?6c   $
??Wrap=?6z   5f      ?? 5g      -   5        $??btnExit??5g      #???  6%   .??6c   離開6g   5h      5i      5j      