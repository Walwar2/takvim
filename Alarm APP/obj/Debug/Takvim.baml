   M S B A M L   `   `   `  ����D  @Alarm APP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Alarm_APP	Alarm_APP  5   	       Alarm_APP.Takvim   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Alarm_APP     5   	     � Title$  Takvim��5   	   $	��650��6   $	��800��6%   .��5	      � ���#F0F0F0 � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��10q�6   .+�5      &   System.Windows.Controls.Calendar  -   5        $��	cldSample��5         SelectionMode$ MultipleRange=�6(   
����������#CCCCCC$��1q�6o   �� 5      $��Seçilen tarihler:��6   $��16(�61   $
��Bold"�6?   ���#333333$��0,10,0,5q�6f   �� 5      $	��150��6X   
����������#CCCCCC$��1q�6�   ��6   ��  �� ElementName$ 	cldSample�� �� Path$ SelectedDates�6�   �� 5       �� Orientation$ 
Horizontal=�6   $��Center=�62   $
��0,10q�6O   .+�5      	� -   5         	�Name$ radioButton1��5      $��	Toplantı��64    	� 	GroupName$ OlayTipi��6G   $��5q�6\   	� 5      -   5        $ radioButton2��5      $��Dersler��64   $ OlayTipi��6F   $��5q�6[   	� 5      -   5        $ radioButton3��5      $��İşler��64   $ OlayTipi��6D   $��5q�6Y   5      �� 5      -   5         ��Name$ setAlarmButton��5      $��	Alarm Kur��6-   $	��120��6^   $��40��6j   ���#007ACC
�������$��0,20,0,0q�6�   �� 5      -   5        $ 	alarmEdit��5      $��Alarm Düzenle��6(   $	��120��6X   $��40��6d   ���#4CAF50
�������$��0,10,0,0q�6�   �� 5!      -   5        $ testAlarmButton��5!      $��Alarmı Test Et��6.   $	��150��6e   $��40��6q   ���#E65100
�������$��0,10,0,0q�6�   5"      5#      5$      