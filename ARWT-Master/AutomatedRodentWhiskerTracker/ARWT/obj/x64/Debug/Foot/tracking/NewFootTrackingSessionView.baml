   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ARWT.Foot.trackingARWT.Foot.tracking  5      1clr-namespace:ARWT.BehavioursARWT.Behaviours  5      3    -ARWT.Foot.tracking.NewFootTrackingSessionView   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      2local clr-namespace:ARWT.Foot.tracking     5      4
behavioursclr-namespace:ARWT.Behaviours     5        � Title$  Foot Tracking Settings��5      $
��1080��6-   $
��1920��6;    � WindowState$ 	Maximized=�6H   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5      ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing 5      �� $ ClosingCommand� � 	Resources 5       �� 
TargetType �� Property	 �� Value��%�   '��       '�� G     �� 5   
   " ��6   .�5      ��  � ��$	 5q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$	 5q�6'   5      5      .��5      � 
 � RowDefinitions
 5   
   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $	��450 �6   �� 5      .+�5"   
   �� $��Foot Tracking Settings��6   .��5#      �� $��0��6   $
��Auto=�6+   $
��Auto=�6O   .��5$      � 
 5%      �� 5&      $
��Auto �6(   �� 5'      $
��Auto �6(   �� 5(      $
��Auto �6(   �� 5)      $
��Auto �6(   �� 5*      $
��Auto �6(   �� 5+      $
��Auto �6(   �� 5,      �� 5-      $
��Auto �6(    5/      �� 50      $
��Auto �6+   �� 51      $��* �6+   .+�54      �� $��0��6    $��0��6.   $��Downsample scale factor��6?   $��5q�6_   �� 55      $��0��6M   $��1��6Z   ��6   �� $ scaleFactor�$ TwoWay=�6Z   �� 57      $��1��6    $��0��6.   $'��!Erosion Kernel Size (must be odd)��6?   $��5q�6i   �� 58      $��1��6L   $��1��6Y   ��6   �� $ 
KernelSize�$ TwoWay=�6Y   �� 5:      $��3��6    $��0��6.   $��Contour Distance��6?   $��	5,0,5,4.8q�6X   $��17��6k   $��Bottom=�6w   �� 5;      $��3��6P   $��1��6]   ��6   �� $ contourDistance�$ TwoWay=�6]   �� 5=      $��2��6    $��0��6.   $��Erosions Iterations��6?   $��5q�6[   �� 5>      $��2��6R   $��1��6_   ��6   �� $ erosionIterations�$ TwoWay=�6_   �� 5@      $��4��6    $��0��6.   $��Area of Contour��6?   $��5q�6W   �� 5A      $��4��6   $��1��6+   $��5q�6=    � Value 6H   �� *area+$ TwoWay=� � Minimum 6m   �� *Settings.areaMin+ � Maximum 6�   �� *Settings.areaMax+�� 5C      $��8��6!   $��0��6/   $��2��6@    �� Orientation$ 
Horizontal=�6U   $��Center=�6o   .+�5D      �� $	��125��6!   $��Preview��6/   ��6B   �� $ PreviewCommand��� 5E      $	��125��6!   $��Ok��6/   ��6=   �� $ 	OkCommand��� 5F      $	��125��6!   $��Cancel��6/   ��6A   �� $ CancelCommand�5G      5H      5I      5J      �� 5K   
   
�������$��1q�6%   $��2q�6:    �� Padding$ 2q�6F   $��0��6S   $��1��6a   ._�5L      � 
 5M      �� 5N      �� 5O      $
��Auto �6$   .+�5Q      �� $��0��6   ��6&   �� $ DisplayImage��� 5R      $��Bottom=�6G   $
��Left=�6c   $��24(�6   
���  ����6   �� $ FrameNumberDisplay�6�   �� 5S      $��1��6    6'   �� $ Frame�$ TwoWay=� 6R   �� $ minFrame� 6u   �� $ maxFrame�5T      5U      5V      5W      