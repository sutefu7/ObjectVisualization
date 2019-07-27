using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;

[assembly: System.Diagnostics.DebuggerVisualizer(
    typeof(ObjectVisualization.XmlElementVisualizer),
    typeof(ObjectVisualization.XmlElementVisualizerObjectSource),
    Target = typeof(XmlElement),
    Description = "XmlElement Visualizer")]
namespace ObjectVisualization
{
    public class XmlElementVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object inObject, Stream outStream)
        {
            // XmlElement -> string に変換
            var element = inObject as XmlElement;
            var xmlString = element.OuterXml;

            // シリアライズして渡す
            var writer = new StreamWriter(outStream);
            writer.Write(xmlString);
            writer.Flush();
        }
    }

    public class XmlElementVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var xmlString = new StreamReader(objectProvider.GetData()).ReadToEnd();
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);

            var element = doc.DocumentElement;
            var converter = new XmlDocumentFamilyToFrameworkElementConverter();
            var viewPart = converter.Convert(element);
            var browserPart = converter.CreateWebBrowserPanel(xmlString);

            var dlg = new XmlWindow();
            dlg.VariableElement = viewPart;
            dlg.BrowserElement = browserPart;
            dlg.ShowDialog();
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(XmlElementVisualizer), typeof(XmlElementVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
