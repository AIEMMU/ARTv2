   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ARWT.View.SettingsARWT.View.Settings  5   	   1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   1clr-namespace:ARWT.ConvertersARWT.Converters  5   	   1    +ARWT.View.Settings.BatchWhiskerSettingsView   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:ARWT.View.Settings     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	   4
convertersclr-namespace:ARWT.Converters     5   	     � Title$  SettingsView��5   	   $	��480��6   $	��640��6+   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5
   	   ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing 5   	   �� $ ClosingCommand� � 	Resources 5      2   ,ARWT.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter9   3ARWT.Converters.InverseBooleanToVisibilityConverter ' #InverseBooleanToVisibilityConverter �� 
TargetType �� Property �� Value��%U  &	        &	      '�� &     '�� m     '�� �       5   
   62     5   
   69   �� 5   
   " ��6   .�5      ��  � ��$ 2q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$ Center=�62   �� 5       � ��$ 1q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$ 1q�6'   5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5!   
   � $��0��6   	 5"      �� 5#      $��* �6    �� 5$      $
��Auto �6    
 � ColumnDefinitions
 5&      �� 5'      $��* �6#   �� 5(      $��* �6#   .+�5+      �� $��0��6   $��2��6$   $��0��65   $��2q�6�   ��6E   �� $
 Mice� �� SelectedItem 6g   �� $ SelectedMouse�$ TwoWay=� �� View 5,      �� 5-      .P�5.      �� $
��File��6)    �� Width$	 120��67    �� DisplayMemberBinding 6C   �� *VideoFileName+5/      51      �� 52      
�������$��1q�6(   $��2q�6<    �� Padding$ 2q�6G   $��0��6S   $��1��6a   ._�53      � $��2q�6v   y�6   �� $ 	ShowVideo� �� 	Converter# [  	 54      �� 55      $��* �6(   �� 56      $
��Auto �6(   .+�59      �� $��0��6   ��6)   �� $ DisplayImage��� 5:      $��1��6    � Value 6*   �� $ SliderValue�$ TwoWay=� � Minimum 6Z   �� $ Minimum� � Maximum 6{   �� $ Maximum�5;      5<      �� 5=      
�������$��1q�6(   $��2q�6<   $ 2q�6G   $��1��6S   $��1��6`   {�6p   �� $ 	ShowVideo�._�5>      �� $��Whisker Settings��6   $��0��65   $��5q�6E   .��5?      � 	 5@      �� 5A      $
��Auto �6,   �� 5B      $
��Auto �6,   �� 5C      $
��Auto �6,   �� 5D      $
��Auto �6,   �� 5E      $
��Auto �6,   �� 5F      $
��Auto �6,   �� 5G      $
��Auto �6,   
 5I      �� 5J      $��22.182 �6/   �� 5K      $��154.618 �6/   �� 5L      $��* �6/   .+�5O      �� $��Crop Scale Factor: ��6$   $��0��6?   $��0��6L   $��2��6\   $��1,1,0.8,0.6q�6p   �� 5P      $��0��6H   $��2��6U   $��0.2,0,-0.2,17.2q�6e   $��2��6~   ��6"   �� $ CropScaleFactor�6~   �� 5R      $(��"Resolution Increase Scale Factor: ��6$   $��1��6N   $��0��6[   $��2��6k   $��1,0.4,0.8,1.2q�6   �� 5S      $��1��6V   $��2��6c   $��0.2,0.4,-0.2,22.4q�6s   $��2��6�   ��6"   �� $# ResolutionIncreaseScaleFactor�6�   �� 5U      $��Center=�6$   $��Interpolation Type: ��6?   $��2��6[   $��0��6h   $��2��6x   $��1,3.8,0.8,3.4q�6�   �� 5V      $��2��6#   $��2��60   $��5.2,4.8,4.8,5.4q�6�   $��-2q�6�    6@   �� $ InterpolationType�$ TwoWay=���6}   �� $ InterpolationTypes�6�   �� 5X      $��Remove Duds:��6$   $��3��68   $��0��6E   $��2��6U   $��1,5.6,0.8,4.8q�6i   6�   �� 5Y      $��3��6V   $��2��6c   $��5.2,4.6,4.8,4.8q�6s    d� 	IsChecked 6#   �� $ 
RemoveDuds�$ TwoWay=�6s   �� 5[      $!��Minimum Whisker Threshold: ��6$   $��4��6G   $��0��6T   $��2��6d   $��1,1.2,0.8,0.4q�6x   �� 5\      $��4��6F   $��2��6S   $��0.2,0.2,-0.2,17q�6c   $��2��6|   ��6"   �� $ LineThreshold�6|   �� 5^      $��Lower Bound: ��6$   $��5��69   $��0��6F   $��2��6V   $��1,0.6,0.8,1q�6j   �� 5_      $��4��6C   $��2��6P   $��0.2,17.2,-0.2,0q�6`   $��2��6y   ��6"   �� $ 
LowerBound�6y   �� 5a      $��Upper Bound: ��6$   $��6��69   $��0��6F   $��2��6V   $��1,1,0.8,0.6q�6j   �� 5b      $��6��6C   $��2��6P   $��0.2,0,-0.2,-0.4q�6`   ��6"   �� $ 
UpperBound�6`   5c      5d      5e      5f      �� 5h   
   $��1��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   .+�5i      �� $��Ok��6   ��6"   �� $ 	OkCommand��� 5j      $��Cancel��6   ��6&   �� $ CancelCommand�5k      5l      5m      