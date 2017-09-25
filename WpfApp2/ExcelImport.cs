using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using Microsoft.Office.Interop.Excel;
using System.IO;
using Spire.Xls;
using System.Data;

namespace WpfApp2
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class ExcelImport  
    {
        private Microsoft.Office.Interop.Excel.Application ExcelApp;
        private Microsoft.Office.Interop.Excel.Workbook WorkBookExcel;
        private Microsoft.Office.Interop.Excel.Worksheet WorkSheetExcel;
        private Microsoft.Office.Interop.Excel.Range excelRange;

        public void OpenClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.Filter = "Файл Excel|*.XLSX;*.XLS;*.XLSM";
            var result = openDialog.ShowDialog();
            if (result == false)
            {
                MessageBox.Show("Файл не выбран!", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information);
                return;
            }
            //string fileName = System.IO.Path.GetFileName(openDialog.FileName);

            //ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            //Книга.
            //WorkBookExcel = ExcelApp.Workbooks.Open(openDialog.FileName);

            Workbook workbook = new Workbook();
            workbook.LoadFromFile(openDialog.FileName);
            Worksheet sheet = WorkBookExcel.Worksheets[0];
            DataTable dataTable = sheet.ExportDataTable();

            //Таблица.
            // WorkSheetExcel = ExcelApp.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;
            //    RangeExcel = null;
            WorkSheetExcel = (Microsoft.Office.Interop.Excel.Worksheet)WorkBookExcel.Sheets[1];


            excelRange = WorkSheetExcel.UsedRange;

            var lastCell = WorkSheetExcel.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);
            string[,] list = new string[lastCell.Column, lastCell.Row];

            for (int i = 0; i < (int)lastCell.Column; i++)
                for (int j = 0; j < (int)lastCell.Row; j++)
                    list[i, j] = WorkSheetExcel.Cells[j + 1, i + 1].Text.ToString();//считал текст в строку

            WorkBookExcel.Close(false, Type.Missing, Type.Missing); //закрыть не сохраняя
            ExcelApp.Quit(); // вышел из Excel
            GC.Collect(); // убрал за собой

        }
    }
}
