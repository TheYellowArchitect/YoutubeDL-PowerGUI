   M S B A M L   `   `   `  ����M  IYoutubeDL PowerGUI, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	        WpfApp1.YoutubeDLGUIWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  YoutubeDL PowerGUI��5   	   $	��300��6$   $	��700��61   $y�Visible=�6=   ���	#FFFF2C2C
�������.��5
      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��0.4* �6   �� 5      $
��0.3* �6   �� 5      $
��0.5* �6   .+�5   
   �� -   5        $��DataGridBorder��5      $��0��6'   
���   �$��1q�6I   $
��Left=�6]   $	��Top=�6x   $
��auto��6�   $��	10,10,0,0q�6�   $
��auto��6�   ._�5      &   System.Windows.Controls.DataGrid  -   5        $��DownloadGrid��5         AutoGenerateColumns .     
IsReadOnly . $	��Top=�5      $��Stretch=�6)      GridLinesVisibility$ 
Horizontal=�6G      HeadersVisibility$	 All=�6h      RowBackground �#0c0c0c   AlternatingRowBackground �#191919
�������   RowHeaderWidth$ 0Q�5      $��00q�6$   $	   HorizontalScrollBarVisibility$	 Disabled=�60   "
   VerticalScrollBarVisibility$

 Auto=�5       $	��192��64   $y�	Collapsed=�5!         ColumnHeaderStyle 5$      �� 5%       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6    .�5&      ��  �� Property � �� �� Value$ 35��67   �� 5'       � ��$ 5q�68   �� 5(       � �� �#9900006;   �� 5,       � ��
 �����6;   5-         Columns 55      0  *System.Windows.Controls.DataGridTextColumn  56      ,  &System.Windows.Controls.DataGridColumn   Header$ Title��6-   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *Title+  57      $	 URL��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$	 280 6R    6:   �� *URL+6R     58      $ 	Size (MB)��6-    6@   �� *Size+  59      $ Progress��6-    6?   �� *
Progress+  5:      $ Time Remaining��6-    6E   �� *TimeRemaining+  5;      $ Download Speed (MB)��6-    6J   �� *DownloadSpeed+5=      5B      �� 5G   
   $��1��6    �� Orientation$ Vertical=�6"   $��Center=�69   $��Center=�6V   .+�5H      �� -   5        $��IntroDownloadLabel��5H      $/��)Enter the video link you wish to download��6.   $��Center=�6b   $��Center=�6   $��15(�6�   $��NSimSun)�6�   $
��Bold"�6�   �� 5J      $ 
Horizontal=�6   .+�5K      �� -   5        $��CommandPromptTextBox��5K      $��Center=�66   $��Center=�6S   $��22��6n   $	��350��6{   $��Lucida Console)�6�   
���   �
�������$
��Bold"�6�    �� MaxLines$ 1��6�   $��0��6�   6	  �� 5L      -   5        $��EnterButton��5L      $��Center=�6,   $��Center=�6I   $��	-2,-2,0,0q�6d   $��22��6x   $��22��6�      CustomButtonStyle#��[  6�   .��5M      �� $��download.png��6   $��Center=�62   $
��Left=�6M    � ToolTip 5O      _� 5P      $M��GLiterally the same as pressing Enter - Not everyone is a keyboard user.��6"   5R      5S      5V      �� 5_   
   $ Vertical=�6   $��2��6,   $	��Top=�69   .+�5b      �� -   5        $��RenameTextBoxBorder��5b      ���#990000$��3q�6F    �� CornerRadius$ 4��6Z   $��Center=�6k   $	��Top=�6�   ._�5c      �� -   5        $��RenameTextBox��5c      $��Center=�6/   $��Center=�6L   $��0��6g   $��0��6r   $ 1��6|   $��0q�6�   6�   5d      �� 5g      $ 
Horizontal=�6   $��Center=�62   $	��Top=�6O   .+�5i      �� -   5        $��DownloadLabel��5i   3   $
��Left=�6   $��
Download: ��6H   $	��Top=�6]   $��
-92,-3,0,0q�6v   $��NSimSun)�6�   $
��Bold"�6�   $��14(�6�   �� 5k      -	   5        $��ButtonPanel��5k      $ 
Horizontal=�62   $
��Left=�6K   .+�5m      �� -
   5        $��DownloadVideoButton��5m      $��_Video��68   $��Center=�6I   $��0,0,0,0q�6f   $	��Top=�6w   $��75��6�   $��1��6�   ���	#FF440000 5n      _� 5o      .��6&   hfThis button can even download entire Playlists and Channels!! It always downloads them in .mp4 format!5r      5t      �� 5v      -   5        $��DownloadMusicButton��5v      $��_Music��68   $��Center=�6I   $��20,0,0,0q�6f   $	��Top=�6x   $��75��6�   $��2��6�   ���	#FF440000 5w      _� 5x      .��6&   " Auto-Converts the link onto .mp35z      5|      �� 5~      -   5        $��DownloadCustom1��5~      $��_Custom��64   $��Center=�6F   $��20,0,0,0q�6c   $	��Top=�6u   $��75��6�   $��3��6�   ���	#FF440000 5      _� 5�      .��6&   ��The options/filters you have added onto the text bar, are PERMANENTLY stored for future use, so you won't have to type that again!5�      5�      5�      5�      �� 5�      $ 
Horizontal=�6   $
��Left=�62   $	��Top=�6M   $��20��6e   $��20q�6q   .+�5�      �� -   5        $��TestOpenDialogueText��5�      $	��115��68   6G   5�      5�      �� 5�   
   $��3��6   $	��Top=�6"   $ 
Horizontal=�6:   .+�5�      �� $ Vertical=�6   $	��Top=�60   .+�5�      �� -   5        $��SaveDialogueButton��5�      $	��120��63   $
��auto��6B   $��40��6O   
������ 
������ $��4��6�   ��5�      �� 5�      $	��Top=�6%   $��Center=�6=   $ Vertical=�6Z   
������ .+�5�      �� $	��Top=�6)   $��Center=�6A   $ 
Horizontal=�6^   
������ $��20��6�   .+�5�   "   �� $��137.ico��6(   $��Center=�69   $
��Left=�6T   �� 5�   "   $��Save Location��6(   $��Center=�6@   $
��Left=�5�   $   $��1q�6?   
�������$��10(�5�   6   $
��Bold"�6D   5�      �� 5�      -   5        $��YoutubeDLSavePathUI��5�   (   $��C:\YoutubeDLVideos��6C   $	��Top=�6]   $
��Left=�6u   $��8(�6�   5�       5�      �� 5�      $ Vertical=�6%   .+�5�      �� .��6$   ?=Where should videos (or whatever else you download) be saved?6c   �� 5�      .��6$   Click to set that location!6A   5�      5�      5�      �� 5�      .+�5�      �� -   5        $��OutputLogMinimizeButton��5�      $��15��68   $��15��6D   $��-��6O   $��8(�6[   $
��Bold)�6h    K� ZIndex$ 2��6�   $��5��6�    5�      _� 5�      .��6"   GEMinimize the Console Log! You are unlikely to need its output anyway!5�      5�      �� 5�      -   5        $��OutputLogBorder��5�      
�����w�$��3q�6M   $ 2��6a   $��Center=�6r   $	��Top=�6�   $	��500��5�      $
��auto��6%   $	��100��62   $ 1��6?   ._�5�      �� $
��Auto=�6#   $
��Auto=�6F   .��5�      �� -   5         ��Name$ 	OutputLog��5�   $   5�      5�      5     5     5	     �� 5  
   -   5        $��
HelpButton��5     $��3��6   $
��auto��60   $��30��6=   
������ $��Right=�6|   $��Bottom=�6�   #��[  6�    J� Opacity$	 0.8Q�6�   .��5     �� $��Help.png��6   $��Center=�6&   $��15��6A   $��15��6M    5     �� 5     $ Vertical=�6   .+�5     �� .��6   31You do not know what to type, aside of the video?6O   �� 5     .��6   86Click for a list of all commands - of all the choices!6T   5     5     5      5!     