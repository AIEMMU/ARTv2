   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   '    !ARWT.View.CropImage.CropImageView   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	     � Title$  CropImageView��5   	   $	��720��6   $
��1280��6,    � WindowState$ 	Maximized=�69   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5	   	   ��  �� Path$ Close�   Closing 5
   	   �� $ ClosingCommand�.��5      � -   5         �Name$
 Grid��5       � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ Image��5      $��0��6�   $	��Top=�6�   $��Center=�6�   ��6!   �� $ DisplayImage� �� NotifyOnTargetUpdated . 6�   �� 5   
   -   5        	 ��Name$	 Canvas��5      $��0��6!   
������ .+�5      �� -   5        
 ��Name$
 	Rectangle��5      
���  �� �� StrokeDashArray$ 1R�68   $��1��6L   �� 5      -   5        $
 PixelRectangle��5      $y�Hidden=�60    �� Left 6D   �� $ X� �� Mode$ TwoWay=�
 �� Top 6p   �� $ Y�$ TwoWay=���6�   �� $ Width�$ TwoWay=���6�   �� $ Height�$ TwoWay=�5      �� 5   
    �� Orientation$ 
Horizontal=�6   $��1��6.   $��Center=�6;   .+�5      �� $��Ok��6   ��6"   �� $ 	OkCommand��� 5      $��Cancel��6   ��6&   �� $ CancelCommand�5      5      5      