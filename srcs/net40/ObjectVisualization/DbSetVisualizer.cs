using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.DbSetVisualizer),
    typeof(ObjectVisualization.DbSetVisualizerObjectSource),
    TargetTypeName = "System.Data.Entity.DbSet, EntityFramework",
    Description = "DbSet Visualizer")]
namespace ObjectVisualization
{
    public class DbSetVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            // DbSet -> DataSet に変換
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

    public class DbSetVisualizer : DialogDebuggerVisualizer
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
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DbSetVisualizer), typeof(DbSetVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
