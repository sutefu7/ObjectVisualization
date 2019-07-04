using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ObjectVisualization
{
    public static class ObjectExtensions
    {
        // 別呼び出し用
        public static T Dump<T>(this T instance,
                                [CallerFilePath] string sourceFilePath = "",
                                [CallerMemberName] string memberName = "",
                                [CallerLineNumber] int sourceLineNumber = 0)
        {
            ObjectWatcher.Instance.Dump(instance, sourceFilePath, memberName, sourceLineNumber);
            return instance;
        }

        public static T DumpBaseTypeTree<T>(this T instance)
        {
            ObjectWatcher.Instance.DumpBaseTypeTree(instance);
            return instance;
        }
    }

    // dobon.net さんのサンプルコード、net1.1 向け
    public class ObjectWatcher
    {
        private static ObjectWatcher _Instance = null;
        public static ObjectWatcher Instance
        {
            get
            {
                if (_Instance is null)
                {
                    _Instance = new ObjectWatcher();

                    // WinForms 画面を呼び出すと、ビジュアルスタイルが適用されないバグの対応
                    System.Windows.Forms.Application.EnableVisualStyles();
                }

                return _Instance;
            }
        }

        private MainWindow _Window = null;
        private ManualResetEvent _StartEvent = null;
        private LanguageTypes _LanguageTypes;

        public void Show(LanguageTypes languageTypes = LanguageTypes.CSharp)
        {
            if (!(_Window is null))
                return;

            _LanguageTypes = languageTypes;
            _StartEvent = new ManualResetEvent(false);

            var t = new Thread(new ThreadStart(Run));
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            _StartEvent.WaitOne();
        }

        private void ShowIfNotShown()
        {
            if (_Window is null)
            {
                if (CalledVBNETSource())
                    Show(LanguageTypes.VBNET);
                else
                    Show();
            }
        }

        private void Run()
        {
            _Window = new MainWindow();
            _Window.Loaded += Window_Loaded;
            _Window.ShowDialog();
        }

        public void Close()
        {
            if (_Window is null)
                return;

            _Window.Dispatcher.Invoke(() =>
            {
                _Window.Close();
                _Window = null;
            });
        }

        public void Dump(object instance,
                         [CallerFilePath] string sourceFilePath = "",
                         [CallerMemberName] string memberName = "",
                         [CallerLineNumber] int sourceLineNumber = 0)
        {
            // Show メソッドを呼ぶ前に呼ばれた場合、強制的に Show メソッドを呼ぶ
            ShowIfNotShown();

            // 別スレッドから読み込もうとすると InvalidOperationException が発生するバグの対応（このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。）
            // UI スレッドにいる間に、Freeze 可能な場合、Freeze するように対応
            if (instance is BitmapSource)
            {
                var source = instance as BitmapSource;
                if (source.CanFreeze && !source.IsFrozen)
                    source.Freeze();
            }


            _Window.Dispatcher.Invoke(() =>
            {
                var helper = new ViewCreator(_LanguageTypes);
                var headerBlock = helper.CreateCallerInfoData(sourceFilePath, memberName, sourceLineNumber);
                var newData = helper.CreateViewData(instance);

                // 次の出力結果との間を開けて、見やすく調整する
                newData.Margin = new Thickness(10, 5, 10, 55);

                // ※MainWindow / Resources 内で中央に寄るように設定している、子コントロールの場合、中央に表示したいため
                // トップコントロールが TextBlock の場合、中央に寄ってしまうので、左寄せに調整
                if (newData is TextBlock block)
                    block.HorizontalAlignment = HorizontalAlignment.Left;
                
                _Window.AddData(headerBlock);
                _Window.AddData(newData);

                // デバッグでステップ実行する際に分かりやすく見せるため、逐次表示させる
                DoEvents();
            });
        }

        public void DumpCallTree()
        {
            // Show メソッドを呼ぶ前に呼ばれた場合、強制的に Show メソッドを呼ぶ
            ShowIfNotShown();

            var t = new StackTrace();
            var count = t.FrameCount;
            var items = new List<string>();

            for (var i = 0; i < count; i++)
            {
                var f = new StackFrame(i, true);
                var sourceFilePath = f.GetFileName();
                if (string.IsNullOrEmpty(sourceFilePath))
                    continue;

                var method = f.GetMethod();
                var classType = method.ReflectedType;
                var className = classType.Name;
                var methodName = method.Name;

                // このライブラリ内のコールツリーは除外
                if (classType.Namespace == "ObjectVisualization")
                    continue;

                var fi = new FileInfo(sourceFilePath);
                var callInfo = $"{className}.{methodName} メソッド\r\n{fi.Directory.Name}/{fi.Name}: {f.GetFileLineNumber()} 行目:";
                items.Insert(0, callInfo);
            }

            _Window.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                var helper = new ViewCreator(_LanguageTypes);
                var newData = helper.CreateCallTree(items);
                newData.Margin = new Thickness(10, 5, 10, 55);

                _Window.AddData(newData);
                DoEvents();
            }));

        }

        public void DumpBaseTypeTree(object instance)
        {
            if (instance is null)
                throw new ArgumentNullException("null が渡されました");

            var t = instance.GetType();
            if (instance is Type)
                t = instance as Type;
            else
                t = instance.GetType();

            DumpBaseTypeTree(t);
        }

        public void DumpBaseTypeTree(Type t)
        {
            // Show メソッドを呼ぶ前に呼ばれた場合、強制的に Show メソッドを呼ぶ
            ShowIfNotShown();

            _Window.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                var helper = new ViewCreator(_LanguageTypes);
                var newData = helper.CreateBaseTypeTree(t);
                newData.Margin = new Thickness(10, 5, 10, 55);

                _Window.AddData(newData);
                DoEvents();
            }));

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _Window.Loaded -= Window_Loaded;
            _StartEvent.Set();
        }

        private void DoEvents()
        {
            var frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(obj =>
            {
                ((DispatcherFrame)obj).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }

        private bool CalledVBNETSource()
        {
            var asms = AppDomain.CurrentDomain.GetAssemblies();
            var classNames = asms
                .SelectMany(x => x.GetTypes())
                .Where(y => y.IsClass)
                .Select(z => string.IsNullOrEmpty(z.FullName) ? z.Name : z.FullName);

            return classNames.Any(x => x.EndsWith(".My.MySettings"));
        }
    }
}
