using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ObjectVisualization
{
    // CLR 型をもとに、C#/VBNET 言語用のキーワードに変換するヘルパークラスです。
    public static class LanguageConverter
    {
        private static DataTable _Table = null;
        private static DataView _View = null;

        static LanguageConverter()
        {
            _Table = new DataTable("ConvertTable");
            _Table.Columns.Add("clr", typeof(string));
            _Table.Columns.Add("cs", typeof(string));
            _Table.Columns.Add("vb", typeof(string));

            _Table.Rows.Add(new object[] { "Void", "void", "Void" });

            _Table.Rows.Add(new object[] { "null", "null", "Nothing" });
            _Table.Rows.Add(new object[] { "True", "true", "True" });
            _Table.Rows.Add(new object[] { "False", "false", "False" });

            _Table.Rows.Add(new object[] { "Boolean", "bool", "Boolean" });
            _Table.Rows.Add(new object[] { "Byte", "byte", "Byte" });
            _Table.Rows.Add(new object[] { "SByte", "sbyte", "SByte" });
            _Table.Rows.Add(new object[] { "Char", "char", "Char" });
            _Table.Rows.Add(new object[] { "Decimal", "decimal", "Decimal" });
            _Table.Rows.Add(new object[] { "Double", "double", "Double" });
            _Table.Rows.Add(new object[] { "Single", "float", "Single" });
            _Table.Rows.Add(new object[] { "Int32", "int", "Integer" });
            _Table.Rows.Add(new object[] { "UInt32", "uint", "UInteger" });
            _Table.Rows.Add(new object[] { "Int64", "long", "Long" });
            _Table.Rows.Add(new object[] { "UInt64", "ulong", "ULong" });
            _Table.Rows.Add(new object[] { "Object", "object", "Object" });
            _Table.Rows.Add(new object[] { "Int16", "short", "Short" });
            _Table.Rows.Add(new object[] { "UInt16", "ushort", "UShort" });
            _Table.Rows.Add(new object[] { "String", "string", "String" });

            _View = _Table.DefaultView;
        }

        public static string ToCSharp(string clrTypeName)
        {
            return ToTargetLanguage("cs", clrTypeName);
        }

        public static string ToVBNET(string clrTypeName)
        {
            return ToTargetLanguage("vb", clrTypeName);
        }

        private static string ToTargetLanguage(string lang, string clrTypeName)
        {
            // 完全一致する場合、置換する
            // キーワードが前後にある場合、独自クラス型なので置換しない
            // ジェネリックの場合、置換する
            
            foreach (DataRowView row in _View)
            {
                var keyword = row["clr"].ToString();
                if (clrTypeName == keyword)
                {
                    clrTypeName = row[lang].ToString();
                }
                else if (Regex.IsMatch(clrTypeName, $@"^{keyword}[a-zA-Z]"))
                {
                    // nop. StringBuilder など
                }
                else if (Regex.IsMatch(clrTypeName, $@"[a-zA-Z]{keyword}$"))
                {
                    // nop. ExString など
                }
                else if (Regex.IsMatch(clrTypeName, $@"^[a-zA-Z]+{keyword}[a-zA-Z]+$"))
                {
                    // nop. ExStringEx など
                }
                else if (Regex.IsMatch(clrTypeName, $@"([^a-zA-Z])({keyword})([^a-zA-Z])"))
                {
                    // ex. Dictionary<Tuple<String, String, StringBuilder, String>, Tuple<String, String, StringBuilder, String>> など
                    clrTypeName = Regex.Replace(clrTypeName, $@"([^a-zA-Z])({keyword})([^a-zA-Z])", $"$1{row[lang]}$3");
                }
            }

            return clrTypeName;
        }
    }
}
