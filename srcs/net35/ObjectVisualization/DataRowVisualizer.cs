using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.DataRowVisualizer),
    typeof(ObjectVisualization.DataRowVisualizerObjectSource),
    Target = typeof(DataRow),
    Description = "DataRow Visualizer")]
namespace ObjectVisualization
{
    public class DataRowVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            // DataRow -> DataSet に変換
            var row = inObject as DataRow;
            var columns = row.Table.Columns;

            var ds = new DataSet();
            var table = ds.Tables.Add();

            foreach (DataColumn column in columns)
            {
                var newColumn = new DataColumn()
                {
                    ColumnName = column.ColumnName,
                    DataType = column.DataType,
                    Caption = column.Caption
                };
                table.Columns.Add(newColumn);
            }

            table.Rows.Add(row.ItemArray);

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

    public class DataRowVisualizer : DialogDebuggerVisualizer
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
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DataRowVisualizer), typeof(DataRowVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
