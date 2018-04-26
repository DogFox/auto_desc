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

using System.Data.OleDb;
using System.Data.SqlClient;


namespace WpfApp2
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class ExcelImport
    { 
        private part new_part = new part();
        private PartsDataContext pdc = new PartsDataContext();

        public void OpenClick_OLD(object sender, RoutedEventArgs e)
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

            this.importdatafromexcel(openDialog.FileName);
        }

        public void importdatafromexcel(string excelfilepath)
        {
            //declare variables - edit these based on your particular situation
            string ssqltable = "parts_import";
            // make sure your sheet name is correct, here sheet name is sheet1, so you can change your sheet name if have different
            //string myexceldataquery = "select ГРУППА, [№ ПРОИЗВ.], НАИМЕНОВАНИЕ, МОДЕЛЬ, [ЦЕНА, РУБ], НАЛичие, КРАТНОСТЬ, КОД from [price$]";
            string myexceldataquery = "select * from [price$]";
            try
            {
                //create our connection strings
                string sexcelconnectionstring = @"provider=Microsoft.ACE.OLEDB.12.0;data source=" + excelfilepath +
                ";extended properties=" + "\"excel 8.0;hdr=yes;\"";
                string ssqlconnectionstring = "Data Source=46.229.187.177\\WIN-15HHO7JCTJO,1433;Initial Catalog=auto76;Persist Security Info=True;User ID=EXCEL_IMPORT;Password=123456789";
                //execute a query to erase any previous data from our destination table
                string sclearsql = "delete from " + ssqltable;
                SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
                SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
                sqlconn.Open();
                sqlcmd.ExecuteNonQuery();
                sqlconn.Close();
                //series of commands to bulk copy data from the excel file into our sql table
                OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
                OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);
                oledbconn.Open();
                OleDbDataReader dr = oledbcmd.ExecuteReader();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(ssqlconnectionstring);
                bulkcopy.DestinationTableName = ssqltable;
                while (dr.Read())
                {
                    bulkcopy.WriteToServer(dr);
                }
                dr.Close();
                oledbconn.Close();

                var ins_row = "insert into dbo.parts " +
                               "( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) " +
                               "select c1, c2, c3, c4, c5, c6, c7, c8, 1 from dbo.parts_import" ;
                ConnectToBase.ExecuteQuery(ins_row);

            }
            catch (Exception ex)
            {
                //handle exception
            }
        }
    }
}
