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
using System.Globalization;

namespace WpfApp2
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class ExcelImport
    {
        private ConnectToBase bc = new ConnectToBase();
        private part new_part = new part();
        private PartsDataContext pdc = new PartsDataContext();

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

            Workbook workbook = new Workbook();
            workbook.LoadFromFile(openDialog.FileName);
            Worksheet sheet = workbook.Worksheets[0];
            DataTable dataTable = sheet.ExportDataTable();

            for (int index = 2; index < dataTable.Rows.Count - 1; index++)
            {
                /*var ins_row = "insert into dbo.parts " +
                                "( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) " +
                                "values( '" + dataTable.Rows[index][0].ToString() + "', '" + dataTable.Rows[index][1].ToString() + "', '" + dataTable.Rows[index][2].ToString() +
                                        "', '" + dataTable.Rows[index][3].ToString() + "', '" + dataTable.Rows[index][4].ToString() + "', '" + dataTable.Rows[index][5].ToString() +
                                        "', '" + dataTable.Rows[index][6].ToString() + "', '" + dataTable.Rows[index][7].ToString() + "', 1 )";

                bc.ExecuteQuery(ins_row);*/
                new_part = new part();

                new_part.producer = dataTable.Rows[index][0].ToString();
                new_part.part_number = dataTable.Rows[index][1].ToString();
                new_part.name = dataTable.Rows[index][2].ToString();
                new_part.model = dataTable.Rows[index][3].ToString();
                new_part.sup_price = Convert.ToSingle( dataTable.Rows[index][4].ToString(), CultureInfo.InvariantCulture);
                new_part.count = Convert.ToInt32( dataTable.Rows[index][5].ToString());
                new_part.ratio = Convert.ToInt32( dataTable.Rows[index][6].ToString());
                new_part.code = dataTable.Rows[index][7].ToString();
                new_part.sup_id = 1;

                this.pdc.parts.InsertOnSubmit(new_part);
                this.pdc.SubmitChanges();
            }
        }
    }
}
