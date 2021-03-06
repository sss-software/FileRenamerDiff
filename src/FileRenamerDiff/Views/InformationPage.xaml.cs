﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileRenamerDiff.Views
{
    /// <summary>
    /// InformationPage.xaml の相互作用ロジック
    /// </summary>
    public partial class InformationPage : UserControl
    {
        public InformationPage()
        {
            InitializeComponent();
        }

        private void OpenHyperlink(object sender, ExecutedRoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {e.Parameter}") { CreateNoWindow = true });
        }
    }
}
