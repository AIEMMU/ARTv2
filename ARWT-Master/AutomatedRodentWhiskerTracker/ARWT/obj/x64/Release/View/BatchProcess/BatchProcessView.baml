   M S B A M L   `   `   `  ���� ?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem -    'ARWT.View.BatchProcess.BatchProcessView   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     4
behavioursclr-namespace:ARWT.Behaviours     9system&clr-namespace:System;assembly=mscorlib       � Title$  BatchProcessView��$	��720��$
��1280�� � WindowState$ 	Maximized=�+   %ARWT.Behaviours.WindowClosingBehavior   Close ��  �� Path$ Close�   Closing �� $ ClosingCommand� � 	Resources  �� 
TargetType �� Property �� Value��%y   '��       '� (     �� " ��.���  � ��$ Stretch=��� " �.���  � ��$ 15����  � ��$ Center=�.��� 	 � RowDefinitions	 �� $��* ��� $
��Auto ��� $
��Auto �.+�� $��0��
 � ColumnDefinitions
 �� $��* �	 �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��0�� �� Orientation$ 
Horizontal=�$��Center=�.+��� $��Add File������ $ AddTgFileCommand��� $��
Add Folder������ $ AddTgFolderCommand��� $��Set Output Folder������ $ SetOutputFolderCommand��� $��Get Output Folder������ $ LoadOutputFolderCommand��� $��Remove File������ $ RemoveTgFileCommand��� $��
Clear List������ $ ClearTgFilesCommand��� $��Settings������ $ BatchSettingsCommand��� $��Process������ $ ProcessCommand��� $��Export������ $ ExportBatchCommand��� $��Videos to run at once: ��$��Center=��� $��Center=� �� SelectedItem �� $ DegreesOfParallel� �� Mode$ TwoWay=�����  �� Type" ��.s���1����2����3����4����5����6���� $��1��$��0��$��Stretch=����� $ TgItemsSource� �� $ SelectedTgItem�$ TwoWay=� �� View �� .P��� $
��Name�� �� Width$	 400�� �� DisplayMemberBinding �� *Name+�� $	 140��$��Progress�� �� CellTemplate �� .R��  � Maximum$	 100Q�$	��120�� � Value �� $ Progress�$ OneWay=��� $ 50��$��Review�� �� .R��� $��25��$��25������ $ ReviewCommand�.���� $��R���� $��Video Has Been Reviewed��$	 400�� �� *
Reviewed+���� " ��.���  �@��
IsSelected �� $ 
IsSelected�