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

    #region Tuple 定義


    public class Tuple<T1>
    {
        public T1 Item1 { get; }

        internal Tuple(T1 item1)
        {
            this.Item1 = item1;
        }
    }

    public class Tuple<T1, T2>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }

        internal Tuple(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }

    public class Tuple<T1, T2, T3>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }

        internal Tuple(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }
    }

    public class Tuple<T1, T2, T3, T4>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }

        internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
        }
    }

    public class Tuple<T1, T2, T3, T4, T5>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }
        public T5 Item5 { get; }

        internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
        }
    }

    public class Tuple<T1, T2, T3, T4, T5, T6>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }
        public T5 Item5 { get; }
        public T6 Item6 { get; }

        internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
        }
    }

    public class Tuple<T1, T2, T3, T4, T5, T6, T7>
    {
        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
        public T4 Item4 { get; }
        public T5 Item5 { get; }
        public T6 Item6 { get; }
        public T7 Item7 { get; }

        internal Tuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
            this.Item5 = item5;
            this.Item6 = item6;
            this.Item7 = item7;
        }
    }

    public static class Tuple
    {
        public static Tuple<T1> Create<T1>(T1 item1)
        {
            return new Tuple<T1>(item1);
        }

        public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
        {
            return new Tuple<T1, T2>(item1, item2);
        }

        public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
        {
            return new Tuple<T1, T2, T3>(item1, item2, item3);
        }

        public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }

        public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
        {
            return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        }

        public static Tuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        {
            return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        }
    }


    #endregion


    public enum LanguageTypes
    {
        CSharp,
        VBNET,
    }

    public class ViewCreator
    {
        private readonly LanguageTypes _LanguageTypes;

        public ViewCreator(LanguageTypes languageTypes = LanguageTypes.CSharp)
        {
            _LanguageTypes = languageTypes;
        }


        #region 公開機能


        public FrameworkElement CreateCallerInfoData(string className, string methodName)
        {
            var sourceInfo = $"{className}.{methodName} メソッド内:";
            var headerBlock = new TextBlock() { Text = sourceInfo, Foreground = Brushes.Gray, HorizontalAlignment = HorizontalAlignment.Left };

            return headerBlock;
        }

        public FrameworkElement CreateCallerInfoData(string sourceFilePath, string memberName, int sourceLineNumber)
        {
            var sourceFile = new FileInfo(sourceFilePath);
            var sourceInfo = $"{sourceFile.Directory.Name}/{sourceFile.Name}: {memberName} メソッド: {sourceLineNumber} 行目";
            var block1 = new TextBlock() { Text = sourceInfo, Foreground = Brushes.Gray, HorizontalAlignment = HorizontalAlignment.Left };

            return block1;
        }

        public FrameworkElement CreateViewData(object instance, int recursiveCallCount = 0)
        {
            // ここで判定を変えたら、以下のメソッドも再確認する必要あり
            // GetRecursiveLimitData()
            // ToDataTable()

            var result = default(FrameworkElement);
            var t = instance?.GetType();

            if (instance is null)
            {
                var title = ToTagetLanguageKeyword("null");
                result = CreatePrimitiveData(title);
            }
            else if (instance is DBNull)
            {
                result = CreatePrimitiveData("DBNull");
            }
            else if (instance is Type || instance is IntPtr || instance is UIntPtr || t.Name == "RuntimeModule")
            {
                // int, string などのクラスは展開しないで、文字列表示するように修正
                var value = GetVariableTypeNameCore(instance.ToString());
                if (value == "Void" && _LanguageTypes == LanguageTypes.VBNET)
                    value = $"{value}（≒Subメソッド）";

                result = CreatePrimitiveData(value, Brushes.Blue);
            }
            else if (IsImageType(t))
            {
                if (IsBitmapType(t))
                {
                    var bitmap = instance as System.Drawing.Bitmap;
                    var source = ToBitmapSource(bitmap);
                    result = CreateImageData(source);
                }
                else if (IsBitmapSourceType(t))
                {
                    var source = instance as BitmapSource;
                    result = CreateImageData(source);
                }
                else
                {
                    result = CreatePrimitiveData($"※未対応（{t.FullName}）");
                }
            }
            else if (IsPrimitiveType(t))
            {
                result = CreateViewDataForPrimitiveType(instance, t, recursiveCallCount);
            }
            else if (IsEnumType(t))
            {
                var title = $"{t.Name}.{instance.ToString()}";
                result = CreatePrimitiveData(title, Brushes.Green);
            }
            else if (IsDelegateType(t))
            {
                // メソッドのシグネチャーを表示
                result = CreateDelegateData(t);
            }
            else if (IsExceptionType(t))
            {
                result = CreateExceptionData(instance, t);
            }
            else if (IsDataSetFamilyType(t))
            {
                result = CreateViewDataForDataSetFamilyType(instance, t, recursiveCallCount);
            }
            else if (IsXmlDocumentFamilyType(t))
            {
                result = CreateViewDataForXmlDocumentFamilyType(instance, t, recursiveCallCount);
            }
            else if (IsXDocumentFamilyType(t))
            {
                result = CreateViewDataForXDocumentFamilyType(instance, t, recursiveCallCount);
            }
            else if (IsEntityFrameworkFamilyType(t))
            {
                result = CreateViewDataForEntityFrameworkFamilyType(instance, t, recursiveCallCount);
            }
            else if (IsCollectionType(t))
            {
                // 誤判定バグの対応（DataView がコレクション扱いされてしまうなど）。コレクション系は後半に判定するように対応
                var items = instance as IEnumerable;
                result = CreateCollectionData(items, recursiveCallCount);
            }
            else
            {
                // 何かのクラス、構造体、または匿名型
                // 公開フィールド、プロパティ
                result = CreateMemberData(instance, t, recursiveCallCount);
            }

            return result;
        }

        private FrameworkElement CreateViewDataForPrimitiveType(object instance, Type t, int recursiveCallCount)
        {
            var result = default(FrameworkElement);

            if (IsBoolType(t))
            {
                var value = instance.ToString();
                value = ToTagetLanguageKeyword(value);
                result = CreatePrimitiveData(value);
            }
            else if (IsNumberType(t))
            {
                var title = String.Format("{0:#,0}", instance);
                result = CreatePrimitiveData(title);
            }
            else if (IsDateTimeFamilyType(t))
            {
                var value = instance.ToString();
                result = CreatePrimitiveData(value);
            }
            else if (IsStringFamilyType(t))
            {
                // 空文字の場合何も表示されず分かりにくいため、調整する
                var s = instance as string;
                if (string.IsNullOrEmpty(s))
                {
                    s = "(空欄)";
                }


                if (s == "__nullFromDataTable__")
                {
                    var title = ToTagetLanguageKeyword("null");
                    result = CreatePrimitiveData(title);
                }
                else if (s == "__DBNullFromDataTable__")
                {
                    result = CreatePrimitiveData("DBNull");
                }
                else if (IsXml(s))
                {
                    result = CreateReplaceMessageLinkLabel("XmlLayout", s, "インラインブラウザで表示できます");
                }
                else if (IsCsv(s))
                {
                    result = CreateReplaceMessageLinkLabel("CsvLayout", s, "表形式で表示できます");
                }
                else
                {
                    result = CreatePrimitiveData(s, Brushes.Brown, false);
                }
            }

            return result;
        }

        private FrameworkElement CreateViewDataForDataSetFamilyType(object instance, Type t, int recursiveCallCount)
        {
            var ds = default(DataSet);
            var element = default(FrameworkElement);

            if (typeof(DataSet).IsAssignableFrom(t))
            {
                ds = instance as DataSet;
                element = CreateDataSetData(instance, recursiveCallCount);
            }
            else if (typeof(DataTable).IsAssignableFrom(t))
            {
                var table = instance as DataTable;
                ds = ToDataSet(table);

                var title = t.Name;
                element = CreateDataTableData(instance, recursiveCallCount, title);
            }
            else if (typeof(DataRow).IsAssignableFrom(t))
            {
                var row = instance as DataRow;
                ds = ToDataSet(row);

                var title = t.Name;
                element = CreateDataRowData(instance, recursiveCallCount, title);
            }
            else if (typeof(DataView).IsAssignableFrom(t))
            {
                var view = instance as DataView;
                ds = ToDataSet(view);
                element = CreateDataViewData(instance, recursiveCallCount);
            }
            else if (typeof(DataRowView).IsAssignableFrom(t))
            {
                var row = instance as DataRowView;
                ds = ToDataSet(row);
                element = CreateDataRowViewData(instance, recursiveCallCount);
            }

            var result = CreateShowSubFormLinkLabel(ds, element, recursiveCallCount);
            return result;
        }

        private FrameworkElement CreateViewDataForXmlDocumentFamilyType(object instance, Type t, int recursiveCallCount)
        {
            var result = default(FrameworkElement);

            if (t.Equals(typeof(XmlDocument)))
            {
                var doc = instance as XmlDocument;
                result = CreateXmlData(doc, recursiveCallCount);
            }
            else if (t.Equals(typeof(XmlDeclaration)))
            {
                var element = instance as XmlDeclaration;
                result = CreateXmlDeclarationData(element);
            }
            else if (t.Equals(typeof(XmlAttribute)))
            {
                var element = instance as XmlAttribute;
                result = CreateXmlAttributeData(element);
            }
            else if (t.Equals(typeof(XmlComment)))
            {
                var element = instance as XmlComment;
                result = CreateXmlCommentData(element);
            }
            else if (t.Equals(typeof(XmlElement)))
            {
                var element = instance as XmlElement;
                result = CreateXmlElementData(element, recursiveCallCount, true);
            }

            return result;
        }

        private FrameworkElement CreateViewDataForXDocumentFamilyType(object instance, Type t, int recursiveCallCount)
        {
            var result = default(FrameworkElement);

            if (t.Equals(typeof(XDocument)))
            {
                var doc = instance as XDocument;
                result = CreateXmlData(doc, recursiveCallCount);
            }
            else if (t.Equals(typeof(XDeclaration)))
            {
                var element = instance as XDeclaration;
                result = CreateXDeclarationData(element);
            }
            else if (t.Equals(typeof(XAttribute)))
            {
                var element = instance as XAttribute;
                result = CreateXAttributeData(element);
            }
            else if (t.Equals(typeof(XComment)))
            {
                var element = instance as XComment;
                result = CreateXCommentData(element);
            }
            else if (t.Equals(typeof(XElement)))
            {
                var element = instance as XElement;
                result = CreateXElementData(element, recursiveCallCount, true);
            }

            return result;
        }

        private FrameworkElement CreateViewDataForEntityFrameworkFamilyType(object instance, Type t, int recursiveCallCount)
        {
            var ds = default(DataSet);
            var element = default(FrameworkElement);

            if (IsDbContextType(t))
            {
                ds = ToDataSetForDbContext(instance, t);
                element = CreateDbContextData(instance, t, recursiveCallCount);
            }
            else if (IsDbSetType(t))
            {
                ds = ToDataSetForDbSet(instance);
                element = CreateDbSetData(instance, t, recursiveCallCount);
            }

            var result = CreateShowSubFormLinkLabel(ds, element, recursiveCallCount);
            return result;
        }


        #endregion


        #region 型チェック関連


        // 論理型かどうか
        private bool IsBoolType(Type t)
        {
            if (t.Equals(typeof(bool))) return true;
            return false;
        }

        // 数値系かどうか
        private bool IsNumberType(Type t)
        {
            if (t.Equals(typeof(byte))) return true;
            if (t.Equals(typeof(sbyte))) return true;
            if (t.Equals(typeof(decimal))) return true;
            if (t.Equals(typeof(double))) return true;
            if (t.Equals(typeof(float))) return true;
            if (t.Equals(typeof(int))) return true;
            if (t.Equals(typeof(uint))) return true;
            if (t.Equals(typeof(long))) return true;
            if (t.Equals(typeof(ulong))) return true;
            if (t.Equals(typeof(short))) return true;
            if (t.Equals(typeof(ushort))) return true;

            return false;
        }

        // 日付型かどうか
        private bool IsDateTimeFamilyType(Type t)
        {
            if (t.Equals(typeof(DateTime))) return true;
            if (t.Equals(typeof(DateTimeOffset))) return true;
            if (t.Equals(typeof(TimeSpan))) return true;
            return false;
        }

        // 文字列型かどうか
        private bool IsStringFamilyType(Type t)
        {
            if (t.Equals(typeof(char))) return true;
            if (t.Equals(typeof(string))) return true;
            return false;
        }

        // 組み込みの型かどうか
        private bool IsPrimitiveType(Type t)
        {
            if (IsBoolType(t)) return true;
            if (IsNumberType(t)) return true;
            if (IsDateTimeFamilyType(t)) return true;
            if (IsStringFamilyType(t)) return true;
            return false;
        }

        // コレクション系かどうか
        private bool IsCollectionType(Type t)
        {
            if (t.Equals(typeof(string))) return false;
            if (typeof(IEnumerable).IsAssignableFrom(t)) return true;
            return false;
        }

        // クラス型かどうか
        private bool IsClassType(Type t)
        {
            if (t.IsClass) return true;
            return false;
        }

        // 匿名型かどうか
        private bool IsAnonymousTypeType(Type t)
        {
            if (t.Name.Contains("f__AnonymousType")) return true;
            return false;
        }

        // 構造体型かどうか
        private bool IsStructType(Type t)
        {
            if (t.IsValueType && ((t.Attributes & TypeAttributes.SequentialLayout) == TypeAttributes.SequentialLayout)) return true;
            return false;
        }

        // 列挙体型かどうか
        private bool IsEnumType(Type t)
        {
            //if (t.IsValueType && typeof(System.Enum).IsAssignableFrom(t)) return true;
            if (t.IsEnum) return true;
            return false;
        }

        // デリゲート型かどうか
        private bool IsDelegateType(Type t)
        {
            if (typeof(System.Delegate).IsAssignableFrom(t)) return true;
            return false;
        }

        // 例外エラー型かどうか
        private bool IsExceptionType(Type t)
        {
            if (typeof(Exception).IsAssignableFrom(t)) return true;
            return false;
        }

        // DataSet 系かどうか（型無し、型付き含む）
        private bool IsDataSetFamilyType(Type t)
        {
            if (typeof(DataSet).IsAssignableFrom(t)) return true;
            if (typeof(DataTable).IsAssignableFrom(t)) return true;
            if (typeof(DataRow).IsAssignableFrom(t)) return true;
            if (typeof(DataView).IsAssignableFrom(t)) return true;
            if (typeof(DataRowView).IsAssignableFrom(t)) return true;
            return false;
        }

        // XmlDocument 系かどうか
        private bool IsXmlDocumentFamilyType(Type t)
        {
            if (t.Equals(typeof(XmlDocument))) return true;
            if (t.Equals(typeof(XmlDeclaration))) return true;
            if (t.Equals(typeof(XmlAttribute))) return true;
            if (t.Equals(typeof(XmlComment))) return true;
            if (t.Equals(typeof(XmlElement))) return true;
            return false;
        }

        // XDocument 系かどうか
        private bool IsXDocumentFamilyType(Type t)
        {
            if (t.Equals(typeof(XDocument))) return true;
            if (t.Equals(typeof(XDeclaration))) return true;
            if (t.Equals(typeof(XAttribute))) return true;
            if (t.Equals(typeof(XComment))) return true;
            if (t.Equals(typeof(XElement))) return true;
            return false;
        }

        // Entity Framework 系かどうか
        private bool IsEntityFrameworkFamilyType(Type t)
        {
            if (IsDbContextType(t)) return true;
            if (IsDbSetType(t)) return true;
            return false;
        }

        // DbContext 型かどうか
        private bool IsDbContextType(Type t)
        {
            if (t.BaseType?.Name == "DbContext" && t.BaseType.FullName.StartsWith("System.Data.Entity.DbContext")) return true;
            return false;
        }

        // DbSet 型かどうか
        private bool IsDbSetType(Type t)
        {
            if (t.Name.StartsWith("DbSet") && t.FullName.StartsWith("System.Data.Entity.DbSet")) return true;
            return false;
        }

        // 文字列が xml 形式かどうか
        private bool IsXml(string s)
        {
            // タグ系以外ははじく
            s = s.Trim();
            if (!s.StartsWith("<")) return false;

            // html4, 5 系をはじく
            if (s.ToLower().StartsWith("<!doctype html")) return false;
            if (s.ToLower().StartsWith("<html")) return false;

            try
            {
                var dummy = new XmlDocument();
                dummy.LoadXml(s);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 文字列が csv 形式かどうか
        private bool IsCsv(string s)
        {
            var lines = s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            // 最低2行データがあること
            if (lines.Count() < 2)
                return false;

            // 1行目と2行目をサンプリングデータとしてみた場合、カンマ or タブ区切りがあること
            var check1 = Regex.IsMatch(lines[0], "^(.+)([,\t])(.*)");
            var check2 = Regex.IsMatch(lines[1], "^(.+)([,\t])(.*)");
            if (check1 && check2)
                return true;
            else
                return false;
        }

        // 画像系かどうか
        private bool IsImageType(Type t)
        {
            if (IsBitmapType(t)) return true;
            if (IsBitmapSourceType(t)) return true;
            return false;
        }

        // (WinForms) System.Drawing.Bitmap かどうか
        private bool IsBitmapType(Type t)
        {
            if (typeof(System.Drawing.Bitmap).IsAssignableFrom(t)) return true;
            return false;
        }

        // (WPF) System.Windows.Media.Imaging.BitmapSource かどうか
        private bool IsBitmapSourceType(Type t)
        {
            if (typeof(BitmapSource).IsAssignableFrom(t)) return true;
            return false;
        }

        #endregion


        #region 共通作成関連


        private TextBlock CreatePrimitiveData(string value, Brush foreColor = null, bool setColoring = true)
        {
            if (setColoring)
            {
                if (value == "null" || value == "Nothing") foreColor = Brushes.Blue;
                if (value == "DBNull") foreColor = Brushes.Green;
                if (value.ToLower() == "true") foreColor = Brushes.Blue;
                if (value.ToLower() == "false") foreColor = Brushes.Blue;
                if (foreColor == null) foreColor = Brushes.Black;
            }

            return new TextBlock() { Text = value, Foreground = foreColor, TextWrapping = TextWrapping.Wrap };
        }

        private StackPanel CreateMemberData(object instance, Type t, int recursiveCallCount)
        {
            var typeName = GetVariableTypeName(t);
            var items = GetFieldAndPropertyMembers(instance, t);

            return CreateMemberData(typeName, items, recursiveCallCount);
        }

        private StackPanel CreateMemberData(string typeName, List<Tuple<string, Type, object>> items, int recursiveCallCount)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            // メンバー名と値分
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            // 型名
            var rowIndex = 0;
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);
            Grid.SetColumnSpan(titlePanel, 2);

            var titleBlock = new TextBlock() { Text = typeName, FontWeight = FontWeights.UltraBold };
            titlePanel.Children.Add(titleBlock);

            // メンバー数分
            for (var i = 0; i < items.Count; i++)
            {
                var memberName = items[i].Item1;
                var memberType = items[i].Item2;
                var memberInstance = items[i].Item3;

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

                var memberTypeName = GetVariableTypeName(memberType);
                SetToolTip(nameBlock, memberTypeName, memberName);


                // メンバー値
                var valuePanel = new DockPanel();
                grid1.Children.Add(valuePanel);
                Grid.SetRow(valuePanel, rowIndex);
                Grid.SetColumn(valuePanel, 1);

                var valueLine1 = new Line() { X2 = 1 };
                valuePanel.Children.Add(valueLine1);
                DockPanel.SetDock(valueLine1, Dock.Top);

                // 再帰呼び出し回数の制限処理
                var memberElement = GetRecursiveLimitData(memberInstance, recursiveCallCount);
                valuePanel.Children.Add(memberElement);
            }

            return stackPanel1;
        }

        private FrameworkElement CreateContinueLinkLabel(object memberInstance)
        {
            var linkGrid = new Grid();
            linkGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            linkGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var typeName = GetVariableTypeName(memberInstance);
            var linkBlock = new TextBlock();
            var linkCore = new Hyperlink(new Run(typeName));
            linkBlock.Inlines.Add(linkCore);
            linkGrid.Children.Add(linkBlock);
            Grid.SetRow(linkBlock, 0);
            Grid.SetColumn(linkBlock, 0);

            // 最初はリンクを表示させておく。リンクをクリックしたら、リンクを隠して、再取得分のビューを表示させる
            linkCore.Click += (s, e) =>
            {
                var memberElement = CreateViewData(memberInstance);
                linkGrid.Children.Add(memberElement);
                Grid.SetRow(memberElement, 0);
                Grid.SetColumn(memberElement, 0);

                linkBlock.Visibility = Visibility.Hidden;

            };

            return linkGrid;
        }

        private FrameworkElement CreateReplaceMessageLinkLabel(string kindName, string s, string message)
        {
            var isXmlLayout = (kindName == "XmlLayout");
            var isCsvLayout = (kindName == "CsvLayout");

            var linkGrid = new Grid() { HorizontalAlignment = HorizontalAlignment.Left };
            linkGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            linkGrid.ColumnDefinitions.Add(new ColumnDefinition());

            var linkBlock = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Left };
            var linkCore = new Hyperlink(new Run(message));
            linkBlock.Inlines.Add(linkCore);

            var linkCheck = new CheckBox() { Content = "1 行目は、ヘッダー列として扱う", VerticalAlignment = VerticalAlignment.Center };
            var headerPanel = new StackPanel() { Orientation = Orientation.Horizontal };
            headerPanel.Children.Add(linkBlock);
            headerPanel.Children.Add(linkCheck);

            // チェック欄は表形式用。該当しない場合や列名に重複があれば、非表示に変える
            if (isXmlLayout)
            {
                linkCheck.Visibility = Visibility.Hidden;
            }
            else if (isCsvLayout)
            {
                // １つ目のデータに重複があるかチェック
                var table = ToDataTableForCsv(s);
                var row = table.Rows[0];

                var dic = new Dictionary<string, int>();
                foreach (DataColumn column in table.Columns)
                {
                    var name = $"{row[column.ColumnName]}";
                    if (dic.ContainsKey(name))
                        dic[name]++;
                    else
                        dic.Add(name, 1);
                }

                foreach (var item in dic)
                {
                    if (1 < item.Value)
                    {
                        linkCheck.Visibility = Visibility.Hidden;
                        break;
                    }
                }
            }

            var linkPanel = new StackPanel();
            linkPanel.Children.Add(headerPanel);

            var strElement = CreatePrimitiveData(s, Brushes.Brown, false);
            strElement.HorizontalAlignment = HorizontalAlignment.Left;
            linkPanel.Children.Add(strElement);

            linkGrid.Children.Add(linkPanel);
            Grid.SetRow(linkPanel, 0);
            Grid.SetColumn(linkPanel, 0);

            linkCore.Click += (s2, e2) =>
            {
                var memberElement = default(FrameworkElement);
                if (isXmlLayout)
                {
                    memberElement = CreateXmlData(s);
                }
                else if (isCsvLayout)
                {
                    var isChecked = linkCheck.IsChecked.HasValue ? linkCheck.IsChecked.Value : false;
                    memberElement = CreateCsvData(s, isChecked);
                }

                linkGrid.Children.Add(memberElement);
                Grid.SetRow(memberElement, 0);
                Grid.SetColumn(memberElement, 0);

                linkPanel.Visibility = Visibility.Hidden;

            };

            return linkGrid;
        }

        private FrameworkElement CreateShowSubFormLinkLabel(DataSet ds, FrameworkElement element, int recursiveCallCount)
        {
            // 再帰（入れ子）の場合は表示しない、データが無い場合は表示しない
            if (0 < recursiveCallCount)
                return element;
            else if (ds.Tables.Count == 0)
                return element;
            else if (0 < ds.Tables.Count)
            {
                var isNoData = true;
                foreach (DataTable table in ds.Tables)
                {
                    if (0 < table.Rows.Count)
                    {
                        isNoData = false;
                        break;
                    }
                }

                if (isNoData)
                    return element;
            }


            var linkGrid = new Grid();
            linkGrid.ColumnDefinitions.Add(new ColumnDefinition());
            linkGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            linkGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            var linkBlock = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Left };
            var linkCore = new Hyperlink(new Run("DataSet ビジュアライザーで見る"));
            linkBlock.Inlines.Add(linkCore);

            linkGrid.Children.Add(linkBlock);
            Grid.SetRow(linkBlock, 0);
            Grid.SetColumn(linkBlock, 0);

            var margin = element.Margin;
            margin.Top = 0;
            element.Margin = margin;

            linkGrid.Children.Add(element);
            Grid.SetRow(element, 1);
            Grid.SetColumn(element, 0);

            // その時点のデータのままにしたいため、インスタンスをコピーして保持しておく
            //（LINQ と同じ考え方で、そのまま使おうとすると後続処理で変更が加わった後の、最新データに変わってしまう）
            var freezeDS = ds.Copy();

            linkCore.Click += (s, e) =>
            {
                using (var dlg = new DataSetVisualizerForm())
                {
                    dlg.Target = freezeDS;
                    dlg.ShowDialog();
                }
            };

            return linkGrid;
        }

        private FrameworkElement CreateShowSubWindowLinkLabel(object instance, string xmlData, FrameworkElement xmlGrid, int recursiveCallCount)
        {
            // 再帰（入れ子）の場合は表示しない
            if (0 < recursiveCallCount)
                return xmlGrid;

            var linkGrid = new Grid();
            linkGrid.ColumnDefinitions.Add(new ColumnDefinition());
            linkGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            linkGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            var linkBlock = new TextBlock() { HorizontalAlignment = HorizontalAlignment.Left };
            var linkCore = new Hyperlink(new Run("XML ビジュアライザーで見る"));
            linkBlock.Inlines.Add(linkCore);

            linkGrid.Children.Add(linkBlock);
            Grid.SetRow(linkBlock, 0);
            Grid.SetColumn(linkBlock, 0);

            var margin = xmlGrid.Margin;
            margin.Top = 0;
            xmlGrid.Margin = margin;

            linkGrid.Children.Add(xmlGrid);
            Grid.SetRow(xmlGrid, 1);
            Grid.SetColumn(xmlGrid, 0);

            // InvalidOperationException が発生するバグの対応（指定された要素は、既に別の要素の論理子です。まず接続を切断してください）
            // インスタンスをを使いまわさず、クリックするたびに一意のインスタンスを生成して使うように修正
            linkCore.Click += (s, e) =>
            {
                var structPanel = default(FrameworkElement);
                if (instance is XmlDocument)
                {
                    var element = instance as XmlDocument;
                    structPanel = CreateXmlDocumentData(element, recursiveCallCount);
                }
                else if (instance is XmlElement)
                {
                    var element = instance as XmlElement;
                    structPanel = CreateXmlElementData(element, recursiveCallCount);
                }
                else if (instance is XDocument)
                {
                    var element = instance as XDocument;
                    structPanel = CreateXDocumentData(element, recursiveCallCount);
                }
                else if (instance is XElement)
                {
                    var element = instance as XElement;
                    structPanel = CreateXElementData(element, recursiveCallCount);
                }

                var browserPanel = CreateXmlData(xmlData);

                var dlg = new XmlWindow();
                dlg.VariableElement = structPanel;
                dlg.BrowserElement = browserPanel;
                dlg.ShowDialog();
            };

            return linkGrid;
        }

        private StackPanel CreateExceptionData(object instance, Type t)
        {
            var exceptionName = GetVariableTypeName(t);
            var exceptionMessage = (instance as Exception).Message;

            return CreateSingleData(exceptionName, exceptionMessage);
        }

        private StackPanel CreateSingleData(string title, string value)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            // タイトルと値用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, 0);
            Grid.SetColumn(titlePanel, 0);

            var titleLine = new Line() { X2 = 1 };
            titlePanel.Children.Add(titleLine);
            DockPanel.SetDock(titleLine, Dock.Bottom);

            var titleBlock = new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold };
            titlePanel.Children.Add(titleBlock);

            // 値
            var messagePanel = new DockPanel();
            grid1.Children.Add(messagePanel);
            Grid.SetRow(messagePanel, 1);
            Grid.SetColumn(messagePanel, 0);

            var messageBlock = CreateViewData(value);
            messagePanel.Children.Add(messageBlock);

            return stackPanel1;
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

        private FrameworkElement CreateCsvData(string s, bool isFirstColumn = false)
        {
            var table = ToDataTableForCsv(s, isFirstColumn);
            var element = CreateDataTableData(table, 0, "string");

            var foundQuote = false;
            var foundNewLine = false;
            foreach (DataRow row in table.Rows)
            {
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    var value = $"{row[i]}";
                    if (value.Contains("（改行）"))
                        foundNewLine = true;

                    if (value.Contains(@""""))
                        foundQuote = true;
                }
            }

            var message = string.Empty;
            if (foundQuote)
                message = "文字列内に空白やタブ文字がある場合、ダブルコーテーションで囲っています。";

            if (foundNewLine)
            {
                if (string.IsNullOrEmpty(message))
                    message = "文字列内に改行がある場合、認識文字列として置換しています。";
                else
                    message = $"{message}また、文字列内に改行がある場合、認識文字列として置換しています。";
            }

            var stackPanel1 = new StackPanel();
            stackPanel1.Children.Add(new TextBlock()
            {
                Text = message,
                Foreground = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Left,
            });

            var m = element.Margin;
            m.Top = 0;
            element.Margin = m;
            stackPanel1.Children.Add(element);

            return stackPanel1;
        }


        #endregion


        #region コレクションデータ作成関連


        private StackPanel CreateCollectionData(IEnumerable items, int recursiveCallCount)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            var count = 0;
            foreach (var item in items)
                count++;

            var title = GetVariableTypeName(items, count);
            if (count == 0)
            {
                CreateZeroItemsCollectionData(grid1, title);
            }
            else
            {
                // いったん DataTable 上で表示データを準備する、それを xaml にマッピングしていく
                var table = ToDataTable(items);

                if (table.Columns.Count == 1)
                    CreateSingleColumnCollectionData(grid1, table, title, recursiveCallCount);
                else
                    CreateMultiColumnsCollectionData(grid1, table, title, recursiveCallCount);
            }

            return stackPanel1;
        }

        // コレクション系の型でデータ数が 0 件
        private void CreateZeroItemsCollectionData(Grid grid1, string title = "")
        {
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            var headerPanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(headerPanel);
            Grid.SetRow(headerPanel, 0);
            Grid.SetColumn(headerPanel, 0);

            if (string.IsNullOrEmpty(title))
                title = "(0 items)";

            headerPanel.Children.Add(new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold });
        }

        private void CreateSingleColumnCollectionData(Grid grid1, DataTable table, string title, int recursiveCallCount)
        {
            // 列名無しのコレクション表示をする

            // 型名
            var rowIndex = 0;
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);

            titlePanel.Children.Add(new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold });

            // コレクションデータ
            for (var i = 0; i < table.Rows.Count; i++)
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

                // 再帰呼び出し回数の制限処理
                var row = table.Rows[i];
                var memberInstance = row[0];
                var memberElement = GetRecursiveLimitData(memberInstance, recursiveCallCount);
                memberPanel.Children.Add(memberElement);
            }
        }

        private void CreateMultiColumnsCollectionData(Grid grid1, DataTable table, string title, int recursiveCallCount, bool isCalledDataTable = false)
        {
            // 列名ありのコレクション表示をする

            // 型名
            var rowIndex = 0;
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            for (var i = 0; i < table.Columns.Count; i++)
                grid1.ColumnDefinitions.Add(new ColumnDefinition());

            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);
            Grid.SetColumnSpan(titlePanel, table.Columns.Count);

            titlePanel.Children.Add(new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold });

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

                    // 渡されたのがコレクション系の場合、型は全て object 型にしている。その代わり Caption に本来の型を登録しているため、抽出して使う
                    // 渡されたのが DataSet 系の場合、型をそのまま使う
                    if (memberTypeName.StartsWith("__MemberType__"))
                    {
                        memberTypeName = memberTypeName.Substring(memberTypeName.IndexOf("__MemberType__") + "__MemberType__".Length);
                        SetToolTip(headerBlock, memberTypeName, memberName);
                    }
                    else
                    {
                        var memberType = table.Columns[i].DataType;
                        memberTypeName = GetVariableTypeName(memberType);
                        SetToolTip(headerBlock, memberTypeName, memberName);
                    }
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

                    if (memberInstance is DBNull && !(isCalledDataTable))
                    {
                        // コレクション系から渡ってきた DataTable 内の DBNull は、インスタンスが持っていないフィールド・プロパティのことになるので、空欄を表示する
                        //（ただし、空文字にすると「(空欄)」の文字列を表示してしまうため、スペースをセットする）
                        memberInstance = " ";
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

                    // 再帰呼び出し回数の制限処理
                    var memberElement = GetRecursiveLimitData(memberInstance, recursiveCallCount);
                    memberPanel.Children.Add(memberElement);
                }
            }
        }

        private void SetToolTip(FrameworkElement targetElement, string typeName, string memberName)
        {
            var headerTip = new ToolTip();
            var tipElement = default(StackPanel);

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


        #region Delegate 作成関連


        private StackPanel CreateDelegateData(Type t)
        {
            var title = GetVariableTypeName(t);
            var signature = CreateMethodSignatureData(t);

            return CreateSingleData(title, signature);
        }

        // オープンジェネリック型(List<T>)を対象としてしまったが、ほとんどクローズドジェネリック型(List<string> など)が渡ってくるかも
        private string CreateMethodSignatureData(Type t, string methodName = "Sample")
        {
            var sb = new StringBuilder();
            var constraints = default(List<string>);
            var invokeMethod = t.GetMethod("Invoke");
            var methodArguments = "()";
            var returnType = "Void";

            // ジェネリック定義している場合
            if (t.IsGenericTypeDefinition)
            {
                var arguments = t.GetGenericArguments();
                constraints = GetGenericTypeNames(arguments);
            }

            // デリゲートに紐づけたメソッドから情報を得る
            if (!(invokeMethod is null))
            {
                methodArguments = GetMethodArguments(invokeMethod);
                returnType = GetVariableTypeName(invokeMethod.ReturnType);
            }

            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                if (returnType == "Void")
                    returnType = "void";

                // void Aaa<T, M>(int i, string s)
                sb.Append($"{returnType} {methodName}");
                if (!(constraints is null))
                {
                    sb.Append($"{constraints[0]}");
                    sb.Append($"{methodArguments}");

                    if (!string.IsNullOrEmpty(constraints[1]))
                        sb.Append($"{constraints[1]}");
                }
                else
                {
                    sb.Append($"{methodArguments}");
                }

                sb.Append("\n{");
                sb.Append("\n    ");
                sb.Append("\n}");
            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                // Sub Aaa(Of T, M)(i As Integer, s As String)
                // Function Aaa(Of T, M)(i As Integer, s As String) As Integer
                if (returnType == "Void")
                    sb.Append($"Sub ");
                else
                    sb.Append($"Function ");

                sb.Append($"{methodName}");

                if (!(constraints is null))
                    sb.Append($"{constraints[0]}");

                sb.Append($"{methodArguments}");

                if (returnType != "Void")
                    sb.Append($" As {returnType}");

                sb.Append("\n    ");
                if (returnType == "Void")
                    sb.Append($"\nEnd Sub");
                else
                    sb.Append($"\nEnd Function");

            }

            return sb.ToString();
        }

        // C# の場合、型名と制約が離れている。VBNET の場合、ひとまとまりになっている
        // C# の仕様に合わせて、戻り値１行目が型名リスト、２行目が制約リスト（C# 専用）とする
        private List<string> GetGenericTypeNames(Type[] arguments)
        {
            var firstBuilder = new StringBuilder();
            var secondBuilder = new StringBuilder();

            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                foreach (var argument in arguments)
                {
                    if (firstBuilder.Length != 0)
                        firstBuilder.Append(", ");

                    // 制約リスト
                    var items = new List<string>();
                    var attributes = argument.GenericParameterAttributes;
                    var check = attributes & GenericParameterAttributes.SpecialConstraintMask;

                    // 種類を指定している場合
                    if ((check & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint)
                        items.Add("class");

                    if ((check & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint)
                        items.Add("NotNullableValueType");

                    if ((check & GenericParameterAttributes.DefaultConstructorConstraint) == GenericParameterAttributes.DefaultConstructorConstraint)
                        items.Add("new()");

                    // 何らかのクラス、またはインターフェースを指定している場合
                    if (argument.GetGenericParameterConstraints().Any())
                    {
                        foreach (var constraintsType in argument.GetGenericParameterConstraints())
                            items.Add(constraintsType.Name);
                    }

                    firstBuilder.Append(argument.Name);
                    if (items.Any())
                    {
                        var constraints = string.Join(", ", items.ToArray());
                        secondBuilder.Append($"\n    where {argument.Name} : {constraints}");
                    }
                }

                firstBuilder.Insert(0, "<");
                firstBuilder.Append(">");
            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                foreach (var argument in arguments)
                {
                    if (firstBuilder.Length != 0)
                        firstBuilder.Append(", ");

                    // 制約リスト
                    var items = new List<string>();
                    var attributes = argument.GenericParameterAttributes;
                    var check = attributes & GenericParameterAttributes.SpecialConstraintMask;

                    // 種類を指定している場合
                    if ((check & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint)
                        items.Add("Class");

                    if ((check & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint)
                        items.Add("NotNullableValueType");

                    if ((check & GenericParameterAttributes.DefaultConstructorConstraint) == GenericParameterAttributes.DefaultConstructorConstraint)
                        items.Add("New");

                    // 何らかのクラス、またはインターフェースを指定している場合
                    if (argument.GetGenericParameterConstraints().Any())
                    {
                        foreach (var constraintsType in argument.GetGenericParameterConstraints())
                            items.Add(constraintsType.Name);
                    }

                    firstBuilder.Append(argument.Name);
                    if (items.Any())
                    {
                        var constraints = string.Join(", ", items.ToArray());
                        firstBuilder.Append($" As {"{"}{constraints}{"}"}");
                    }
                }

                firstBuilder.Insert(0, "(Of ");
                firstBuilder.Append(")");
            }

            return new List<string>() { firstBuilder.ToString(), secondBuilder.ToString() };
        }

        private string GetMethodArguments(MethodBase info)
        {
            return GetMethodArguments(info.GetParameters());
        }

        private string GetMethodArguments(ParameterInfo[] arguments)
        {
            var sb = new StringBuilder();

            foreach (var argument in arguments)
            {
                if (sb.Length != 0)
                    sb.Append(", ");

                var isIn = argument.IsIn;
                var isOut = argument.IsOut;
                var isRef = argument.ParameterType.ToString().EndsWith("&");
                var isOptional = argument.IsOptional;
                var defaultValue = argument.DefaultValue;
                var isParamArray = argument.GetCustomAttributes(true).Any(x => x.GetType().Equals(typeof(ParamArrayAttribute)));
                var parameterName = argument.Name;
                var parameterType = GetVariableTypeName(argument.ParameterType);

                if (_LanguageTypes == LanguageTypes.CSharp)
                {
                    if (isIn) sb.Append("in ");
                    if (isOut) sb.Append("out ");
                    if (isRef) sb.Append("ref ");
                    if (isParamArray) sb.Append("params ");

                    sb.Append($"{parameterType} {parameterName}");

                    if (isOptional) sb.Append($" = {defaultValue}");
                }
                else if (_LanguageTypes == LanguageTypes.VBNET)
                {
                    if (isRef)
                    {
                        parameterType = parameterType.Substring(0, parameterType.LastIndexOf("&"));
                        if (isOptional)
                            sb.Append($"ByRef Optional {parameterName} As {parameterType} = {defaultValue}");
                        else
                            sb.Append($"ByRef {parameterName} As {parameterType}");
                    }
                    else
                    {
                        if (isOptional)
                            sb.Append($"ByVal Optional {parameterName} As {parameterType} = {defaultValue}");
                        else if (isParamArray)
                            sb.Append($"ByVal ParamArray {parameterName} As {parameterType}");
                        else
                            sb.Append($"ByVal {parameterName} As {parameterType}");
                    }
                }
            }

            return $"({sb.ToString()})";
        }


        #endregion


        #region DataSet 作成関連


        private StackPanel CreateDataSetData(object instance, int recursiveCallCount)
        {
            var ds = instance as DataSet;

            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            // DataTable が無い
            var title = GetVariableTypeName(ds, ds.Tables.Count);
            if (ds.Tables.Count == 0)
            {
                CreateZeroItemsCollectionData(grid1, title);
            }
            else
            {
                // DataTableがある、データが無い
                // DataTableがある、データがある
                CreateDataSetData(grid1, ds, title, recursiveCallCount);
            }

            return stackPanel1;
        }

        private void CreateDataSetData(Grid grid1, DataSet ds, string title, int recursiveCallCount)
        {
            // 型名
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            var rowIndex = 0;
            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);

            titlePanel.Children.Add(new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold });

            // コレクションデータ
            for (var i = 0; i < ds.Tables.Count; i++)
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

                var table = ds.Tables[i];
                var memberElement = CreateViewData(table, recursiveCallCount + 1);
                memberPanel.Children.Add(memberElement);
            }
        }

        private StackPanel CreateDataViewData(object instance, int recursiveCallCount)
        {
            var title = "DataView";
            var view = instance as DataView;
            var table = view.ToTable();

            return CreateDataTableData(table, recursiveCallCount, title);
        }

        private StackPanel CreateDataTableData(object instance, int recursiveCallCount, string title = "DataTable")
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            var table = instance as DataTable;

            if (table.Rows.Count == 1)
                title = $"{title} (1 item)";
            else
                title = $"{title} ({table.Rows.Count} items)";

            if (table.Columns.Count == 0)
                CreateZeroItemsCollectionData(grid1, title);
            else
                CreateMultiColumnsCollectionData(grid1, table, title, recursiveCallCount, true);

            return stackPanel1;
        }

        private StackPanel CreateDataRowViewData(object instance, int recursiveCallCount)
        {
            var title = "DataRowView";
            var view = instance as DataRowView;
            var row = view.Row;

            return CreateDataRowData(row, recursiveCallCount, title);
        }

        private StackPanel CreateDataRowData(object instance, int recursiveCallCount, string title = "DataRow")
        {
            var row = instance as DataRow;
            var columns = row.Table.Columns;
            var items = new List<Tuple<string, Type, object>>();

            foreach (DataColumn column in columns)
            {
                var columnName = column.ColumnName;
                items.Add(Tuple.Create(columnName, column.DataType, row[columnName]));
            }

            return CreateMemberData(title, items, recursiveCallCount);
        }


        #endregion


        #region Entity Framework 作成関連


        // NuGet して参照追加して扱うのが楽だが、Entity Framework の dll を2次配布したくないので、リフレクションで頑張る
        private StackPanel CreateDbContextData(object instance, Type t, int recursiveCallCount)
        {
            // DbContext クラスに登録した各DBテーブルメンバー（DbSet<TableClass>）を取得（＝その他管理系のメンバーは除外）
            // DbSet<> は IEnumerable を継承している
            var members = t.GetProperties()
                .Where(x => typeof(IEnumerable).IsAssignableFrom(x.PropertyType))
                .Select(x => x.GetValue(instance, null) as IEnumerable);

            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            // 該当なし(null) 
            var title = GetVariableTypeName(instance, members.Count());
            if (members is null || members.Count() == 0)
            {
                CreateZeroItemsCollectionData(grid1, title);
            }
            else
            {
                CreateDbContextData(grid1, members, title, recursiveCallCount);
            }

            return stackPanel1;
        }

        private void CreateDbContextData(Grid grid1, IEnumerable<IEnumerable> items, string title, int recursiveCallCount)
        {
            // 型名
            var rowIndex = 0;
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid1.ColumnDefinitions.Add(new ColumnDefinition());

            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);

            titlePanel.Children.Add(new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold });

            // コレクションデータ
            foreach (var item in items)
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

                var memberElement = CreateViewData(item, recursiveCallCount + 1);
                memberPanel.Children.Add(memberElement);
            }

        }

        // NuGet して参照追加して扱うのが楽だが、Entity Framework の dll を2次配布したくないので、リフレクションで頑張る
        private StackPanel CreateDbSetData(object instance, Type t, int recursiveCallCount)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            var items = instance as IEnumerable;
            var itemsCount = 0;
            foreach (var item in items)
                itemsCount++;

            var title = GetVariableTypeName(instance, itemsCount);
            if (itemsCount == 0)
            {
                CreateZeroItemsCollectionData(grid1, title);
            }
            else
            {
                // 余計な列は削除
                var table = ToDataTable(items);
                if (table.Columns.Contains("_entityWrapper"))
                {
                    var column = table.Columns["_entityWrapper"];
                    table.Columns.Remove(column);
                }

                CreateMultiColumnsCollectionData(grid1, table, title, recursiveCallCount);
            }

            return stackPanel1;
        }


        #endregion


        #region XmlDocument 作成関連


        private DockPanel CreateXmlData(string xmlData)
        {
            var dockPanel1 = new DockPanel();

            xmlData = xmlData.Trim();
            if (!xmlData.StartsWith(@"<?xml version="))
            {
                xmlData = $@"<?xml version=""1.0""?>{xmlData}";

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
            browser1.NavigateToString(xmlData);

            // 画面のスクロールが途中で止まってしまうバグの対応、ウェブブラウザの範囲が分かるように外枠を追加
            var border1 = new Border() { BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1) };
            border1.Child = browser1;
            dockPanel1.Children.Add(border1);
            return dockPanel1;
        }

        private FrameworkElement CreateXmlData(XmlDocument doc, int recursiveCallCount)
        {
            var structData = CreateXmlDocumentData(doc, recursiveCallCount);
            var xmlData = doc.OuterXml;

            if (string.IsNullOrEmpty(xmlData))
            {
                return structData;
            }
            else
            {
                // 左にデータの構造化図、右にブラウザのプレビュー表示
                var grid1 = new Grid();
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid1.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid1.ColumnDefinitions.Add(new ColumnDefinition());

                grid1.Children.Add(structData);
                Grid.SetRow(structData, 0);
                Grid.SetColumn(structData, 0);

                var browserPanel = CreateXmlData(xmlData);
                grid1.Children.Add(browserPanel);
                Grid.SetRow(browserPanel, 0);
                Grid.SetColumn(browserPanel, 1);

                var element = CreateShowSubWindowLinkLabel(doc, xmlData, grid1, recursiveCallCount);
                return element;
            }
        }

        private StackPanel CreateXmlDocumentData(XmlDocument doc, int recursiveCallCount)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            // タイトル用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var rowIndex = 0;
            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);

            var title = GetVariableTypeName(doc);
            var titleBlock = new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold };
            titlePanel.Children.Add(titleBlock);

            if (!doc.HasChildNodes)
                return stackPanel1;

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
                    nodeElement = CreateXmlElementData(element, recursiveCallCount + 1);
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

            return stackPanel1;
        }

        private StackPanel CreateXmlAttributeData(XmlAttribute attr)
        {
            var typeName = "XmlAttribute";
            var attributeName = attr.Name;
            object attributeValue = attr.Value;

            var items = new List<Tuple<string, Type, object>>
            {
                Tuple.Create(attributeName, typeof(string), attributeValue),
            };

            return CreateMemberData(typeName, items, 0);
        }

        private StackPanel CreateXmlDeclarationData(XmlDeclaration dec)
        {
            // 未設定の場合、"null", "（空欄）" を表示させたくないため、無い場合はスペースをセットする
            var typeName = "XmlDeclaration";
            object version = string.IsNullOrEmpty(dec.Version) ? " " : dec.Version;
            object encode = string.IsNullOrEmpty(dec.Encoding) ? " " : dec.Encoding;
            object standalone = string.IsNullOrEmpty(dec.Standalone) ? " " : dec.Standalone;

            var items = new List<Tuple<string, Type, object>>
            {
                Tuple.Create("Version", version.GetType(), version),
                Tuple.Create("Encoding",encode.GetType(), encode),
                Tuple.Create("Standalone",standalone.GetType(), standalone),
            };

            return CreateMemberData(typeName, items, 0);
        }

        private StackPanel CreateXmlCommentData(XmlComment comment)
        {
            var variableName = "XmlComment";
            var variableValue = comment.Value;

            return CreateSingleData(variableName, variableValue);
        }

        private FrameworkElement CreateXmlElementData(XmlElement element, int recursiveCallCount, bool isStartCall = false)
        {
            // Dump したいデータが XmlElement の場合、プレビュー表示する
            if (isStartCall)
            {
                var structData = CreateXmlElementData(element, recursiveCallCount);
                var xmlData = element.OuterXml;

                if (string.IsNullOrEmpty(xmlData))
                {
                    return structData;
                }
                else
                {
                    // 左にデータの構造化図、右にブラウザのプレビュー表示
                    var topGrid = new Grid();
                    topGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    topGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    topGrid.ColumnDefinitions.Add(new ColumnDefinition());

                    topGrid.Children.Add(structData);
                    Grid.SetRow(structData, 0);
                    Grid.SetColumn(structData, 0);

                    var browserPanel = CreateXmlData(xmlData);
                    topGrid.Children.Add(browserPanel);
                    Grid.SetRow(browserPanel, 0);
                    Grid.SetColumn(browserPanel, 1);

                    var linkElement = CreateShowSubWindowLinkLabel(element, xmlData, topGrid, recursiveCallCount);
                    return linkElement;
                }
            }

            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            // タイトル用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var rowIndex = 0;
            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);

            var title = GetVariableTypeName(element);
            var titleBlock = new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold };
            titlePanel.Children.Add(titleBlock);

            if (!element.HasAttributes && !element.HasChildNodes)
                return stackPanel1;

            // タイトル / 型名、子タグ個数、タグ名
            if (element.HasAttributes)
            {
                // 謎仕様？子ノードがある時の属性の取得順番について、逆順に取得してしまうので、逆順の逆順に取得する
                var attrs = default(IEnumerable<XmlAttribute>);
                if (element.HasChildNodes)
                    attrs = element.Attributes.Cast<XmlAttribute>().Reverse();
                else
                    attrs = element.Attributes.Cast<XmlAttribute>();

                foreach (XmlAttribute attr in attrs)
                {
                    grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    rowIndex++;

                    // メンバー数が多すぎるため?、指定メンバーのみに絞る
                    //var nodeElement = CreateMemberData(attr, attr.GetType());
                    var nodeElement = CreateXmlAttributeData(attr);

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
                        nodeElement = CreateViewData(castedElement.Value);
                    }
                    else if (node is XmlElement)
                    {
                        var castedElement = node as XmlElement;
                        nodeElement = CreateXmlElementData(castedElement, recursiveCallCount + 1);
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

            return stackPanel1;
        }


        #endregion


        #region XDocument 作成関連


        private FrameworkElement CreateXmlData(XDocument doc, int recursiveCallCount)
        {
            var structData = CreateXDocumentData(doc, recursiveCallCount);
            var xmlData = doc.ToString();

            if (!(doc.Declaration is null))
                xmlData = $"{doc.Declaration.ToString()}{xmlData}";

            if (string.IsNullOrEmpty(xmlData))
            {
                return structData;
            }
            else
            {
                // 左にデータの構造化図、右にブラウザのプレビュー表示
                var grid1 = new Grid();
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid1.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid1.ColumnDefinitions.Add(new ColumnDefinition());

                grid1.Children.Add(structData);
                Grid.SetRow(structData, 0);
                Grid.SetColumn(structData, 0);

                var browserPanel = CreateXmlData(xmlData);
                grid1.Children.Add(browserPanel);
                Grid.SetRow(browserPanel, 0);
                Grid.SetColumn(browserPanel, 1);

                var element = CreateShowSubWindowLinkLabel(doc, xmlData, grid1, recursiveCallCount);
                return element;
            }
        }

        private StackPanel CreateXDocumentData(XDocument doc, int recursiveCallCount)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            // タイトル用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var rowIndex = 0;
            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);

            var title = GetVariableTypeName(doc);
            var titleBlock = new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold };
            titlePanel.Children.Add(titleBlock);

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
                return stackPanel1;

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
                    nodeElement = CreateXElementData(element, recursiveCallCount + 1);
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

            return stackPanel1;
        }

        private StackPanel CreateXAttributeData(XAttribute attr)
        {
            var typeName = "XAttribute";
            var attributeName = attr.Name.ToString();
            object attributeValue = attr.Value;

            // xmlns プリフェックスの定義を取得できないため、手動置換する
            if (attributeName.Contains("{http://www.w3.org/2000/xmlns/}"))
            {
                attributeName = attributeName.Replace("{http://www.w3.org/2000/xmlns/}", "xmlns:");
            }

            var items = new List<Tuple<string, Type, object>>()
            {
                Tuple.Create(attributeName,typeof(string), attributeValue)
            };

            return CreateMemberData(typeName, items, 0);
        }

        private StackPanel CreateXDeclarationData(XDeclaration dec)
        {
            // 未設定の場合、"null", "（空欄）" を表示させたくないため、無い場合はスペースをセットする
            var typeName = "XDeclaration";
            object version = string.IsNullOrEmpty(dec.Version) ? " " : dec.Version;
            object encode = string.IsNullOrEmpty(dec.Encoding) ? " " : dec.Encoding;
            object standalone = string.IsNullOrEmpty(dec.Standalone) ? " " : dec.Standalone;

            var items = new List<Tuple<string, Type, object>>
            {
                Tuple.Create("Version",version.GetType(), version),
                Tuple.Create("Encoding",encode.GetType(), encode),
                Tuple.Create("Standalone",standalone.GetType(), standalone),
            };

            return CreateMemberData(typeName, items, 0);
        }

        private StackPanel CreateXCommentData(XComment comment)
        {
            var variableName = "XComment";
            var variableValue = comment.Value;

            return CreateSingleData(variableName, variableValue);
        }

        private FrameworkElement CreateXElementData(XElement element, int recursiveCallCount, bool isStartCall = false)
        {
            // Dump したいデータが XElement の場合、プレビュー表示する
            if (isStartCall)
            {
                var structData = CreateXElementData(element, recursiveCallCount);
                var xmlData = element.ToString();

                if (string.IsNullOrEmpty(xmlData))
                {
                    return structData;
                }
                else
                {
                    // 左にデータの構造化図、右にブラウザのプレビュー表示
                    var topGrid = new Grid();
                    topGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    topGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    topGrid.ColumnDefinitions.Add(new ColumnDefinition());

                    topGrid.Children.Add(structData);
                    Grid.SetRow(structData, 0);
                    Grid.SetColumn(structData, 0);

                    var browserPanel = CreateXmlData(xmlData);
                    topGrid.Children.Add(browserPanel);
                    Grid.SetRow(browserPanel, 0);
                    Grid.SetColumn(browserPanel, 1);

                    var linkElement = CreateShowSubWindowLinkLabel(element, xmlData, topGrid, recursiveCallCount);
                    return linkElement;
                }
            }

            // 外枠
            var stackPanel1 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(10) };

            // グリッドの外枠の線と、メモリデータを表現するためのグリッド
            var border1 = new Border() { BorderBrush = Brushes.Black, BorderThickness = new Thickness(1) };
            stackPanel1.Children.Add(border1);

            var grid1 = new Grid();
            border1.Child = grid1;

            // タイトル用
            grid1.ColumnDefinitions.Add(new ColumnDefinition());
            grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // タイトル
            var rowIndex = 0;
            var titlePanel = new DockPanel() { Background = Brushes.LightGray };
            grid1.Children.Add(titlePanel);
            Grid.SetRow(titlePanel, rowIndex);
            Grid.SetColumn(titlePanel, 0);

            var title = GetVariableTypeName(element);
            var titleBlock = new TextBlock() { Text = title, FontWeight = FontWeights.UltraBold };
            titlePanel.Children.Add(titleBlock);

            if (!element.HasAttributes && !element.Nodes().Any())
                return stackPanel1;

            // タイトル / 型名、子タグ個数、タグ名
            if (element.HasAttributes)
            {
                // 謎仕様？子ノードがある時の属性の取得順番について、逆順に取得してしまうので、逆順の逆順に取得する
                var attrs = default(IEnumerable<XAttribute>);
                if (element.Nodes().Any())
                    attrs = element.Attributes().Reverse();
                else
                    attrs = element.Attributes();

                foreach (XAttribute attr in attrs)
                {
                    grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    rowIndex++;

                    // メンバー数が多すぎるため?、指定メンバーのみに絞る
                    //var nodeElement = CreateMemberData(attr, attr.GetType());
                    var nodeElement = CreateXAttributeData(attr);

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
                        nodeElement = CreateViewData(castedElement.Value);
                    }
                    else if (node is XElement)
                    {
                        var castedElement = node as XElement;
                        nodeElement = CreateXElementData(castedElement, recursiveCallCount + 1);
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

            return stackPanel1;
        }


        #endregion


        #region コールツリー作成


        public FrameworkElement CreateCallTree(List<string> items)
        {
            // 外枠
            var stackPanel1 = new StackPanel() { Margin = new Thickness(10) };

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


        #region 継承ツリー作成


        // 継承元クラスと継承元インターフェースを階層的に表示する
        public FrameworkElement CreateBaseTypeTree(Type t)
        {
            var className = GetVariableTypeName(t);
            var items = new List<string>();
            items.Add(className);
            
            var dic = new Dictionary<string, List<string>>();
            var interfaces = t.GetInterfaces();
            if (!(interfaces is null) && 0 < interfaces.Length)
            {
                var values = new List<string>();
                foreach (var interfaceType in interfaces)
                    values.Add(GetVariableTypeName(interfaceType));
                
                dic[className] = values;
            }

            while (!(t.BaseType is null))
            {
                t = t.BaseType;
                className = GetVariableTypeName(t);
                items.Add(className);

                interfaces = t.GetInterfaces();
                if (!(interfaces is null) && 0 < interfaces.Length)
                {
                    var values = new List<string>();
                    foreach (var interfaceType in interfaces)
                        values.Add(GetVariableTypeName(interfaceType));

                    dic[className] = values;
                }
            }

            // 継承元インターフェースは現在のクラスに集約されるみたいなので、
            // 祖先クラスからさかのぼって、かぶっていたら子クラスのインターフェースは除去する用に調整
            var reserves = new List<string>();
            for (var i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                if (!dic.ContainsKey(item))
                    continue;

                var values = dic[item];
                var newValues = new List<string>();
                foreach (var value in values)
                {
                    if (!reserves.Contains(value))
                    {
                        reserves.Add(value);
                        newValues.Add(value);
                    }
                }
                dic[item] = newValues;
            }

            // 外枠
            var stackPanel1 = new StackPanel() { Margin = new Thickness(10) };

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
                var callBlock = new TextBlock() { Text = item, Margin = new Thickness(10, 10, 10, 10) };
                grid1.Children.Add(callBlock);
                Grid.SetRow(callBlock, 0);
                Grid.SetColumn(callBlock, 1);

                // インターフェースがあれば表示する
                if (dic.ContainsKey(item))
                {
                    var values = dic[item];
                    for (var k = 0; k < values.Count; k++)
                    {
                        var columnIndex = k + 2;
                        grid1.ColumnDefinitions.Add(new ColumnDefinition());

                        var rec2 = new Rectangle() { Stroke = Brushes.LightPink, Fill = Brushes.LavenderBlush, RadiusX = 5.0, RadiusY = 5.0, Margin = new Thickness(5, 0, 0, 0) };
                        grid1.Children.Add(rec2);
                        Grid.SetRow(rec2, 0);
                        Grid.SetColumn(rec2, columnIndex);
                        
                        var callBlock2 = new TextBlock() { Text = values[k], Margin = new Thickness(15, 10, 10, 10) };
                        grid1.Children.Add(callBlock2);
                        Grid.SetRow(callBlock2, 0);
                        Grid.SetColumn(callBlock2, columnIndex);

                    }
                }
            }

            return stackPanel1;
        }


        #endregion





        #region 共通ヘルパー関連


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

            var name = GetVariableTypeName(doc, itemsCount);
            name = ToXmlItemsCountUnit(name);

            return name;
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

            var name = GetVariableTypeName(doc, itemsCount);
            name = ToXmlItemsCountUnit(name);

            return name;
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

            var name = GetVariableTypeName(element, itemsCount, element.Name);
            name = ToXmlItemsCountUnit(name);

            return name;
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

            var tagName = GetXElementName(element);
            var name = GetVariableTypeName(element, itemsCount, tagName);
            name = ToXmlItemsCountUnit(name);

            return name;
        }

        private string GetXElementName(XElement element)
        {
            var prefex = string.Empty;
            var name = element.Name.LocalName;

            if (string.IsNullOrEmpty(element.Name.NamespaceName))
                return name;
            else
                prefex = element.Name.NamespaceName;

            var attrs = element.Document.Descendants().Where(x => x.HasAttributes).SelectMany(y => y.Attributes());
            foreach (var attr in attrs)
            {
                if (prefex == attr.Value)
                {
                    prefex = attr.Name.LocalName;
                    break;
                }
            }

            return $"{prefex}:{name}";
        }

        private string GetVariableTypeName(object instanceOrType, int itemsCount = -1, string tagName = "")
        {
            var t = default(Type);
            if (instanceOrType is Type)
                t = instanceOrType as Type;
            else
                t = instanceOrType.GetType();

            var name = GetVariableTypeNameCore(t);
            name = TrimTitle(name);

            if (-1 < itemsCount)
            {
                if (itemsCount == 1)
                {
                    if (string.IsNullOrEmpty(tagName))
                        name = $"{name} (1 item)";
                    else
                        name = $"{name} <{tagName}> (1 item)";
                }
                else
                {
                    if (string.IsNullOrEmpty(tagName))
                        name = $"{name} ({itemsCount} items)";
                    else
                        name = $"{name} <{tagName}> ({itemsCount} items)";
                }
            }

            return name;
        }

        private string GetVariableTypeNameCore(Type t)
        {
            var name = ToTagetLanguageKeyword(t.Name);
            var check = t.FullName is null ? t.Name : t.FullName;

            if (IsAnonymousTypeType(t))
            {
                name = "匿名型";
            }
            else if (Regex.IsMatch(check, @"(System.Linq.Enumerable\+)(<*)(\w+Iterator)(>*)"))
            {
                name = "匿名型コレクション";
            }
            else if (0 < t.GetGenericArguments().Length)
            {
                // ジェネリック系
                var arguments = t.GetGenericArguments().Select(x => GetVariableTypeName(x)).ToArray();
                name = Regex.Replace(name, @"`\d+", string.Empty);

                if (_LanguageTypes == LanguageTypes.CSharp)
                {
                    name = $"{name}<{string.Join(", ", arguments)}>";
                }
                else if (_LanguageTypes == LanguageTypes.VBNET)
                {
                    name = $"{name}(Of {string.Join(", ", arguments)})";
                }
            }

            if (_LanguageTypes == LanguageTypes.VBNET)
            {
                if (name.Contains("["))
                    name = name.Replace("[", "(");

                if (name.Contains("]"))
                    name = name.Replace("]", ")");
            }

            return name;
        }

        private string GetVariableTypeNameCore(string s)
        {
            // clr 型で以下のような文字列が来るので不要情報を除去したり置換したりする
            // "System.System.Action`2[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"
            // "System.Collections.Generic.Dictionary`2[System.Collections.Generic.Dictionary`2[System.String[],System.Int32],System.Collections.Generic.Dictionary`2[System.String,System.Int32[]]]"

            // 名前空間を除去、アセンブリ情報を除去、ジェネリック定義数を除去、各ジェネリック型の残った不要情報を除去 -> Action[[Int32, mscorlib],[Int32, mscorlib]] ジェネリック型毎のかっことか残情報とか
            s = Regex.Replace(s, @"([\w.]+)[.]", "");
            s = Regex.Replace(s, @"(, Version=[\d\.]+)(, Culture=[\w\.]+)(, PublicKeyToken=[\w\.]+)", "");
            s = Regex.Replace(s, @"`\d+", "");
            s = Regex.Replace(s, @"\[([\w\d]+), [\w\d\.]+\]", "$1");

            // 言語別のジェネリック記述方法への置換、配列の置換
            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                s = Regex.Replace(s, @"\[", "<");
                s = Regex.Replace(s, @"\]", ">");
                s = Regex.Replace(s, @"<>", "[]");
            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                s = Regex.Replace(s, @"\[", "(Of ");
                s = Regex.Replace(s, @"\]", ")");
                s = Regex.Replace(s, @"\(Of \)", "()");
            }
            else
            {
                // nop.
            }

            // ジェネリック型のカンマ後のスペース追加
            s = Regex.Replace(s, @",", ", ");
            s = ToTagetLanguageKeyword(s);
            return s;
        }

        private FrameworkElement GetRecursiveLimitData(object memberInstance, int recursiveCallCount)
        {
            // 再帰呼び出し回数の制限処理
            // 4 回目の呼び出しで停止させる（さらに表示させるかどうかは、ユーザー選択させる）
            var memberElement = default(FrameworkElement);

            if (4 <= recursiveCallCount)
            {
                if (memberInstance is null || memberInstance is DBNull || IsPrimitiveType(memberInstance.GetType()))
                {
                    memberElement = CreateViewData(memberInstance, recursiveCallCount + 1);
                }
                else if (memberInstance is Type || memberInstance is IntPtr || memberInstance is UIntPtr || memberInstance.GetType().Name == "RuntimeModule")
                {
                    memberElement = CreateViewData(memberInstance, recursiveCallCount + 1);
                }
                else
                {
                    memberElement = CreateContinueLinkLabel(memberInstance);
                }
            }
            else
            {
                // 通常の場合。さらにデータ値を掘り下げる
                memberElement = CreateViewData(memberInstance, recursiveCallCount + 1);
            }

            return memberElement;
        }

        // VBNET / "List(Of String, item) (1 item)" があり得るか？
        private string ToXmlItemsCountUnit(string name)
        {
            if (name.EndsWith(" item)"))
            {
                name = name.Substring(0, name.LastIndexOf(" item)"));
                name = $"{name} child item)";
            }
            else if (name.EndsWith(" items)"))
            {
                name = name.Substring(0, name.LastIndexOf(" items)"));
                name = $"{name} child items)";
            }

            return name;
        }

        private string ToTagetLanguageKeyword(string value)
        {
            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                value = LanguageConverter.ToCSharp(value);
            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                value = LanguageConverter.ToVBNET(value);
            }
            else
            {
                // nop.
            }

            return value;
        }


        private const string FirstColumnName = "__SingleField__";

        private DataTable ToDataTable(IEnumerable items)
        {
            // 列は、各データのフィールド、プロパティ名、またはそれ以外
            var table = new DataTable("CollectionTable");
            table.Columns.Add(FirstColumnName, typeof(object));

            foreach (var item in items)
            {
                var t = item?.GetType();
                if (item is null)
                {
                    // null をそのままセットすると DBNull として扱われてしまうため、文字列に変換しておいて、判定側で特殊対応する
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = "__nullFromDataTable__";
                    table.Rows.Add(oneRow);
                }
                else if (item is DBNull)
                {
                    // DBNull をデータ有無の判定データとして使っているため、表示できないバグの対応
                    // 文字列に変換しておいて、判定側で特殊対応する
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = "__DBNullFromDataTable__";
                    table.Rows.Add(oneRow);
                }
                else if (item is Type || item is IntPtr || item is UIntPtr || t.Name == "RuntimeModule")
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsImageType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsPrimitiveType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsCollectionType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsEnumType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsDelegateType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsExceptionType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsDataSetFamilyType(t))
                {
                    if (typeof(DataRowView).IsAssignableFrom(t))
                    {
                        var view = item as DataRowView;
                        var row = view.Row;
                        ToDataTableForDataRow(row, table);
                    }
                    else if (typeof(DataRow).IsAssignableFrom(t))
                    {
                        var row = item as DataRow;
                        ToDataTableForDataRow(row, table);
                    }
                    else
                    {
                        var oneRow = table.NewRow();
                        oneRow[FirstColumnName] = item;
                        table.Rows.Add(oneRow);
                    }
                }
                else if (IsXmlDocumentFamilyType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsXDocumentFamilyType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else if (IsEntityFrameworkFamilyType(t))
                {
                    var oneRow = table.NewRow();
                    oneRow[FirstColumnName] = item;
                    table.Rows.Add(oneRow);
                }
                else
                {
                    // クラス、構造体、匿名型
                    ToDataTableForMember(item, t, table);
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

        private void ToDataTableForDataRow(DataRow row, DataTable table)
        {
            var columns = row.Table.Columns;

            // メンバー数分、列作成
            for (var i = 0; i < columns.Count; i++)
            {
                var memberName = columns[i].ColumnName;
                var memberType = columns[i].DataType;

                if (!table.Columns.Contains(memberName))
                {
                    var column = new DataColumn();
                    column.ColumnName = memberName;
                    column.DataType = typeof(object);
                    column.Caption = $"__MemberType__{GetVariableTypeName(memberType)}";

                    table.Columns.Add(column);
                }
            }

            // １行分（１つ分のデータ）を登録（メンバー名に該当する場合はその値、それ以外は DBNull）
            var newRow = table.NewRow();
            for (var i = 0; i < columns.Count; i++)
            {
                var columnName = columns[i].ColumnName;
                newRow[columnName] = row[columnName];
            }
            table.Rows.Add(newRow);
        }

        private void ToDataTableForMember(object item, Type t, DataTable table)
        {
            // クラス、構造体、匿名型
            // メンバー名がかぶった場合まとめて扱う。型が違っていても無視
            var items = GetFieldAndPropertyMembers(item, t);

            // メンバー数分、列作成
            for (var i = 0; i < items.Count; i++)
            {
                var memberName = items[i].Item1;
                var memberType = items[i].Item2;
                if (!table.Columns.Contains(memberName))
                {
                    var column = new DataColumn();
                    column.ColumnName = memberName;
                    column.DataType = typeof(object);
                    column.Caption = $"__MemberType__{GetVariableTypeName(memberType)}";

                    table.Columns.Add(column);
                }
            }

            // １行分（１つ分のデータ）を登録
            var row = table.NewRow();
            for (var i = 0; i < items.Count; i++)
            {
                var memberName = items[i].Item1;
                var memberInstance = items[i].Item3;

                row[memberName] = memberInstance;
            }
            table.Rows.Add(row);
        }

        // C#でstringに格納されているCSVを処理する
        // http://gozuk16.hatenablog.com/entry/2015/11/20/214106
        //
        private DataTable ToDataTableForCsv(string csvData, bool isFirstColumn = false)
        {
            var table = new DataTable("CsvTable");
            var isFirstLoop = true;

            using (Stream stream = new MemoryStream(Encoding.Default.GetBytes(csvData)))
            {
                var parser = new TextFieldParser(stream, Encoding.GetEncoding("Shift_JIS"));
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(new string[] { ",", "\t" });
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = false;

                while (!parser.EndOfData)
                {
                    try
                    {
                        var items = parser.ReadFields().Select(x =>
                        {
                            if (x.Contains("\r\n"))
                                return x.Replace("\r\n", "（改行）");
                            else
                                return x;
                        });

                        // 列名
                        // 1行目を列名として扱うか否か
                        if (isFirstLoop)
                        {
                            if (isFirstColumn)
                            {
                                foreach (var item in items)
                                    table.Columns.Add(item, typeof(object));

                                isFirstLoop = false;
                                continue;
                            }
                            else
                            {
                                for (var i = 0; i < items.Count(); i++)
                                    table.Columns.Add($"Column{i}", typeof(object));
                            }

                            isFirstLoop = false;
                        }

                        // 未登録の列が追加であった場合に備えてチェック
                        // 1: aaa,bbb
                        // 2: aaa,bbb,ccc
                        if (table.Columns.Count < items.Count())
                        {
                            for (var i = table.Columns.Count; i < items.Count(); i++)
                                table.Columns.Add($"Column{i}", typeof(object));
                        }

                        // データ
                        var row = table.NewRow();
                        for (var i = 0; i < items.Count(); i++)
                            row[i] = items.ElementAt(i);

                        table.Rows.Add(row);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

            }

            // DBNull、空文字はスペースに置換する（"DBNull"、"（空欄）" という文字列が表示されてしまうため）
            // 文字列中に、スペースやタブ文字が含まれている場合、ダブルコーテーションで囲む
            foreach (DataRow row in table.Rows)
            {
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    if (row[i] is DBNull)
                    {
                        row[i] = " ";
                    }
                    else
                    {
                        var s = $"{row[i]}";
                        if (string.IsNullOrEmpty(s))
                            row[i] = " ";
                        else if (Regex.IsMatch(s, @"([\s\t]+)(\w)") || Regex.IsMatch(s, @"(\w)([\s\t]+)"))
                            row[i] = $@"""{s}""";
                    }
                }
            }

            return table;
        }

        private DataSet ToDataSetForDbContext(object instance, Type t)
        {
            // DbContext クラスに登録した各DBテーブルメンバー（DbSet<TableClass>）を取得（＝その他管理系のメンバーは除外）
            // DbSet<> は IEnumerable を継承している
            var members = t.GetProperties()
                .Where(x => typeof(IEnumerable).IsAssignableFrom(x.PropertyType))
                .Select(x => x.GetValue(instance, null) as IEnumerable);

            var result = new DataSet();
            foreach (var member in members)
            {
                var memberType = member.GetType();
                if (IsDbSetType(memberType))
                {
                    var ds = ToDataSetForDbSet(member);
                    if (0 < ds.Tables.Count)
                    {
                        var tableNames = result.Tables.Cast<DataTable>().Select(x => x.TableName).ToList();
                        foreach (DataTable table in ds.Tables)
                        {
                            // テーブル名が重複している場合、登録できないので名称変更する
                            var tableName = table.TableName;
                            if (tableNames.Contains(tableName))
                            {
                                if (Regex.IsMatch(tableName, @"_(\d+)$"))
                                {
                                    var value = tableName.Substring(tableName.LastIndexOf("_") + 1);
                                    tableName = tableName.Substring(0, tableName.LastIndexOf("_"));

                                    var number = int.Parse(value) + 1;
                                    tableName = $"{tableName}_{number}";
                                }
                                else
                                {
                                    tableName = $"{tableName}_1";
                                }

                                table.TableName = tableName;
                            }

                            result.Tables.Add(table.Copy());
                        }
                    }
                }

            }

            return result;
        }

        private DataSet ToDataSetForDbSet(object instance)
        {
            var items = instance as IEnumerable;
            var itemsCount = 0;
            foreach (var item in items)
                itemsCount++;

            var ds = new DataSet();
            if (0 < itemsCount)
            {
                // 余計な列は削除
                var table = ToDataTable(items);
                if (table.Columns.Contains("_entityWrapper"))
                {
                    var column = table.Columns["_entityWrapper"];
                    table.Columns.Remove(column);
                }

                ds.Tables.Add(table);
            }

            return ds;
        }

        private DataSet ToDataSet(DataRowView row)
        {
            return ToDataSet(row.Row);
        }

        private DataSet ToDataSet(DataRow row)
        {
            var table = new DataTable(row.Table.TableName);
            var columns = row.Table.Columns;

            foreach (DataColumn column in columns)
                table.Columns.Add(column.ColumnName, column.DataType);

            table.Rows.Add(row.ItemArray);
            return ToDataSet(table);
        }

        private DataSet ToDataSet(DataView view)
        {
            return ToDataSet(view.ToTable());
        }

        private DataSet ToDataSet(DataTable table)
        {
            // すでに DataSet に属している場合、table.DataSet を返却してしまうと別のテーブルも見えてしまうため、コピーして参照を切る
            var ds = new DataSet();
            ds.Tables.Add(table.Copy());

            return ds;
        }

        private List<Tuple<string, Type, object>> GetFieldAndPropertyMembers(object instance, Type t)
        {
            var fieldTypes = t.GetFields();
            var propertyTypes = t.GetProperties();
            var items = new List<Tuple<string, Type, object>>();

            // フィールドとプロパティメンバーをまとめて扱う。同名でかぶってしまうことは無いはず？
            foreach (var info in fieldTypes)
            {
                var memberName = info.Name;
                var memberType = info.FieldType;
                var memberInstance = info.GetValue(instance);
                items.Add(Tuple.Create(memberName, memberType, memberInstance));
            }

            foreach (var info in propertyTypes)
            {
                var memberName = info.Name;
                var memberType = info.PropertyType;
                var memberInstance = default(object);

                // Getter が無い場合に備える
                try
                {
                    memberInstance = info.GetValue(instance, null);
                }
                catch (Exception ex)
                {
                    memberInstance = ex;
                }

                items.Add(Tuple.Create(memberName, memberType, memberInstance));
            }

            return items;
        }

        private string TrimTitle(string value, int overLength = 80)
        {
            if (overLength <= value.Length)
            {
                value = $"{value.Substring(0, overLength)}...";
            }

            return value;
        }

        // C#における「ビットマップ形式の画像データを相互変換」まとめ
        // https://qiita.com/YSRKEN/items/a24bf2173f0129a5825c
        // (WinForms) System.Drawing.Bitmap を、(WPF) System.Windows.Media.Imaging.BitmapSource に変換する
        private BitmapSource ToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            var result = default(BitmapSource);
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                result = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }

            return result;
        }


        #endregion


    }
}
