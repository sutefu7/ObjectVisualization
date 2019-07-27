using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace ObjectVisualization
{
    public class XmlDocumentFamilyToFrameworkElementConverter
    {
        public FrameworkElement Convert(XmlDocument doc)
        {
            // StackPanel だけだと Border が常に画面いっぱいに伸びてしまうので、調整
            var grid1 = new Grid();
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.RowDefinitions.Add(new RowDefinition());

            var dataPart = CreateXmlDocumentData(doc);
            grid1.Children.Add(dataPart);
            Grid.SetRow(dataPart, 0);

            var dummyPart = new Rectangle();
            grid1.Children.Add(dummyPart);
            Grid.SetRow(dummyPart, 1);

            return grid1;
        }

        public FrameworkElement Convert(XmlElement element)
        {
            // StackPanel だけだと Border が常に画面いっぱいに伸びてしまうので、調整
            var grid1 = new Grid();
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.RowDefinitions.Add(new RowDefinition());

            var dataPart = CreateXmlElementData(element);
            grid1.Children.Add(dataPart);
            Grid.SetRow(dataPart, 0);

            var dummyPart = new Rectangle();
            grid1.Children.Add(dummyPart);
            Grid.SetRow(dummyPart, 1);

            return grid1;
        }
        
        private StackPanel CreateXmlDocumentData(XmlDocument doc)
        {
            var start = CreateLayout();
            var grid1 = start.Item2;

            // タイトル用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var rowIndex = 0;
            var title = GetVariableTypeName(doc);
            AddExpandCollapseButton(grid1, title);
            
            if (!doc.HasChildNodes)
                return start.Item1;

            foreach (XmlNode node in doc.ChildNodes)
            {
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                rowIndex++;

                var nodeElement = default(FrameworkElement);
                if (node is XmlDeclaration)
                {
                    var element = node as XmlDeclaration;

                    // メンバー数が多すぎるため、指定メンバーのみに絞る
                    //nodeElement = CreateMemberData(element, element.GetType());
                    nodeElement = CreateXmlDeclarationData(element);
                }
                else if (node is XmlComment)
                {
                    var element = node as XmlComment;
                    nodeElement = CreateXmlCommentData(element);
                }
                else if (node is XmlElement)
                {
                    var element = node as XmlElement;
                    nodeElement = CreateXmlElementData(element);
                }
                else
                {
                    // 未対応分
                    nodeElement = CreatePrimitiveData(node.ToString());
                }

                var memberPanel = new DockPanel();
                grid1.Children.Add(memberPanel);
                Grid.SetRow(memberPanel, rowIndex);
                Grid.SetColumn(memberPanel, 0);

                var memberLine1 = new Line() { X2 = 1 };
                memberPanel.Children.Add(memberLine1);
                DockPanel.SetDock(memberLine1, Dock.Top);

                memberPanel.Children.Add(nodeElement);
            }

            return start.Item1;
        }

        private StackPanel CreateXmlAttributeData(List<XmlAttribute> attrs)
        {
            var typeName = attrs[0].GetType().Name;
            if (1 < attrs.Count)
                typeName = $"{typeName} ({attrs.Count} items)";
            
            var items = new List<Tuple<string, string>>();
            foreach (var attr in attrs)
                items.Add(Tuple.Create(attr.Name, attr.Value));

            return CreateMemberData(typeName, items);
        }

        private StackPanel CreateXmlDeclarationData(XmlDeclaration dec)
        {
            var typeName = dec.GetType().Name;
            var version = string.IsNullOrEmpty(dec.Version) ? string.Empty : dec.Version;
            var encode = string.IsNullOrEmpty(dec.Encoding) ? string.Empty : dec.Encoding;
            var standalone = string.IsNullOrEmpty(dec.Standalone) ? string.Empty : dec.Standalone;

            var items = new List<Tuple<string, string>>
            {
                Tuple.Create("Version", version),
                Tuple.Create("Encoding",encode),
                Tuple.Create("Standalone", standalone),
            };

            return CreateMemberData(typeName, items);
        }

        private StackPanel CreateXmlCommentData(XmlComment comment)
        {
            var variableName = comment.GetType().Name;
            var variableValue = comment.Value;

            return CreateSingleData(variableName, variableValue);
        }

        private FrameworkElement CreateXmlElementData(XmlElement element)
        {
            var start = CreateLayout();
            var grid1 = start.Item2;

            // タイトル用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var rowIndex = 0;
            var title = GetVariableTypeName(element);
            AddExpandCollapseButton(grid1, title);

            if (!element.HasAttributes && !element.HasChildNodes)
                return start.Item1;

            // タイトル / 型名、子タグ個数、タグ名
            if (element.HasAttributes)
            {
                // 謎仕様？子ノードがある時の属性の取得順番について、逆順に取得してしまうので、逆順の逆順に取得する
                var attrs = default(IEnumerable<XmlAttribute>);
                if (element.HasChildNodes)
                    attrs = element.Attributes.Cast<XmlAttribute>().Reverse();
                else
                    attrs = element.Attributes.Cast<XmlAttribute>();

                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                rowIndex++;

                // メンバー数が多すぎるため?、指定メンバーのみに絞る
                //var nodeElement = CreateMemberData(attr, attr.GetType());
                var nodeElement = CreateXmlAttributeData(attrs.ToList());

                var memberPanel = new DockPanel();
                grid1.Children.Add(memberPanel);
                Grid.SetRow(memberPanel, rowIndex);
                Grid.SetColumn(memberPanel, 0);

                var memberLine1 = new Line() { X2 = 1 };
                memberPanel.Children.Add(memberLine1);
                DockPanel.SetDock(memberLine1, Dock.Top);

                memberPanel.Children.Add(nodeElement);
            }

            if (element.HasChildNodes)
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    rowIndex++;

                    var nodeElement = default(FrameworkElement);
                    if (node is XmlComment)
                    {
                        var castedElement = node as XmlComment;
                        nodeElement = CreateXmlCommentData(castedElement);
                    }
                    else if (node is XmlText)
                    {
                        var castedElement = node as XmlText;
                        nodeElement = CreatePrimitiveData(castedElement.Value);
                    }
                    else if (node is XmlElement)
                    {
                        var castedElement = node as XmlElement;
                        nodeElement = CreateXmlElementData(castedElement);
                    }
                    else
                    {
                        // 未対応分
                        nodeElement = CreatePrimitiveData(node.ToString());
                    }

                    var memberPanel = new DockPanel();
                    grid1.Children.Add(memberPanel);
                    Grid.SetRow(memberPanel, rowIndex);
                    Grid.SetColumn(memberPanel, 0);

                    var memberLine1 = new Line() { X2 = 1 };
                    memberPanel.Children.Add(memberLine1);
                    DockPanel.SetDock(memberLine1, Dock.Top);

                    memberPanel.Children.Add(nodeElement);
                }
            }

            return start.Item1;
        }


        // XDocumentFamilyToFrameworkElementConverter クラスからも使用する

        public TextBlock CreatePrimitiveData(string value)
        {
            return new TextBlock() { Text = value, Foreground = Brushes.Brown, TextWrapping = TextWrapping.Wrap };
        }

        public StackPanel CreateSingleData(string title, string value)
        {
            var start = CreateLayout();
            var grid1 = start.Item2;

            // タイトルと値用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            AddExpandCollapseButton(grid1, title);

            // 値
            var messagePanel = new DockPanel();
            grid1.Children.Add(messagePanel);
            Grid.SetRow(messagePanel, 1);
            Grid.SetColumn(messagePanel, 0);

            var messageLine = new Line() { X2 = 1 };
            messagePanel.Children.Add(messageLine);
            DockPanel.SetDock(messageLine, Dock.Top);

            var messageBlock = CreatePrimitiveData(value);
            messagePanel.Children.Add(messageBlock);

            return start.Item1;
        }

        public StackPanel CreateMemberData(string typeName, List<Tuple<string, string>> items)
        {
            var start = CreateLayout();
            var grid1 = start.Item2;

            // メンバー名と値分
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // 型名
            AddExpandCollapseButton(grid1, typeName, 2);

            // メンバー数分
            var rowIndex = 0;
            for (var i = 0; i < items.Count; i++)
            {
                var memberName = items[i].Item1;
                var memberValue = items[i].Item2;

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
                
                // メンバー値
                var valuePanel = new DockPanel();
                grid1.Children.Add(valuePanel);
                Grid.SetRow(valuePanel, rowIndex);
                Grid.SetColumn(valuePanel, 1);

                var valueLine1 = new Line() { X2 = 1 };
                valuePanel.Children.Add(valueLine1);
                DockPanel.SetDock(valueLine1, Dock.Top);
                
                var memberElement = CreatePrimitiveData(memberValue);
                valuePanel.Children.Add(memberElement);
            }

            return start.Item1;
        }

        public Tuple<StackPanel, Grid> CreateLayout()
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


        // how to do a TemplateBinding in code?
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/977c9cbe-f5a6-4d9b-afa5-9d02476b13e8/how-to-do-a-templatebinding-in-code?forum=wpf
        // 
        public void AddExpandCollapseButton(Grid grid1, string title, int columnCount = 0)
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

        private string GetVariableTypeName(XmlDocument doc)
        {
            // xml 宣言、コメントは子タグ数に含めない
            var itemsCount = doc.ChildNodes.Count;
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node is XmlDeclaration)
                    itemsCount--;

                if (node is XmlComment)
                    itemsCount--;
            }
            
            var typeName = doc.GetType().Name;
            if (itemsCount == 1)
                typeName = $"{typeName} (1 child item)";
            else
                typeName = $"{typeName} ({itemsCount} child items)";
            
            return typeName;
        }

        private string GetVariableTypeName(XmlElement element)
        {
            // コメントは子タグ数に含めない
            var itemsCount = element.ChildNodes.Count;
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node is XmlComment)
                    itemsCount--;
            }

            var typeName = element.GetType().Name;
            if (itemsCount == 1)
                typeName = $"{typeName} <{element.Name}> (1 child item)";
            else
                typeName = $"{typeName} <{element.Name}> ({itemsCount} child items)";

            return typeName;
        }

        public DockPanel CreateWebBrowserPanel(string xmlString)
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
    }
}
