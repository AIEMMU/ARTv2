   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ARWT.View.SettingsARWT.View.Settings  1clr-namespace:ARWT.BehavioursARWT.Behaviours  1clr-namespace:ARWT.ConvertersARWT.Converters  %    ARWT.View.Settings.SettingsView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:ARWT.View.Settings     4
behavioursclr-namespace:ARWT.Behaviours     4
convertersclr-namespace:ARWT.Converters       � Title$  SettingsView��$	��480��$	��640��+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing �� $ ClosingCommand� � 	Resources 2   ,ARWT.Converters.BooleanToVisibilityConverter    BooleanToVisibilityConverter9   3ARWT.Converters.InverseBooleanToVisibilityConverter ' #InverseBooleanToVisibilityConverter �� 
TargetType �� Property �� Value��%�   &	        &	      '�� 
     '�� ,     '�� h         �� " ��.���  � ��$ 2q��� " ��.���  � ��$ Center=���  � ��$ 1q��� " ��.���  � ��$ 1q�.��� 	 � RowDefinitions	 �� $��* ��� $
��Auto �.+�� $��0��	 �� $��* ��� $
��Auto �
 � ColumnDefinitions
 �� $��* ��� $��* �.+��� $��0��$��2��$��0��$��2q����� $
 Mice� �� SelectedItem �� $ SelectedMouse�$ TwoWay=� �� View �� .P��� $
��File�� �� Width$	 120�� �� DisplayMemberBinding �� *VideoFileName+�� 
�������$��1q�$��2q� �� Padding$ 2q�$��0��$��1��._�� $��2q�y��� $ 	ShowVideo� �� 	Converter# [  	 �� $��* ��� $
��Auto �.+��� $��0������ $ DisplayImage��� $��1�� � Value �� $ SliderValue�$ TwoWay=� � Minimum �� $ Minimum� � Maximum �� $ Maximum��� 
�������$��1q�$��2q�$ 2q�$��1��$��1��{��� $ 	ShowVideo�._�� 	 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �
 �� $
��Auto ��� $��* �.+��� $��0��$��0��$��Gap Distance: ���� $��0��$��1�� �� $ GapDistance�$ TwoWay=� �� $ GapDistanceMin� �� $ GapDistanceMax��� $��1��$��0��$��Binary Treshold: ���� $��1��$��1�� �� $ BinaryThreshold�$ TwoWay=� �� $ BinaryThresholdMin� �� $ BinaryThresholdMax��� $��2��$��0��$��Binary Treshold 2: ���� $��2��$��1�� �� $ BinaryThreshold2�$ TwoWay=� �� $ BinaryThreshold2Min� �� $ BinaryThreshold2Max��� $��Smooth Motion: ��$��3��$��0���� $��3��$��1��$��Center=�$��2q� d� 	IsChecked �� $ SmoothMotion�$ TwoWay=��� $��Frame Rate: ��$��4��$��0���� $��4��$��1������ $ 	FrameRate�$ TwoWay=��� $��ROI: ��$��5��$��0����  �� Orientation$ 
Horizontal=�$��5��$��1��.+��� $	��Set������ $ SetRoiCommand��� $��Remove������ $ RemoveRoiCommand��� $��
Whiskers: ��$��6��$��0���� $ 
Horizontal=�$��6��$��1��.+��� $��Settings������ $ WhiskerSettingsCommand��� $��Feet: ��$��7��$��0���� $ 
Horizontal=�$��7��$��1��.+��� $��Settings������ $ FeetSettingsCommand��� $��1��$ 
Horizontal=�$��Center=�.+��� $��Ok������ $ 	OkCommand��� $��Cancel������ $ CancelCommand�