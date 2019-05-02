using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace ARWT.Services
{
    public class ExcelService
    {
        public static string[,] GetRawData(string fileName)
        {
            Application excelApp = new Application();
            string[,] result;

            try
            {
                Workbook workBook = excelApp.Workbooks.Open(fileName);

                Worksheet worksheet = workBook.Sheets[1];
                Range xlRange = worksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                result = new string[rowCount, colCount];

                for (int i = 1; i <= rowCount; i++)
                {
                    for (int j = 1; j <= colCount; j++)
                    {
                        result[i - 1, j - 1] = xlRange.Cells[i, j].Value2;
                    }
                }
                workBook.Close(false, fileName, null);
                Marshal.ReleaseComObject(workBook);
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return result;
        }

        public static T?[,] GetData<T>(string fileName) where T : struct
        {
            Application excelApp = new Application();
            T?[,] result;

            try
            {
                Workbook workBook = excelApp.Workbooks.Open(fileName);

                Worksheet worksheet = workBook.Sheets[1];
                Range xlRange = worksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                result = new T?[rowCount,colCount];

                for (int i = 1; i <= rowCount; i++)
                {
                    for (int j = 1; j <= colCount; j++)
                    {
                        if (xlRange.Cells[i, j].Value2 is T)
                        {
                            result[i - 1, j - 1] = (T)xlRange.Cells[i, j].Value2;
                        }
                        else
                        {
                            result[i - 1, j - 1] = null;
                        }
                    }
                }
                workBook.Close(false, fileName, null);
                Marshal.ReleaseComObject(workBook);
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return result;
        }

        public static T?[,] GetData<T>(string fileName, out int rowCount, out int columnCount) where T : struct
        {
            Application excelApp = new Application();
            T?[,] result;
            rowCount = 0;
            columnCount = 0;

            try
            {
                Workbook workBook = excelApp.Workbooks.Open(fileName);

                Worksheet worksheet = workBook.Sheets[1];
                Range xlRange = worksheet.UsedRange;

                rowCount = xlRange.Rows.Count;
                columnCount = xlRange.Columns.Count;

                result = new T?[rowCount, columnCount];

                for (int i = 1; i <= rowCount; i++)
                {
                    for (int j = 1; j <= columnCount; j++)
                    {
                        if (xlRange.Cells[i, j].Value is T)
                        {
                            result[i - 1, j - 1] = (T)xlRange.Cells[i, j].Value;
                        }
                        else
                        {
                            result[i - 1, j - 1] = null;
                        }
                    }
                }
                workBook.Close(false, fileName, null);
                Marshal.ReleaseComObject(workBook);
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            return result;
        }

        public static void WriteData<T>(T?[,] data, string fileName) where T : struct
        {
            Application excelApp = new Application();

            if (excelApp == null)
            {
                Console.WriteLine("Failed to open excel");
                return;
            }

            Workbook workBook = excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = (Worksheet)workBook.Worksheets[1];

            int rowCount = data.GetLength(0);
            int columnCount = data.GetLength(1);

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    if (data[i - 1, j - 1].HasValue)
                    {
                        ws.Cells[i, j] = data[i - 1, j - 1].Value;
                    }
                }
            }

            workBook.SaveAs(fileName);
            excelApp.Quit();
            Marshal.ReleaseComObject(workBook);
            Marshal.ReleaseComObject(ws);
        }

        public static void WriteData<T>(T[,] data, string fileName) where T : struct
        {
            Application excelApp = new Application();

            if (excelApp == null)
            {
                Console.WriteLine("Failed to open excel");
                return;
            }

            Workbook workBook = excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = (Worksheet)workBook.Worksheets[1];

            int rowCount = data.GetLength(0);
            int columnCount = data.GetLength(1);

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    ws.Cells[i, j] = data[i - 1, j - 1];
                }
            }

            workBook.SaveAs(fileName);
            excelApp.Quit();
            Marshal.ReleaseComObject(workBook);
            Marshal.ReleaseComObject(ws);
        }

        public static void WriteData<T>(T[,] data, string fileName, bool isClass, bool displayWarnings = true) where T : class
        {
            Application excelApp = new Application();

            if (excelApp == null)
            {
                Console.WriteLine("Failed to open excel");
                return;
            }

            excelApp.DisplayAlerts = displayWarnings;
            Workbooks workbooks = excelApp.Workbooks;
            Workbook workBook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            
            Worksheet ws = (Worksheet)workBook.Worksheets[1];

            int rowCount = data.GetLength(0);
            int columnCount = data.GetLength(1);

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    ws.Cells[i, j] = data[i - 1, j - 1];
                }
            }

            FileInfo fileInfo = new FileInfo(fileName);
            while (true)
            {
                if (IsFileLocked(fileInfo))
                {
                    var result = MessageBox.Show(string.Format("File: {0} is in use by another process, please terminate the process before saving", fileName), "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    break;
                }
            }
            

            workBook.SaveAs(fileName);
            workBook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(workBook);
            Marshal.ReleaseComObject(workbooks);
            Marshal.ReleaseComObject(ws);
            Marshal.ReleaseComObject(excelApp);
        }

        public static void WriteData(object[,] data, string fileName, bool displayWarnings = true)
        {
            Application excelApp = new Application();

            if (excelApp == null)
            {
                Console.WriteLine("Failed to open excel");
                return;
            }

            excelApp.DisplayAlerts = displayWarnings;
            Workbooks workbooks = excelApp.Workbooks;
            Workbook workBook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            Worksheet ws = (Worksheet)workBook.Worksheets[1];

            int rowCount = data.GetLength(0);
            int columnCount = data.GetLength(1);

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    ws.Cells[i, j] = data[i - 1, j - 1];
                }
            }

            FileInfo fileInfo = new FileInfo(fileName);
            while (true)
            {
                if (IsFileLocked(fileInfo))
                {
                    var result = MessageBox.Show(string.Format("File: {0} is in use by another process, please terminate the process before saving", fileName), "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    break;
                }
            }


            workBook.SaveAs(fileName);
            workBook.Close();
            excelApp.Quit();
            Marshal.ReleaseComObject(workBook);
            Marshal.ReleaseComObject(workbooks);
            Marshal.ReleaseComObject(ws);
            Marshal.ReleaseComObject(excelApp);
        }

        public static bool IsFileLocked(FileInfo file)
        {
            if (!File.Exists(file.Name))
            {
                return false;
            }

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }


        public static void WriteDataAsCsv<T>(T[,] data, string fileName, bool displayWarnings = true) where T : class
        {
            Application excelApp = new Application();

            if (excelApp == null)
            {
                Console.WriteLine("Failed to open excel");
                return;
            }

            excelApp.DisplayAlerts = displayWarnings;
            Workbook workBook = excelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = (Worksheet)workBook.Worksheets[1];

            int rowCount = data.GetLength(0);
            int columnCount = data.GetLength(1);

            for (int i = 1; i <= rowCount; i++)
            {
                for (int j = 1; j <= columnCount; j++)
                {
                    ws.Cells[i, j] = data[i - 1, j - 1];
                }
            }

            workBook.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlCSVWindows);
            excelApp.Quit();
            Marshal.ReleaseComObject(workBook);
            Marshal.ReleaseComObject(ws);
        }
    }
}
