   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:ARWT.View.NewSessionARWT.View.NewSession  5   	   1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   0    *ARWT.View.NewSession.NewWhiskerSessionView   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:ARWT.View.NewSession     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	     � Title$  Whisker Settings��5
   	   $	��480��6"   $	��640��6/    � WindowState$ 	Maximized=�6;   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5	   	   ��  �� Path$ Close� �� Mode$ TwoWay=� � 	Resources 5       �� 
TargetType �� Property �� Value��%�   '��       '�� G     �� 5   
   " ��6   .�5      ��  � ��$ 5q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$ 5q�6'   5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   
 � ColumnDefinitions
 5      �� 5      $��* �6#   �� 5      $��* �6#   .+�5       �� $��Whisker Settings��6   $��0��61   $��5q�6A   .��5!      � 	 5"      �� 5#      $
��Auto �6(   �� 5$      $
��Auto �6(   �� 5%      $
��Auto �6(   �� 5&      $
��Auto �6(   �� 5'      $
��Auto �6(   �� 5(      $
��Auto �6(   �� 5)      $
��Auto �6(   
 5+      �� 5,      $
��Auto �6+   �� 5-      $��* �6+   .+�50      �� $��Crop Scale Factor: ��6    $��0��6;   $��0��6H   �� 51      $��0��6D   $��1��6Q   ��6   �� $ CropScaleFactor�6Q   �� 53      $(��"Resolution Increase Scale Factor: ��6    $��1��6J   $��0��6W   �� 54      $��1��6R   $��1��6_   ��6   �� $# ResolutionIncreaseScaleFactor�6_   �� 56      $��Center=�6    $��Interpolation Type: ��6;   $��2��6W   $��0��6d   �� 57      $��2��6   $��1��6,   $��5q�6�   $��-2q�6�    �� SelectedItem 6<   �� $ InterpolationType�$ TwoWay=���6y   �� $ InterpolationTypes�6�   �� 59      $��Remove Duds:��6    $��3��64   $��0��6A   6S   �� 5:      $��3��6R   $��1��6_   $��5q�6o    d� 	IsChecked 6   �� $ 
RemoveDuds�$ TwoWay=�6o   �� 5<      $!��Minimum Whisker Threshold: ��6    $��4��6C   $��0��6P   �� 5=      $��4��6B   $��1��6O   ��6   �� $ LineThreshold�6O   �� 5?      $��Lower Bound: ��6    $��5��65   $��0��6B   �� 5@      $��5��6?   $��1��6L   ��6   �� $ 
LowerBound�6L   �� 5B      $��Upper Bound: ��6    $��6��65   $��0��6B   �� 5C      $��6��6?   $��1��6L   ��6   �� $ 
UpperBound�6L   5D      5E      �� 5G      $��1��69   ��6   �� $ DisplayImage�69   5H      �� 5J   
   $��1��6   $��5q�6    � Minimum$ 0Q�6)    � Maximum 65   �� $ MaxFrame� � Value 6W   �� $ FrameNumber�$ TwoWay=��� 5L   
    �� Orientation$ 
Horizontal=�6   $��2��6.   $��Center=�6;   .+�5M      �� $��Preview��6   ��6'   �� $ PreviewCommand��� 5O      $��Ok��6   ��6"   �� $ 	OkCommand�5P      5Q      5R      