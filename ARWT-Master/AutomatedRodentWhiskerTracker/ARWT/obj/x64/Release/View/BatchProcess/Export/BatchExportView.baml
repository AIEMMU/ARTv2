   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  3    -ARWT.View.BatchProcess.Export.BatchExportView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     4
behavioursclr-namespace:ARWT.Behaviours       � Title$  BatchExportView��$	��480��$	��640��+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close� �� Mode$ TwoWay=� � 	Resources  �� 
TargetType �� Property �� Value��%�   '��       '�� Q     �� " ��.���  � ��$	 100����  � ��$ 5q���  � ��$
 Left=��� " ��.���  � ��$ 5q���  � ��$ Center=�.���  � RowDefinitions �� $��* ��� $
��Auto �.+�� $��0��	 � ColumnDefinitions	 �� $��* ��� $��* �.+��� $��0��
 �� DisplayMemberPath$

 Name��$��5q����� $ Videos�� $��1��	 �� $
��Auto ��� $��* � �� $
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
��Auto ��� $��* �.+��� $��0��$��0��$��
Velocity: ���� $��0��$��1�� �� Orientation$ 
Horizontal=�.+��� ���� $ VelocityOptions� �� SelectedItem �� $ SelectedVelocityOption�$ TwoWay=��� $��1��$��0��$��
Rotation: ���� $��1��$��1������ $ RotationOptions� �� $ SelectedRotationOption�$ TwoWay=��� $��2��$��0��$��Units To mm: ���� $��2��$��1��$��5q����� $ 	UnitsToMm�$ TwoWay=��� $��3��$��0��$��Original Frame rate: ���� $��3��$��1��$��5q����� $ OriginalFrameRate�$ TwoWay=��� $��Smoothing Function: ��$��4��$��0���� $��Repeat Smooths:��$��5��$��0���� $��Use DFT for Frequency: ��$��6��$��0���� $��5q�$��4��$��1��$

 Name������ $ SmoothingFunctions� �� $ SelectedSmoothingFunction�$ TwoWay=��� $��5��$��1��$��5q����� $ RepeatSmooths�$ TwoWay=��� $��6��$��1��$��Center=�$��5q� d� 	IsChecked �� $ UseDft�$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� $��7��$��0��$��Still Cut off (mm/s): ���� $��8��$��0��$��Walking Cut off (mm/s): ���� $��7��$��1��$��5q����� $ StillCutOff�$ TwoWay=��� $��8��$��1��$��5q����� $ WalkingCutOff�$ TwoWay=��� $��9��$��0��$)��#Get results only for interactions: ���� $��9��$��1��$��Center=� �� $ InteractionsOnly�$ TwoWay=��� $��10��$��0��$)��#Minimum number of detected frames: ���� $��10��$��1��$��Center=����� $! MinimumNumberDetectedFrames�$ TwoWay=��� $��11��$��0��$��2��$��All Results��.���� $
��Auto=�$
��Auto=�.���  �� $
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
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �	 �� $
��Auto ��� $��* �.+��� $��Max Velocity: ��$��0��$��0���� $��Max Centroid Velocity: ��$��1��$��0���� $��Max Angular Velocity: ��$��2��$��0���� $��Average Velocity: ��$��3��$��0���� $!��Average Centroid Velocity: ��$��4��$��0���� $ ��Average Angular Velocity: ��$��5��$��0���� $��Distance Travelled: ��$��6��$��0���� $#��Centroid Distance Travelled: ��$��7��$��0���� $��0��$��1������ $ MaxVelocity��� $��1��$��1������ $ MaxCentroidVelocity��� $��2��$��1������ $ MaxAngularVelocity��� $��3��$��1������ $ AverageVelocity��� $��4��$��1������ $ AverageCentroidVelocity��� $��5��$��1������ $ AverageAngularVelocity��� $��6��$��1������ $ DistanceTravelled��� $��7��$��1������ $ CentroidDistanceTravelled��� $��Left Whisker Frequency: ��$��8��$��0���� $��Right Whisker Frequency: ��$��9��$��0���� $��Left Whisker Amplitude: ��$��10��$��0���� $��Right Whisker Amplitude: ��$��11��$��0���� $ ��Left Whisker Mean Offset: ��$��12��$��0���� $!��Right Whisker Mean Offset: ��$��13��$��0���� $'��!Left Whisker Standard Deviation: ��$��14��$��0���� $(��"Right Whisker Standard Deviation: ��$��15��$��0���� $/��)Left Whisker 2√Root Mean Square Error: ��$��16��$��0���� $0��*Right Whisker 2√Root Mean Square Error: ��$��17��$��0���� $*��$Left Whisker Avg. Angular Velocity: ��$��18��$��0���� $+��%Right Whisker Avg. Angular Velocity: ��$��19��$��0���� $.��(Left Whisker Avg. Protraction Velocity: ��$��20��$��0���� $/��)Right Whisker Avg. Protraction Velocity: ��$��21��$��0���� $-��'Left Whisker Avg. Retraction Velocity: ��$��22��$��0���� $.��(Right Whisker Avg. Retraction Velocity: ��$��23��$��0���� $��8��$��1������ $ LeftWhiskerFrequency��� $��9��$��1������ $ RightWhiskerFrequency��� $��10��$��1������ $ LeftWhiskerAmplitude��� $��11��$��1������ $ RightWhiskerAmplitude��� $��12��$��1������ $ LeftWhiskerMeanOffset��� $��13��$��1������ $ RightWhiskerMeanOffset��� $��14��$��1������ $ LeftSTD��� $��15��$��1������ $ RightSTD��� $��16��$��1������ $ LeftRMS��� $��17��$��1������ $ RightRMS��� $��18��$��1������ $# LeftWhiskerAvgAngularVelocity��� $��19��$��1������ $$ RightWhiskerAvgAngularVelocity��� $��20��$��1������ $' !LeftWhiskerAvgProtractionVelocity��� $��21��$��1������ $( "RightWhiskerAvgProtractionVelocity��� $��22��$��1������ $&  LeftWhiskerAvgRetractionVelocity��� $��23��$��1������ $' !RightWhiskerAvgRetractionVelocity��� $��1��$ 
Horizontal=�$��Center=�.+��� $��Apply������ $ ApplyCommand��� $��Export������ $ ExportCommand��� $��Export Feet������ $ ExportFeetCommand��� $��Cancel������ $ CancelCommand�