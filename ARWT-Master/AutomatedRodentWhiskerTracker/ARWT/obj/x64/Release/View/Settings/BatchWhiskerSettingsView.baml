   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ARWT.View.SettingsARWT.View.Settings  1clr-namespace:ARWT.BehavioursARWT.Behaviours  1clr-namespace:ARWT.ConvertersARWT.Converters  1    +ARWT.View.Settings.BatchWhiskerSettingsView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     2local clr-namespace:ARWT.View.Settings     4
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
�������$��1q�$��2q�$ 2q�$��1��$��1��{��� $ 	ShowVideo�._��� $��Whisker Settings��$��0��$��5q�.��� 	 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �
 �� $��22.182 ��� $��154.618 ��� $��* �.+��� $��Crop Scale Factor: ��$��0��$��0��$��2��$��1,1,0.8,0.6q��� $��0��$��2��$��0.2,0,-0.2,17.2q�$��2������ $ CropScaleFactor��� $(��"Resolution Increase Scale Factor: ��$��1��$��0��$��2��$��1,0.4,0.8,1.2q��� $��1��$��2��$��0.2,0.4,-0.2,22.4q�$��2������ $# ResolutionIncreaseScaleFactor��� $��Center=�$��Interpolation Type: ��$��2��$��0��$��2��$��1,3.8,0.8,3.4q��� $��2��$��2��$��5.2,4.8,4.8,5.4q�$��-2q� �� $ InterpolationType�$ TwoWay=����� $ InterpolationTypes��� $��Remove Duds:��$��3��$��0��$��2��$��1,5.6,0.8,4.8q��� $��3��$��2��$��5.2,4.6,4.8,4.8q� d� 	IsChecked �� $ 
RemoveDuds�$ TwoWay=��� $!��Minimum Whisker Threshold: ��$��4��$��0��$��2��$��1,1.2,0.8,0.4q��� $��4��$��2��$��0.2,0.2,-0.2,17q�$��2������ $ LineThreshold��� $��Lower Bound: ��$��5��$��0��$��2��$��1,0.6,0.8,1q��� $��4��$��2��$��0.2,17.2,-0.2,0q�$��2������ $ 
LowerBound��� $��Upper Bound: ��$��6��$��0��$��2��$��1,1,0.8,0.6q��� $��6��$��2��$��0.2,0,-0.2,-0.4q����� $ 
UpperBound��� $��1�� �� Orientation$ 
Horizontal=�$��Center=�.+��� $��Ok������ $ 	OkCommand��� $��Cancel������ $ CancelCommand�