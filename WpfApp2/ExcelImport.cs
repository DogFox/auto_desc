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
//using Microsoft.Office.Interop.Excel;
using System.IO;
using Spire.Xls;
using System.Data;
using System.Globalization;
using System.Data.OleDb;
using System.Data.SqlClient;
using LumenWorks.Framework.IO.Csv;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public static class SqlBulkCopyExtensions
    {
        public static SqlBulkCopyColumnMapping AddColumnMapping(this SqlBulkCopy sbc, int sourceColumnOrdinal, int targetColumnOrdinal)
        {
            var map = new SqlBulkCopyColumnMapping(sourceColumnOrdinal, targetColumnOrdinal);
            sbc.ColumnMappings.Add(map);

            return map;
        }

        public static SqlBulkCopyColumnMapping AddColumnMapping(this SqlBulkCopy sbc, string sourceColumn, string targetColumn)
        {
            var map = new SqlBulkCopyColumnMapping(sourceColumn, targetColumn);
            sbc.ColumnMappings.Add(map);

            return map;
        }
    }

    public class ExcelImport
    { 
        public void OpenClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openDialog = new Microsoft.Win32.OpenFileDialog();
            openDialog.Filter = "Файл Excel|*.XLSX;*.XLS;*.XLSM;*.CSV";
            var result = openDialog.ShowDialog();
            if (result == false)
            {
                MessageBox.Show("Файл не выбран!", "Информация", MessageBoxButton.YesNo, MessageBoxImage.Information);
                return;
            }
            FileInfo file = new FileInfo(openDialog.FileName);
            StringComparison comp = StringComparison.OrdinalIgnoreCase;
            string name = file.Name;

            if (file.Extension != ".csv")
            {
                this.importdatafromexcel(openDialog.FileName);
            }
            else
            {
                bool headers = false;
                if (name.Contains("forum", comp))
                    headers = true;
                if (name.Contains("minsk", comp) || name.Contains("podolsk", comp))
                    headers = true;
                //try
                //{
                    this.importdatafromcsv(openDialog.FileName, headers);
                //}
                //catch( Exception ex)
                //{
                //     MessageBox.Show("Ошибка в файле прайса." , "Error", MessageBoxButton.OK);
                //}
            }

            string ins_row = "";
            string del_row = "";
            if (name.Contains( "forum", comp ) )
            {
                del_row = @"delete from dbo.parts
                            where sup_id = (select top 1 id from dbo.suppliers
										                            where name = 'Форум')";

                ins_row = @" insert into dbo.parts
                                ( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) 

                            select c1, c2, c3, '', isnull( Try_convert(float, c4 ), 0 ) c4
					                                , isnull( Try_convert(int, c5 ), 0 ) c5
					                                , isnull( Try_convert(int, c6 ), 0 ) c6, c7, (select top 1 id from dbo.suppliers
										                            where name = 'Форум')
                                from dbo.parts_import_csv
                                ";
            }
            if (name.Contains("mikado", comp))
            {
                del_row = @"delete from dbo.parts
                            where sup_id = (select top 1 id from dbo.suppliers
				                             where name = 'Микадо')";

                ins_row = @" insert into dbo.parts
                             ( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) 

                            select c3, c2, c4, '', isnull( Try_convert(float, c5 ), 0 ) c5
					                             , isnull( Try_convert(int, c7 ), 0 ) c7
					                             , isnull( Try_convert(int, c6 ), 0 ) c6, c1, (select top 1 id from dbo.suppliers
																	                            where name = 'Микадо')
                             from dbo.parts_import_csv
                                ";
            }
            if (name.Contains("minsk", comp) || name.Contains("podolsk", comp))
            {
                del_row = @"delete from dbo.parts
                            where sup_id = (select top 1 id from dbo.suppliers
				                             where name = 'Шате-М')";

                ins_row = @" insert into dbo.parts
                             ( producer, part_number, name, model, sup_price, count, ratio, code, sup_id ) 

                            select c1, c2, c3, '', isnull( Try_convert(float, c7 ), 0 ) c7
					                                , isnull( Try_convert(int, c4 ), 0 ) c4
					                                , isnull( Try_convert(int, c5 ), 0 ) c5, c8, (select top 1 id from dbo.suppliers
																	                            where name = 'Шате-М')
                             from dbo.parts_import_csv
                                ";

            }

            ConnectToBase.ExecuteQuery(del_row);
            ConnectToBase.ExecuteQuery(ins_row);


        }
        public void ClearTable( string table )
        {
            string ssqlconnectionstring = ConnectToBase.GetConnectionString();
            //execute a query to erase any previous data from our destination table
            string sclearsql = "delete from " + table;
            SqlConnection sqlconn = new SqlConnection(ssqlconnectionstring);
            SqlCommand sqlcmd = new SqlCommand(sclearsql, sqlconn);
            sqlconn.Open();
            sqlcmd.ExecuteNonQuery();
            sqlconn.Close();
        }
        public void importdatafromcsv(string excelfilepath, bool headers)
        {
            string ssqltable = "dbo.parts_import_csv";
            this.ClearTable(ssqltable);

            TextReader textReader = new StreamReader(excelfilepath, Encoding.GetEncoding("windows-1251"));
            //CsvReader csvReader = new CsvReader(new StreamReader(excelfilepath), false, '\t', '"', '"', '#', LumenWorks.Framework.IO.Csv.ValueTrimmingOptions.QuotedOnly);
            Encoding enc = Encoding.GetEncoding(1251);
            using (var reader = new CsvReader(textReader, headers, ';','\0','\0','#', LumenWorks.Framework.IO.Csv.ValueTrimmingOptions.QuotedOnly))
            {

                reader.Columns = new List<LumenWorks.Framework.IO.Csv.Column>
                {
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c1", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c2", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c3", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c4", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c5", Type = typeof(string) },
                   new LumenWorks.Framework.IO.Csv.Column { Name = "c6", Type = typeof(string) },
                };

                // Now use SQL Bulk Copy to move the data
                using (var sbc = new SqlBulkCopy(ConnectToBase.GetConnectionString()))
                {
                    sbc.DestinationTableName = ssqltable;
                    sbc.BatchSize = 10000;
                    sbc.WriteToServer(reader);
                }
            }
        }

        public void importdatafromexcel(string excelfilepath)
        {
            FileInfo file = new FileInfo(excelfilepath);
            //declare variables - edit these based on your particular situation
            try
            {
            
                string sexcelconnectionstring;
                string myexceldataquery;
                myexceldataquery = "select * from [Sheet1$]";

                sexcelconnectionstring = @"provider=Microsoft.ACE.OLEDB.12.0;data source=" + excelfilepath +
                                                        ";extended Properties=\"Excel 12.0 Macro; HDR = Yes; IMEX = 1\"";

                string ssqltable = "dbo.parts_import";
                this.ClearTable(ssqltable);
                //series of commands to bulk copy data from the excel file into our sql table
                OleDbConnection oledbconn = new OleDbConnection(sexcelconnectionstring);
                OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, oledbconn);
                oledbconn.Open();
                OleDbDataReader dr = oledbcmd.ExecuteReader();
                SqlBulkCopy bulkcopy = new SqlBulkCopy(ConnectToBase.GetConnectionString());
                bulkcopy.DestinationTableName = ssqltable;
                while (dr.Read())
                {
                    bulkcopy.WriteToServer(dr);
                }
                dr.Close();
                oledbconn.Close();
            }
            catch (Exception ex)
            {
                //handle exception
            }
        }
    }
}
