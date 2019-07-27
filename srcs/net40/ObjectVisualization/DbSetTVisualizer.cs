using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.DbSetTVisualizer),
    typeof(ObjectVisualization.DbSetTVisualizerObjectSource),
    TargetTypeName = "System.Data.Entity.DbSet`1, EntityFramework",
    Description = "DbSet<T> Visualizer")]
namespace ObjectVisualization
{
    public class DbSetTVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            try
            {
                // DbSet<T> -> DataSet に変換
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }
    }

    public class DbSetTVisualizer : DialogDebuggerVisualizer
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
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DbSetTVisualizer), typeof(DbSetTVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
