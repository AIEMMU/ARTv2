   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  1clr-namespace:ARWT.ConvertersARWT.Converters  | xSystem.Windows.Controls.DataVisualization.Toolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting �rclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit)System.Windows.Controls.DataVisualization .    (ARWT.View.BatchProcess.Review.ReviewView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     4
behavioursclr-namespace:ARWT.Behaviours     4
convertersclr-namespace:ARWT.Converters     �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit     �visualizationToolkitrclr-namespace:System.Windows.Controls.DataVisualization;assembly=System.Windows.Controls.DataVisualization.Toolkit     $
��1080��$
��1920��  � WindowState$  	Maximized=�+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing �� $ ClosingCommand� � Title �� $ WindowTitle� � 	Resources 2   ,ARWT.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter9   3ARWT.Converters.InverseBooleanToVisibilityConverter ' #InverseBooleanToVisibilityConverter �� 
TargetType �� Property	 �� Value  InvisibleDataPointB  <System.Windows.Controls.DataVisualization.Charting.DataPoint  InvisibleDataPoint2  InvisibleLegend6  0System.Windows.Controls.DataVisualization.Legend��%r  &	        &	      '�� 
     '�� ,     '�� h     &	 �     &	 �     &	          �� " ��.���  � ��$	 2q��� " ��.���  � ��$	 Center=���  � ��$	 1q��� " ��.���  � ��$	 1q��� "  .���  � ��
	 ��  ���  � ��	 Z� �� "  .���  � ��
	 �  ����  � ��	 Z� �� "  .���  � ��$	 0��.��� 
 � RowDefinitions
 �� $��* ��� $
��Auto �.+�� $��0��
 �� $��3* ��� $��10 ��� $
��Auto � � ColumnDefinitions �� $��* ��� $��2* �.+��� 
�������$��1q�$��2q� �� Padding$ 2q�$��0��$��1��._�� $��2q�y��� $ 	ShowVideo� �� 	Converter# [  
 �� $��* ��� $
��Auto �.+��� $��0������ $ DisplayImage��� $��0��$��Bottom=�$
��Left=�$��24(�
���  ������ $ FrameNumberDisplay��� $��1�� �� IsSelectionRangeEnabled . 
����  � � Value �� $ SliderValue�$ TwoWay=� � Minimum �� $ Minimum� � Maximum �� $ Maximum� �� SelectionStart �� $ AnalyseStart�$ TwoWay=� �� SelectionEnd �� $ 
AnalyseEnd�$ TwoWay=��� $��0��$��1��$��2q�y��� $ 	ShowVideo�# [ ���� $ DisplayText��� 
�������$��1q�$��2q�$ 2q�$��0��$��0��._��� $
��Auto=�$
��Auto=�.��� y��� $ 	ShowVideo�# [  
 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $��* �.+��� $��0��$��0��$��Gap Distance: ���� $��0��$��1�� �� $ GapDistance�$ TwoWay=� �� $ GapDistanceMin� �� $ GapDistanceMax�{��� $ VideoSelected��� $��1��$��0��$��Binary Treshold: ���� $��1��$��1�� �� $ BinaryThreshold�$ TwoWay=� �� $ BinaryThresholdMin� �� $ BinaryThresholdMax�{��� $ VideoSelected��� $��2��$��0��$��Binary Treshold 2: ���� $��2��$��1�� �� $ BinaryThreshold2�$ TwoWay=� �� $ BinaryThreshold2Min� �� $ BinaryThreshold2Max�{��� $ VideoSelected��� $��Smooth Motion: ��$��3��$��0���� $��3��$��1��$��Center=�$��5q� d� 	IsChecked �� $ SmoothMotion�$ TwoWay=��� $��Frame Rate: ��$��4��$��0���� $��4��$��1��$��5q����� $ 	FrameRate�$ TwoWay=��� $��Find Whiskers: ��$��5��$��0���� $��5��$��1��$��5q� �� $ IncludeWhiskers�$ TwoWay=��� $��Find Feet: ��$��6��$��0���� $��6��$��1��$��5q� �� $ IncludeFeet�$ TwoWay=��� $��Whisker Settings��$��7��$��0��$��2��$��5q�y��� $ IncludeWhiskers�# [   ��%X   '��       '�� "     �� " ��.���  � ��$	 5q��� " ��.���  � ��$	 5q�.��� 
 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $��* �.+��� $��Crop Scale Factor: ��$��0��$��0���� $��0��$��1������ $ CropScaleFactor��� $(��"Resolution Increase Scale Factor: ��$��1��$��0���� $��1��$��1������ $# ResolutionIncreaseScaleFactor��� $��Center=�$��Interpolation Type: ��$��2��$��0���� $��2��$��1��$��5q�$��-2q� �� SelectedItem �� $ InterpolationType�$ TwoWay=����� $ InterpolationTypes��� $��Remove Duds:��$��3��$��0���� $��3��$��1��$��5q� �� $ 
RemoveDuds�$ TwoWay=��� $!��Minimum Whisker Threshold: ��$��4��$��0���� $��4��$��1������ $ LineThreshold��� $��Lower Bound: ��$��5��$��0���� $��5��$��1������ $ 
LowerBound��� $��Upper Bound: ��$��6��$��0���� $��6��$��1������ $ 
UpperBound��� $��Smoothing Function: ��$��7��$��0���� $��Repeat Smooths:��$��8��$��0���� $��5q�$��7��$��1�� �� DisplayMemberPath$
 Name������ $ SmoothingFunctions� �� $ SelectedSmoothingFunction�$ TwoWay=��� $��8��$��1������ $ RepeatSmooths�$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��Foot Settings��$��8��$��0��$��2��$��5q�y��� $ IncludeWhiskers�# [   ��%X   '��       '�� "     �� " ��.���  � ��$	 5q��� " ��.���  � ��$	 5q�.��� 
 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $��* �.+��� $��0��$��0��$��Downsample scale factor��$��5q��� $��0��$��1������ $ scaleFactor�$ TwoWay=��� $��1��$��0��$'��!Erosion Kernel Size (must be odd)��$��5q��� $��1��$��1������ $ 
KernelSize�$ TwoWay=��� $��3��$��0��$��Contour Distance��$��	5,0,5,4.8q�$��17��$��Bottom=��� $��3��$��1������ $ contourDistance�$ TwoWay=��� $��2��$��0��$��Erosions Iterations��$��5q��� $��2��$��1������ $ erosionIterations�$ TwoWay=��� $��5��$��0��$��Contour Area���� $��5��$��1�� �� $ areaTheshold�$ TwoWay=� �� $ areaThresholdMin� �� $ areaThresholdMax�{��� $ VideoSelected��� $��10��$��0��$��2�� �� Orientation$ 
Horizontal=�$��Center=�.+��� $��Preview������ $ PreviewCommand��� $��Review Data������ $ ReviewCommand��� $��Review Feet������ $ ReviewFootCommand�{��� *AreFootResults+$ TwoWay=��� $��11��$��0��$��2��$ 
Horizontal=�$��Center=�.+��� $��Center=�$��Start������ $ SetStartFrameCommand��� $��Center=�$	��End������ $ SetEndFrameCommand��� $��12��$��0��$��2��$ 
Horizontal=�$��Center=�.+��� $��Center=�$��Re-Run������ $ ReRunCommand��� $��Center=�$
��Save������ $ SaveCommand��� $��Center=�$��
Export Raw������ $ ExportCSVCommand��� $��Center=�$��Export Mean whisker������ $ ExportMeanCSVCommand��� $��1��$ 
Horizontal=�$��Center=�.+��� $��Ok������ $ 	OkCommand��� $��Cancel������ $ CancelCommand�