   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:ARWT.View.NewSessionARWT.View.NewSession  1clr-namespace:ARWT.BehavioursARWT.Behaviours  0    *ARWT.View.NewSession.NewWhiskerSessionView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:ARWT.View.NewSession     4
behavioursclr-namespace:ARWT.Behaviours       � Title$  Whisker Settings��$	��480��$	��640�� � WindowState$ 	Maximized=�+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close� �� Mode$ TwoWay=� � 	Resources  �� 
TargetType �� Property �� Value��%X   '��       '�� "     �� " ��.���  � ��$ 5q��� " ��.���  � ��$ 5q�.��� 	 � RowDefinitions	 �� $��* ��� $
��Auto ��� $
��Auto �.+�� $��0��
 � ColumnDefinitions
 �� $��* ��� $��* �.+��� $��Whisker Settings��$��0��$��5q�.��� 	 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �
 �� $
��Auto ��� $��* �.+��� $��Crop Scale Factor: ��$��0��$��0���� $��0��$��1������ $ CropScaleFactor��� $(��"Resolution Increase Scale Factor: ��$��1��$��0���� $��1��$��1������ $# ResolutionIncreaseScaleFactor��� $��Center=�$��Interpolation Type: ��$��2��$��0���� $��2��$��1��$��5q�$��-2q� �� SelectedItem �� $ InterpolationType�$ TwoWay=����� $ InterpolationTypes��� $��Remove Duds:��$��3��$��0���� $��3��$��1��$��5q� d� 	IsChecked �� $ 
RemoveDuds�$ TwoWay=��� $!��Minimum Whisker Threshold: ��$��4��$��0���� $��4��$��1������ $ LineThreshold��� $��Lower Bound: ��$��5��$��0���� $��5��$��1������ $ 
LowerBound��� $��Upper Bound: ��$��6��$��0���� $��6��$��1������ $ 
UpperBound��� $��1������ $ DisplayImage��� $��1��$��5q� � Minimum$ 0Q� � Maximum �� $ MaxFrame� � Value �� $ FrameNumber�$ TwoWay=���  �� Orientation$ 
Horizontal=�$��2��$��Center=�.+��� $��Preview������ $ PreviewCommand��� $��Ok������ $ 	OkCommand�