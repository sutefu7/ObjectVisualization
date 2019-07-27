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
using System.Xml.Linq;

namespace ObjectVisualization
{
    public class XDocumentFamilyToFrameworkElementConverter
    {
        private readonly XmlDocumentFamilyToFrameworkElementConverter converter;

        public XDocumentFamilyToFrameworkElementConverter()
        {
            converter = new XmlDocumentFamilyToFrameworkElementConverter();
        }

        public FrameworkElement Convert(XDocument doc)
        {
            // StackPanel だけだと Border が常に画面いっぱいに伸びてしまうので、調整
            var grid1 = new Grid();
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.RowDefinitions.Add(new RowDefinition());

            var dataPart = CreateXDocumentData(doc);
            grid1.Children.Add(dataPart);
            Grid.SetRow(dataPart, 0);

            var dummyPart = new Rectangle();
            grid1.Children.Add(dummyPart);
            Grid.SetRow(dummyPart, 1);

            return grid1;
        }

        public FrameworkElement Convert(XElement element)
        {
            // StackPanel だけだと Border が常に画面いっぱいに伸びてしまうので、調整
            var grid1 = new Grid();
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.RowDefinitions.Add(new RowDefinition());

            var dataPart = CreateXElementData(element);
            grid1.Children.Add(dataPart);
            Grid.SetRow(dataPart, 0);

            var dummyPart = new Rectangle();
            grid1.Children.Add(dummyPart);
            Grid.SetRow(dummyPart, 1);

            return grid1;
        }
        
        private StackPanel CreateXDocumentData(XDocument doc)
        {
            var start = converter.CreateLayout();
            var grid1 = start.Item2;

            // タイトル用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var rowIndex = 0;
            var title = GetVariableTypeName(doc);
            converter.AddExpandCollapseButton(grid1, title);

            var dec = doc.Declaration;
            if (!(dec is null))
            {
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                rowIndex++;

                var nodeElement = CreateXDeclarationData(dec);

                var memberPanel = new DockPanel();
                grid1.Children.Add(memberPanel);
                Grid.SetRow(memberPanel, rowIndex);
                Grid.SetColumn(memberPanel, 0);

                var memberLine1 = new Line() { X2 = 1 };
                memberPanel.Children.Add(memberLine1);
                DockPanel.SetDock(memberLine1, Dock.Top);

                memberPanel.Children.Add(nodeElement);
            }

            if (!doc.Document.Nodes().Any())
                return start.Item1;

            foreach (XNode node in doc.Document.Nodes())
            {
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                rowIndex++;

                var nodeElement = default(FrameworkElement);
                if (node is XComment)
                {
                    var element = node as XComment;
                    nodeElement = CreateXCommentData(element);
                }
                else if (node is XElement)
                {
                    var element = node as XElement;
                    nodeElement = CreateXElementData(element);
                }
                else
                {
                    // 未対応分
                    nodeElement = converter.CreatePrimitiveData(node.ToString());
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

        private StackPanel CreateXAttributeData(List<XAttribute> attrs)
        {
            var item = attrs[0];
            var typeName = item.GetType().Name;
            if (1 < attrs.Count)
                typeName = $"{typeName} ({attrs.Count} items)";

            var items = GetNamespaces(item.Document);
            var valueType = typeof(string);
            var valueTypeName = valueType.Name;
            var result = new List<Tuple<string, string>>();

            foreach (var attr in attrs)
            {
                var nsName = attr.Name.NamespaceName;
                var localName = attr.Name.LocalName;

                localName = ResolveNamespace(items, nsName, localName);
                result.Add(Tuple.Create(localName, attr.Value));
            }

            return converter.CreateMemberData(typeName, result);
        }

        private StackPanel CreateXDeclarationData(XDeclaration dec)
        {
            var typeName = dec.GetType().Name;
            var version = string.IsNullOrEmpty(dec.Version) ? string.Empty : dec.Version;
            var encode = string.IsNullOrEmpty(dec.Encoding) ? string.Empty : dec.Encoding;
            var standalone = string.IsNullOrEmpty(dec.Standalone) ? string.Empty : dec.Standalone;

            var items = new List<Tuple<string, string>>
            {
                Tuple.Create("Version", version),
                Tuple.Create("Encoding", encode),
                Tuple.Create("Standalone", standalone),
            };

            return converter.CreateMemberData(typeName, items);
        }

        private StackPanel CreateXCommentData(XComment comment)
        {
            var variableName = comment.GetType().Name;
            var variableValue = comment.Value;

            return converter.CreateSingleData(variableName, variableValue);
        }

        private FrameworkElement CreateXElementData(XElement element)
        {
            var start = converter.CreateLayout();
            var grid1 = start.Item2;

            // タイトル用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var rowIndex = 0;
            var title = GetVariableTypeName(element);
            converter.AddExpandCollapseButton(grid1, title);

            if (!element.HasAttributes && !element.Nodes().Any())
                return start.Item1;

            // タイトル / 型名、子タグ個数、タグ名
            if (element.HasAttributes)
            {
                // 謎仕様？子ノードがある時の属性の取得順番について、逆順に取得してしまうので、逆順の逆順に取得する
                var attrs = default(IEnumerable<XAttribute>);
                if (element.Nodes().Any())
                    attrs = element.Attributes().Reverse();
                else
                    attrs = element.Attributes();

                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                rowIndex++;

                // メンバー数が多すぎるため?、指定メンバーのみに絞る
                //var nodeElement = CreateMemberData(attr, attr.GetType());
                var nodeElement = CreateXAttributeData(attrs.ToList());

                var memberPanel = new DockPanel();
                grid1.Children.Add(memberPanel);
                Grid.SetRow(memberPanel, rowIndex);
                Grid.SetColumn(memberPanel, 0);

                var memberLine1 = new Line() { X2 = 1 };
                memberPanel.Children.Add(memberLine1);
                DockPanel.SetDock(memberLine1, Dock.Top);

                memberPanel.Children.Add(nodeElement);
            }

            if (element.Nodes().Any())
            {
                foreach (XNode node in element.Nodes())
                {
                    grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    rowIndex++;

                    var nodeElement = default(FrameworkElement);
                    if (node is XComment)
                    {
                        var castedElement = node as XComment;
                        nodeElement = CreateXCommentData(castedElement);
                    }
                    else if (node is XText)
                    {
                        var castedElement = node as XText;
                        nodeElement = converter.CreatePrimitiveData(castedElement.Value);
                    }
                    else if (node is XElement)
                    {
                        var castedElement = node as XElement;
                        nodeElement = CreateXElementData(castedElement);
                    }
                    else
                    {
                        // 未対応分
                        nodeElement = converter.CreatePrimitiveData(node.ToString());
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
        
        private string GetVariableTypeName(XDocument doc)
        {
            // コメントは子タグ数に含めない
            var itemsCount = doc.Document.Nodes().Count();
            foreach (XNode node in doc.Document.Nodes())
            {
                if (node is XComment)
                    itemsCount--;
            }

            var typeName = doc.GetType().Name;
            if (itemsCount == 1)
                typeName = $"{typeName} (1 child item)";
            else
                typeName = $"{typeName} ({itemsCount} child items)";

            return typeName;
        }

        private string GetVariableTypeName(XElement element)
        {
            // コメントは子タグ数に含めない
            var itemsCount = element.Nodes().Count();
            foreach (XNode node in element.Nodes())
            {
                if (node is XComment)
                    itemsCount--;
            }

            var typeName = element.GetType().Name;
            var tagName = GetXElementName(element);

            if (itemsCount == 1)
                typeName = $"{typeName} <{tagName}> (1 child item)";
            else
                typeName = $"{typeName} <{tagName}> ({itemsCount} child items)";

            return typeName;
        }

        private string GetXElementName(XElement element)
        {
            var items = GetNamespaces(element.Document);
            var nsName = element.Name.NamespaceName;
            var localName = element.Name.LocalName;
            localName = ResolveNamespace(items, nsName, localName);

            return localName;
        }

        private List<Tuple<string, string, string>> GetNamespaces(XDocument doc)
        {
            var items = doc.Descendants()
                .Where(x => x.HasAttributes)
                .SelectMany(x => x.Attributes())
                .Select(x => Tuple.Create(x.Name.NamespaceName, x.Name.LocalName, x.Value))
                .ToList();

            // xmlns プリフェックスの定義を取得できないため、手動置換する
            items.Add(Tuple.Create(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/"));
            return items;
        }

        private string ResolveNamespace(List<Tuple<string, string, string>> items, string nsName, string localName)
        {
            if (!string.IsNullOrEmpty(nsName))
            {
                var foundName = items.FirstOrDefault(x => x.Item3 == nsName); // Item3: Value
                if (foundName is null)
                {
                    if (nsName.EndsWith("/"))
                        nsName = nsName.Substring(0, nsName.Length - 1);

                    nsName = nsName.Substring(nsName.LastIndexOf("/") + 1);
                }
                else
                {
                    nsName = foundName.Item2; // Item2: LocalName
                }

                localName = $"{nsName}:{localName}";
            }

            return localName;
        }

        public DockPanel CreateWebBrowserPanel(string xmlString)
        {
            return converter.CreateWebBrowserPanel(xmlString);
        }
    }
}
