﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

namespace ObjectVisualization
{
    /// <summary>
    /// ObjectWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ObjectWindow : Window
    {
        public ObjectWindow()
        {
            InitializeComponent();
        }
        
        public void AddData(UIElement newData)
        {
            StackPanel1.Children.Add(newData);
        }

        private void CloseMe(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
