   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  )    #ARWT.View.NewSession.NewSessionView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     4
behavioursclr-namespace:ARWT.Behaviours       � Title$  NewSessionView��$	��480��$	��640�� � WindowState$ 	Maximized=�+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close� �� Mode$ TwoWay=� � 	Resources  �� 
TargetType �� Property �� Value��%^   '��       '�� "     �� " ��.���  � ��$ 5q��� " ��.���  � ��$ Stretch=�.��� 	 � RowDefinitions	 �� $��* ��� $
��Auto �.+�� $��0��
 � ColumnDefinitions
 �� $��* ��� $��* �.+�� $��0��	 �� $��* ��� $��* �.+�� $��0��	 �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �
 �� $
��Auto ��� $��* �.+��� $��Frame: ��$��0��$��0���� $��0��$��1��$��5q� � Value �� $ FrameNumber� � Maximum �� $ MaxFrame��� $��ROI: ��$��1��$��0����  �� Orientation$ 
Horizontal=�$��1��$��1��.+��� $	��Set������ $ SetRoiCommand��� $��Remove������ $ RemoveRoiCommand��� $��Binary Threshold: ��$��2��$��0��� $��2��$��1��
 �� $��* ��� $
��Auto �.+��� $��0�� � Minimum$ 0Q�$��5q� �� $ MaxThreshold� �� $ ThresholdValue�$ TwoWay=��� $��1��$��5q�$��40������ $ ThresholdValue�$ TwoWay=��� $��Gap Distance: ��$��3��$��0��� $��3��$��1��
 �� $��* ��� $
��Auto �.+��� $��0��$	 0.1Q�$	 500Q�$��5q� �� $ GapDistance�$ TwoWay=��� $��1��$��5q�$��40������ $ GapDistance�$ TwoWay=��� $��Motion Threshold: ��$��4��$��0���� $��4��$��1��$��5q�$��40��$
��Left=����� $ LineThreshold�$ TwoWay=��� $��Interaction Threshold: ��$��5��$��0���� $��5��$��1��$��5q�$��40��$
��Left=����� $ InteractionValue�$ TwoWay=��� $��Motion Length: ��$��6��$��0���� $��6��$��1��$��5q�$��40��$
��Left=����� $ MotionLength�$ TwoWay=��� $��Find Whiskers: ��$��7��$��0���� $��7��$��1��$��5q�$
��Left=� d� 	IsChecked �� $ FindWhiskers�$ TwoWay=��� $��1��$��5q����� $ 	Boundries� �� View �� .P��� $
��Name�� �� Width$	 120�� �� CellTemplate �� .R��� $��5q�$��Center=�$��Stretch=����� $
 Name�$ TwoWay=��� $��Enabled��$ 50�� �� .R��� $��5q�$��Center=� �� $ Enabled�$ TwoWay=�� $��1��	 �� $��* �
 �� $��* ��� $��* �.+��� $��0��$��0������ $ DisplayImage��� $��0��$��1������ $ 
ExtraImage��� $��1��$ 
Horizontal=�$��Center=�.+��� $��Preview������ $ PreviewCommand��� $��Ok������ $ 	OkCommand��� $��Cancel������ $ CancelCommand�