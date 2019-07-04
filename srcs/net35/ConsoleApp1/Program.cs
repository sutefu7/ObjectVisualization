using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ObjectVisualization;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectWatcher.Instance.Show();
            //ObjectWatcher.Instance.Show(LanguageTypes.VBNET);

            //Test1();
            //Test2();
            //Test3();
            //Test4();
            //Test5();
            //Test6();
            //Test7();
            //Test8();
            //Test9();
            //Test10();
            //Test11();
            //Test12();
            //Test13();
            Test14();
            Test15();




            Console.ReadKey();
            ObjectWatcher.Instance.Close();
        }

        delegate void AaaDelegate<T1, T2>(T1 t1, T2 t2)
            where T1 : class
            where T2 : class, new();

        static void Aaa<T1, T2>(T1 t1, T2 t2)
            where T1 : class
            where T2 : class, new()
        {

        }

        static void Test15()
        {
            var s = "hello";
            s.DumpBaseTypeTree();

            //var t = s.GetType();
            //t.DumpBaseTypeTree();

            //ObjectWatcher.Instance.DumpBaseTypeTree(s);
            //ObjectWatcher.Instance.DumpBaseTypeTree(t);

            var items1 = new CustomList();
            items1.DumpBaseTypeTree();
        }

        static void Test14()
        {
            ObjectWatcher.Instance.DumpCallTree();
        }

        static void Test13()
        {
            var s1 = $@"aaa1,aaa2
bbb,bbb,bbb
ccc,ccc,ccc,ccc
";
            ObjectWatcher.Instance.Dump(s1);

            var s2 = $@"name,age
taro,25
jiro,22
hanako,18
";
            ObjectWatcher.Instance.Dump(s2);


            //using (var client = new HttpClient())
            //{
            //    var bytes = client.GetByteArrayAsync(@"https://raw.githubusercontent.com/sutefu7/WpfEventViewer/master/Docs/image01.png").Result;
            //    using (var stream = new MemoryStream(bytes))
            //    {
            //        var bitmap = new System.Drawing.Bitmap(stream).Dump();
            //    }
            //}




            //var bitmap = default(System.Drawing.Bitmap);
            //var bytes = new System.Net.WebClient().DownloadData("https://www.linqpad.net/images/LINQPad.png");
            //using (var stream = new MemoryStream(bytes))
            //{
            //    bitmap = new System.Drawing.Bitmap(stream);
            //}

            //var eatThis = new
            //{
            //    Image = bitmap,
            //    XML = XElement.Parse("<root type='foo'><inner>test</inner></root>"),
            //    System.Globalization.CultureInfo.CurrentCulture.Calendar,
            //};
            //eatThis.Dump();


            var items1 = new List<string>() { "aaa", "bbb" };
            var items2 = new List<object>() { items1 };
            var items3 = new List<object>() { items2 };
            var items4 = new List<object>() { items3 };
            var items5 = new List<object>() { items4 };
            var items6 = new List<object>() { items5 };
            var items7 = new List<object>() { items6 };
            var items8 = new List<object>() { items7 };
            var items9 = new List<object>() { items8 };
            var items10 = new List<object>() { items9 };
            var items11 = new List<object>() { items10 };
            items11.Dump();


            Action ac1 = () => Console.WriteLine("");
            Action<int, int> ac2 = (x, y) => Console.WriteLine("");
            Func<object> fc1 = () => 1;
            Func<int, int, object> fc2 = (x, y) => 1;

            ac1.Dump();
            ac2.Dump();
            fc1.Dump();
            fc2.Dump();

            Expression<Action> e1 = () => Console.WriteLine("");
            Expression<Action<int, int>> e2 = (x, y) => Console.WriteLine("");
            Expression<Func<object>> e3 = () => 1;
            Expression<Func<int, int, object>> e4 = (x, y) => 1;

            // e5, e6 は同じ式
            Expression<Func<int, int>> e5 = x => x + 5;

            var x6 = System.Linq.Expressions.Expression.Parameter(typeof(int), "x");
            var e6 =
              System.Linq.Expressions.Expression.Lambda<Func<int, int>>(
                System.Linq.Expressions.Expression.Add(x6, System.Linq.Expressions.Expression.Constant(5)),
                x6);

            e1.Dump();
            e2.Dump();
            e3.Dump();
            e4.Dump();
            e5.Dump();
            e6.Dump();


        }

        static void Test12()
        {
            ObjectWatcher.Instance.Dump(@"aaa");
            ObjectWatcher.Instance.Dump($@"aaa{"\t"}");

            var s1 = $@"aaa{"\t"}
bbb{"\t"}";
            ObjectWatcher.Instance.Dump(s1);

            var s2 = $@"aaa{"\t"}
bbb{"\t"}
";
            ObjectWatcher.Instance.Dump(s2);

            var s3 = $@"aaa{"\t"}aaa
bbb{"\t"}bbb
";
            ObjectWatcher.Instance.Dump(s3);




            ObjectWatcher.Instance.Dump($@"""aaa""");
            ObjectWatcher.Instance.Dump($@"""aaa""{"\t"}");

            var s4 = $@"""aaa""{"\t"}
""bbb""{"\t"}";
            ObjectWatcher.Instance.Dump(s4);

            var s5 = $@"""aaa""{"\t"}
""bbb""{"\t"}
";
            ObjectWatcher.Instance.Dump(s5);

            var s6 = $@"""aaa""{"\t"}aaa
""bbb""{"\t"}bbb
";
            ObjectWatcher.Instance.Dump(s6);

            var s7 = $@"""aaa""{"\t"}""aaa""
""bbb""{"\t"}""bbb""
";
            ObjectWatcher.Instance.Dump(s7);


            


            var s8 = $@"あああ   {"\t"} いいい     {"\t"} ううう
""あああ"" {"\t"} ""いいい""   {"\t"} "" ううう ""
""あああ"" {"\t"} ""い""""いい"" {"\t"} ""ううう""
""あああ"" {"\t"} ""い{"\t"}いい""  {"\t"} ""ううう""
あああ
あああ   {"\t"} いいい
""あああ"" {"\t"} ""い
いい""{"\t"}""ううう""
";
            ObjectWatcher.Instance.Dump(s8);

            var s9 = $@"aaa{"\t"}aaa
bbb{"\t"}bbb{"\t"}bbb
ccc{"\t"}ccc{"\t"}ccc{"\t"}ccc
";
            ObjectWatcher.Instance.Dump(s9);

            var s10 = $@"aaa,aaa
bbb,bbb{"\t"}bbb
""ccc,ccc"",""ccc{"\t"}ccc""
";
            ObjectWatcher.Instance.Dump(s10);

            var s11 = $@"aaa,aaa
bbb,bbb,""bbb
ccc,ccc"",""ccc,ccc""
";
            ObjectWatcher.Instance.Dump(s11);

        }

        static void Test11()
        {
            ObjectWatcher.Instance.Dump(@"aaa");
            ObjectWatcher.Instance.Dump(@"aaa,");

            var s1 = @"aaa,
bbb,";
            ObjectWatcher.Instance.Dump(s1);

            var s2 = @"aaa,
bbb,
";
            ObjectWatcher.Instance.Dump(s2);

            var s3 = @"aaa,aaa
bbb,bbb
";
            ObjectWatcher.Instance.Dump(s3);




            ObjectWatcher.Instance.Dump(@"""aaa""");
            ObjectWatcher.Instance.Dump(@"""aaa"",");

            var s4 = @"""aaa"",
""bbb"",";
            ObjectWatcher.Instance.Dump(s4);

            var s5 = @"""aaa"",
""bbb"",
";
            ObjectWatcher.Instance.Dump(s5);

            var s6 = @"""aaa"",aaa
""bbb"",bbb
";
            ObjectWatcher.Instance.Dump(s6);

            var s7 = @"""aaa"",""aaa""
""bbb"",""bbb""
";
            ObjectWatcher.Instance.Dump(s7);





            var s8 = @"あああ   , いいい     , ううう
""あああ"" , ""いいい""   , "" ううう ""
""あああ"" , ""い""""いい"" , ""ううう""
""あああ"" , ""い,いい""  , ""ううう""
あああ
あああ   , いいい
""あああ"" , ""い
いい"",""ううう""
";
            ObjectWatcher.Instance.Dump(s8);

            var s9 = @"aaa,aaa
bbb,bbb,bbb
ccc,ccc,ccc,ccc
";
            ObjectWatcher.Instance.Dump(s9);

        }

        static void Test10()
        {
            var s1 = "http://test.co.jp/";
            ObjectWatcher.Instance.Dump(s1);

            var uri1 = new Uri(s1);
            ObjectWatcher.Instance.Dump(uri1);

            var s2 = "https://sample.com/";
            ObjectWatcher.Instance.Dump(s2);

            var uri2 = new Uri(s2);
            ObjectWatcher.Instance.Dump(uri2);
        }

        static void Test9()
        {
            var bmpFile1 = new BitmapImage(new Uri("image1.png", UriKind.Relative));
            bmpFile1.Freeze();
            ObjectWatcher.Instance.Dump(bmpFile1);

            var bmpFile2 = new BitmapImage(new Uri("image2.jpg", UriKind.Relative));
            ObjectWatcher.Instance.Dump(bmpFile2);

            var bmpFile3 = System.Drawing.Bitmap.FromFile("image1.png");
            ObjectWatcher.Instance.Dump(bmpFile3);

            var bmpFile4 = System.Drawing.Bitmap.FromFile("image2.jpg");
            ObjectWatcher.Instance.Dump(bmpFile4);

        }

        static void Test8()
        {
            var x = Tuple.Create("hello", "hello", "hello", "hello", "hello", "hello", "hello");
            ObjectWatcher.Instance.Dump(x);
            //x.Dump();

            var y = Tuple.Create(x, x, x, x, x, x, x);
            ObjectWatcher.Instance.Dump(y);
            //y.Dump();

            var x1 = new { Item1 = "hello", Item2 = "hello", Item3 = "hello", Item4 = "hello", Item5 = "hello", Item6 = "hello", Item7 = "hello" };
            ObjectWatcher.Instance.Dump(x1);
            //x1.Dump();

            var y1 = Tuple.Create(x1, x1, x1, x1, x1, x1, x1);
            ObjectWatcher.Instance.Dump(y1);
            //y1.Dump();


        }

        static void Test7()
        {
            var s1 = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<xbrli:xbrl xmlns:xlink=""http://www.w3.org/1999/xlink"" xmlns:jpcrp_cor=""http://disclosure.edinet-fsa.go.jp/taxonomy/jpcrp/2017-02-28/jpcrp_cor"" xmlns:xbrli=""http://www.xbrl.org/2003/instance"" xmlns:link=""http://www.xbrl.org/2003/linkbase"">
  <link:schemaRef xlink:type=""simple"" xlink:href=""jpaud-aai-cc-001_X99001-000_2017-03-31_01_2017-08-20.xsd""/>
  <xbrli:context id=""FilingDateInstant"">
<xbrli:entity>
<xbrli:identifier scheme=""http://disclosure.edinet-fsa.go.jp"">X99001-000</xbrli:identifier>
</xbrli:entity>
<xbrli:period>
<xbrli:instant>2017-08-20</xbrli:instant>
</xbrli:period>
</xbrli:context>
  <jpcrp_cor:IndependentAuditorsReportConsolidatedTextBlock contextRef=""FilingDateInstant"">
&lt;table style=""width: 486.5pt; min-width: 486.5pt""&gt;
&lt;colgroup&gt;
&lt;col style=""width: 218.0pt; min-width: 218.0pt""/&gt;
&lt;col style=""width: 73.5pt; min-width: 73.5pt""/&gt;
&lt;col style=""width: 55.5pt; min-width: 55.5pt""/&gt;
&lt;col style=""width: 139.5pt; min-width: 139.5pt""/&gt;
&lt;/colgroup&gt;
&lt;tbody&gt;
&lt;tr style=""height: 15.0pt; min-height: 15.0pt""&gt;
&lt;td colspan=""4"" style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; text-align: center; line-height: 14.5pt""&gt;
&lt;span style=""font-size: 10.5pt; font-weight: bold; text-decoration: underline""&gt;独立監査人の監査報告書及び内部統制監査報告書&lt;/span&gt;
&lt;/p&gt;&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr style=""height: 21.75pt; min-height: 21.75pt""&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; text-align: right; line-height: 12.0pt""&gt;
平成29年６月28日&lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr style=""height: 21.75pt; min-height: 21.75pt""&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 36.0pt; margin-left: 0pt; line-height: 12.0pt""&gt;
&lt;span style=""font-size: 10.5pt""&gt; Ａ株式会社&lt;/span&gt;
&lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr style=""height: 21.75pt; min-height: 21.75pt""&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 0pt; margin-left: 10.5pt; line-height: 12.0pt""&gt;
&lt;span style=""font-size: 10.5pt""&gt; 取締役会  御中&lt;/span&gt;
&lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr style=""height: 21.75pt; min-height: 21.75pt""&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td colspan=""3"" style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 90.0pt; margin-left: 0pt; line-height: 12.0pt""&gt;
&lt;span style=""font-size: 10.5pt""&gt; ○○○○&lt;/span&gt; &lt;span style=""font-size: 10.5pt""&gt; 監査法人&lt;/span&gt;
&lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr style=""height: 29.25pt; min-height: 29.25pt""&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 27.0pt; margin-left: 0pt; text-align: right; line-height: 12.0pt""&gt;
指定社員&lt;/p&gt;
&lt;p style=""margin-right: 9.0pt; margin-left: 0pt; text-align: right; line-height: 12.0pt""&gt;
業務執行社員&lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; line-height: 12.0pt""&gt;
公認会計士&lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 36.0pt; margin-left: 9.0pt; line-height: 12.0pt""&gt;
&lt;span style=""font-size: 10.5pt""&gt; ○○&lt;/span&gt; &lt;span style=""font-size: 10.5pt""&gt;  ○○    印&lt;/span&gt;
&lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr style=""height: 29.25pt; min-height: 29.25pt""&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 27.0pt; margin-left: 0pt; text-align: right; line-height: 12.0pt""&gt;
指定社員&lt;/p&gt;
&lt;p style=""margin-right: 9.0pt; margin-left: 0pt; text-align: right; line-height: 12.0pt""&gt;
業務執行社員&lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; line-height: 12.0pt""&gt;
公認会計士&lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 36.0pt; margin-left: 9.0pt; line-height: 12.0pt""&gt;
&lt;span style=""font-size: 10.5pt""&gt; ○○&lt;/span&gt; &lt;span style=""font-size: 10.5pt""&gt;  ○○    印&lt;/span&gt;
&lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr style=""height: 29.25pt; min-height: 29.25pt""&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 27.0pt; margin-left: 0pt; text-align: right; line-height: 12.0pt""&gt;
指定社員&lt;/p&gt;
&lt;p style=""margin-right: 9.0pt; margin-left: 0pt; text-align: right; line-height: 12.0pt""&gt;
業務執行社員&lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin: 0pt; line-height: 12.0pt""&gt;
公認会計士&lt;/p&gt;
&lt;/td&gt;
&lt;td style=""padding: 0pt""&gt;
&lt;p style=""margin-right: 36.0pt; margin-left: 9.0pt; line-height: 12.0pt""&gt;
&lt;span style=""font-size: 10.5pt""&gt; ○○&lt;/span&gt; &lt;span style=""font-size: 10.5pt""&gt;  ○○    印&lt;/span&gt;
&lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;/tbody&gt;
&lt;/table&gt;
&lt;p style=""line-height: 13.5pt""&gt;  &lt;/p&gt;
&lt;table&gt;
&lt;colgroup&gt;
&lt;col style=""width: 487.5pt; min-width: 487.5pt""/&gt;
&lt;/colgroup&gt;
&lt;tbody&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;＜財務諸表監査＞&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  当監査法人は、金融商品取引法第193条の２第１項の規定に基づく監査証明を行うため、「経理の状況」に掲げられて&lt;br/&gt;いるＡ株式会社の平成28年４月１日から平成29年３月31日までの連結会計年度の連結財務諸表、すなわち、連結貸借対照表、連結損益計算書、連結包括利益計算書、連結株主資本等変動計算書、連結キャッシュ・フロー計算書、連結財務諸表作成のための基本となる重要な事項、その他の注記及び連結附属明細表について監査を行った。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;連結財務諸表に対する経営者の責任&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  経営者の責任は、我が国において一般に公正妥当と認められる企業会計の基準に準拠して連結財務諸表を作成し適正に表示することにある。これには、不正又は誤謬による重要な虚偽表示のない連結財務諸表を作成し適正に表示するために経営者が必要と判断した内部統制を整備及び運用することが含まれる。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;監査人の責任&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  当監査法人の責任は、当監査法人が実施した監査に基づいて、独立の立場から連結財務諸表に対する意見を表明することにある。当監査法人は、我が国において一般に公正妥当と認められる監査の基準に準拠して監査を行った。監査の基準は、当監査法人に連結財務諸表に重要な虚偽表示がないかどうかについて合理的な保証を得るために、監査計画を策定し、これに基づき監査を実施することを求めている。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  監査においては、連結財務諸表の金額及び開示について監査証拠を入手するための手続が実施される。監査手続は、当監査法人の判断により、不正又は誤謬による連結財務諸表の重要な虚偽表示のリスクの評価に基づいて選択及び適用される。財務諸表監査の目的は、内部統制の有効性について意見表明するためのものではないが、当監査法人は、リスク評価の実施に際して、状況に応じた適切な監査手続を立案するために、連結財務諸表の作成と適正な表示に関連する内部統制を検討する。また、監査には、経営者が採用した会計方針及びその適用方法並びに経営者によって行われた見積りの評価も含め全体としての連結財務諸表の表示を検討することが含まれる。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  当監査法人は、意見表明の基礎となる十分かつ適切な監査証拠を入手したと判断している。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;監査意見&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  当監査法人は、上記の連結財務諸表が、我が国において一般に公正妥当と認められる企業会計の基準に準拠して、Ａ株式会社及び連結子会社の平成29年３月31日現在の財政状態並びに同日をもって終了する連結会計年度の経営成績及びキャッシュ・フローの状況をすべての重要な点において適正に表示しているものと認める。&lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p style=""line-height: 13.5pt""&gt;  &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;＜内部統制監査＞&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  当監査法人は、金融商品取引法第193条の２第２項の規定に基づく監査証明を行うため、Ａ株式会社の平成29年３月31日現在の内部統制報告書について監査を行った。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;内部統制報告書に対する経営者の責任&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  経営者の責任は、財務報告に係る内部統制を整備及び運用し、我が国において一般に公正妥当と認められる財務報告に係る内部統制の評価の基準に準拠して内部統制報告書を作成し適正に表示することにある。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  なお、財務報告に係る内部統制により財務報告の虚偽の記載を完全には防止又は発見することができない可能性がある。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;監査人の責任&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  当監査法人の責任は、当監査法人が実施した内部統制監査に基づいて、独立の立場から内部統制報告書に対する意見を表明することにある。当監査法人は、我が国において一般に公正妥当と認められる財務報告に係る内部統制の監査の基準に準拠して内部統制監査を行った。財務報告に係る内部統制の監査の基準は、当監査法人に内部統制報告書に重要な虚偽表示がないかどうかについて合理的な保証を得るために、監査計画を策定し、これに基づき内部統制監査を実施することを求めている。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  内部統制監査においては、内部統制報告書における財務報告に係る内部統制の評価結果について監査証拠を入手するための手続が実施される。内部統制監査の監査手続は、当監査法人の判断により、財務報告の信頼性に及ぼす影響の重要性に基づいて選択及び適用される。また、内部統制監査には、財務報告に係る内部統制の評価範囲、評価手続及び評価結果について経営者が行った記載を含め、全体としての内部統制報告書の表示を検討することが含まれる。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  当監査法人は、意見表明の基礎となる十分かつ適切な監査証拠を入手したと判断している。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;監査意見&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  当監査法人は、Ａ株式会社が平成29年３月31日現在の財務報告に係る内部統制は有効であると表示した上記の内部統制報告書が、我が国において一般に公正妥当と認められる財務報告に係る内部統制の評価の基準に準拠して、財務報告に係る内部統制の評価結果について、すべての重要な点において適正に表示しているものと認める。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;利害関係&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt;
  会社と当監査法人又は業務執行社員との間には、公認会計士法の規定により記載すべき利害関係はない。&lt;/p&gt;
&lt;p style=""margin: 0pt; margin-bottom: 0pt""&gt; &lt;/p&gt;
&lt;p style=""margin-right: 9.0pt; margin-left: 0pt; text-align: right""&gt;
以  上&lt;/p&gt;
&lt;p style=""margin-right: 9.0pt; margin-left: 0pt; text-align: right""&gt;
  &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td&gt;
&lt;p style=""line-height: 1.0pt""&gt;  &lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;tr&gt;
&lt;td style=""border: none; border-top: solid black 0.75pt""&gt;
&lt;p style=""margin-right: 0pt; margin-left: 54.0pt; text-indent: -54.0pt""&gt;
  （注）１．上記は、監査報告書の原本に記載された事項を電子化したものであり、その原本は当社が別途保管しております。&lt;/p&gt;
&lt;p style=""margin-right: 0pt; margin-left: 54.0pt; text-indent: -18.0pt""&gt;
２．連結財務諸表の範囲にはＸＢＲＬデータ自体は含まれていません。&lt;/p&gt;
&lt;/td&gt;
&lt;/tr&gt;
&lt;/tbody&gt;
&lt;/table&gt;
</jpcrp_cor:IndependentAuditorsReportConsolidatedTextBlock>
</xbrli:xbrl>
";

            ObjectWatcher.Instance.Dump(s1);

            var x1 = new XmlDocument();
            x1.LoadXml(s1);
            ObjectWatcher.Instance.Dump(x1);

            var x2 = XDocument.Parse(s1);
            ObjectWatcher.Instance.Dump(x2);


        }

        static void Test6b()
        {
            var s1 = @"<?xml version=""1.0"" encoding=""shift_jis"" standalone=""no"" ?>
<!DOCTYPE email SYSTEM ""BBB.dtd"">
<email>値は&value;です。</email>
";

            //var x1 = XDocument.Parse(s1);
            //ObjectWatcher.Instance.Dump(x1);

            //var x2 = new XmlDocument();
            //x2.XmlResolver = null;
            //x2.LoadXml(s1);
            //ObjectWatcher.Instance.Dump(x2);
        }

        static void Test6()
        {
            var s1 = @"<root>12</root>";

            var s2 = @"<root><food>apple</food></root>";

            var s3 = @"<?xml version=""1.0""?><!--コメント１--><root>false</root>";

            var s4 = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?><!--コメント１--><root><!--コメント２--><food>apple</food></root>";

            var s5 = @"<?xml version=""1.0"" encoding=""utf-8""?>  
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema""  
  attributeFormDefault=""unqualified""   
  elementFormDefault=""qualified""  
  targetNamespace=""http://www.contoso.com/books"">   
  <xs:element name=""books"">  
    <xs:complexType>  
      <xs:sequence>  
        <xs:element maxOccurs=""unbounded"" name=""book"">  
          <xs:complexType>  
            <xs:sequence>  
              <xs:element name=""title"" type=""xs:string"" />  
              <xs:element name=""price"" type=""xs:decimal"" />  
            </xs:sequence>  
            <xs:attribute name=""genre"" type=""xs:string"" use=""required"" />  
            <xs:attribute name=""ISBN"" type=""xs:string"" use=""required"" />  
            <xs:attribute name=""publicationdate"" type=""xs:date"" use=""required"" />  
          </xs:complexType>  
        </xs:element>  
      </xs:sequence>  
    </xs:complexType>  
  </xs:element>  
</xs:schema>
";

            var x0 = new XDocument();
            ObjectWatcher.Instance.Dump(x0);

            var x1 = XDocument.Parse(s1);
            ObjectWatcher.Instance.Dump(x1);

            var x2 = XDocument.Parse(s2);
            ObjectWatcher.Instance.Dump(x2);

            var x3 = XDocument.Parse(s3);
            ObjectWatcher.Instance.Dump(x3);

            var x4 = XDocument.Parse(s4);
            ObjectWatcher.Instance.Dump(x4);

            var x5 = XDocument.Parse(s5);
            ObjectWatcher.Instance.Dump(x5);

            var x6 = x4.Document.Elements().ElementAt(0).Nodes().ElementAt(0);
            ObjectWatcher.Instance.Dump(x6);

            var x7 = x4.Document.Elements().ElementAt(0).Nodes().ElementAt(1);
            ObjectWatcher.Instance.Dump(x7);

            var x7b = x4.Document.Elements().ElementAt(0);
            ObjectWatcher.Instance.Dump(x7b);

            var x8 = x4.Declaration;
            ObjectWatcher.Instance.Dump(x8);

            var x9 = x5.Document.Elements().ElementAt(0).Attributes().FirstOrDefault();
            ObjectWatcher.Instance.Dump(x9);
        }

        static void Test5()
        {
            var s1 = @"<root>12</root>";
            ObjectWatcher.Instance.Dump(s1);

            var s2 = @"<root><food>apple</food></root>";
            ObjectWatcher.Instance.Dump(s2);

            var s3 = @"<?xml version=""1.0""?><!--コメント１--><root>false</root>";
            ObjectWatcher.Instance.Dump(s3);

            var s4 = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?><!--コメント１--><root><!--コメント２--><food>apple</food></root>";
            ObjectWatcher.Instance.Dump(s4);

            var s5 = @"<?xml version=""1.0"" encoding=""utf-8""?>  
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema""  
  attributeFormDefault=""unqualified""   
  elementFormDefault=""qualified""  
  targetNamespace=""http://www.contoso.com/books"">   
  <xs:element name=""books"">  
    <xs:complexType>  
      <xs:sequence>  
        <xs:element maxOccurs=""unbounded"" name=""book"">  
          <xs:complexType>  
            <xs:sequence>  
              <xs:element name=""title"" type=""xs:string"" />  
              <xs:element name=""price"" type=""xs:decimal"" />  
            </xs:sequence>  
            <xs:attribute name=""genre"" type=""xs:string"" use=""required"" />  
            <xs:attribute name=""ISBN"" type=""xs:string"" use=""required"" />  
            <xs:attribute name=""publicationdate"" type=""xs:date"" use=""required"" />  
          </xs:complexType>  
        </xs:element>  
      </xs:sequence>  
    </xs:complexType>  
  </xs:element>  
</xs:schema>
";
            ObjectWatcher.Instance.Dump(s5);

            var x0 = new XmlDocument();
            ObjectWatcher.Instance.Dump(x0);

            var x1 = new XmlDocument();
            x1.LoadXml(s1);
            ObjectWatcher.Instance.Dump(x1);

            var x2 = new XmlDocument();
            x2.LoadXml(s2);
            ObjectWatcher.Instance.Dump(x2);

            var x3 = new XmlDocument();
            x3.LoadXml(s3);
            ObjectWatcher.Instance.Dump(x3);

            var x4 = new XmlDocument();
            x4.LoadXml(s4);
            ObjectWatcher.Instance.Dump(x4);

            var x5 = new XmlDocument();
            x5.LoadXml(s5);
            ObjectWatcher.Instance.Dump(x5);

            var x6 = x4.ChildNodes[2].ChildNodes[0];
            ObjectWatcher.Instance.Dump(x6);

            var x7 = x4.ChildNodes[2].ChildNodes[1];
            ObjectWatcher.Instance.Dump(x7);

            var x7b = x4.ChildNodes[2];
            ObjectWatcher.Instance.Dump(x7b);

            var x8 = x4.ChildNodes[0];
            ObjectWatcher.Instance.Dump(x8);

            var x9 = x5.ChildNodes[1].Attributes[0];
            ObjectWatcher.Instance.Dump(x9);
        }

        static void Test4()
        {
            Action ac1 = () => Console.WriteLine("");
            ObjectWatcher.Instance.Dump(ac1);

            Action<string> ac2 = x => Console.WriteLine(x);
            ObjectWatcher.Instance.Dump(ac2);

            Action<int, DateTime> ac3 = (x, y) => Console.WriteLine(x);
            ObjectWatcher.Instance.Dump(ac3);

            AaaDelegate<Shape, Class1> ac4 = Aaa;
            ObjectWatcher.Instance.Dump(ac4);

            Func<object> fc1 = () => 1;
            ObjectWatcher.Instance.Dump(fc1);

            Func<bool, object> fc2 = x => 1;
            ObjectWatcher.Instance.Dump(fc2);

            Func<decimal, double, object> fc3 = (x, y) => 1;
            ObjectWatcher.Instance.Dump(fc3);
            
            var items18 = new List<System.Delegate> { ac1, ac2, ac3, fc1, fc2, fc3 };
            ObjectWatcher.Instance.Dump(items18);

            var item3 = new IndexOutOfRangeException("test");
            ObjectWatcher.Instance.Dump(item3);

            var item4 = Tuple.Create(10, "hello");
            ObjectWatcher.Instance.Dump(item4);

            var item5 = Enumerable.Range(1, 3).Select(x => item4);
            ObjectWatcher.Instance.Dump(item5);

            var item6 = Tuple.Create(1, 2, Tuple.Create("aaa", "bbb"), Tuple.Create(new string[] { "aaa", "bbb" }, new string[] { "aaa", "bbb" }));
            ObjectWatcher.Instance.Dump(item6);

            var item7 = new string[2, 2];
            for (var i = 0; i < 2; i++)
            {
                for (var k = 0; k < 2; k++)
                {
                    item7[i, k] = $"aaa{i}{k}";
                }
            }
            ObjectWatcher.Instance.Dump(item7);
        }

        static void Test3()
        {
            // 型無しDataSet
            var ds = new DataSet("AaaDataSet");
            ObjectWatcher.Instance.Dump(ds);

            var table1 = new DataTable("AaaTable");
            ds.Tables.Add(table1);
            ObjectWatcher.Instance.Dump(ds);

            table1.Columns.Add("Id", typeof(int));
            table1.Columns.Add("Name", typeof(string));
            table1.Columns.Add("IsMale", typeof(bool));
            ObjectWatcher.Instance.Dump(ds);

            table1.Rows.Add(new object[] { 1, "aaa", true });
            table1.Rows.Add(new object[] { 2, DBNull.Value, DBNull.Value });
            table1.Rows.Add(new object[] { 3, "ccc", true });
            ObjectWatcher.Instance.Dump(ds);

            var table2 = new DataTable("BbbTable");
            ds.Tables.Add(table2);
            ObjectWatcher.Instance.Dump(ds);

            table2.Columns.Add("Id", typeof(int));
            table2.Columns.Add("Name", typeof(string));
            table2.Columns.Add("Birthday", typeof(DateTime));
            ObjectWatcher.Instance.Dump(ds);

            table2.Rows.Add(new object[] { 4, "ddd", DateTime.Now });
            table2.Rows.Add(new object[] { 5, DBNull.Value, DBNull.Value });
            table2.Rows.Add(new object[] { 6, "fff", DateTime.Now });
            ObjectWatcher.Instance.Dump(ds);

            ObjectWatcher.Instance.Dump(ds);
            ObjectWatcher.Instance.Dump(table1);
            ObjectWatcher.Instance.Dump(table1.Rows[0]);
            ObjectWatcher.Instance.Dump(table2);
            ObjectWatcher.Instance.Dump(table2.Rows[0]);

            var ds1b = new DataSet("aaa");
            var table1b = ds1b.Tables.Add("Table1");

            for (var i = 0; i < 50; i++)
            {
                table1b.Columns.Add($"Name{i}", typeof(string));
                var row = table1b.NewRow();
                row[i] = $"aaa{i}";
                table1b.Rows.Add(row);
            }

            var table2b = ds1b.Tables.Add("Table2");

            for (var i = 0; i < 50; i++)
            {
                table2b.Columns.Add($"Age{i}", typeof(int));
                var row = table2b.NewRow();
                row[i] = i;
                table2b.Rows.Add(row);
            }

            ObjectWatcher.Instance.Dump(ds1b);
            ObjectWatcher.Instance.Dump(ds1b.Tables[0].Rows[0]);
            ObjectWatcher.Instance.Dump(ds1b.Tables[1].Rows[0]);


            // 型付DataSet
            var ds2 = new WindowsFormsApp1.DataSet1();
            ObjectWatcher.Instance.Dump(ds2);
            ObjectWatcher.Instance.Dump(ds2.PersonTable);
            ObjectWatcher.Instance.Dump(ds2.AnimalTable);

            var persons = ds2.PersonTable;
            persons.Rows.Add(new object[] { 1, "aaa", DateTime.Now });
            persons.Rows.Add(new object[] { 2, DBNull.Value, DBNull.Value });
            persons.Rows.Add(new object[] { 3, "ccc", DateTime.Now });

            var animals = ds2.AnimalTable;
            animals.Rows.Add(new object[] { 4, "ddd" });
            animals.Rows.Add(new object[] { 5, DBNull.Value });
            animals.Rows.Add(new object[] { 6, "eee" });

            ObjectWatcher.Instance.Dump(ds2);
            ObjectWatcher.Instance.Dump(ds2.PersonTable);
            ObjectWatcher.Instance.Dump(ds2.PersonTable[0]);
            ObjectWatcher.Instance.Dump(ds2.AnimalTable);
            ObjectWatcher.Instance.Dump(ds2.AnimalTable[0]);

            var items1 = new CustomList();
            ObjectWatcher.Instance.Dump(items1);

            items1.Add(1);
            items1.Add("hello");
            items1.Add(false);
            ObjectWatcher.Instance.Dump(items1);

            var items2 = new CustomList2();
            ObjectWatcher.Instance.Dump(items2);

            items2.Add(1);
            items2.Add("hello");
            items2.Add(false);
            ObjectWatcher.Instance.Dump(items2);

            var ts = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            ObjectWatcher.Instance.Dump(ts);

            // net35版のdllがnugetには無い？最古バージョンがnet40ターゲットだった
            // Entity Framework(空の Code First モデル)
            // todo, 勝手に LocalDB に登録してしまうみたい？登録されてしまったデータを消したい
            //var ds3 = new Model1();
            //var books = ds3.Books;
            //var authors = ds3.Authors;
            //var items = new List<object>();

            //foreach (var book in books)
            //    items.Add(book);

            //foreach (var item in items)
            //    books.Remove(item as Book);

            //items.Clear();

            //foreach (var author in authors)
            //    items.Add(author);

            //foreach (var item in items)
            //    authors.Remove(item as Author);

            //items.Clear();
            //ds3.SaveChanges();

            //ObjectWatcher.Instance.Dump(ds3);

            //authors.Add(new Author() { Id = 10, Name = "aaa" });
            //authors.Add(new Author() { Id = 20, Name = "bbb" });
            //authors.Add(new Author() { Id = 30, Name = "ccc" });
            //ds3.SaveChanges();

            //books.Add(new Book() { Id = 1, Title = "Aaa", Author = authors.FirstOrDefault() });
            //books.Add(new Book() { Id = 2, Title = "Bbb", Author = authors.FirstOrDefault() });
            //books.Add(new Book() { Id = 3, Title = "Ccc", Author = authors.FirstOrDefault() });
            //books.Add(new Book() { Id = 4, Title = "Ddd", Author = authors.FirstOrDefault() });
            //ds3.SaveChanges();

            //ObjectWatcher.Instance.Dump(ds3);
            //ObjectWatcher.Instance.Dump(ds3.Authors);
            //ObjectWatcher.Instance.Dump(ds3.Books);
        }

        static void Test2()
        {
            ObjectWatcher.Instance.Dump(new DataTable("AaaTable"));

            var items8b = default(DataTable);
            ObjectWatcher.Instance.Dump(items8b);

            var items9 = new DataTable("AaaTable");
            items9.Columns.Add("Id", 1.GetType());
            items9.Columns.Add("Name", "".GetType());
            items9.Columns.Add("IsMale", true.GetType());
            ObjectWatcher.Instance.Dump(items9);

            items9.Rows.Add(new object[] { 1, "aaa", true });
            items9.Rows.Add(new object[] { 2, "bbb", false });
            items9.Rows.Add(new object[] { 3, "ccc", true });
            ObjectWatcher.Instance.Dump(items9);

            var items9b = items9.DefaultView;
            items9b.RowFilter = "1 < Id";
            ObjectWatcher.Instance.Dump(items9b);

            var row1 = items9.Rows[0];
            ObjectWatcher.Instance.Dump(row1);

            var row2 = items9b[0];
            ObjectWatcher.Instance.Dump(row2);

            var items10 = new DataTable("AaaTable");
            items10.Columns.Add("Id", 1.GetType());
            ObjectWatcher.Instance.Dump(items10);

            items10.Rows.Add(new object[] { 1 });
            ObjectWatcher.Instance.Dump(items10);

            var row3 = items10.Rows[0];
            ObjectWatcher.Instance.Dump(row3);

            var items10b = items10.AsDataView();
            ObjectWatcher.Instance.Dump(items10b);
            
            ObjectWatcher.Instance.Dump(items10b[0]);

            var items11 = items9.AsEnumerable();
            ObjectWatcher.Instance.Dump(items9);
            ObjectWatcher.Instance.Dump(items11);

            //// error
            // var items12 = items9b.AsQueryable();
            // ObjectWatcher.Instance.Dump(items12);

            var items13 = new ArrayList() { items9, items9b };
            ObjectWatcher.Instance.Dump(items13);

            var items14 = new ArrayList() { items9, items9 };
            ObjectWatcher.Instance.Dump(items14);

            var items15 = new object[] { items9, items9 };
            ObjectWatcher.Instance.Dump(items15);

            var items16 = new DataTable[] { items9, items9 };
            ObjectWatcher.Instance.Dump(items16);

            var items17 = new List<DataTable>() { items9, items9 };
            ObjectWatcher.Instance.Dump(items17);

            var size = new Size(10, 10);
            ObjectWatcher.Instance.Dump(size);
            ObjectWatcher.Instance.Dump(size);

            var item1 = new { Index = 1, Data = 2 };
            ObjectWatcher.Instance.Dump(item1);

            var item1b = new List<object> { item1, item1, item1 };
            ObjectWatcher.Instance.Dump(item1b);

            var item2 = new { Index = 1 };
            ObjectWatcher.Instance.Dump(item2);

            var item2b = new List<object> { item2, item2, item2 };
            ObjectWatcher.Instance.Dump(item2b);

            var dic = new Dictionary<int, int>();
            dic.Add(1, 1);
            ObjectWatcher.Instance.Dump(dic);
        }

        static void Test1()
        {
            ObjectWatcher.Instance.Dump(10);
            ObjectWatcher.Instance.Dump(1234567890);
            ObjectWatcher.Instance.Dump(int.MinValue);
            ObjectWatcher.Instance.Dump(int.MaxValue);
            ObjectWatcher.Instance.Dump(null);
            ObjectWatcher.Instance.Dump(DBNull.Value);
            ObjectWatcher.Instance.Dump(true);
            ObjectWatcher.Instance.Dump(false);
            ObjectWatcher.Instance.Dump(DateTime.Now);
            ObjectWatcher.Instance.Dump("true");
            ObjectWatcher.Instance.Dump("True");
            ObjectWatcher.Instance.Dump("null");
            ObjectWatcher.Instance.Dump("Null");
            ObjectWatcher.Instance.Dump("Nothing");
            ObjectWatcher.Instance.Dump("DBNull");
            ObjectWatcher.Instance.Dump("string");
            ObjectWatcher.Instance.Dump("String");

            var s0 = "hello, world";
            ObjectWatcher.Instance.Dump(s0);

            object o = s0;
            ObjectWatcher.Instance.Dump(o);

            var s1 = "1234567890";
            ObjectWatcher.Instance.Dump(s1);

            var s2 = new StringBuilder();
            foreach (var i in Enumerable.Range(1, 20))
            {
                s2.AppendLine("hello, world");
            }
            ObjectWatcher.Instance.Dump(s2);
            ObjectWatcher.Instance.Dump(s2.ToString());

            var b = true;
            ObjectWatcher.Instance.Dump(b);

            object n = null;
            ObjectWatcher.Instance.Dump(n);

            object n2 = DBNull.Value;
            ObjectWatcher.Instance.Dump(n2);

            var p = new Point(10, 10);
            ObjectWatcher.Instance.Dump(p);

            var items0 = new List<string>();
            ObjectWatcher.Instance.Dump(items0);

            var items0b = new string[] { };
            ObjectWatcher.Instance.Dump(items0b);

            var items0c = new List<Point>();
            ObjectWatcher.Instance.Dump(items0c);

            var items0d = new List<Point>();
            items0d.Add(null);
            ObjectWatcher.Instance.Dump(items0d);

            var items = new List<string[]>();
            items.Add(new string[] { "aa", "aa" });
            items.Add(new string[] { "bb", "bb" });
            items.Add(null);
            ObjectWatcher.Instance.Dump(items);

            var items2 = new List<Point>();
            items2.Add(new Point(10, 10));
            items2.Add(new Point(20, 20));
            items2.Add(null);
            ObjectWatcher.Instance.Dump(items2);

            var items3 = new Point[] { new Point(10, 10), new Point(20, 20) };
            ObjectWatcher.Instance.Dump(items3);

            var items4 = Enumerable.Range(1, 3);
            ObjectWatcher.Instance.Dump(items4);

            var items5 = Enumerable.Range(1, 3).Select((x, i) => new { Index = i, Data = x * x, Datas = new string[] { "aa", "aa" } });
            ObjectWatcher.Instance.Dump(items5);

            var items5b = Enumerable.Range(1, 3).Select((x, i) => new { Index = i, Data = x * x, Datas = new List<string>() { "aa", "aa" } });
            ObjectWatcher.Instance.Dump(items5b);

            var items6 = new System.Collections.ArrayList();
            items6.Add(new Point(10, 10));
            items6.Add(new Point(20, 20));
            items6.Add(null);
            items6.Add(DBNull.Value);
            items6.Add("aaa");
            items6.Add(12);
            items6.Add(new Shape(30, 30));
            ObjectWatcher.Instance.Dump(items6);

            var items7 = Enumerable.Range(1, 3).Select((x, i) => new { Index = i, Data = x, Datas = items6 });
            ObjectWatcher.Instance.Dump(items7);

            var items8 = new System.Collections.ArrayList();
            items8.Add(new Point(10, 10));
            items8.Add(items6);
            items8.Add(items7);
            items8.Add(null);
            items8.Add(DBNull.Value);
            items8.Add("aaa");
            items8.Add(12);
            items8.Add(new Shape(30, 30));
            ObjectWatcher.Instance.Dump(items8);
        }
    }

    class Class1
    {
        public Class1()
        {

        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class Shape
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Shape(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    struct Size
    {
        public int X;
        public int Y;
        public Size Empty
        {
            get
            {
                return new Size();
            }
        }
        public int X2
        {
            set
            {
                X = value;
            }
        }

        public Size(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    class CustomList : ArrayList
    {
        public new void Add(object value)
        {
            base.Add(value);
        }
    }

    class CustomList2 : IEnumerable
    {
        List<object> _Items = null;

        public void Add(object value)
        {
            if (_Items is null)
                _Items = new List<object>();

            _Items.Add(value);
        }

        public int Count()
        {
            if (_Items is null)
                _Items = new List<object>();

            return _Items.Count;
        }

        public IEnumerator GetEnumerator()
        {
            if (_Items is null)
                _Items = new List<object>();

            for (var i = 0; i < _Items.Count; i++)
                yield return _Items[i];

        }
    }

}
