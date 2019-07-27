using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.XDocumentVisualizer),
    typeof(ObjectVisualization.XDocumentVisualizerObjectSource),
    Target = typeof(XDocument),
    Description = "XDocument Visualizer")]
namespace ObjectVisualization
{
    public class XDocumentVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            // XDocument -> string に変換
            var doc = inObject as XDocument;
            var xmlString = doc.Declaration is null ? $"{doc}" : $"{doc.Declaration}{doc}";

            // シリアライズして渡す
            var writer = new StreamWriter(outStream);
            writer.Write(xmlString);
            writer.Flush();
        }
    }

    public class XDocumentVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var xmlString = new StreamReader(objectProvider.GetData()).ReadToEnd();
            var doc = XDocument.Parse(xmlString);

            var converter = new XDocumentFamilyToFrameworkElementConverter();
            var viewPart = converter.Convert(doc);
            var browserPart = converter.CreateWebBrowserPanel(xmlString);

            var dlg = new XmlWindow();
            dlg.VariableElement = viewPart;
            dlg.BrowserElement = browserPart;
            dlg.ShowDialog();
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(XDocumentVisualizer), typeof(XDocumentVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
