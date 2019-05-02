/*Automated Rodent Tracker - A program to automatically track rodents
Copyright(C) 2015 Brett Michael Hewitt

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see<http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using ARWT.Commands;
using ARWT.Model.Results;
using ARWT.ModelInterface.Datasets;
using ARWT.ModelInterface.Results;
using ARWT.ModelInterface.Smoothing;
using ARWT.Repository;
using ARWT.RepositoryInterface;
using ARWT.Resolver;
using ARWT.Services;
using ARWT.View.BatchProcess.Export;
using ARWT.View.Settings;
using ARWT.ViewModel.BatchProcess.BatchExport;
using ARWT.ViewModel.Datasets;
using ARWT.ViewModel.Settings;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Point = System.Drawing.Point;

namespace ARWT.ViewModel.BatchProcess
{
    public class BatchProcessViewModel : WindowBaseModel
    {
        private ActionCommand m_AddTgFileCommand;
        private ActionCommand m_AddTgFolderCommand;
        private ActionCommand m_RemoveTgFileCommand;
        private ActionCommand m_ClearTgFilesCommand;
        private ActionCommand m_ProcessCommand;
        private ActionCommand m_SetOutputFolderCommand;
        private ActionCommand m_LoadOutputFolderCommand;
        private ActionCommand m_ExportAllCommand;
        private ActionCommand m_BatchSettingsCommand;
        //private ActionCommandWithParameter m_ClosingCommand;

        public ActionCommand AddTgFileCommand
        {
            get
            {
                return m_AddTgFileCommand ?? (m_AddTgFileCommand = new ActionCommand()
                {
                    ExecuteAction = AddTgFile,
                    CanExecuteAction = CanButtonExecute,
                });
            }
        }

        public ActionCommand AddTgFolderCommand
        {
            get
            {
                return m_AddTgFolderCommand ?? (m_AddTgFolderCommand = new ActionCommand()
                {
                    ExecuteAction = AddTgFolder,
                    CanExecuteAction = CanButtonExecute,
                });
            }
        }

        public ActionCommand RemoveTgFileCommand
        {
            get
            {
                return m_RemoveTgFileCommand ?? (m_RemoveTgFileCommand = new ActionCommand()
                {
                    ExecuteAction = RemoveTgFile,
                    CanExecuteAction = CanRemoveTgFile,
                });
            }
        }

        public ActionCommand ClearTgFilesCommand
        {
            get
            {
                return m_ClearTgFilesCommand ?? (m_ClearTgFilesCommand = new ActionCommand()
                {
                    ExecuteAction = ClearTgList,
                    CanExecuteAction = CanButtonExecute,
                });
            }
        }

        public ActionCommand ProcessCommand
        {
            get
            {
                return m_ProcessCommand ?? (m_ProcessCommand = new ActionCommand()
                {
                    ExecuteAction = ProcessVideos,
                    CanExecuteAction = CanProcessVideos,
                });
            }
        }

        public ActionCommand SetOutputFolderCommand
        {
            get
            {
                return m_SetOutputFolderCommand ?? (m_SetOutputFolderCommand = new ActionCommand()
                {
                    ExecuteAction = SetOutputFolder,
                    CanExecuteAction = CanSetOutputFolder,
                });
            }
        }

        public ActionCommand LoadOutputFolderCommand
        {
            get
            {
                return m_LoadOutputFolderCommand ?? (m_LoadOutputFolderCommand = new ActionCommand()
                {
                    ExecuteAction = LoadOutputFolder,
                    CanExecuteAction = CanLoadOutputFolder,
                });
            }
        }

        public ActionCommand ExportAllCommand
        {
            get
            {
                return m_ExportAllCommand ?? (m_ExportAllCommand = new ActionCommand()
                {
                    ExecuteAction = ExportAll,
                    CanExecuteAction = CanButtonExecute,
                });
            }
        }

        public ActionCommand BatchSettingsCommand
        {
            get
            {
                return m_BatchSettingsCommand ?? (m_BatchSettingsCommand = new ActionCommand()
                {
                    ExecuteAction = ShowBatchSettings,
                    CanExecuteAction = CanShowBatchSettings,
                });
            }
        }

        private ObservableCollection<SingleMouseViewModel> m_TgItemsSource;
        private SingleMouseViewModel m_SelectedTgItem;

        public static object TgLock = new object();
        public ObservableCollection<SingleMouseViewModel> TgItemsSource
        {
            get
            {
                lock (TgLock)
                {
                    return m_TgItemsSource;
                }
            }
            set
            {
                if (Equals(m_TgItemsSource, value))
                {
                    return;
                }

                m_TgItemsSource = value;

                NotifyPropertyChanged();
                LoadOutputFolderCommand.RaiseCanExecuteChangedNotification();
                BatchSettingsCommand.RaiseCanExecuteChangedNotification();
            }
        }

        public SingleMouseViewModel SelectedTgItem
        {
            get
            {
                return m_SelectedTgItem;
            }
            set
            {
                if (Equals(m_SelectedTgItem, value))
                {
                    return;
                }

                m_SelectedTgItem = value;

                NotifyPropertyChanged();
                RemoveTgFileCommand.RaiseCanExecuteChangedNotification();
            }
        }

        private bool m_Running = false;
        public bool Running
        {
            get
            {
                return m_Running;
            }
            set
            {
                if (Equals(m_Running, value))
                {
                    return;
                }

                m_Running = value;

                NotifyPropertyChanged();
                RaiseButtonNotifications();
            }
        }

        private string m_OutputFolder = "";
        public string OutputFolder
        {
            get
            {
                return m_OutputFolder;
            }
            set
            {
                if (Equals(m_OutputFolder, value))
                {
                    return;
                }

                m_OutputFolder = value;

                NotifyPropertyChanged();
            }
        }

        public BatchProcessViewModel()
        {
            //LookForLabbook();
            ResetData();
        }

        private void ResetData()
        {
            ClearTgList();
        }

        private void ClearTgList()
        {
            SelectedTgItem = null;
            TgItemsSource = new ObservableCollection<SingleMouseViewModel>();
            BatchSettingsCommand.RaiseCanExecuteChangedNotification();
        }

        private void AddTgFile()
        {
            string fileLocation = FileBrowser.BroseForVideoFiles();

            if (string.IsNullOrWhiteSpace(fileLocation))
            {
                return;
            }


            ISingleMouse newFile = ModelResolver.Resolve<ISingleMouse>();
            newFile.AddFile(GetSingleFile(fileLocation));
            newFile.Name = Path.GetFileNameWithoutExtension(fileLocation);
            newFile.Reviewed = "Video Has not been reviewed";

            SingleMouseViewModel viewModel = new SingleMouseViewModel(newFile);

            ObservableCollection<SingleMouseViewModel> currentList = new ObservableCollection<SingleMouseViewModel>(TgItemsSource);
            currentList.Add(viewModel);
            TgItemsSource = currentList;
        }

        private ISingleFile GetSingleFile(string fileName)
        {
            ISingleFile singleFile = ModelResolver.Resolve<ISingleFile>();

            singleFile.VideoFileName = fileName;
            singleFile.VideoNumber = Path.GetFileNameWithoutExtension(fileName);

            return singleFile;
        }

        private void AddTgFolder()
        {
            string folderLocation =  FileBrowser.BrowseForFolder();

            if (string.IsNullOrWhiteSpace(folderLocation))
            {
                return;
            }

            ObservableCollection<SingleMouseViewModel> currentList = new ObservableCollection<SingleMouseViewModel>(TgItemsSource);

            string[] files = Directory.GetFiles(folderLocation);

            foreach (string file in files)
            {
                string extension = Path.GetExtension(file);

                if (CheckIfExtensionIsVideo(extension))
                {
                    ISingleMouse newFile = ModelResolver.Resolve<ISingleMouse>();
                    newFile.AddFile(GetSingleFile(file));
                    newFile.Name = Path.GetFileNameWithoutExtension(file);
                    newFile.Reviewed = "Video Has not been reviewed";
                    currentList.Add(new SingleMouseViewModel(newFile));

                }
            }

            TgItemsSource = currentList;
        }

        private bool CheckIfExtensionIsVideo(string extension)
        {
            return extension.Contains("avi") || extension.Contains("mpg") || extension.Contains("mpeg") || extension.Contains("mp4") || extension.Contains("mov");
        }

        private void RemoveTgFile()
        {
            TgItemsSource.Remove(SelectedTgItem);
            NotifyPropertyChanged("TgItemsSource");
            BatchSettingsCommand.RaiseCanExecuteChangedNotification();
        }

        private bool CanRemoveTgFile()
        {
            if (SelectedTgItem == null || Running)
            {
                return false;
            }

            return true;
        }
        
        private static object lockObject = new object();
        private bool m_Continue = true;

        public bool Continue
        {
            get
            {
                lock (lockObject)
                {
                    return m_Continue;
                }
            }
            set
            {
                lock (lockObject)
                {
                    m_Continue = value;
                }
            }
        }

        private int _DegreesOfParallel = 1;
        public int DegreesOfParallel
        {
            get
            {
                return _DegreesOfParallel;
            }
            set
            {
                if (Equals(_DegreesOfParallel, value))
                {
                    return;
                }

                _DegreesOfParallel = value;

                NotifyPropertyChanged();
            }
        }

        private object RunningLockObject = new object();
        private bool _CheckRunning;

        public bool CheckRunning
        {
            get
            {
                lock (RunningLockObject)
                {
                    return _CheckRunning;
                }
            }
            set
            {
                lock (RunningLockObject)
                {
                    _CheckRunning = value;
                }
            }
        }

        private void RunVideo()
        {
            IEnumerable<SingleMouseViewModel> allMice = TgItemsSource;
            List<string> errorMsgs = new List<string>();
            //Parallel.ForEach(allMice, new ParallelOptions() {MaxDegreeOfParallelism = DegreesOfParallel }, (mouse, state) =>
            foreach (SingleMouseViewModel mouse in allMice)
            {
                if (!Running)
                {
                    return;
                }


                CheckRunning = true;
                Task.Factory.StartNew(() => mouse.RunFiles(OutputFolder)).ContinueWith(x =>
                {
                    if (x.Status != TaskStatus.RanToCompletion)
                    {
                        errorMsgs.Add(mouse.Videos[0]);
                    }

                    CheckRunning = false;
                });

                while (CheckRunning)
                {

                }
                
            }//);

            Application.Current.Dispatcher.Invoke(() =>
            {
                StringBuilder sb = new StringBuilder();
                if (errorMsgs.Any())
                {
                    sb.AppendLine("The following videos encountered errors");
                    foreach (string error in errorMsgs)
                    {
                        sb.AppendLine(error);
                    }

                    MessageBox.Show(sb.ToString());

                }
                Running = false;
                //ExportAll();
            });
        }

        private void GenerateAllResults()
        {
            string saveLocation = FileBrowser.SaveFile("Excel|*.xlsx");

            if (string.IsNullOrWhiteSpace(saveLocation))
            {
                return;
            }

            List<IMouseDataExtendedResult> tgResultsList = new List<IMouseDataExtendedResult>();

            foreach (SingleMouseViewModel mouse in TgItemsSource)
            {
                if (mouse.Results !=null)
                {
                    tgResultsList.AddRange(mouse.VideoFiles.Select(singleFile => mouse.Results[singleFile]));
                }
                
            }

            IMouseDataExtendedResult[] tgResults = tgResultsList.ToArray();

            int rows = tgResults.Length + 10;
            const int columns = 19;

            object[,] finalResults = new object[rows, columns];

            finalResults[0, 0] = "Name";
            finalResults[0, 1] = "Start Frame";
            finalResults[0, 2] = "End Frame";
            finalResults[0, 3] = "Duration";
            finalResults[0, 4] = "Distance";
            finalResults[0, 5] = "Centroid Distance";
            finalResults[0, 6] = "Max Velocity";
            finalResults[0, 7] = "Max Centroid Velocity";
            finalResults[0, 8] = "Max Ang Velocity";
            finalResults[0, 9] = "Average Velocity";
            finalResults[0, 10] = "Average Centroid Velocity";
            finalResults[0, 11] = "Average Angular Velocity";
            finalResults[0, 12] = "Time spent running";
            finalResults[0, 13] = "Time spent walking";
            finalResults[0, 14] = "Time spent still";
            finalResults[0, 15] = "Time spent moving";
            finalResults[0, 16] = "Time spent rotating";
            finalResults[0, 17] = "Time spent not rotating";
            finalResults[0, 18] = "Time spent interacting";
            //Time spent running Time spent walking  Time spent still Time spent moving   Time spent rotating time spent not rotating Time spent interacting
            try
            {
                int tgLength = tgResults.Length;
                for (int i = 1; i <= tgLength; i++)
                {
                    finalResults[i, 0] = tgResults[i - 1].Name;
                    finalResults[i, 1] = tgResults[i - 1].StartFrame;
                    finalResults[i, 2] = tgResults[i - 1].EndFrame;
                    finalResults[i, 3] = tgResults[i - 1].Duration;
                    finalResults[i, 4] = tgResults[i - 1].DistanceTravelled;
                    finalResults[i, 5] = tgResults[i - 1].CentroidDistanceTravelled;
                    finalResults[i, 6] = tgResults[i - 1].MaxSpeed;
                    finalResults[i, 7] = tgResults[i - 1].MaxCentroidSpeed;
                    finalResults[i, 8] = tgResults[i - 1].MaxAngularVelocty;
                    finalResults[i, 9] = tgResults[i - 1].AverageVelocity;
                    finalResults[i, 10] = tgResults[i - 1].AverageCentroidVelocity;
                    finalResults[i, 11] = tgResults[i - 1].AverageAngularVelocity;
                }

                ExcelService.WriteData(finalResults, saveLocation, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //try
            //{
            //    GenerateBatchResults();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }

        public struct BrettTuple<T1, T2>
        {
            public readonly T1 Item1;
            public readonly T2 Item2;

            public BrettTuple(T1 item1, T2 item2)
            {
                Item1 = item1;
                Item2 = item2;
            }
        }

        private bool CanProcessVideos()
        {
            return !Running;
        }

        private void ProcessVideos()
        {
            if (string.IsNullOrWhiteSpace(OutputFolder))
            {
                var result = MessageBox.Show("Results will be saved in the same folder as the videos", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (!Running)
            {
                ResetProgress();

                Running = true;
                Task.Factory.StartNew(RunVideo).ContinueWith(x =>
                {
                    if (x.IsFaulted) throw x.Exception;
                });
                
                //RunVideo();
            }
            else
            {
                Continue = false;
                Running = false;
            }
        }

        private void ResetProgress()
        {
            foreach (var mouse in TgItemsSource)
            {
                mouse.ResetProgress();
            }
        }

        private void SetOutputFolder()
        {
            string folderLocation = FileBrowser.BrowseForFolder();

            if (string.IsNullOrWhiteSpace(folderLocation))
            {
                return;
            }

            OutputFolder = folderLocation;
        }

        private bool CanSetOutputFolder()
        {
            return !Running;
        }

        protected override void WindowClosing(CancelEventArgs closingEventArgs)
        {
            if (!Running)
            {
                return;
            }

            var result = MessageBox.Show("The program is currently running, are you sure you want to cancel it?", "Batch Running", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                IEnumerable<SingleMouseViewModel> allMice = TgItemsSource;
                foreach (SingleMouseViewModel mouse in allMice)
                {
                    mouse.Stop = true;
                }
                Running = false;
                return;
            }

            if (closingEventArgs != null)
            {
                closingEventArgs.Cancel = true;
            }
        }

        //private void Closing(object args)
        //{
        //    if (!Running)
        //    {
        //        return;
        //    }

        //    var result = MessageBox.Show("The program is currently running, are you sure you want to cancel it?", "Batch Running", MessageBoxButton.YesNo, MessageBoxImage.Question);

        //    if (result == MessageBoxResult.Yes)
        //    {
        //        IEnumerable<SingleMouseViewModel> allMice = TgItemsSource.Concat(NtgItemsSource);
        //        foreach (SingleMouseViewModel mouse in allMice)
        //        {
        //            mouse.Stop = true;
        //        }
        //        Running = false;
        //        return;
        //    }

        //    CancelEventArgs cancelEventArgs = args as CancelEventArgs;
        //    if (cancelEventArgs != null)
        //    {
        //        cancelEventArgs.Cancel = true;
        //    }
        //}

        private void LoadOutputFolder()
        {
            IRepository repo = RepositoryResolver.Resolve<IRepository>();
            string initialLocation = repo.GetValue<string>("OutputFolderLocation");

            string folderLocation = FileBrowser.BrowseForFolder(initialLocation);

            if (string.IsNullOrWhiteSpace(folderLocation))
            {
                return;
            }

            OutputFolder = folderLocation;
            //repo.SetValue("OutputFolderLocation", folderLocation);
            //repo.Save();

            ProcessVideos();
            
            ExportAllCommand.RaiseCanExecuteChangedNotification();
        }

        private void ExportAll()
        {
            var msgBoxResult = MessageBox.Show("Would you like to export the results?", "Save results?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (msgBoxResult == MessageBoxResult.Yes)
            {
                GenerateAllResults();
            }
        }

        private bool CanLoadOutputFolder()
        {
            //return !string.IsNullOrWhiteSpace()
            if (Running)
            {
                return false;
            }

            return TgItemsSource.Count > 0;
        }

        private void ShowBatchSettings()
        {
            SingleMouseViewModel[] selectedModels = GetSelectedViewModels();

            if (!selectedModels.Any())
            {
                selectedModels = TgItemsSource.ToArray();
            }
            
            SettingsView view = new SettingsView();
            SettingsViewModel viewModel = new SettingsViewModel(selectedModels);
            view.DataContext = viewModel;
            view.ShowDialog();

            if (viewModel.ExitResult != WindowExitResult.Ok)
            {
                return;
            }

            foreach (SingleMouseViewModel mouse in selectedModels)
            {
                mouse.WhiskerSettings = viewModel.WhiskerSettings;
                mouse.FootSettings = viewModel.FootVideoSettings;
                mouse.GapDistance = viewModel.GapDistance;
                mouse.ThresholdValue = viewModel.BinaryThreshold;
                mouse.ThresholdValue2 = viewModel.BinaryThreshold2;
                mouse.SmoothMotion = viewModel.SmoothMotion;
                mouse.FrameRate = viewModel.FrameRate;
                mouse.ROI = viewModel.ROI;
            }
        }

        private SingleMouseViewModel[] GetSelectedViewModels()
        {
            return TgItemsSource.Where(x => x.IsSelected).ToArray();
        }

        private ActionCommand _ExportBatchCommand;

        public ActionCommand ExportBatchCommand
        {
            get
            {
                return _ExportBatchCommand ?? (_ExportBatchCommand = new ActionCommand()
                {
                    ExecuteAction = ExportBatch,
                    CanExecuteAction = CanButtonExecute,
                });
            }
        }

        private void ExportBatch()
        {
            BatchExportViewModel viewModel = new BatchExportViewModel(TgItemsSource);
            BatchExportView view = new BatchExportView();
            view.DataContext = viewModel;
            view.ShowDialog();
        }
        

        private bool CanShowBatchSettings()
        {
            if (Running)
            {
                return false;
            }

            return TgItemsSource.Any();
        }

        private void RaiseButtonNotifications()
        {
            AddTgFileCommand.RaiseCanExecuteChangedNotification();
            AddTgFolderCommand.RaiseCanExecuteChangedNotification();
            RemoveTgFileCommand.RaiseCanExecuteChangedNotification();
            ClearTgFilesCommand.RaiseCanExecuteChangedNotification();
            ProcessCommand.RaiseCanExecuteChangedNotification();
            SetOutputFolderCommand.RaiseCanExecuteChangedNotification();
            LoadOutputFolderCommand.RaiseCanExecuteChangedNotification();
            ExportAllCommand.RaiseCanExecuteChangedNotification();
            BatchSettingsCommand.RaiseCanExecuteChangedNotification();
        }

        private bool CanButtonExecute()
        {
            return !Running;
        }
    }
}
