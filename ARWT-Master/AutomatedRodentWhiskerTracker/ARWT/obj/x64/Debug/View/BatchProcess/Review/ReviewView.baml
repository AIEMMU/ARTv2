   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   1clr-namespace:ARWT.ConvertersARWT.Converters  5   	   | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting 5   	   �rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization 5   	   .    (ARWT.View.BatchProcess.Review.ReviewView   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	   4
convertersclr-namespace:ARWT.Converters     5   	   �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   �visualizationToolkitrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   $
��1080��5
   ,   $
��1920��6:     � WindowState$  	Maximized=�6G   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5   	   ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing 5	   	   �� $ ClosingCommand� � Title 5
   	   �� $ WindowTitle� � 	Resources 5      2   ,ARWT.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter9   3ARWT.Converters.InverseBooleanToVisibilityConverter ' #InverseBooleanToVisibilityConverter �� 
TargetType �� Property	 �� Value  InvisibleDataPointB  <System.Windows.Controls.DataVisualization.Charting.DataPoint  InvisibleDataPoint2  InvisibleLegend6  0System.Windows.Controls.DataVisualization.Legend��%�  &	        &	      '�� &     '�� m     '�� �     &	 #    &	 �    &	 �      5   
   62     5   
   69   �� 5   
   " ��6   .�5      ��  � ��$	 2q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$	 Center=�62   �� 5       � ��$	 1q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$	 1q�6'   5      �� 5   
   "  6+   .�5      ��  � ��
	 ��  �6+   �� 5       � ��	 6)   Z� 5      �� 5   
   "  6,   .�5      ��  � ��
	 �  ��6+   �� 5        � ��	 6)   Z� 5!      �� 5"   
   "  6(   .�5#      ��  � ��$	 0��6&   5$      5%      .��5&      � 
 � RowDefinitions
 5'   
   �� 5(      $��* �6   �� 5)      $
��Auto �6   .+�5,   
   � $��0��6   
 5-      �� 5.      $��3* �6    �� 5/      $��10 �6    �� 50      $
��Auto �6     � ColumnDefinitions 52      �� 53      $��* �6#   �� 54      $��2* �6#   .+�59      �� 
�������$��1q�6(   $��2q�6<    �� Padding$ 2q�6G   $��0��6S   $��1��6a   ._�5:      � $��2q�6v   y�6   �� $ 	ShowVideo� �� 	Converter# [  
 5;      �� 5<      $��* �6(   �� 5=      $
��Auto �6(   .+�5@      �� $��0��6   ��6)   �� $ DisplayImage��� 5A      $��0��6I   $��Bottom=�6Y   $
��Left=�6t   $��24(�6�   
���  ����6    �� $ FrameNumberDisplay�6�   �� 5B      $��1��6    �� IsSelectionRangeEnabled . 
����  � � Value 6*   �� $ SliderValue�$ TwoWay=� � Minimum 6Z   �� $ Minimum� � Maximum 6{   �� $ Maximum� �� SelectionStart 6�   �� $ AnalyseStart�$ TwoWay=� �� SelectionEnd 6�   �� $ 
AnalyseEnd�$ TwoWay=�6+  5D      5E      �� 5F      $��0��6   $��1��6&   $��2q�6�   y�66   �� $ 	ShowVideo�# [ ��6�   �� $ DisplayText�6�   �� 5I      
�������$��1q�6(   $��2q�6<   $ 2q�6G   $��0��6S   $��0��6`   ._�5J      �� $
��Auto=�6   $
��Auto=�6D   .��5K      � y�6   �� $ 	ShowVideo�# [  
 5L      �� 5M      $
��Auto �6,   �� 5N      $
��Auto �6,   �� 5O      $
��Auto �6,   �� 5P      $
��Auto �6,   �� 5Q      $
��Auto �6,   �� 5R      $
��Auto �6,   �� 5S      $
��Auto �6,   �� 5T      $
��Auto �6,   �� 5U      $
��Auto �6,   �� 5V      $
��Auto �6,   �� 5W      $
��Auto �6,   �� 5X      $
��Auto �6,   �� 5Y      $
��Auto �6,   �� 5Z      $
��Auto �6,    5\      �� 5]      $
��Auto �6/   �� 5^      $��* �6/   .+�5a      �� $��0��6$   $��0��61   $��Gap Distance: ��6A   �� 5b      $��0��6!   $��1��6.    6>   �� $ GapDistance�$ TwoWay=� 6n   �� $ GapDistanceMin� 6�   �� $ GapDistanceMax�{�6�   �� $ VideoSelected��� 5d      $��1��6$   $��0��61   $��Binary Treshold: ��6A   �� 5e      $��1��6!   $��1��6.    6>   �� $ BinaryThreshold�$ TwoWay=� 6r   �� $ BinaryThresholdMin� 6�   �� $ BinaryThresholdMax�{�6�   �� $ VideoSelected��� 5g      $��2��6$   $��0��61   $��Binary Treshold 2: ��6A   �� 5h      $��2��6!   $��1��6.    6>   �� $ BinaryThreshold2�$ TwoWay=� 6s   �� $ BinaryThreshold2Min� 6�   �� $ BinaryThreshold2Max�{�6�   �� $ VideoSelected��� 5j      $��Smooth Motion: ��6$   $��3��6;   $��0��6H   �� 5k      $��3��6X   $��1��6e   $��Center=�6u   $��5q�6�    d� 	IsChecked 6#   �� $ SmoothMotion�$ TwoWay=�6�   �� 5n      $��Frame Rate: ��6$   $��4��68   $��0��6E   �� 5o      $��4��6O   $��1��6\   $��5q�6l   ��6"   �� $ 	FrameRate�$ TwoWay=�6l   �� 5q      $��Find Whiskers: ��6$   $��5��6;   $��0��6H   �� 5r      $��5��6#   $��1��60   $��5q�6x    6@   �� $ IncludeWhiskers�$ TwoWay=�6x   �� 5t      $��Find Feet: ��6$   $��6��67   $��0��6D   �� 5u      $��6��6#   $��1��60   $��5q�6t    6@   �� $ IncludeFeet�$ TwoWay=�6t   �� 5w      $��Whisker Settings��6#   $��7��6=   $��0��6J   $��2��6Z   $��5q�6n   y�6y   �� $ _IncludeWhiskers�# [   5x      ��%�   '��       '�� G     �� 5y   "   " ��6(   .�5z   &   ��  � ��$	 5q�6?   5{   #   �� 5|   "   " ��6(   .�5}   &   ��  � ��$	 5q�6?   5~   #   5      .��5�      � 
 5�   "   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64    5�   "   �� 5�   &   $
��Auto �67   �� 5�   &   $��* �67   .+�5�   "   �� $��Crop Scale Factor: ��6,   $��0��6G   $��0��6T   �� 5�   "   $��0��6P   $��1��6]   ��6*   �� $ CropScaleFactor�6]   �� 5�   "   $(��"Resolution Increase Scale Factor: ��6,   $��1��6V   $��0��6c   �� 5�   "   $��1��6^   $��1��6k   ��6*   �� $# ResolutionIncreaseScaleFactor�6k   �� 5�   "   $��Center=�6,   $��Interpolation Type: ��6G   $��2��6c   $��0��6p   �� 5�   "   $��2��6+   $��1��68   $��5q�6�   $��-2q�6�    �� SelectedItem 6H   �� $ InterpolationType�$ TwoWay=���6�   �� $ InterpolationTypes�6�   �� 5�   "   $��Remove Duds:��6,   $��3��6@   $��0��6M   6_   �� 5�   "   $��3��6^   $��1��6k   $��5q�6{    6+   �� $ 
RemoveDuds�$ TwoWay=�6{   �� 5�   "   $!��Minimum Whisker Threshold: ��6,   $��4��6O   $��0��6\   �� 5�   "   $��4��6N   $��1��6[   ��6*   �� $ LineThreshold�6[   �� 5�   "   $��Lower Bound: ��6,   $��5��6A   $��0��6N   �� 5�   "   $��5��6K   $��1��6X   ��6*   �� $ 
LowerBound�6X   �� 5�   "   $��Upper Bound: ��6,   $��6��6A   $��0��6N   �� 5�   "   $��6��6K   $��1��6X   ��6*   �� $ 
UpperBound�6X   �� 5�   "   $��Smoothing Function: ��6,   $��7��6H   $��0��6U   �� 5�   "   $��Repeat Smooths:��6,   $��8��6C   $��0��6P   �� 5�   "   $��5q�6+   $��7��66   $��1��6C    �� DisplayMemberPath$
 Name��6�   ��6S   �� $ SmoothingFunctions� 6�   �� $ SelectedSmoothingFunction�$ TwoWay=��� 5�   "   $��8��6�   $��1��6�   ��6*   �� $ RepeatSmooths�$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6�   5�      5�      �� 5�      $��Foot Settings��6#   $��8��6:   $��0��6G   $��2��6W   $��5q�6k   y�6v   �� $ IncludeFeet�# [   5�      ��%�   '��       '�� G     �� 5�   "   " ��6(   .�5�   &   ��  � ��$	 5q�6?   5�   #   �� 5�   "   " ��6(   .�5�   &   ��  � ��$	 5q�6?   5�   #   5�      .��5�      � 
 5�   "   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64   �� 5�   &   $
��Auto �64    5�   "   �� 5�   &   $
��Auto �67   �� 5�   &   $��* �67   .+�5�   "   �� $��0��6,   $��0��6:   $��Downsample scale factor��6K   $��5q�6k   �� 5�   "   $��0��6Y   $��1��6f   ��6*   �� $ scaleFactor�$ TwoWay=�6f   �� 5�   "   $��1��6,   $��0��6:   $'��!Erosion Kernel Size (must be odd)��6K   $��5q�6u   �� 5�   "   $��1��6X   $��1��6e   ��6*   �� $ 
KernelSize�$ TwoWay=�6e   �� 5�   "   $��3��6,   $��0��6:   $��Contour Distance��6K   $��	5,0,5,4.8q�6d   $��17��6w   $��Bottom=�6�   �� 5�   "   $��3��6\   $��1��6i   ��6*   �� $ contourDistance�$ TwoWay=�6i   �� 5�   "   $��2��6,   $��0��6:   $��Erosions Iterations��6K   $��5q�6g   �� 5�   "   $��2��6^   $��1��6k   ��6*   �� $ erosionIterations�$ TwoWay=�6k   �� 5�   "   $��5��6,   $��0��69   $��Contour Area��6I   �� 5�   "   $��5��6)   $��1��66    6F   �� $ areaTheshold�$ TwoWay=� 6w   �� $ areaThresholdMin� 6�   �� $ areaThresholdMax�{�6�   �� $ VideoSelected�5�      5�      �� 5�      $��10��6%   $��0��63   $��2��6C    �� Orientation$ 
Horizontal=�6W   $��Center=�6p   .+�5�      �� $��Preview��6%   ��67   �� $ PreviewCommand��� 5�      $��Review Data��6%   ��6;   �� $ ReviewCommand��� 5�      $��Review Feet��6%   ��6;   �� $ ReviewFootCommand�{�6f   �� *AreFootResults+$ TwoWay=�5�      �� 5�      $��11��6%   $��0��63   $��2��6C   $ 
Horizontal=�6W   $��Center=�6p   .+�5�      �� $��Center=�6%   $��Start��6B   ��6R   �� $ SetStartFrameCommand��� 5�      $��Center=�6%   $	��End��6B   ��6P   �� $ SetEndFrameCommand�5�      �� 5�      $��12��6%   $��0��63   $��2��6C   $ 
Horizontal=�6W   $��Center=�6p   .+�5�      �� $��Center=�6%   $��Re-Run��6B   ��6S   �� $ ReRunCommand��� 5�      $��Center=�6%   $
��Save��6B   ��6Q   �� $ SaveCommand��� 5�      $��Center=�6%   $��
Export Raw��6B   ��6W   �� $ ExportCSVCommand��� 5�      $��Center=�6%   $��Export Mean whisker��6B   ��6`   �� $ ExportMeanCSVCommand�5�      5�      5�      5�      5�      �� 5�   
   $��1��6   $ 
Horizontal=�6"   $��Center=�6;   .+�5�      �� $��Ok��6   ��6"   �� $ 	OkCommand��� 5�      $��Cancel��6   ��6&   �� $ CancelCommand�5�      5�      5�      