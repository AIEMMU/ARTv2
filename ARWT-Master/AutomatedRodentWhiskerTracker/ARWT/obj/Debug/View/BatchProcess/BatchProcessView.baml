   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   -    'ARWT.View.BatchProcess.BatchProcessView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	   9system&clr-namespace:System;assembly=mscorlib     5   	     � Title$  BatchProcessView��5   	   $	��720��6"   $
��1280��6/    � WindowState$ 	Maximized=�6<   +   %ARWT.Behaviours.WindowClosingBehavior   Close 5   	   ��  �� Path$ Close�   Closing 5   	   �� $ ClosingCommand� � 	Resources 5	       �� 
TargetType �� Property �� Value��%�   '��       '� M     �� 5
   
   " ��6   .�5      ��  � ��$ Stretch=�6;   5      �� 5   
   " �6   .�5      ��  � ��$ 15��6'   �� 5       � ��$ Center=�62   5      5      .��5      � 	 � RowDefinitions	 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   � $��0��6   
 � ColumnDefinitions
 5      �� 5      $��* �6#   	 5       �� 5!      $
��Auto �6    �� 5"      $��* �6    �� 5#      $
��Auto �6    .+�5)      �� $��1��6   $��0��6$   $��Stretch=�6�   ��64   �� $ TgItemsSource� �� SelectedItem 6_   �� $ SelectedTgItem� �� Mode$ TwoWay=� �� View 5*      �� 5+      .P�5,      �� $
��Name��6)    �� Width$	 400��67    �� DisplayMemberBinding 6C   �� *Name+�� 5-      $	 140��6)   $��Progress��65    �� CellTemplate 5.      �� 5/   "   .R�50   &   �  � Maximum$	 100Q�62   $	��120��6m    � Value 6@   �� $ Progress�$ OneWay=�6m   51   #   53      �� 54      $ 50��6)   $��Review��64    55      �� 56   "   .R�57   %   �� $��25��6S   $��25��6^   ��6,   �� $ ReviewCommand�.��58   )   �� $��R��6/   59   &   5:   #   5<      5=      ��5?      �� 5@      " ��6   .�5A      ��  �@��
IsSelected 67   �� $ 
IsSelected�5B      5D      �� 5E      $��2��6   $��0��6&    �� Orientation$ 
Horizontal=�66   $��Center=�6O   .+�5F      �� $��Add File��6   ��6,   �� $ AddTgFileCommand��� 5G      $��
Add Folder��6   ��6.   �� $ AddTgFolderCommand��� 5H      $��Remove File��6   ��6/   �� $ RemoveTgFileCommand��� 5I      $��
Clear List��6   ��6.   �� $ ClearTgFilesCommand�5J      5K      �� 5M   
   $��1��6   �� 5O   
   $��2��6   $ 
Horizontal=�6"   $��Center=�6;   .+�5P      �� $��Set Output Folder��6   ��61   �� $ SetOutputFolderCommand��� 5Q      $��Get Output Folder��6   ��61   �� $ LoadOutputFolderCommand��� 5R      $��Process��6   ��6'   �� $ ProcessCommand��� 5S      $��Batch Export��6   ��6,   �� $ ExportBatchCommand��� 5T      $��
Export all��6   ��6*   �� $ ExportAllCommand��� 5U      $��Settings��6   ��6(   �� $ BatchSettingsCommand��� 5V      $��Videos to run at once: ��6   $��Center=�67   �� 5W      $��Center=�6T    6   �� $ DegreesOfParallel�$ TwoWay=���5X      �� 5Y       �� Type" ��6   .s�5Z      ��1��6*   ��5[      2��6*   ��5\      3��6*   ��5]      4��6*   ��5^      5��6*   ��5_      6��6*   5`      5b      5c      5d      5e      