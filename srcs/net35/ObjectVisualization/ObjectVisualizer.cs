using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.ObjectVisualizer),
    typeof(ObjectVisualization.ObjectVisualizerObjectSource),
    Target = typeof(System.WeakReference),
    Description = "Object Visualizer")]
namespace ObjectVisualization
{
    public class ObjectVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            var wr = inObject as WeakReference;
            inObject = wr.Target;
            
            // Object -> string に変換
            var converter = new ObjectToStringConverter(ObjectVisualizer.GetLanguageTypes());
            var xmlString = string.Empty;

            if (inObject is Type)
            {
                xmlString = converter.CreateBaseTypeTree(inObject as Type);
            }
            else if (inObject is StackFrame[])
            {
                xmlString = converter.CreateCallTree(inObject as StackFrame[]);
            }
            else
            {
                xmlString = converter.Convert(inObject);
            }

            // シリアライズして渡す
            var writer = new StreamWriter(outStream);
            writer.Write(xmlString);
            writer.Flush();
        }
    }

    public class ObjectVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            // 今回渡ってきたデータを、いったん、テキストファイルに保存する
            var workDirectory = Path.Combine(GetDllDirectory(), "ObjectVisualization_Work");
            var infoDirectory = Path.Combine(workDirectory, "ObjectInfo");
            if (!Directory.Exists(infoDirectory))
                Directory.CreateDirectory(infoDirectory);

            var xmlString = new StreamReader(objectProvider.GetData()).ReadToEnd();
            var historyFile = Path.Combine(infoDirectory, $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff")}.txt");
            File.WriteAllText(historyFile, xmlString, new UTF8Encoding());

            // 全過去履歴を読み込んで表示する
            var dlg = new ObjectWindow();
            var converter = new StringToWPFViewConverter(ObjectVisualizer.GetLanguageTypes());
            var observers = new Dictionary<CheckBox, string>();
            var txtFiles = Directory.GetFiles(infoDirectory, "*.txt");

            var previousXmlString = string.Empty;
            var previousElement = default(FrameworkElement);

            foreach (var txtFile in txtFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(txtFile);
                var datas = fileName.Split('_');
                var createTime = $"{datas[0]}/{datas[1]}/{datas[2]} {datas[3]}:{datas[4]}:{datas[5]}.{datas[6]}";

                var timeBlock = new TextBlock() { Text = createTime, Foreground = Brushes.Gray, HorizontalAlignment = HorizontalAlignment.Left };
                var timeCheck = new CheckBox() { Content = "履歴に残しておき、次回も表示する", VerticalAlignment = VerticalAlignment.Center };
                var timePanel = new StackPanel() { Orientation = Orientation.Horizontal };
                timePanel.Children.Add(timeBlock);
                timePanel.Children.Add(timeCheck);

                xmlString = File.ReadAllText(txtFile, new UTF8Encoding());
                var element = default(FrameworkElement);

                if (xmlString.StartsWith("BaseTypeTree:"))
                {
                    element = converter.CreateBaseTypeTree(xmlString);
                }
                else if (xmlString.StartsWith("CallTree:"))
                {
                    element = converter.CreateCallTree(xmlString);
                }
                else
                {
                    element = converter.Convert(xmlString);
                }

                // ※MainWindow / Resources 内で中央に寄るように設定している、子コントロールの場合、中央に表示したいため
                // トップコントロールが TextBlock の場合、中央に寄ってしまうので、左寄せに調整
                if (element is TextBlock)
                {
                    var block1 = element as TextBlock;
                    block1.HorizontalAlignment = HorizontalAlignment.Left;
                }

                var grid1 = new Grid();
                grid1.ColumnDefinitions.Add(new ColumnDefinition());
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid1.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                grid1.Children.Add(timePanel);
                Grid.SetRow(timePanel, 0);
                Grid.SetColumn(timePanel, 0);

                grid1.Children.Add(element);
                Grid.SetRow(element, 1);
                Grid.SetColumn(element, 0);

                // 次の出力結果との間を開けて、見やすく調整する
                grid1.Margin = new Thickness(10, 5, 10, 55);

                dlg.AddData(grid1);
                observers.Add(timeCheck, txtFile);
                
                // 差異データチェック
                if (!xmlString.StartsWith("BaseTypeTree:") && !xmlString.StartsWith("CallTree:"))
                {
                    if (previousXmlString != xmlString && IsSameTypeLayout(previousXmlString, xmlString))
                        SetColorDifferenceData(previousElement, element);

                    // 次用にバックアップ。element 変数に色付けしてしまったので、色付け前をバックアップさせる
                    var dummy = converter.Convert(xmlString);
                    if (dummy is TextBlock)
                    {
                        var block1 = dummy as TextBlock;
                        block1.HorizontalAlignment = HorizontalAlignment.Left;
                    }
                    
                    previousElement = dummy;
                }
                
                previousXmlString = xmlString;
            }

            dlg.Closed += (s, e) =>
            {
                // 履歴を消していいものは消す
                foreach (var key in observers.Keys)
                {
                    if (!key.IsChecked.HasValue || !key.IsChecked.Value)
                    {
                        var txtFile = observers[key];
                        File.Delete(txtFile);
                    }
                }
            };

            dlg.ShowDialog();
        }
        
        private bool IsSameTypeLayout(string source, string dest)
        {
            // それぞれの Primitive タグの Value 値を置換、比較一致したら return true
            var pattern = @"(<Primitive[\s\w=""]+>)(.*?)(</Primitive>)";
            var replacement = @"$1$3";

            var x = Regex.Replace(source, pattern, replacement);
            var y = Regex.Replace(dest, pattern, replacement);

            return x == y;
        }

        private void SetColorDifferenceData(DependencyObject previous, DependencyObject current)
        {
            var children1 = LogicalTreeHelper.GetChildren(previous).Cast<object>().ToList();
            var children2 = LogicalTreeHelper.GetChildren(current).Cast<object>().ToList();

            for (var i = 0; i < children1.Count; i++)
            {
                if (children1[i] is TextBlock)
                {
                    var block1 = children1[i] as TextBlock;
                    var block2 = children2[i] as TextBlock;

                    if (block1.Text != block2.Text)
                    {
                        var panel1 = block2.Parent as DockPanel;
                        if (!(panel1 is null))
                            panel1.Background = Brushes.MistyRose;
                    }
                }

                if (children1[i] is DependencyObject)
                {
                    var obj1 = children1[i] as DependencyObject;
                    var obj2 = children2[i] as DependencyObject;
                    SetColorDifferenceData(obj1, obj2);
                }
            }
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(ObjectVisualizer), typeof(ObjectVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }

        public static LanguageTypes GetLanguageTypes()
        {
            return CalledVBNETSource() ? LanguageTypes.VBNET : LanguageTypes.CSharp;
        }

        public static bool CalledVBNETSource()
        {
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            var classNames = asms
                .SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass)
                .Select(x => string.IsNullOrEmpty(x.FullName) ? x.Name : x.FullName);

            return classNames.Any(x => x.EndsWith(".My.MySettings"));
        }

        public static string GetDllDirectory()
        {
            var items = AppDomain.CurrentDomain.GetAssemblies();
            var asm = items.FirstOrDefault(x => x.FullName.Contains("ObjectVisualization"));
            if (asm is null)
            {
                var desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                return desktopDirectory;
            }
            else
            {
                var fi = new FileInfo(asm.Location);
                return fi.DirectoryName;
            }
        }
    }
}
