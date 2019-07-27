using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;

namespace ObjectVisualization
{
    public class StringToWPFViewConverter
    {
        private readonly LanguageTypes _LanguageTypes;

        public StringToWPFViewConverter(LanguageTypes languageTypes = LanguageTypes.CSharp)
        {
            _LanguageTypes = languageTypes;
        }


        #region 公開機能


        public FrameworkElement Convert(string value)
        {
            var doc = XDocument.Parse(value);
            var root = doc.Root;
            var info = root.Element("ObjectInfo");
            var child = info.Elements().FirstOrDefault();
            var element = CreateViewData(child);

            // ※MainWindow / Resources 内で中央に寄るように設定している、子コントロールの場合、中央に表示したいため
            // トップコントロールが TextBlock の場合、中央に寄ってしまうので、左寄せに調整
            if (element is TextBlock)
                element.HorizontalAlignment = HorizontalAlignment.Left;
            
            return element;
        }

        private FrameworkElement CreateViewData(XElement element)
        {
            var result = default(FrameworkElement);

            if (IsPrimitive(element))
            {
                result = CreateViewDataForPrimitive(element);
            }
            else if (IsSingle(element))
            {
                result = CreateViewDataForSingle(element);
            }
            else if (IsMember(element))
            {
                result = CreateViewDataForMember(element);
            }
            else if (IsArray(element))
            {
                result = CreateViewDataForArray(element);
            }
            else
            {
                var instance = $"未対応（key: {element.Name}, value: {element.Value}）";
                var memberElement = CreatePrimitiveElement(instance);
                result = CreateViewDataForPrimitive(memberElement);
            }

            return result;
        }

        private FrameworkElement CreateViewDataForPrimitive(XElement element)
        {
            var item = new PrimitiveData(element);
            item.TypeName = ToNormalString(item.TypeName);
            item.Value = ToNormalString(item.Value);

            if (item.TypeName == "BitmapImage")
            {
                return CreateImageData(item);
            }
            else
            {
                return CreatePrimitiveData(item);
            }
        }

        private FrameworkElement CreateViewDataForSingle(XElement element)
        {
            var item = new SingleData(element);
            item.Header = ToNormalString(item.Header);

            return CreateSingleData(item);
        }

        private FrameworkElement CreateViewDataForMember(XElement element)
        {
            var item = new MemberData(element);
            item.Header = ToNormalString(item.Header);

            return CreateMemberData(item);
        }

        private FrameworkElement CreateViewDataForArray(XElement element)
        {
            var start = CreateLayout();
            var grid1 = start.Item2;

            var item = new ArrayData(element);
            item.Header = ToNormalString(item.Header);

            if (IsZeroItems(element))
            {
                CreateZeroItemsCollectionData(grid1, item);
            }
            else if (IsSingleColumns(element) || IsXmlOperationClass(element))
            {
                CreateSingleColumnCollectionData(grid1, item);
            }
            else if (IsMultiColumns(element))
            {
                var table = ToDataTable(item);
                var isCalledDataTable = (item.FamilyType == "DataTable");
                CreateMultiColumnCollectionData(grid1, item, table, isCalledDataTable);
            }

            return start.Item1;
        }

        private Tuple<StackPanel, Grid> CreateLayout()
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            return Tuple.Create(stackPanel1, grid1);
        }


        #endregion


        #region 種類チェック


        // int, string, 等
        private bool IsPrimitive(XElement element)
        {
            return (element.Name == "Primitive");
        }

        // delegate, exception, 等、タイトルと文字列データ
        private bool IsSingle(XElement element)
        {
            return IsTargetType(element, "Single");
        }

        // class, struct, anonymousType, 等
        private bool IsMember(XElement element)
        {
            return IsTargetType(element, "Member");
        }

        // IEnumerable 全般
        private bool IsArray(XElement element)
        {
            return IsTargetType(element, "Array");
        }

        private bool IsTargetType(XElement element, string key)
        {
            return (element.Name == "Node" && element.Attribute("Type").Value == key);
        }

        private bool IsZeroItems(XElement element)
        {
            return !element.HasElements;
        }

        private bool IsSingleColumns(XElement element)
        {
            return !IsMultiColumns(element);
        }

        private bool IsMultiColumns(XElement element)
        {
            return element.Elements().Any(x => IsMember(x));
        }

        private bool IsXmlOperationClass(XElement element)
        {
            return (IsXmlDocumentFamily(element) || IsXDocumentFamily(element));
        }

        private bool IsXmlDocumentFamily(XElement element)
        {
            var typeName = element.Attribute("Header").Value;
            return (typeName.StartsWith("XmlDocument") || typeName.StartsWith("XmlElement"));
        }

        private bool IsXDocumentFamily(XElement element)
        {
            var typeName = element.Attribute("Header").Value;
            return (typeName.StartsWith("XDocument") || typeName.StartsWith("XElement"));
        }


        #endregion


        #region 作成補助、変換


        // C＃で文字列をブラシ/ブラシカラー名に変換
        // https://codeday.me/jp/qa/20190105/132932.html
        //
        private XElement CreatePrimitiveElement(string value)
        {
            var memberElement = new XElement("Primitive");

            if (_LanguageTypes == LanguageTypes.CSharp)
                memberElement.Add(new XAttribute("Type", "string"));
            else if (_LanguageTypes == LanguageTypes.VBNET)
                memberElement.Add(new XAttribute("Type", "String"));

            memberElement.Add(new XAttribute("Color", "Brown"));
            memberElement.Value = value;
            return memberElement;
        }

        private TextBlock CreatePrimitiveData(string typeName, string value, string colorName)
        {
            var item = new PrimitiveData(typeName, colorName, value);
            return CreatePrimitiveData(item);
        }

        private TextBlock CreatePrimitiveData(PrimitiveData item)
        {
            Brush foreColor = (SolidColorBrush)new BrushConverter().ConvertFromString(item.ColorName);
            return new TextBlock() { Text = item.Value, Foreground = foreColor, TextWrapping = TextWrapping.Wrap };
        }


        // Converting WPF BitmapImage to and from Base64 using JpegBitmapEncoder fails for loading JPG files
        // https://stackoverflow.com/questions/40404625/converting-wpf-bitmapimage-to-and-from-base64-using-jpegbitmapencoder-fails-for
        //
        private FrameworkElement CreateImageData(PrimitiveData item)
        {
            // string -> byte[]
            var bytes = System.Convert.FromBase64String(item.Value);

            // byte[] -> BitmapImage
            using (var stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);

                try
                {
                    var bm = new BitmapImage();
                    bm.BeginInit();
                    bm.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bm.CacheOption = BitmapCacheOption.OnLoad;
                    bm.StreamSource = stream;
                    bm.EndInit();

                    return CreateImageData(bm);

                }
                catch (Exception ex)
                {
                    var value = $"※画像データは未対応です\r\n{ex.ToString()}";
                    var element = CreatePrimitiveElement(value);
                    var pItem = new PrimitiveData(element);
                    return CreatePrimitiveData(pItem);
                }
            }
        }

        // スライダーでイメージを拡大/縮小表示する
        // https://blog.hiros-dot.net/?page_id=3948
        //
        // How to SetBinding to ScaleTransform's ScaleX in C# code (not XAML)?
        // https://stackoverflow.com/questions/5141967/how-to-setbinding-to-scaletransforms-scalex-in-c-sharp-code-not-xaml
        //
        private FrameworkElement CreateImageData(BitmapSource source)
        {
            var slider1 = new Slider()
            {
                Orientation = Orientation.Horizontal,
                Minimum = 1,
                Maximum = 200,
                Value = 100,
            };

            var image1 = new Image()
            {
                Source = source,
                RenderTransformOrigin = new Point(0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 10, 0, 0),
            };

            var transform1 = new TransformGroup();
            transform1.Children.Add(new ScaleTransform() { ScaleX = 0.01, ScaleY = 0.01 });

            var scaleTransform1 = new ScaleTransform();
            transform1.Children.Add(scaleTransform1);

            var bindingX = new Binding("Value");
            bindingX.Source = slider1;
            BindingOperations.SetBinding(scaleTransform1, ScaleTransform.ScaleXProperty, bindingX);

            var bindingY = new Binding("Value");
            bindingY.Source = slider1;
            BindingOperations.SetBinding(scaleTransform1, ScaleTransform.ScaleYProperty, bindingY);

            image1.RenderTransform = transform1;




            //var scrollViewer1 = new ScrollViewer()
            //{
            //    HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
            //    VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            //};
            //scrollViewer1.Content = image1;

            var stackPanel1 = new StackPanel();
            stackPanel1.Children.Add(slider1);
            stackPanel1.Children.Add(image1);

            return stackPanel1;
        }


        // how to do a TemplateBinding in code?
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/977c9cbe-f5a6-4d9b-afa5-9d02476b13e8/how-to-do-a-templatebinding-in-code?forum=wpf
        // 
        private void AddExpandCollapseButton(Grid grid1, string title, int columnCount = 0)
        {
            //// タイトル
            var baseGrid = new Grid();
            grid1.Children.Add(baseGrid);
            Grid.SetRow(baseGrid, 0);
            Grid.SetColumn(baseGrid, 0);

            if (1 < columnCount)
                Grid.SetColumnSpan(baseGrid, columnCount);

            var rectangle1 = new Rectangle() { Fill = Brushes.LightGray };
            var textblock1 = new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold, Margin = new Thickness(30, 5, 30, 5), HorizontalAlignment = HorizontalAlignment.Center };
            var stackpanel1 = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            var button1 = new Button() { Content = "<<", Margin = new Thickness(5) };

            // クリックイベントで開閉切り替えする
            button1.Click += (s, e) =>
            {
                // １つ目はタイトルなのでスキップ、残りの行を扱う
                var rows = grid1.RowDefinitions.Cast<RowDefinition>().Skip(1).ToList();

                if (button1.Content.ToString() == "<<")
                {
                    rows.ForEach(x => x.Height = new GridLength(0, GridUnitType.Pixel));
                    button1.Content = ">>";
                }
                else
                {
                    rows.ForEach(x => x.Height = new GridLength(1, GridUnitType.Star));
                    button1.Content = "<<";
                }
            };

            // Button の見た目を TextBlock に変える
            var textblockFactory = new FrameworkElementFactory(typeof(TextBlock));
            textblockFactory.SetValue(TextBlock.TextProperty, new TemplateBindingExtension(Button.ContentProperty));

            var template1 = new ControlTemplate(typeof(Button));
            template1.VisualTree = textblockFactory;
            button1.Template = template1;

            stackpanel1.Children.Add(button1);
            baseGrid.Children.Add(rectangle1);
            baseGrid.Children.Add(textblock1);
            baseGrid.Children.Add(stackpanel1);
        }

        private StackPanel CreateSingleData(SingleData item)
        {
            var start = CreateLayout();
            var grid1 = start.Item2;

            // タイトルと値用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            AddExpandCollapseButton(grid1, item.Header);

            // 値
            var messagePanel = new DockPanel();
            grid1.Children.Add(messagePanel);
            Grid.SetRow(messagePanel, 1);
            Grid.SetColumn(messagePanel, 0);

            var messageLine = new Line() { X2 = 1 };
            messagePanel.Children.Add(messageLine);
            DockPanel.SetDock(messageLine, Dock.Top);

            var messageBlock = CreatePrimitiveData(item.PrimitiveInfo);
            messagePanel.Children.Add(messageBlock);

            return start.Item1;
        }

        private StackPanel CreateMemberData(MemberData item)
        {
            var start = CreateLayout();
            var grid1 = start.Item2;

            // メンバー名と値分
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // 型名
            AddExpandCollapseButton(grid1, item.Header, 2);

            // メンバー数分
            var rowIndex = 0;
            var items = item.Properties;

            for (var i = 0; i < items.Count; i++)
            {
                var pi = items[i];
                pi.TypeName = ToNormalString(pi.TypeName);

                var memberName = pi.Name;
                var memberTypeName = pi.TypeName;
                var memberValue = pi.Value;

                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                rowIndex++;


                // メンバー名
                var namePanel = new DockPanel() { Background = Brushes.LightGray };
                grid1.Children.Add(namePanel);
                Grid.SetRow(namePanel, rowIndex);
                Grid.SetColumn(namePanel, 0);

                var nameLine1 = new Line() { X2 = 1 };
                namePanel.Children.Add(nameLine1);
                DockPanel.SetDock(nameLine1, Dock.Top);

                var nameLine2 = new Line() { Y2 = 1 };
                namePanel.Children.Add(nameLine2);
                DockPanel.SetDock(nameLine2, Dock.Right);

                var nameBlock = new TextBlock() { Text = memberName, FontWeight = FontWeights.UltraBold };
                namePanel.Children.Add(nameBlock);

                SetToolTip(nameBlock, memberTypeName, memberName);


                // メンバー値
                var valuePanel = new DockPanel();
                grid1.Children.Add(valuePanel);
                Grid.SetRow(valuePanel, rowIndex);
                Grid.SetColumn(valuePanel, 1);

                var valueLine1 = new Line() { X2 = 1 };
                valuePanel.Children.Add(valueLine1);
                DockPanel.SetDock(valueLine1, Dock.Top);

                var memberElement = CreateViewData(memberValue);
                valuePanel.Children.Add(memberElement);
            }

            return start.Item1;
        }

        // コレクション系の型でデータ数が 0 件
        private void CreateZeroItemsCollectionData(Grid grid1, ArrayData item)
        {
            CreateZeroItemsCollectionData(grid1, item.Header);
        }

        private void CreateZeroItemsCollectionData(Grid grid1, string typeName = "")
        {
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            var headerPanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(headerPanel);
            Grid.SetRow(headerPanel, 0);
            Grid.SetColumn(headerPanel, 0);

            if (string.IsNullOrEmpty(typeName))
                typeName = "(0 items)";

            headerPanel.Children.Add(new TextBlock() { Text = $"{typeName}", FontWeight = FontWeights.UltraBold });
        }

        private void CreateSingleColumnCollectionData(Grid grid1, ArrayData item)
        {
            // 列名無しのコレクション表示をする
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // 型名
            AddExpandCollapseButton(grid1, item.Header);

            // コレクションデータ
            var rowIndex = 0;
            var items = item.Members;

            for (var i = 0; i < items.Count; i++)
            {
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                rowIndex++;

                var memberPanel = new DockPanel();
                grid1.Children.Add(memberPanel);
                Grid.SetRow(memberPanel, rowIndex);
                Grid.SetColumn(memberPanel, 0);

                var memberLine = new Line() { X2 = 1 };
                memberPanel.Children.Add(memberLine);
                DockPanel.SetDock(memberLine, Dock.Top);

                var memberElement = CreateViewData(items[i]);
                memberPanel.Children.Add(memberElement);
            }
        }

        private void CreateMultiColumnCollectionData(Grid grid1, ArrayData item, DataTable table, bool isCalledDataTable)
        {
            // 列名ありのコレクション表示をする
            var rowIndex = 0;
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (var i = 0; i < table.Columns.Count; i++)
                grid1.ColumnDefinitions.Add(new ColumnDefinition());

            // 型名
            AddExpandCollapseButton(grid1, item.Header, table.Columns.Count);

            // ヘッダー列（フィールド、プロパティメンバー名）
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            rowIndex++;

            for (var i = 0; i < table.Columns.Count; i++)
            {
                var headerPanel = new DockPanel() { Background = Brushes.LightGray };
                grid1.Children.Add(headerPanel);
                Grid.SetRow(headerPanel, rowIndex);
                Grid.SetColumn(headerPanel, i);

                var headerLine1 = new Line() { X2 = 1 };
                headerPanel.Children.Add(headerLine1);
                DockPanel.SetDock(headerLine1, Dock.Top);

                if (i < table.Columns.Count - 1)
                {
                    var headerLine2 = new Line() { Y2 = 1 };
                    headerPanel.Children.Add(headerLine2);
                    DockPanel.SetDock(headerLine2, Dock.Right);
                }

                var headerBlock = new TextBlock() { FontWeight = FontWeights.UltraBold };
                var memberName = table.Columns[i].ColumnName;
                var memberTypeName = table.Columns[i].Caption;
                if (memberName == FirstColumnName)
                {
                    headerBlock.Text = "（無所属）";
                }
                else
                {
                    headerBlock.Text = memberName;

                    // 型は全て object 型にしている。その代わり Caption に本来の型を登録しているため、抽出して使う
                    // 渡されたのが DataSet 系の場合、型をそのまま使う
                    memberTypeName = memberTypeName.Substring(memberTypeName.IndexOf("__MemberType__") + "__MemberType__".Length);
                    SetToolTip(headerBlock, memberTypeName, memberName);
                }

                headerPanel.Children.Add(headerBlock);
            }

            // コレクションデータ
            for (var i = 0; i < table.Rows.Count; i++)
            {
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                rowIndex++;

                var row = table.Rows[i];
                for (var k = 0; k < table.Columns.Count; k++)
                {
                    var memberName = table.Columns[k].ColumnName;
                    var memberInstance = row[memberName];
                    var memberElement = default(XElement);

                    if (memberInstance is DBNull && !isCalledDataTable)
                    {
                        // コレクション系から渡ってきた DataTable 内の DBNull は、インスタンスが持っていないフィールド・プロパティのことになるので、空欄を表示する
                        //（ただし、空文字にすると「(空欄)」の文字列を表示してしまうため、スペースをセットする）
                        memberElement = CreatePrimitiveElement(" ");
                    }
                    else
                    {
                        memberElement = memberInstance as XElement;
                    }

                    var memberPanel = new DockPanel();
                    grid1.Children.Add(memberPanel);
                    Grid.SetRow(memberPanel, rowIndex);
                    Grid.SetColumn(memberPanel, k);

                    var headerLine1 = new Line() { X2 = 1 };
                    memberPanel.Children.Add(headerLine1);
                    DockPanel.SetDock(headerLine1, Dock.Top);

                    if (k < table.Columns.Count - 1)
                    {
                        var headerLine2 = new Line() { Y2 = 1 };
                        memberPanel.Children.Add(headerLine2);
                        DockPanel.SetDock(headerLine2, Dock.Right);
                    }

                    var addElement = CreateViewData(memberElement);
                    memberPanel.Children.Add(addElement);
                }
            }
        }

        private DockPanel CreateWebBrowserPanel(string xmlString)
        {
            var dockPanel1 = new DockPanel();

            xmlString = xmlString.Trim();
            if (!xmlString.StartsWith(@"<?xml version="))
            {
                xmlString = $@"<?xml version=""1.0""?>{xmlString}";

                var messageBlock = new TextBlock()
                {
                    Text = "※1 行目の xml 宣言タグは自動追加です。データには含まれていません。",
                    Foreground = Brushes.Gray,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(1),
                };
                dockPanel1.Children.Add(messageBlock);
                DockPanel.SetDock(messageBlock, Dock.Top);
            }

            var browser1 = new WebBrowser();
            browser1.NavigateToString(xmlString);

            // 画面のスクロールが途中で止まってしまうバグの対応、ウェブブラウザの範囲が分かるように外枠を追加
            var border1 = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1) };
            border1.Child = browser1;
            dockPanel1.Children.Add(border1);
            return dockPanel1;
        }


        // 「<」「>」「"」は、置換しないと不正になってしまうので相互変換
        // 「&lt;」「&gt;」「""」
        // ただし、もともと「&lt;」「&gt;」「""」を含む文字列は変えたくないため、制御文字列もくっつける
        private string ToNormalString(string s)
        {
            if (s.Contains("__lt__"))
                s = s.Replace("__lt__", "<");

            if (s.Contains("__gt__"))
                s = s.Replace("__gt__", ">");

            if (s.Contains(@"__dobule_quote__"))
                s = s.Replace(@"__dobule_quote__", @"""");

            return s;
        }

        private object ToObject(PrimitiveData item)
        {
            return ToObject(item.TypeName, item.Value);
        }

        private object ToObject(string typeName, string value)
        {
            if (typeName == "null")
                return null;

            if (typeName == "DBNull")
                return DBNull.Value;

            object result = null;
            switch (typeName)
            {
                case "bool": result = bool.Parse(value); break;
                case "byte": result = byte.Parse(value); break;
                case "sbyte": result = sbyte.Parse(value); break;
                case "decimal": result = decimal.Parse(value); break;
                case "double": result = double.Parse(value); break;
                case "float": result = float.Parse(value); break;
                case "int": result = int.Parse(value); break;
                case "uint": result = uint.Parse(value); break;
                case "long": result = long.Parse(value); break;
                case "ulong": result = ulong.Parse(value); break;
                case "short": result = short.Parse(value); break;
                case "ushort": result = ushort.Parse(value); break;
                case "DateTime": result = DateTime.Parse(value); break;
                case "DateTimeOffset": result = DateTimeOffset.Parse(value); break;
                case "TimeSpan": result = TimeSpan.Parse(value); break;
                case "char": result = char.Parse(value); break;
                case "string": result = value; break;
                default: result = value; break;
            }

            return result;
        }


        private const string FirstColumnName = "Object";

        private DataTable ToDataTable(ArrayData item)
        {
            // 列名を集める
            var table = new DataTable();
            table.Columns.Add(FirstColumnName, typeof(object));

            foreach (var member in item.Members)
            {
                if (IsMember(member))
                {
                    var memberItem = new MemberData(member);
                    foreach (var pi in memberItem.Properties)
                    {
                        if (!table.Columns.Contains(pi.Name))
                        {
                            var column = new DataColumn();
                            column.ColumnName = pi.Name;
                            column.DataType = typeof(object);
                            column.Caption = $"__MemberType__{pi.TypeName}";

                            table.Columns.Add(column);
                        }
                    }
                }
            }

            // データを配置する
            foreach (var member in item.Members)
            {
                if (IsMember(member))
                {
                    var memberItem = new MemberData(member);
                    var row = table.NewRow();
                    foreach (var pi in memberItem.Properties)
                        row[pi.Name] = pi.Value;

                    table.Rows.Add(row);
                }
                else
                {
                    // Primitive, Single, Array
                    var row = table.NewRow();
                    row[FirstColumnName] = member;
                    table.Rows.Add(row);
                }
            }

            // 最初の列にデータが無い場合、列自体を削除する
            var isDeleteColumn = true;
            foreach (DataRow row in table.Rows)
            {
                if (!(row[FirstColumnName] is DBNull))
                {
                    isDeleteColumn = false;
                    break;
                }
            }

            if (isDeleteColumn)
            {
                var column = table.Columns[FirstColumnName];
                table.Columns.Remove(column);
            }

            return table;
        }
        
        private void SetToolTip(FrameworkElement targetElement, string typeName, string memberName)
        {
            var headerTip = new ToolTip();
            var tipElement = default(StackPanel);
            typeName = ToNormalString(typeName);

            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                // Xxx xxx
                // typeName memberName
                tipElement = new StackPanel() { Orientation = Orientation.Horizontal };
                tipElement.Children.Add(new TextBlock() { Text = typeName, Foreground = Brushes.Blue, Margin = new Thickness(0) });
                tipElement.Children.Add(new TextBlock() { Text = $" {memberName}", Foreground = Brushes.Black, Margin = new Thickness(0) });

            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                // xxx As Xxx
                // memberName As typeName
                tipElement = new StackPanel() { Orientation = Orientation.Horizontal };
                tipElement.Children.Add(new TextBlock() { Text = $"{memberName} ", Foreground = Brushes.Black, Margin = new Thickness(0) });
                tipElement.Children.Add(new TextBlock() { Text = "As ", Foreground = Brushes.Blue, Margin = new Thickness(0) });
                tipElement.Children.Add(new TextBlock() { Text = typeName, Foreground = Brushes.Blue, Margin = new Thickness(0) });
            }

            headerTip.Content = tipElement;
            ToolTipService.SetToolTip(targetElement, headerTip);
        }


        #endregion


        #region 補助クラス


        private class PrimitiveData
        {
            public string TypeName { get; set; }
            public string ColorName { get; }
            public string Value { get; set; }

            public PrimitiveData(XElement element)
            {
                TypeName = element.Attribute("Type").Value;
                ColorName = element.Attribute("Color").Value;
                Value = element.Value;
            }

            public PrimitiveData(string typeName, string colorName, string value)
            {
                TypeName = typeName;
                ColorName = colorName;
                Value = value;
            }
        }

        private class SingleData
        {
            public string Header { get; set; }
            public PrimitiveData PrimitiveInfo { get; }

            public SingleData(XElement element)
            {
                Header = element.Attribute("Header").Value;
                var item = element.Elements().FirstOrDefault();
                PrimitiveInfo = new PrimitiveData(item);
            }
        }

        private class PropertyData
        {
            public string Name { get; }
            public string TypeName { get; set; }
            public XElement Value { get; } // Primitive, Node(Single), Node(Member), Node(Array)

            public PropertyData(XElement element)
            {
                Name = element.Attribute("Name").Value;
                TypeName = element.Attribute("Type").Value;
                Value = element.Elements().FirstOrDefault();
            }
        }

        private class MemberData
        {
            public string Header { get; set; }
            public List<PropertyData> Properties { get; }

            public MemberData(XElement element)
            {
                Header = element.Attribute("Header").Value;

                Properties = new List<PropertyData>();
                var items = element.Elements();
                foreach (var item in items)
                {
                    Properties.Add(new PropertyData(item));
                }
            }
        }

        private class ArrayData
        {
            public string Header { get; set; }
            public string FamilyType { get; set; }
            public List<XElement> Members { get; }

            public ArrayData(XElement element)
            {
                Header = element.Attribute("Header").Value;
                FamilyType = element.Attribute("FamilyType").Value;

                Members = new List<XElement>();
                if (element.HasElements)
                {
                    var items = element.Elements();
                    foreach (var item in items)
                        Members.Add(item);
                }
            }
        }


        #endregion


        #region 継承元クラスツリー関連


        // 継承元クラスと継承元インターフェースを階層的に表示する
        // GetVariableTypeName() 内で ToEscapeString() していて、これはもうしょうがないので、表示直前に ToNormalString() して戻しています。
        public FrameworkElement CreateBaseTypeTree(string value)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Margin = new Thickness(10) };
            var items = value.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();

            for (var i = 0; i < items.Count; i++)
            {
                var grid1 = new Grid() { HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(10 * i, 10, 10, 10) };
                grid1.RowDefinitions.Add(new RowDefinition());
                grid1.ColumnDefinitions.Add(new ColumnDefinition());
                grid1.ColumnDefinitions.Add(new ColumnDefinition());
                stackPanel1.Children.Add(grid1);

                var arrowBlock = new TextBlock() { Text = "→", Margin = new Thickness(10, 10, 10, 10) };
                grid1.Children.Add(arrowBlock);
                Grid.SetRow(arrowBlock, 0);
                Grid.SetColumn(arrowBlock, 0);

                var rec1 = new Rectangle() { Stroke = Brushes.LightSkyBlue, Fill = Brushes.AliceBlue, RadiusX = 5.0, RadiusY = 5.0 };
                grid1.Children.Add(rec1);
                Grid.SetRow(rec1, 0);
                Grid.SetColumn(rec1, 1);

                var item = items[i];
                if (item.Contains("__sep__"))
                {
                    var subItems = item.Split(new string[] { "__sep__" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    var callBlock = new TextBlock() { Text = ToNormalString(subItems[0]), Margin = new Thickness(10, 10, 10, 10) };
                    grid1.Children.Add(callBlock);
                    Grid.SetRow(callBlock, 0);
                    Grid.SetColumn(callBlock, 1);

                    for (var k = 1; k < subItems.Count; k++)
                    {
                        var columnIndex = k + 1;
                        grid1.ColumnDefinitions.Add(new ColumnDefinition());

                        var rec2 = new Rectangle() { Stroke = Brushes.LightPink, Fill = Brushes.LavenderBlush, RadiusX = 5.0, RadiusY = 5.0, Margin = new Thickness(5, 0, 0, 0) };
                        grid1.Children.Add(rec2);
                        Grid.SetRow(rec2, 0);
                        Grid.SetColumn(rec2, columnIndex);

                        var callBlock2 = new TextBlock() { Text = ToNormalString(subItems[k]), Margin = new Thickness(15, 10, 10, 10) };
                        grid1.Children.Add(callBlock2);
                        Grid.SetRow(callBlock2, 0);
                        Grid.SetColumn(callBlock2, columnIndex);
                    }
                }
                else
                {
                    var callBlock = new TextBlock() { Text = ToNormalString(item), Margin = new Thickness(10, 10, 10, 10) };
                    grid1.Children.Add(callBlock);
                    Grid.SetRow(callBlock, 0);
                    Grid.SetColumn(callBlock, 1);
                }
            }

            return stackPanel1;
        }


        #endregion


        #region コールツリー関連


        public FrameworkElement CreateCallTree(string value)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Margin = new Thickness(10) };
            var items = value.Split(new string[] { "__sep__" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToList();

            for (var i = 0; i < items.Count; i++)
            {
                var grid1 = new Grid() { HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(10 * i, 10, 10, 10) };
                grid1.RowDefinitions.Add(new RowDefinition());
                grid1.ColumnDefinitions.Add(new ColumnDefinition());
                grid1.ColumnDefinitions.Add(new ColumnDefinition());
                stackPanel1.Children.Add(grid1);

                var arrowBlock = new TextBlock() { Text = "→", Margin = new Thickness(10, 10, 10, 10) };
                grid1.Children.Add(arrowBlock);
                Grid.SetRow(arrowBlock, 0);
                Grid.SetColumn(arrowBlock, 0);

                var rec1 = new Rectangle() { Stroke = Brushes.Blue, Fill = Brushes.AliceBlue, RadiusX = 5.0, RadiusY = 5.0 };
                grid1.Children.Add(rec1);
                Grid.SetRow(rec1, 0);
                Grid.SetColumn(rec1, 1);

                var item = items[i];
                var callBlock = new TextBlock() { Text = item, Margin = new Thickness(10, 10, 10, 10) };
                grid1.Children.Add(callBlock);
                Grid.SetRow(callBlock, 0);
                Grid.SetColumn(callBlock, 1);
            }

            return stackPanel1;
        }


        #endregion

    }
}
