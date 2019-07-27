using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.XmlDocumentVisualizer),
    typeof(ObjectVisualization.XmlDocumentVisualizerObjectSource),
    Target = typeof(XmlDocument),
    Description = "XmlDocument Visualizer")]
namespace ObjectVisualization
{
    public class XmlDocumentVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            // XmlDocument -> string に変換
            var doc = inObject as XmlDocument;
            var xmlString = doc.OuterXml;

            // シリアライズして渡す
            var writer = new StreamWriter(outStream);
            writer.Write(xmlString);
            writer.Flush();
        }
    }

    public class XmlDocumentVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var xmlString = new StreamReader(objectProvider.GetData()).ReadToEnd();
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);

            var converter = new XmlDocumentFamilyToFrameworkElementConverter();
            var viewPart = converter.Convert(doc);
            var browserPart = converter.CreateWebBrowserPanel(xmlString);

            var dlg = new XmlWindow();
            dlg.VariableElement = viewPart;
            dlg.BrowserElement = browserPart;
            dlg.ShowDialog();
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(XmlDocumentVisualizer), typeof(XmlDocumentVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
