   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ARWT.View.SettingsARWT.View.Settings  5   	   1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   1clr-namespace:ARWT.ConvertersARWT.Converters  5   	   %    ARWT.View.Settings.SettingsView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:ARWT.View.Settings     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	   4
convertersclr-namespace:ARWT.Converters     5   	     � Title$  SettingsView��5   	   $	��480��6   $	��640��6+   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5
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
�������$��1q�6(   $��2q�6<   $ 2q�6G   $��1��6S   $��1��6`   {�6p   �� $ 	ShowVideo�._�5>      � 	 5?      �� 5@      $
��Auto �6(   �� 5A      $
��Auto �6(   �� 5B      $
��Auto �6(   �� 5C      $
��Auto �6(   �� 5D      $
��Auto �6(   �� 5E      $
��Auto �6(   �� 5F      $
��Auto �6(   
 5H      �� 5I      $
��Auto �6+   �� 5J      $��* �6+   .+�5M      �� $��0��6    $��0��6-   $��Gap Distance: ��6=   �� 5N      $��0��6   $��1��6*    6:   �� $ GapDistance�$ TwoWay=� 6j   �� $ GapDistanceMin� 6�   �� $ GapDistanceMax��� 5P      $��1��6    $��0��6-   $��Binary Treshold: ��6=   �� 5Q      $��1��6   $��1��6*    6:   �� $ BinaryThreshold�$ TwoWay=� 6n   �� $ BinaryThresholdMin� 6�   �� $ BinaryThresholdMax��� 5S      $��2��6    $��0��6-   $��Binary Treshold 2: ��6=   �� 5T      $��2��6   $��1��6*    6:   �� $ BinaryThreshold2�$ TwoWay=� 6o   �� $ BinaryThreshold2Min� 6�   �� $ BinaryThreshold2Max��� 5V      $��Smooth Motion: ��6    $��3��67   $��0��6D   �� 5W      $��3��6T   $��1��6a   $��Center=�6q   $��2q�6�    d� 	IsChecked 6   �� $ SmoothMotion�$ TwoWay=�6�   �� 5Z      $��Frame Rate: ��6    $��4��64   $��0��6A   �� 5[      $��4��6K   $��1��6X   ��6   �� $ 	FrameRate�$ TwoWay=�6X   �� 5]      $��ROI: ��6    $��5��6-   $��0��6:   �� 5^       �� Orientation$ 
Horizontal=�6!   $��5��6:   $��1��6G   .+�5_      �� $	��Set��6!   ��6/   �� $ SetRoiCommand��� 5`      $��Remove��6!   ��62   �� $ RemoveRoiCommand�5a      �� 5c      $��
Whiskers: ��6    $��6��62   $��0��6?   �� 5d      $ 
Horizontal=�6!   $��6��6:   $��1��6G   .+�5e      �� $��Settings��6!   ��64   �� $ WhiskerSettingsCommand�5f      5g      5h      5i      �� 5k   
   $��1��6   $ 
Horizontal=�6"   $��Center=�6;   .+�5l      �� $��Ok��6   ��6"   �� $ 	OkCommand��� 5m      $��Cancel��6   ��6&   �� $ CancelCommand�5n      5o      5p      