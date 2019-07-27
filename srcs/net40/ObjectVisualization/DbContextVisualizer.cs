using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.DbContextVisualizer),
    typeof(ObjectVisualization.DbContextVisualizerObjectSource),
    TargetTypeName = "System.Data.Entity.DbContext, EntityFramework",
    Description = "DbContext Visualizer")]
namespace ObjectVisualization
{
    public class DbContextVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            // DbContext -> DataSet に変換
            var t = inObject.GetType();

            // DbContext クラスに登録した各DBテーブルメンバー（DbSet<TableClass>）を取得（＝その他管理系のメンバーは除外）
            // DbSet<> は IEnumerable を継承している
            var members = t.GetProperties()
                .Where(x => typeof(IEnumerable).IsAssignableFrom(x.PropertyType))
                .Select(x => new { Name = x.Name, Items = x.GetValue(inObject, null) as IEnumerable });

            var converter = new IEnumerableToDataSetConverter();
            var ds = new DataSet(t.Name);

            foreach (var member in members)
            {
                var table = converter.ToDataTable(member.Items);
                table.TableName = member.Name;
                ds.Tables.Add(table);
            }
            
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

    public class DbContextVisualizer : DialogDebuggerVisualizer
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
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DbContextVisualizer), typeof(DbContextVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
