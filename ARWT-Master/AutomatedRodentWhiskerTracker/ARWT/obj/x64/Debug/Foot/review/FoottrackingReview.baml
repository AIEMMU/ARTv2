   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ARWT.Foot.reviewARWT.Foot.review  5   	   1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   )    #ARWT.Foot.review.FoottrackingReview   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ARWT.Foot.review     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	     � Title$  FoottrackingReview��5   	   $	��450��6$   $	��800��61   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5
   	   ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing 5   	   �� $ ClosingCommand�.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��1��6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   .+�5      �� $��Center=�6   $��Center=�61   $��0��6L   ��6\   �� $ Image��� 5      $��0��6A   $��Bottom=�6Q   $
��Left=�6l   $��16(�6�   
���  ����6   �� $ FrameNumberDisplay�6�   � 5      $��1��6    5       �� 5!      $��* �6$   �� 5"      $
��Auto �6$   .+�5&      �� $��1��6    �� Orientation$ 
Horizontal=�6*   $��Center=�6C   $��5q�6`   .+�5'      �� $��Export Data��6   ��63   �� $ ExportRawDataCommand��� 5*      $��
Export CSV��6   ��62   �� $ ExportCSVCommand�5+      5,      5-      �� 5/   
   $��2��6   $��Stretch=�6    � Minimum$ 0Q�6<   	 �� TickFrequency$	 1Q�6l   $��5q�6�   
 � Maximum
 6H   �� $ 
FrameCount� � Value 6   �� $ CurrentFrame�$ TwoWay=�6�   52      53      