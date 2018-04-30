﻿using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : MetroWindow
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void Ok_click( object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LoginDialog_KeyDown(object sender, KeyEventArgs e)
        {
            // ... Test for Enter key.
            switch (e.Key)
            {
                case Key.Enter:
                    this.Ok_click(sender, e);
                    break;
                case Key.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
