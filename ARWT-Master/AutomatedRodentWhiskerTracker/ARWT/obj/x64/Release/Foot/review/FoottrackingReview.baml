   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ARWT.Foot.reviewARWT.Foot.review  1clr-namespace:ARWT.BehavioursARWT.Behaviours  )    #ARWT.Foot.review.FoottrackingReview   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:ARWT.Foot.review     4
behavioursclr-namespace:ARWT.Behaviours       � Title$  FoottrackingReview��$	��450��$	��800��+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close� �� Mode$ TwoWay=�   Closing �� $ ClosingCommand�.���  � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto ��� $
��Auto �.+�� $��1�� � ColumnDefinitions �� $��* ��� $
��Auto �.+��� $��Center=�$��Center=�$��0������ $ Image��� $��0��$��Bottom=�$
��Left=�$��16(�
���  ������ $ FrameNumberDisplay�� $��1�� �� $��* ��� $
��Auto �.+��� $��1�� �� Orientation$ 
Horizontal=�$��Center=�$��5q�.+��� $��Export Data������ $ ExportRawDataCommand��� $��
Export CSV������ $ ExportCSVCommand��� $��2��$��Stretch=� � Minimum$ 0Q�	 �� TickFrequency$	 1Q�$��5q�
 � Maximum
 �� $ 
FrameCount� � Value �� $ CurrentFrame�$ TwoWay=�