using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
        public static T Dump<T>(this T instance)
        {
            var callerInfo = new StackFrame(1, true);
            ObjectWatcher.Instance.Dump(instance, callerInfo);
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

            _Window.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                _Window.Close();
                _Window = null;
            }));
        }

        public void Dump(object instance, StackFrame callerInfo = null)
        {
            if (_Window is null)
                Show();

            // 別スレッドから読み込もうとすると InvalidOperationException が発生するバグの対応（このオブジェクトは別のスレッドに所有されているため、呼び出しスレッドはこのオブジェクトにアクセスできません。）
            // UI スレッドにいる間に、Freeze 可能な場合、Freeze するように対応
            if (instance is BitmapSource)
            {
                var source = instance as BitmapSource;
                if (source.CanFreeze && !source.IsFrozen)
                    source.Freeze();
            }


            // 呼び出し元情報を取得
            if (callerInfo is null)
                callerInfo = new StackFrame(1, true);

            var className = string.Empty;
            var methodName = string.Empty;

            var sourceFilePath = string.Empty;
            var memberName = string.Empty;
            var sourceLineNumber = -1;

#if DEBUG

            sourceFilePath = callerInfo.GetFileName();
            memberName = callerInfo.GetMethod().Name;
            sourceLineNumber = callerInfo.GetFileLineNumber();

#else
            
            var m = callerInfo.GetMethod();
            className = m.DeclaringType.FullName is null ? m.DeclaringType.Name : m.DeclaringType.FullName;
            methodName = m.Name;

#endif
            

            _Window.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                var helper = new ViewCreator(_LanguageTypes);
                var headerBlock = default(FrameworkElement);
                var newData = helper.CreateViewData(instance);

                if (sourceLineNumber == -1)
                    headerBlock = helper.CreateCallerInfoData(className, methodName);
                else
                    headerBlock = helper.CreateCallerInfoData(sourceFilePath, memberName, sourceLineNumber);

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
    }
}
