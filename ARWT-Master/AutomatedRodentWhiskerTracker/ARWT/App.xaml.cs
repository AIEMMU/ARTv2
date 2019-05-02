using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ARWT.View;
using ARWT.ViewModel;

namespace ARWT
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppDomain.CurrentDomain.UnhandledException += ApplicationOnDispatcherUnhandledException;

            MainWindow view = new MainWindow();
            MainWindowViewModel viewModel = new MainWindowViewModel();
            view.DataContext = viewModel;
            view.Show();
        }

        private void ApplicationOnDispatcherUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            MessageBox.Show($"An error has occured, a crash log has been generated at {Environment.CurrentDirectory}\\CrashLog.txt", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            Exception e = (Exception)args.ExceptionObject;
            StringBuilder sb = new StringBuilder();

            sb.Append(e.Message);
            sb.AppendLine();
            sb.Append(e.StackTrace);
            sb.AppendLine();

            string filePath = @"CrashLog.txt";

            if (File.Exists(filePath))
            {
                sb.AppendLine();
                sb.Append(File.ReadAllText(filePath));
                File.WriteAllText(filePath, sb.ToString());
            }
            else
            {
                File.WriteAllText(filePath, sb.ToString());
            }
        }
    }
}
