using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.XElementVisualizer),
    typeof(ObjectVisualization.XElementVisualizerObjectSource),
    Target = typeof(XElement),
    Description = "XElement Visualizer")]
namespace ObjectVisualization
{
    public class XElementVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            // XElement -> string に変換
            var element = inObject as XElement;
            var xmlString = element.ToString();

            // シリアライズして渡す
            var writer = new StreamWriter(outStream);
            writer.Write(xmlString);
            writer.Flush();
        }
    }

    public class XElementVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var xmlString = new StreamReader(objectProvider.GetData()).ReadToEnd();
            var doc = XDocument.Parse(xmlString);
            var element = doc.Elements().FirstOrDefault();

            var converter = new XDocumentFamilyToFrameworkElementConverter();
            var viewPart = converter.Convert(element);
            var browserPart = converter.CreateWebBrowserPanel(xmlString);

            var dlg = new XmlWindow();
            dlg.VariableElement = viewPart;
            dlg.BrowserElement = browserPart;
            dlg.ShowDialog();
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(XElementVisualizer), typeof(XElementVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
