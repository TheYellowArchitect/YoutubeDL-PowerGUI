   M S B A M L   `   `   `  ���� M  IYoutubeDL PowerGUI, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1       WpfApp1.YoutubeDLGUIWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp1       � Title$  YoutubeDL PowerGUI��$	��300��$	��700��$y�Visible=����	#FFFF2C2C
�������.���  � RowDefinitions �� $
��auto ��� $
��0.4* ��� $
��0.3* ��� $
��0.5* �.+��� -   $��DataGridBorder��$��0��
���   �$��1q�$
��Left=�$	��Top=�$
��auto��$��	10,10,0,0q�$
��auto��._�&   System.Windows.Controls.DataGrid  -   $��DownloadGrid��   AutoGenerateColumns .     
IsReadOnly . $	��Top=�$��Stretch=�   GridLinesVisibility$ 
Horizontal=�   HeadersVisibility$	 All=�   RowBackground �#0c0c0c   AlternatingRowBackground �#191919
�������   RowHeaderWidth$ 0Q�$��00q�$	   HorizontalScrollBarVisibility$	 Disabled=�"
   VerticalScrollBarVisibility$

 Auto=�$	��192��$y�	Collapsed=�   ColumnHeaderStyle ��  �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  .���  �� Property � �� �� Value$ 35����  � ��$ 5q���  � �� �#990000��  � ��
 �����   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Title��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Title+  $	 URL��   Width5  /System.Windows.Controls.DataGridLengthConverter$	 280  �� *URL+  $ 	Size (MB)�� �� *Size+  $ Progress�� �� *
Progress+  $ Time Remaining�� �� *TimeRemaining+  $ Download Speed (MB)�� �� *DownloadSpeed+�� $��1�� �� Orientation$ Vertical=�$��Center=�$��Center=�.+��� -   $��IntroDownloadLabel��$/��)Enter the video link you wish to download��$��Center=�$��Center=�$��15(�$��NSimSun)�$
��Bold"��� $ 
Horizontal=�.+��� -   $��CommandPromptTextBox��$��Center=�$��Center=�$��22��$	��350��$��Lucida Console)�
���   �
�������$
��Bold"� �� MaxLines$ 1��$��0���� -   $��EnterButton��$��Center=�$��Center=�$��	-2,-2,0,0q�$��22��$��22��   CustomButtonStyle#��[  .���� $��download.png��$��Center=�$
��Left=� � ToolTip _� $M��GLiterally the same as pressing Enter - Not everyone is a keyboard user.���� $ Vertical=�$��2��$	��Top=�.+��� -   $��RenameTextBoxBorder�����#990000$��3q� �� CornerRadius$ 4��$��Center=�$	��Top=�._��� -   $��RenameTextBox��$��Center=�$��Center=�$��0��$��0��$ 1��$��0q��� $ 
Horizontal=�$��Center=�$	��Top=�.+��� -   $��DownloadLabel��$
��Left=�$��
Download: ��$	��Top=�$��
-92,-3,0,0q�$��NSimSun)�$
��Bold"�$��14(��� -	   $��ButtonPanel��$ 
Horizontal=�$
��Left=�.+��� -
   $��DownloadVideoButton��$��_Video��$��Center=�$��0,0,0,0q�$	��Top=�$��75��$��1�����	#FF440000 _� .��hfThis button can even download entire Playlists and Channels!! It always downloads them in .mp4 format!�� -   $��DownloadMusicButton��$��_Music��$��Center=�$��20,0,0,0q�$	��Top=�$��75��$��2�����	#FF440000 _� .��" Auto-Converts the link onto .mp3�� -   $��DownloadCustom1��$��_Custom��$��Center=�$��20,0,0,0q�$	��Top=�$��75��$��3�����	#FF440000 _� .����The options/filters you have added onto the text bar, are PERMANENTLY stored for future use, so you won't have to type that again!�� $ 
Horizontal=�$
��Left=�$	��Top=�$��20��$��20q�.+��� -   $��TestOpenDialogueText��$	��115���� $��3��$	��Top=�$ 
Horizontal=�.+��� $ Vertical=�$	��Top=�.+��� -   $��SaveDialogueButton��$	��120��$
��auto��$��40��
������ 
������ $��4������ $	��Top=�$��Center=�$ Vertical=�
������ .+��� $	��Top=�$��Center=�$ 
Horizontal=�
������ $��20��.+��� $��137.ico��$��Center=�$
��Left=��� $��Save Location��$��Center=�$
��Left=�$��1q�
�������$��10(�$
��Bold"��� -   $��YoutubeDLSavePathUI��$��C:\YoutubeDLVideos��$	��Top=�$
��Left=�$��8(� �� $ Vertical=�.+��� .��?=Where should videos (or whatever else you download) be saved?�� .��Click to set that location!�� .+��� -   $��OutputLogMinimizeButton��$��15��$��15��$��-��$��8(�$
��Bold)� K� ZIndex$ 2��$��5�� _� .��GEMinimize the Console Log! You are unlikely to need its output anyway!�� -   $��OutputLogBorder��
�����w�$��3q�$ 2��$��Center=�$	��Top=�$	��500��$
��auto��$	��100��$ 1��._��� $
��Auto=�$
��Auto=�.���� -    ��Name$ 	OutputLog���� -   $��
HelpButton��$��3��$
��auto��$��30��
������ $��Right=�$��Bottom=�#��[   J� Opacity$	 0.8Q�.���� $��Help.png��$��Center=�$��15��$��15�� �� $ Vertical=�.+��� .��31You do not know what to type, aside of the video?�� .��86Click for a list of all commands - of all the choices!