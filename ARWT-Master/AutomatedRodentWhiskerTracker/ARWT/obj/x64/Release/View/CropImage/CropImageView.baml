   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  '    !ARWT.View.CropImage.CropImageView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4
behavioursclr-namespace:ARWT.Behaviours       � Title$  CropImageView��$	��720��$
��1280�� � WindowState$ 	Maximized=�+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close�   Closing �� $ ClosingCommand�.��� -    �Name$
 Grid�� � RowDefinitions �� $��* ��� $
��Auto �.+��� -    ��Name$ Image��$��0��$	��Top=�$��Center=����� $ DisplayImage� �� NotifyOnTargetUpdated . �� -   	 ��Name$	 Canvas��$��0��.+��� -   
 ��Name$
 	Rectangle��
���  �� �� StrokeDashArray$ 1R�$��1���� -   $
 PixelRectangle��$y�Hidden=� �� Left �� $ X� �� Mode$ TwoWay=�
 �� Top �� $ Y�$ TwoWay=����� $ Width�$ TwoWay=����� $ Height�$ TwoWay=���  �� Orientation$ 
Horizontal=�$��1��$��Center=�.+��� $��Ok������ $ 	OkCommand��� $��Cancel������ $ CancelCommand�