using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;


// Visual Studio Debugger Visualizer 仮説
// １．インターフェースは対象外（msサイトでは object, Array 以外に対応と記載されていたが）
// ２．そのクラス自体と、クラスの継承先クラスも対象となるみたい

[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(Array), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(ArrayList), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(List<>), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(SortedList), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(SortedList<,>), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(CollectionBase), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(Collection<>), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(ReadOnlyCollectionBase), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(ReadOnlyCollection<>), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(Hashtable), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(HashSet<>), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(Dictionary<,>), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(Stack), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(Stack<>), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(Queue), Description = "DataSet Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(typeof(ObjectVisualization.IEnumerableVisualizer), typeof(ObjectVisualization.IEnumerableVisualizerObjectSource), Target = typeof(Queue<>), Description = "DataSet Visualizer")]
namespace ObjectVisualization
{
    public class IEnumerableVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            // IEnumerable -> DataSet に変換
            var items = inObject as IEnumerable;
            var converter = new IEnumerableToDataSetConverter();
            var ds = converter.Convert(items);

            // シリアライズして渡す
            var xmlString = string.Empty;
            using (var writer = new StringWriter())
            {
                ds.WriteXml(writer, XmlWriteMode.WriteSchema);
                xmlString = writer.ToString();
            }

            var writer2 = new StreamWriter(outStream);
            writer2.Write(xmlString);
            writer2.Flush();
        }
    }

    public class IEnumerableVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var xmlString = new StreamReader(objectProvider.GetData()).ReadToEnd();
            var ds = new DataSet();
            using (var reader = new StringReader(xmlString))
            {
                ds.ReadXml(reader, XmlReadMode.ReadSchema);
            }

            using (var dlg = new DataSetVisualizerForm())
            {
                dlg.Target = ds;
                windowService.ShowDialog(dlg);
            }
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(IEnumerableVisualizer), typeof(IEnumerableVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
