using System;
using System.Collections.Generic;
using System.Linq;
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
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class XmlWindow : Window
    {
        public FrameworkElement VariableElement { get; set; }
        public FrameworkElement BrowserElement { get; set; }

        private FrameworkElement SplitterElement;
        private FrameworkElement FirstContainer;
        private FrameworkElement SecondContainer;
        private FrameworkElement TempContainer;

        public XmlWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 1つ目のデータ
            var scrollViewer1 = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            };
            
            scrollViewer1.Content = VariableElement;
            VariableElement = scrollViewer1;
            FirstContainer = scrollViewer1;

            // 2つ目のデータ
            var scrollViewer2 = new ScrollViewer()
            {
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            };
            scrollViewer2.Content = BrowserElement;
            BrowserElement = scrollViewer2;
            SecondContainer = scrollViewer2;
            
            // Grid の作成
            CreateHorizontalLayout();
        }

        private void BtnHorizontal_Click(object sender, RoutedEventArgs e)
        {
            CreateHorizontalLayout();
        }

        private void BtnVertical_Click(object sender, RoutedEventArgs e)
        {
            CreateVerticalLayout();
        }

        private void BtnSwap_Click(object sender, RoutedEventArgs e)
        {
            Swap();
            
            if (1 < grid1.RowDefinitions.Count)
                CreateVerticalLayout();
            else
                CreateHorizontalLayout();
        }

        private void BtnOnlyVariable_Checked(object sender, RoutedEventArgs e)
        {
            if (btnOnlyBrowser.IsChecked == true)
                btnOnlyBrowser.IsChecked = false;

            ShowOnlyVariable();
        }

        private void BtnOnlyVariable_Unchecked(object sender, RoutedEventArgs e)
        {
            ReVisibleLayout();
        }

        private void BtnOnlyBrowser_Checked(object sender, RoutedEventArgs e)
        {
            if (btnOnlyVariable.IsChecked == true)
                btnOnlyVariable.IsChecked = false;

            ShowOnlyWebBrowser();
        }

        private void BtnOnlyBrowser_Unchecked(object sender, RoutedEventArgs e)
        {
            ReVisibleLayout();
        }

        // 左右レイアウト
        private void CreateHorizontalLayout()
        {
            if (btnOnlyBrowser.IsChecked == true)
                btnOnlyBrowser.IsChecked = false;

            if (btnOnlyVariable.IsChecked == true)
                btnOnlyVariable.IsChecked = false;

            grid1.Children.Clear();
            grid1.RowDefinitions.Clear();
            grid1.ColumnDefinitions.Clear();

            grid1.RowDefinitions.Add(new RowDefinition());
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            // 1つ目のデータ
            grid1.Children.Add(FirstContainer);
            Grid.SetRow(FirstContainer, 0);
            Grid.SetColumn(FirstContainer, 0);

            // 分割線
            var splitter1 = new GridSplitter()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 5,
            };
            SplitterElement = splitter1;

            grid1.Children.Add(splitter1);
            Grid.SetRow(splitter1, 0);
            Grid.SetColumn(splitter1, 1);

            // 2つ目のデータ
            grid1.Children.Add(SecondContainer);
            Grid.SetRow(SecondContainer, 0);
            Grid.SetColumn(SecondContainer, 2);
        }

        // 上下レイアウト
        private void CreateVerticalLayout()
        {
            if (btnOnlyBrowser.IsChecked == true)
                btnOnlyBrowser.IsChecked = false;

            if (btnOnlyVariable.IsChecked == true)
                btnOnlyVariable.IsChecked = false;

            grid1.Children.Clear();
            grid1.RowDefinitions.Clear();
            grid1.ColumnDefinitions.Clear();

            grid1.RowDefinitions.Add(new RowDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.RowDefinitions.Add(new RowDefinition());
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            // 1つ目のデータ
            grid1.Children.Add(FirstContainer);
            Grid.SetRow(FirstContainer, 0);
            Grid.SetColumn(FirstContainer, 0);

            // 分割線
            var splitter1 = new GridSplitter()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Height = 5,
            };
            SplitterElement = splitter1;

            grid1.Children.Add(splitter1);
            Grid.SetRow(splitter1, 1);
            Grid.SetColumn(splitter1, 0);

            // 2つ目のデータ
            grid1.Children.Add(SecondContainer);
            Grid.SetRow(SecondContainer, 2);
            Grid.SetColumn(SecondContainer, 0);
        }

        // データ入れ替え
        private void Swap()
        {
            TempContainer = SecondContainer;
            SecondContainer = FirstContainer;
            FirstContainer = TempContainer;
        }

        // 変数だけ表示
        private void ShowOnlyVariable()
        {
            ReVisibleLayout();

            // ブラウザが非表示にならないバグの対応
            if (BrowserElement.Visibility == Visibility.Visible)
                BrowserElement.Visibility = Visibility.Hidden;

            if (1 < grid1.ColumnDefinitions.Count)
            {
                // 左右表示
                var index1 = Grid.GetColumn(SplitterElement);
                var index2 = Grid.GetColumn(BrowserElement);

                grid1.ColumnDefinitions[index1].Width = new GridLength(0);
                grid1.ColumnDefinitions[index2].Width = new GridLength(0);
            }
            else
            {
                // 上下表示
                var index1 = Grid.GetRow(SplitterElement);
                var index2 = Grid.GetRow(BrowserElement);

                grid1.RowDefinitions[index1].Height = new GridLength(0);
                grid1.RowDefinitions[index2].Height = new GridLength(0);
            }
        }

        // ブラウザだけ表示
        private void ShowOnlyWebBrowser()
        {
            ReVisibleLayout();

            if (1 < grid1.ColumnDefinitions.Count)
            {
                // 左右表示
                var index1 = Grid.GetColumn(SplitterElement);
                var index2 = Grid.GetColumn(VariableElement);

                grid1.ColumnDefinitions[index1].Width = new GridLength(0);
                grid1.ColumnDefinitions[index2].Width = new GridLength(0);
            }
            else
            {
                // 上下表示
                var index1 = Grid.GetRow(SplitterElement);
                var index2 = Grid.GetRow(VariableElement);

                grid1.RowDefinitions[index1].Height = new GridLength(0);
                grid1.RowDefinitions[index2].Height = new GridLength(0);
            }
        }

        private void ReVisibleLayout()
        {
            if (BrowserElement.Visibility == Visibility.Hidden)
                BrowserElement.Visibility = Visibility.Visible;

            if (1 < grid1.RowDefinitions.Count)
            {
                grid1.RowDefinitions[0].Height = new GridLength(1.0, GridUnitType.Star);
                grid1.RowDefinitions[1].Height = GridLength.Auto;
                grid1.RowDefinitions[2].Height = new GridLength(1.0, GridUnitType.Star);
            }
            else
            {
                grid1.ColumnDefinitions[0].Width = new GridLength(1.0, GridUnitType.Star);
                grid1.ColumnDefinitions[1].Width = GridLength.Auto;
                grid1.ColumnDefinitions[2].Width = new GridLength(1.0, GridUnitType.Star);
            }
        }

        private void CloseMe(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
