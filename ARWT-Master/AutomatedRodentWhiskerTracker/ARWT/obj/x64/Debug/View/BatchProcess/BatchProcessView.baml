   M S B A M L   `   `   `  ����?  ;ARWT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ARWT.BehavioursARWT.Behaviours  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   -    'ARWT.View.BatchProcess.BatchProcessView   5   	   U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4
behavioursclr-namespace:ARWT.Behaviours     5   	   9system&clr-namespace:System;assembly=mscorlib     5   	     � Title$  BatchProcessView��5   	   $	��720��6"   $
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
��Auto �6    .+�5%      �� $��0��6   $��0��6&    �� Orientation$ 
Horizontal=�66   $��Center=�6O   .+�5&      �� $��Add File��6   ��6,   �� $ AddTgFileCommand��� 5'      $��
Add Folder��6   ��6.   �� $ AddTgFolderCommand��� 5(      $��Set Output Folder��6   ��65   �� $ SetOutputFolderCommand��� 5)      $��Get Output Folder��6   ��65   �� $ LoadOutputFolderCommand��� 5*      $��Remove File��6   ��6/   �� $ RemoveTgFileCommand��� 5+      $��
Clear List��6   ��6.   �� $ ClearTgFilesCommand��� 51      $��Settings��6   ��6,   �� $ BatchSettingsCommand��� 52      $��Process��6   ��6+   �� $ ProcessCommand��� 53      $��Export��6   ��6*   �� $ ExportBatchCommand��� 56      $��Videos to run at once: ��6   $��Center=�6;   �� 57      $��Center=�6X    �� SelectedItem 6   �� $ DegreesOfParallel� �� Mode$ TwoWay=���58      �� 59       �� Type" ��6"   .s�5:      ��1��6.   ��5;      2��6.   ��5<      3��6.   ��5=      4��6.   ��5>      5��6.   ��5?      6��6.   5@      5B      5C      �� 5G      $��1��6   $��0��6$   $��Stretch=�6�   ��64   �� $ TgItemsSource� 6_   �� $ SelectedTgItem�$ TwoWay=� �� View 5H      �� 5I      .P�5J      �� $
��Name��6)    �� Width$	 400��67    �� DisplayMemberBinding 6C   �� *Name+�� 5K      $	 140��6)   $��Progress��65    �� CellTemplate 5L      �� 5M   "   .R�5N   &   �  � Maximum$	 100Q�62   $	��120��6m    � Value 6@   �� $ Progress�$ OneWay=�6m   5O   #   5Q      �� 5R      $ 50��6)   $��Review��64    5S      �� 5T   "   .R�5U   %   �� $��25��6S   $��25��6^   ��6,   �� $ ReviewCommand�.��5V   )   �� $��R��6/   5W   &   5X   #   5Z      �� 5[      $��Video Has Been Reviewed��6)   $	 400��6J    6V   �� *
Reviewed+5\      ��5^      �� 5_      " ��6   .�5`      ��  �@��
IsSelected 67   �� $ 
IsSelected�5a      5c      5e      5j      5k      