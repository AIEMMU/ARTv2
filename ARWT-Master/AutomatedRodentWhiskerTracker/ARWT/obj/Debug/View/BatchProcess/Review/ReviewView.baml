   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   1clr-namespace:ARWT.ConvertersARWT.Converters  5   	   | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting 5   	   �rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization 5   	   .    (ARWT.View.BatchProcess.Review.ReviewView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	   4
convertersclr-namespace:ARWT.Converters     5   	   �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	   �visualizationToolkitrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit     5   	     � Title$  
ReviewView��5
   	   $
��1080��6   $
��1920��6*    � WindowState$ 	Maximized=�67   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5   	   ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing 5	   	   �� $ ClosingCommand� � 	Resources 5      2   ,ARWT.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter9   3ARWT.Converters.InverseBooleanToVisibilityConverter ' #InverseBooleanToVisibilityConverter �� 
TargetType �� Property	 �� Value  InvisibleDataPointB  <System.Windows.Controls.DataVisualization.Charting.DataPoint  InvisibleDataPoint2  InvisibleLegend6  0System.Windows.Controls.DataVisualization.Legend��%�  &	        &	      '�� &     '�� m     '�� �     &	 #    &	 �    &	 �      5   
   62     5   
   69   �� 5   
   " ��6   .�5      ��  � ��$	 2q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$	 Center=�62   �� 5       � ��$	 1q�6'   5      �� 5   
   " ��6   .�5      ��  � ��$	 1q�6'   5      �� 5   
   "  6+   .�5      ��  � ��
	 ��  �6+   �� 5       � ��	 6)   Z� 5      �� 5   
   "  6,   .�5      ��  � ��
	 �  ��6+   �� 5       � ��	 6)   Z� 5      �� 5    
   "  6(   .�5!      ��  � ��$	 0��6&   5"      5#      .��5$      � 
 � RowDefinitions
 5%   
   �� 5&      $��* �6   �� 5'      $
��Auto �6   .+�5*   
   � $��0��6   
 5+      �� 5,      $��3* �6    �� 5-      $��5 �6    �� 5.      $��* �6     � ColumnDefinitions 50      �� 51      $��* �6#   �� 52      $��3* �6#   .+�55      �� $��1��6   $��0��6(   $��2��68   $��5��6L   $��Stretch=�6W   �� 5A      
�������$��1q�6(   $��2q�6<    �� Padding$ 2q�6G   $��0��6S   $��1��6a   ._�5B      � $��2q�6v   y�6   �� $ 	ShowVideo� �� 	Converter# [  
 5C      �� 5D      $��* �6(   �� 5E      $
��Auto �6(   .+�5H      �� $��0��6   ��6)   �� $ DisplayImage��� 5I      $��1��6    �� IsSelectionRangeEnabled . 
����  � � Value 6*   �� $ SliderValue�$ TwoWay=� � Minimum 6Z   �� $ Minimum� � Maximum 6{   �� $ Maximum� �� SelectionStart 6�   �� $ AnalyseStart�$ TwoWay=� �� SelectionEnd 6�   �� $ 
AnalyseEnd�$ TwoWay=�6+  5J      5K      �� 5L      $��0��6   $��1��6&   $��2q�6�   y�66   �� $ 	ShowVideo�# [ ��6�   �� $ DisplayText�6�   �� 5N      
�������$��1q�6(   $��2q�6<   $ 2q�6G   $��2��6S   $��0��6`   ._�5O      � y�6   �� $ 	ShowVideo�# [  
 5P      �� 5Q      $
��Auto �6(   �� 5R      $
��Auto �6(   �� 5S      $
��Auto �6(   �� 5T      $��* �6(    5V      �� 5W      $
��Auto �6+   �� 5X      $��* �6+   �� 5Y      $
��Auto �6+   �� 5Z      $��* �6+   �� 5[      $
��Auto �6+   �� 5\      $��* �6+   �� 5]      $
��Auto �6+   �� 5^      $��* �6+   .+�5a      �� $��Name: ��6    $��0��6.   $��0��6;   �� 5b      $��0��6:   $��1��6G   ��6   �� $
 Name�6G   �� 5d      $��
Distance: ��6    $��1��62   $��0��6?   �� 5e      $��1��6G   $��1��6T   ��6   �� $ DistanceTravelled�6T   �� 5g      $��Centroid Distance: ��6    $��2��6;   $��0��6H   �� 5h      $��2��6O   $��1��6\   ��6   �� $ CentroidDistanceTravelled�6\   �� 5j      $��Start Frame: ��6    $��0��65   $��2��6B   �� 5k      $��1��6B   $��3��6O   ��6   �� $ AnalyseStart�6O   �� 5m      $��End Frame: ��6    $��1��63   $��2��6@   �� 5n      $��2��6@   $��3��6M   ��6   �� $ 
AnalyseEnd�6M   �� 5p      $��
Duration: ��6    $��2��62   $��2��6?   �� 5q      $��0��6>   $��3��6K   ��6   �� $ Duration�6K   �� 5s      $��Average Velocity: ��6    $��0��6:   $��4��6G   �� 5t      $��0��6A   $��5��6N   ��6   �� $ AvgVelocity�6N   �� 5v      $!��Average Centroid Velocity: ��6    $��1��6C   $��4��6P   �� 5w      $��1��6I   $��5��6V   ��6   �� $ AvgCentroidVelocity�6V   �� 5y      $ ��Average Angular Velocity: ��6    $��2��6B   $��4��6O   �� 5z      $��2��6H   $��5��6U   ��6   �� $ AvgAngularVelocity�6U   �� 5|      $��Max Velocity: ��6    $��0��66   $��6��6C   �� 5}      $��0��6A   $��7��6N   ��6   �� $ MaxVelocity�6N   �� 5      $��Max Centroid Velocity: ��6    $��1��6?   $��6��6L   �� 5�      $��1��6I   $��7��6V   ��6   �� $ MaxCentroidVelocity�6V   �� 5�      $��Max Angular Velocity: ��6    $��2��6>   $��6��6K   �� 5�      $��2��6H   $��7��6U   ��6   �� $ MaxAngularVelocity�6U   �� 5�      $��3��6   $��0��6,   $��8��6<   $��2q�6�   ��6P   �� $ SelectedMouseBehaviours� �� View 5�      �� 5�      .P�5�   "   �� $��Boundary��61    �� Width$	 120��6C    �� DisplayMemberBinding 6O   �� *BoundaryName+�� 5�   "   $��Interaction��61   $	 120��6F    6R   �� *Interaction+�� 5�   "   $��Frame��61   $ 50��6@    6K   �� *FrameNumber+5�      5�      5�      5�      �� 5�      
�������$��1q�6(   $��2q�6<   $ 2q�6G   $��2��6S   $��1��6`   ._�5�      � 
 5�      �� 5�      $��* �6(   �� 5�      $��* �6(   .+�5�      >  8System.Windows.Controls.DataVisualization.Charting.Chart  -   5          Name$ 	LeftChart��5�   �   $��0��6,      Title$ Left Whiskers��69   $��5q�6O   $��Stretch=�6Z      LegendStyle# [ 6v      Series. 5�      C  =System.Windows.Controls.DataVisualization.Charting.LineSeries  H  BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Value��65      IndependentValuePath$	 Key��6P      IsSelectionEnabled .    DataPointStyle# [ 6�      ItemsSource 6k   �� $ LeftGraphWhiskersSmoothed�$ OneWay=�6�     5�      $ Value��65   $	 Key��6P    . # [ 6�    6k   �� $ LeftGraphWhiskers�$ OneWay=�5�          Axes  5�      C	  =System.Windows.Controls.DataVisualization.Charting.LinearAxis	  =
  7System.Windows.Controls.DataVisualization.Charting.Axis! 
  Orientation$! X=�69   D  >System.Windows.Controls.DataVisualization.Charting.DisplayAxis"   Title$" Frame��6I   #   ShowGridLines# . D  >System.Windows.Controls.DataVisualization.Charting.NumericAxis$   Minimum�  �System.Windows.Controls.DataVisualization.Charting.NullableConverter`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]$$ 0 6l   	  5�      $" Angle��69   $! Y=�6G   # . 6W   5�        5�      -   5        $ 
RightChart��5�   �   $��1��6,   $ Right Whiskers��69   $��5q�6P   $��Stretch=�6[   # [ 6w   . 5�        $ Value��65   $	 Key��6P    . # [ 6�    6k   �� $  RightGraphWhiskersSmoothed�$ OneWay=�6�     5�      $ Value��65   $	 Key��6P    . # [ 6�    6k   �� $ RightGraphWhiskers�$ OneWay=�5�        5�      	  $! X=�69   $" Frame��6I   # . $$ 0 6l   	  5�      $" Angle��69   $! Y=�6G   # . 6W   5�      5�      5�      �� 5�      
�������$��1q�6(   $��2q�6<   $ 2q�6G   $��0��6S   $��0��6`   ._�5�      �� $
��Auto=�6   $
��Auto=�6D   .��5�      � y�6   �� $ 	ShowVideo�# [  
 5�      �� 5�      $
��Auto �6,   �� 5�      $
��Auto �6,   �� 5�      $
��Auto �6,   �� 5�      $
��Auto �6,   �� 5�      $
��Auto �6,   �� 5�      $
��Auto �6,   �� 5�      $
��Auto �6,   �� 5�      $
��Auto �6,   �� 5�      $
��Auto �6,    5�      �� 5�      $
��Auto �6/   �� 5�      $��* �6/   .+�5�      �� $��0��6$   $��0��61   $��Gap Distance: ��6A   �� 5�      $��0��6!   $��1��6.    6>   �� $ GapDistance�$ TwoWay=� 6n   �� $ GapDistanceMin� 6�   �� $ GapDistanceMax�{�6�   �� $ VideoSelected��� 5�      $��1��6$   $��0��61   $��Binary Treshold: ��6A   �� 5�      $��1��6!   $��1��6.    6>   �� $ BinaryThreshold�$ TwoWay=� 6r   �� $ BinaryThresholdMin� 6�   �� $ BinaryThresholdMax�{�6�   �� $ VideoSelected��� 5�      $��2��6$   $��0��61   $��Binary Treshold 2: ��6A   �� 5�      $��2��6!   $��1��6.    6>   �� $ BinaryThreshold2�$ TwoWay=� 6s   �� $ BinaryThreshold2Min� 6�   �� $ BinaryThreshold2Max�{�6�   �� $ VideoSelected��� 5�      $��Smooth Motion: ��6$   $��3��6;   $��0��6H   �� 5�      $��3��6X   $��1��6e   $��Center=�6u   $��5q�6�   % d� 	IsChecked% 6#   �� $ SmoothMotion�$ TwoWay=�6�   �� 5�      $��Frame Rate: ��6$   $��4��68   $��0��6E   �� 5�      $��4��6O   $��1��6\   $��5q�6l   ��6"   �� $ 	FrameRate�$ TwoWay=�6l   �� 5�      $��Find Whiskers: ��6$   $��5��6;   $��0��6H   �� 5�      $��5��6#   $��1��60   $��5q�6x   % 6@   �� $ IncludeWhiskers�$ TwoWay=�6x   �� 5�      $��Whisker Settings��6#   $��6��6=   $��0��6J   $��2��6Z   $��5q�6n   y�6y   �� $ IncludeWhiskers�# [   5�      ��%�   '��       '�� G     �� 5�   "   " ��6(   .�5�   &   ��  � ��$	 5q�6?   5�   #   �� 5�   "   " ��6(   .�5�   &   ��  � ��$	 5q�6?   5�   #   5�      .��5�      � 
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
��Auto �64    5�   "   �� 5�   &   $
��Auto �67   �� 5�   &   $��* �67   .+�5�   "   �� $��Crop Scale Factor: ��6,   $��0��6G   $��0��6T   �� 5�   "   $��0��6P   $��1��6]   ��6*   �� $ CropScaleFactor�6]   �� 5�   "   $(��"Resolution Increase Scale Factor: ��6,   $��1��6V   $��0��6c   �� 5�   "   $��1��6^   $��1��6k   ��6*   �� $# ResolutionIncreaseScaleFactor�6k   �� 5�   "   $��Center=�6,   $��Interpolation Type: ��6G   $��2��6c   $��0��6p   �� 5�   "   $��2��6+   $��1��68   $��5q�6�   $��-2q�6�   & �� SelectedItem& 6H   �� $ InterpolationType�$ TwoWay=���6�   �� $ InterpolationTypes�6�   �� 5�   "   $��Remove Duds:��6,   $��3��6@   $��0��6M   6_   �� 5�   "   $��3��6^   $��1��6k   $��5q�6{   % 6+   �� $ 
RemoveDuds�$ TwoWay=�6{   �� 5�   "   $!��Minimum Whisker Threshold: ��6,   $��4��6O   $��0��6\   �� 5�   "   $��4��6N   $��1��6[   ��6*   �� $ LineThreshold�6[   �� 5   "   $��Lower Bound: ��6,   $��5��6A   $��0��6N   �� 5  "   $��5��6K   $��1��6X   ��6*   �� $ 
LowerBound�6X   �� 5  "   $��Upper Bound: ��6,   $��6��6A   $��0��6N   �� 5  "   $��6��6K   $��1��6X   ��6*   �� $ 
UpperBound�6X   �� 5  "   $��Smoothing Function: ��6,   $��7��6H   $��0��6U   �� 5  "   $��Repeat Smooths:��6,   $��8��6C   $��0��6P   �� 5	  "   $��5q�6+   $��7��66   $��1��6C   ' �� DisplayMemberPath$
' Name��6�   ��6S   �� $ SmoothingFunctions�& 6�   �� $ SelectedSmoothingFunction�$ TwoWay=��� 5
  "   $��8��6�   $��1��6�   ��6*   �� $ RepeatSmooths�$ TwoWay=�( �� UpdateSourceTrigger$( PropertyChanged=�6�   �� 5  "   $��Preview��6)   $��9��6c   $��2��6p   $��0��6�   $��Center=�6�   ��6;   �� $ PreviewCommand�6�   5     5     �� 5     $��7��6%   $��0��62   $��2��6B   ) �� Orientation$) 
Horizontal=�6V   $��Center=�6o   .+�5     �� $��Center=�6%   $��Start��6B   ��6R   �� $ SetStartFrameCommand��� 5     $��Center=�6%   $	��End��6B   ��6P   �� $ SetEndFrameCommand�5     �� 5     $��8��6%   $��0��62   $��2��6B   $) 
Horizontal=�6V   $��Center=�6o   .+�5     �� $��Center=�6%   $��Re-Run��6B   ��6S   �� $ ReRunCommand��� 5     $��Center=�6%   $
��Save��6B   ��6Q   �� $ SaveCommand��� 5     $��Center=�6%   $��Export��6B   ��6S   �� $ ExportCommand�5     5     5     5     5     �� 5  
   $��1��6   $) 
Horizontal=�6"   $��Center=�6;   .+�5     �� $��Ok��6   ��6"   �� $ 	OkCommand��� 5      $��Cancel��6   ��6&   �� $ CancelCommand�5!     5"     5#     