   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ARWT.Foot.trackingARWT.Foot.tracking  1clr-namespace:ARWT.BehavioursARWT.Behaviours  3    -ARWT.Foot.tracking.NewFootTrackingSessionView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     2local clr-namespace:ARWT.Foot.tracking     4
behavioursclr-namespace:ARWT.Behaviours       � Title$  Foot Tracking Settings��$
��1080��$
��1920�� � WindowState$ 	Maximized=�+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing �� $ ClosingCommand� � 	Resources  �� 
TargetType �� Property	 �� Value��%X   '��       '�� "     �� " ��.���  � ��$	 5q��� " ��.���  � ��$	 5q�.��� 
 � RowDefinitions
 �� $��* ��� $
��Auto � � ColumnDefinitions �� $	��450 ��� .+��� $��Foot Tracking Settings��.���� $��0��$
��Auto=�$
��Auto=�.��� 
 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� �� $
��Auto � �� $
��Auto ��� $��* �.+��� $��0��$��0��$��Downsample scale factor��$��5q��� $��0��$��1������ $ scaleFactor�$ TwoWay=��� $��1��$��0��$'��!Erosion Kernel Size (must be odd)��$��5q��� $��1��$��1������ $ 
KernelSize�$ TwoWay=��� $��3��$��0��$��Contour Distance��$��	5,0,5,4.8q�$��17��$��Bottom=��� $��3��$��1������ $ contourDistance�$ TwoWay=��� $��2��$��0��$��Erosions Iterations��$��5q��� $��2��$��1������ $ erosionIterations�$ TwoWay=��� $��4��$��0��$��Area of Contour��$��5q��� $��4��$��1��$��5q� � Value �� *area+$ TwoWay=� � Minimum �� *Settings.areaMin+ � Maximum �� *Settings.areaMax+�� $��8��$��0��$��2�� �� Orientation$ 
Horizontal=�$��Center=�.+��� $	��125��$��Preview������ $ PreviewCommand��� $	��125��$��Ok������ $ 	OkCommand��� $	��125��$��Cancel������ $ CancelCommand��� 
�������$��1q�$��2q� �� Padding$ 2q�$��0��$��1��._�� 
 �� �� $
��Auto �.+��� $��0������ $ DisplayImage��� $��Bottom=�$
��Left=�$��24(�
���  ������ $ FrameNumberDisplay��� $��1�� �� $ Frame�$ TwoWay=� �� $ minFrame� �� $ maxFrame�