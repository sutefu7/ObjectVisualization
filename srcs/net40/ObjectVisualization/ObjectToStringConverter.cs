using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualBasic.FileIO;

namespace ObjectVisualization
{
    public class ObjectToStringConverter
    {
        private readonly LanguageTypes _LanguageTypes;

        public ObjectToStringConverter(LanguageTypes languageTypes = LanguageTypes.CSharp)
        {
            _LanguageTypes = languageTypes;
        }



        #region 公開機能


        public string Convert(object instance)
        {
            var doc = new XDocument();
            doc.Declaration = new XDeclaration("1.0", "utf-8", "yes");

            var root = new XElement("ObjectVisualization");
            doc.Add(root);

            var info = new XElement("ObjectInfo");
            root.Add(info);

            Serialize(info, instance);
            
            var xmlData = $"{doc.Declaration}{doc}";
            return xmlData;
        }

        private void Serialize(XElement parent, object instance)
        {
            var t = instance?.GetType();

            if (instance is null)
            {
                var value = ToTagetLanguageKeyword("null");
                CreatePrimitiveElement(parent, "null", value);
            }
            else if (instance is DBNull)
            {
                CreatePrimitiveElement(parent, t.Name, t.Name);
            }
            else if (IsNG(instance, t))
            {
                // int, string などのクラスは展開しないで、文字列表示するように修正
                var value = GetVariableTypeNameCore(instance.ToString());
                if (value == "Void" && _LanguageTypes == LanguageTypes.VBNET)
                    value = $"{value}（≒Subメソッド）";

                CreatePrimitiveElement(parent, t, value, "Blue");
            }
            else if (IsImageFamily(t))
            {
                SerializeForImageType(parent, instance, t);
            }
            else if (IsPrimitive(t))
            {
                SerializeForPrimitiveType(parent, instance, t);
            }
            else if (IsEnum(t))
            {
                var value = $"{t.Name}.{instance.ToString()}";
                CreatePrimitiveElement(parent, t, value, "Green");
            }
            else if (IsDelegate(t))
            {
                // メソッドのシグネチャーを表示
                CreateDelegateElement(parent, t);
            }
            else if (IsException(t))
            {
                CreateExceptionElement(parent, instance, t);
            }
            else if (IsDataSetFamily(t))
            {
                SerializeForDataSetFamilyType(parent, instance, t);
            }
            else if (IsEntityFrameworkFamily(t))
            {
                SerializeForEntityFrameworkFamilyType(parent, instance, t);
            }
            else if (IsXmlDocumentFamily(t))
            {
                SerializeForXmlDocumentFamilyType(parent, instance, t);
            }
            else if (IsXDocumentFamily(t))
            {
                SerializeForXDocumentFamilyType(parent, instance, t);
            }
            else if (IsCollection(t))
            {
                // 誤判定バグの対応（DataView がコレクション扱いされてしまうなど）。コレクション系は後半に判定するように対応
                CreateCollectionElement(parent, instance, t);
            }
            else
            {
                // 何かのクラス、構造体、または匿名型
                // 公開フィールド、プロパティ
                CreateMemberElement(parent, instance, t);
            }
        }

        private void SerializeForImageType(XElement parent, object instance, Type t)
        {
            var source = default(BitmapSource);
            if (IsBitmap(t))
            {
                var bitmap = instance as System.Drawing.Bitmap;
                source = ToBitmapSource(bitmap);
            }
            else if (IsBitmapSource(t))
            {
                source = instance as BitmapSource;
            }

            if (source is null)
                return;

            CreateImageElement(parent, source, t);
        }

        private void SerializeForPrimitiveType(XElement parent, object instance, Type t)
        {
            if (IsBool(t))
            {
                var value = instance.ToString();
                value = ToTagetLanguageKeyword(value);
                CreatePrimitiveElement(parent, t, value);
            }
            else if (IsNumber(t))
            {
                var value = String.Format("{0:#,0}", instance);
                CreatePrimitiveElement(parent, t, value);
            }
            else if (IsDateTimeFamily(t))
            {
                var value = instance.ToString();
                CreatePrimitiveElement(parent, t, value);
            }
            else if (IsStringFamily(t))
            {
                var value = instance.ToString();
                CreatePrimitiveElement(parent, t, value, "Brown");
            }
        }

        private void SerializeForDataSetFamilyType(XElement parent, object instance, Type t)
        {
            if (typeof(DataSet).IsAssignableFrom(t))
            {
                CreateDataSetElement(parent, instance);
            }
            else if (typeof(DataTable).IsAssignableFrom(t))
            {
                var value = t.Name;
                CreateDataTableElement(parent, instance, value);
            }
            else if (typeof(DataRow).IsAssignableFrom(t))
            {
                var value = t.Name;
                CreateDataRowElement(parent, instance, value);
            }
            else if (typeof(DataView).IsAssignableFrom(t))
            {
                CreateDataViewElement(parent, instance);
            }
            else if (typeof(DataRowView).IsAssignableFrom(t))
            {
                CreateDataRowViewElement(parent, instance);
            }
        }

        private void SerializeForEntityFrameworkFamilyType(XElement parent, object instance, Type t)
        {
            if (IsDbContext(t))
            {
                CreateDbContextElement(parent, instance, t);
            }
            else if (IsDbSet(t))
            {
                CreateDbSetElement(parent, instance, t);
            }
        }

        private void SerializeForXmlDocumentFamilyType(XElement parent, object instance, Type t)
        {
            if (t.Equals(typeof(XmlDocument)))
            {
                CreateXmlDocumentElement(parent, instance);
            }
            else if (t.Equals(typeof(XmlDeclaration)))
            {
                var element = instance as XmlDeclaration;
                CreateXmlDeclarationElement(parent, element);
            }
            else if (t.Equals(typeof(XmlAttribute)))
            {
                var element = instance as XmlAttribute;
                var items = new List<XmlAttribute>() { element };
                CreateXmlAttributeElement(parent, items);
            }
            else if (t.Equals(typeof(XmlComment)))
            {
                var element = instance as XmlComment;
                CreateXmlCommentElement(parent, element);
            }
            else if (t.Equals(typeof(XmlElement)))
            {
                CreateXmlElementElement(parent, instance);
            }
        }

        private void SerializeForXDocumentFamilyType(XElement parent, object instance, Type t)
        {
            if (t.Equals(typeof(XDocument)))
            {
                CreateXDocumentElement(parent, instance);
            }
            else if (t.Equals(typeof(XDeclaration)))
            {
                var element = instance as XDeclaration;
                CreateXDeclarationElement(parent, element);
            }
            else if (t.Equals(typeof(XAttribute)))
            {
                var element = instance as XAttribute;
                var items = new List<XAttribute>() { element };
                CreateXAttributeElement(parent, items);
            }
            else if (t.Equals(typeof(XComment)))
            {
                var element = instance as XComment;
                CreateXCommentElement(parent, element);
            }
            else if (t.Equals(typeof(XElement)))
            {
                CreateXElementElement(parent, instance);
            }
        }
        

        #endregion


        #region 型チェック関連


        // 論理型かどうか
        private bool IsBool(Type t)
        {
            if (t.Equals(typeof(bool))) return true;
            return false;
        }

        // 数値系かどうか
        private bool IsNumber(Type t)
        {
            if (t.Equals(typeof(byte))) return true;
            if (t.Equals(typeof(sbyte))) return true;
            if (t.Equals(typeof(decimal))) return true;
            if (t.Equals(typeof(double))) return true;
            if (t.Equals(typeof(float))) return true;
            if (t.Equals(typeof(int))) return true;
            if (t.Equals(typeof(uint))) return true;
            if (t.Equals(typeof(long))) return true;
            if (t.Equals(typeof(ulong))) return true;
            if (t.Equals(typeof(short))) return true;
            if (t.Equals(typeof(ushort))) return true;

            return false;
        }

        // 日付型かどうか
        private bool IsDateTimeFamily(Type t)
        {
            if (t.Equals(typeof(DateTime))) return true;
            if (t.Equals(typeof(DateTimeOffset))) return true;
            if (t.Equals(typeof(TimeSpan))) return true;
            return false;
        }

        // 文字列型かどうか
        private bool IsStringFamily(Type t)
        {
            if (t.Equals(typeof(char))) return true;
            if (t.Equals(typeof(string))) return true;
            return false;
        }

        // 組み込みの型かどうか
        private bool IsPrimitive(Type t)
        {
            if (IsBool(t)) return true;
            if (IsNumber(t)) return true;
            if (IsDateTimeFamily(t)) return true;
            if (IsStringFamily(t)) return true;
            return false;
        }

        // コレクション系かどうか
        private bool IsCollection(Type t)
        {
            if (t.Equals(typeof(string))) return false;
            if (typeof(IEnumerable).IsAssignableFrom(t)) return true;
            return false;
        }

        // クラス型かどうか
        private bool IsClass(Type t)
        {
            if (t.IsClass) return true;
            return false;
        }

        // 匿名型かどうか
        private bool IsAnonymousType(Type t)
        {
            if (t.Name.Contains("f__AnonymousType")) return true;
            return false;
        }

        // 構造体型かどうか
        private bool IsStruct(Type t)
        {
            if (t.IsValueType && ((t.Attributes & TypeAttributes.SequentialLayout) == TypeAttributes.SequentialLayout)) return true;
            return false;
        }

        // 列挙体型かどうか
        private bool IsEnum(Type t)
        {
            //if (t.IsValueType && typeof(System.Enum).IsAssignableFrom(t)) return true;
            if (t.IsEnum) return true;
            return false;
        }

        // デリゲート型かどうか
        private bool IsDelegate(Type t)
        {
            if (typeof(System.Delegate).IsAssignableFrom(t)) return true;
            return false;
        }

        // 例外エラー型かどうか
        private bool IsException(Type t)
        {
            if (typeof(Exception).IsAssignableFrom(t)) return true;
            return false;
        }

        // DataSet 系かどうか（型無し、型付き含む）
        private bool IsDataSetFamily(Type t)
        {
            if (typeof(DataSet).IsAssignableFrom(t)) return true;
            if (typeof(DataTable).IsAssignableFrom(t)) return true;
            if (typeof(DataRow).IsAssignableFrom(t)) return true;
            if (typeof(DataView).IsAssignableFrom(t)) return true;
            if (typeof(DataRowView).IsAssignableFrom(t)) return true;
            return false;
        }

        // XmlDocument 系かどうか
        private bool IsXmlDocumentFamily(Type t)
        {
            if (t.Equals(typeof(XmlDocument))) return true;
            if (t.Equals(typeof(XmlDeclaration))) return true;
            if (t.Equals(typeof(XmlAttribute))) return true;
            if (t.Equals(typeof(XmlComment))) return true;
            if (t.Equals(typeof(XmlElement))) return true;
            return false;
        }

        // XDocument 系かどうか
        private bool IsXDocumentFamily(Type t)
        {
            if (t.Equals(typeof(XDocument))) return true;
            if (t.Equals(typeof(XDeclaration))) return true;
            if (t.Equals(typeof(XAttribute))) return true;
            if (t.Equals(typeof(XComment))) return true;
            if (t.Equals(typeof(XElement))) return true;
            return false;
        }

        // Entity Framework 系かどうか
        private bool IsEntityFrameworkFamily(Type t)
        {
            if (IsDbContext(t)) return true;
            if (IsDbSet(t)) return true;
            return false;
        }

        // DbContext 型かどうか
        private bool IsDbContext(Type t)
        {
            if (t.BaseType?.Name == "DbContext" && t.BaseType.FullName.StartsWith("System.Data.Entity.DbContext")) return true;
            return false;
        }

        // DbSet 型かどうか
        private bool IsDbSet(Type t)
        {
            if (t.Name.StartsWith("DbSet") && t.FullName.StartsWith("System.Data.Entity.DbSet")) return true;
            return false;
        }

        // 文字列が xml 形式かどうか
        private bool IsXml(string s)
        {
            // タグ系以外ははじく
            s = s.Trim();
            if (!s.StartsWith("<")) return false;

            // html4, 5 系をはじく
            if (s.ToLower().StartsWith("<!doctype html")) return false;
            if (s.ToLower().StartsWith("<html")) return false;

            try
            {
                var dummy = new XmlDocument();
                dummy.LoadXml(s);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 文字列が csv 形式かどうか
        private bool IsCsv(string s)
        {
            var lines = s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            // 最低2行データがあること
            if (lines.Count() < 2)
                return false;

            // 1行目と2行目をサンプリングデータとしてみた場合、カンマ or タブ区切りがあること
            var check1 = Regex.IsMatch(lines[0], "^(.+)([,\t])(.*)");
            var check2 = Regex.IsMatch(lines[1], "^(.+)([,\t])(.*)");
            if (check1 && check2)
                return true;
            else
                return false;
        }

        // 画像系かどうか
        private bool IsImageFamily(Type t)
        {
            if (IsBitmap(t)) return true;
            if (IsBitmapSource(t)) return true;
            return false;
        }

        // (WinForms) System.Drawing.Bitmap かどうか
        private bool IsBitmap(Type t)
        {
            if (typeof(System.Drawing.Bitmap).IsAssignableFrom(t)) return true;
            return false;
        }

        // (WPF) System.Windows.Media.Imaging.BitmapSource かどうか
        private bool IsBitmapSource(Type t)
        {
            if (typeof(BitmapSource).IsAssignableFrom(t)) return true;
            return false;
        }

        // 任意に指定する、掘り下げたくない型かどうか
        private bool IsNG(object instance, Type t)
        {
            if (instance is Type) return true;
            if (instance is IntPtr) return true;
            if (instance is UIntPtr) return true;
            if (t.Name == "RuntimeModule") return true;
            if (t.Name == "RuntimeMethodInfo") return true;
            if (t.Name == "RuntimeParameterInfo") return true;

            return false;
        }


        #endregion


        #region 共通作成関連


        // プリミティブ型系
        //
        // 作成後イメージ
        // <Primitive Type="int" Color="Black">1,000</Primitive>
        private void CreatePrimitiveElement(XElement parent, Type t, string value, string color = "")
        {
            var typeName = GetVariableTypeName(t);
            CreatePrimitiveElement(parent, typeName, value, color);
        }

        private void CreatePrimitiveElement(XElement parent, string typeName, string value, string color = "")
        {
            if (string.IsNullOrEmpty(color))
            {
                if (typeName == "null") color = "Blue";
                if (typeName == "DBNull") color = "Green";
                if (value.ToLower() == "true") color = "Blue";
                if (value.ToLower() == "false") color = "Blue";
                if (color == "") color = "Black";
            }

            var element = new XElement("Primitive");
            element.SetAttributeValue("Type", typeName);
            element.SetAttributeValue("Color", color);
            element.Value = value;

            parent.Add(element);
        }

        // 画像系
        //
        // 作成後イメージ
        // <Primitive Type="BitmapImage" Color="Brown">...</Primitive>
        //
        // Converting WPF BitmapImage to and from Base64 using JpegBitmapEncoder fails for loading JPG files
        // https://stackoverflow.com/questions/40404625/converting-wpf-bitmapimage-to-and-from-base64-using-jpegbitmapencoder-fails-for
        // 
        private void CreateImageElement(XElement parent, BitmapSource bm, Type t)
        {
            // BitmapImage -> byte[]
            var bytes = default(byte[]);
            var encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bm));
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);
                bytes = stream.ToArray();
            }

            // byte[] -> string
            var byteString = System.Convert.ToBase64String(bytes);
            var element = new XElement("Primitive");
            element.SetAttributeValue("Type", "BitmapImage");
            element.SetAttributeValue("Color", "Brown");
            element.Value = byteString;

            parent.Add(element);
        }

        // Exception, Delegate
        //
        // 作成後イメージ
        // <Node Type="Single" Header="ArgumentException">
        //   <Primitive Type="string">Property Get メソッドが見つかりませんでした。</Primitive>
        // </Node>
        private void CreateSingleElement(XElement parent, string typeName, string value)
        {
            var element = new XElement("Node");
            element.SetAttributeValue("Type", "Single");
            element.SetAttributeValue("Header", typeName);
            parent.Add(element);

            Serialize(element, value);
        }

        // クラス、構造体、匿名型、系（１つのクラスで、フィールドやプロパティを持った型）
        //
        // 作成後イメージ。プロパティは、プリミティブ型、コレクション系、メンバー系、と可変する。フィールドもプロパティ扱いする（分けない）
        // <Node Type="Member" Header="Person">
        //     <Property Name="Id" Type="int">
        //         <Primitive Type="int" Color="Black">1</Primitive>
        //     </Property>
        //     <Property Name="Age" Type="int">
        //         <Primitive Type="int" Color="Black">23</Primitive>
        //     </Property>
        //     <Property Name="Name" Type="string">
        //         <Primitive Type="string" Color="Brown">null</Primitive>
        //     </Property>
        // </Node>
        private void CreateMemberElement(XElement parent, object instance, Type t)
        {
            var typeName = GetVariableTypeName(instance);
            var items = GetFieldAndPropertyMembers(instance, t);

            CreateMemberElement(parent, typeName, items);
        }

        private void CreateMemberElement(XElement parent, string typeName, List<Tuple<string, string, object>> items)
        {
            var element = new XElement("Node");
            element.SetAttributeValue("Type", "Member");
            element.SetAttributeValue("Header", typeName);
            parent.Add(element);

            foreach (var item in items)
            {
                var memberName = item.Item1;
                var memberTypeName = item.Item2;
                var memberInstance = item.Item3;

                var memberElement = new XElement("Property");
                memberElement.SetAttributeValue("Name", memberName);
                memberElement.SetAttributeValue("Type", memberTypeName);
                element.Add(memberElement);

                if (IsDescendantsLimitCheck(memberElement, memberName))
                {
                    var message = string.Empty;
                    if (memberInstance is null)
                    {
                        message = $"null（※再帰中断）";
                    }
                    else if (memberInstance is DBNull)
                    {
                        message = $"DBNull（※再帰中断）";
                    }
                    else
                    {
                        var value = string.Empty;
                        if (memberInstance is Type)
                            value = GetVariableTypeName(memberInstance as Type);
                        else
                            value = GetVariableTypeName(memberInstance.GetType());

                        message = $"{value}（※再帰中断）";
                    }

                    Serialize(memberElement, message);
                }
                else
                {
                    Serialize(memberElement, memberInstance);
                }
            }
        }

        // コレクション系
        //
        // 作成後イメージ。子データは、プリミティブ型、コレクション系、メンバー系、と可変する
        // <Node Type="Array" Header="List<string> (3 items)">
        //     <Primitive Type="string" Color="Brown">aaa</Primitive>
        //     <Primitive Type="string" Color="Brown">aaa</Primitive>
        //     <Primitive Type="string" Color="Brown">aaa</Primitive>
        // </Node>
        private void CreateCollectionElement(XElement parent, object instance, Type t)
        {
            var items = instance as IEnumerable;
            var count = 0;
            foreach (var item in items)
                count++;

            var typeName = GetVariableTypeName(t, count);
            if (IsDescendantsLimitCheck(parent))
            {
                typeName = $"{typeName}（※再帰中断）";
                Serialize(parent, typeName);
                return;
            }

            if (10 < count)
            {
                typeName = typeName.Substring(0, typeName.LastIndexOf(")"));
                typeName = $"{typeName}, only first 10 items)";
            }

            var element = new XElement("Node");
            element.SetAttributeValue("Type", "Array");
            element.SetAttributeValue("Header", typeName);
            element.SetAttributeValue("FamilyType", "AnyCollection");
            parent.Add(element);

            if (0 < count)
            {
                count = -1;
                foreach (var item in items)
                {
                    // 表示するデータ数を制限する
                    count++;
                    if (10 <= count)
                        break;

                    Serialize(element, item);
                }
            }
        }


        #endregion


        #region Delegate 作成関連


        private void CreateDelegateElement(XElement parent, Type t)
        {
            var value = GetVariableTypeName(t);
            var signature = CreateMethodSignatureData(t);

            CreateSingleElement(parent, value, signature);
        }

        // オープンジェネリック型(List<T>)を対象としてしまったが、ほとんどクローズドジェネリック型(List<string> など)が渡ってくるかも
        private string CreateMethodSignatureData(Type t, string methodName = "Sample")
        {
            var sb = new StringBuilder();
            var constraints = default(List<string>);
            var invokeMethod = t.GetMethod("Invoke");
            var methodArguments = "()";
            var returnType = "Void";

            // ジェネリック定義している場合
            if (t.IsGenericTypeDefinition)
            {
                var arguments = t.GetGenericArguments();
                constraints = GetGenericTypeNames(arguments);
            }

            // デリゲートに紐づけたメソッドから情報を得る
            if (!(invokeMethod is null))
            {
                methodArguments = GetMethodArguments(invokeMethod);
                returnType = GetVariableTypeName(invokeMethod.ReturnType);
            }

            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                if (returnType == "Void")
                    returnType = "void";

                // void Aaa<T, M>(int i, string s)
                sb.Append($"{returnType} {methodName}");
                if (!(constraints is null))
                {
                    sb.Append($"{constraints[0]}");
                    sb.Append($"{methodArguments}");

                    if (!string.IsNullOrEmpty(constraints[1]))
                        sb.Append($"{constraints[1]}");
                }
                else
                {
                    sb.Append($"{methodArguments}");
                }

                sb.Append("\n{");
                sb.Append("\n    ");
                sb.Append("\n}");
            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                // Sub Aaa(Of T, M)(i As Integer, s As String)
                // Function Aaa(Of T, M)(i As Integer, s As String) As Integer
                if (returnType == "Void")
                    sb.Append($"Sub ");
                else
                    sb.Append($"Function ");

                sb.Append($"{methodName}");

                if (!(constraints is null))
                    sb.Append($"{constraints[0]}");

                sb.Append($"{methodArguments}");

                if (returnType != "Void")
                    sb.Append($" As {returnType}");

                sb.Append("\n    ");
                if (returnType == "Void")
                    sb.Append($"\nEnd Sub");
                else
                    sb.Append($"\nEnd Function");

            }

            return sb.ToString();
        }

        // C# の場合、型名と制約が離れている。VBNET の場合、ひとまとまりになっている
        // C# の仕様に合わせて、戻り値１行目が型名リスト、２行目が制約リスト（C# 専用）とする
        private List<string> GetGenericTypeNames(Type[] arguments)
        {
            var firstBuilder = new StringBuilder();
            var secondBuilder = new StringBuilder();

            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                foreach (var argument in arguments)
                {
                    if (firstBuilder.Length != 0)
                        firstBuilder.Append(", ");

                    // 制約リスト
                    var items = new List<string>();
                    var attributes = argument.GenericParameterAttributes;
                    var check = attributes & GenericParameterAttributes.SpecialConstraintMask;

                    // 種類を指定している場合
                    if ((check & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint)
                        items.Add("class");

                    if ((check & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint)
                        items.Add("NotNullableValueType");

                    if ((check & GenericParameterAttributes.DefaultConstructorConstraint) == GenericParameterAttributes.DefaultConstructorConstraint)
                        items.Add("new()");

                    // 何らかのクラス、またはインターフェースを指定している場合
                    if (argument.GetGenericParameterConstraints().Any())
                    {
                        foreach (var constraintsType in argument.GetGenericParameterConstraints())
                            items.Add(constraintsType.Name);
                    }

                    firstBuilder.Append(argument.Name);
                    if (items.Any())
                    {
                        var constraints = string.Join(", ", items.ToArray());
                        secondBuilder.Append($"\n    where {argument.Name} : {constraints}");
                    }
                }

                firstBuilder.Insert(0, "<");
                firstBuilder.Append(">");
            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                foreach (var argument in arguments)
                {
                    if (firstBuilder.Length != 0)
                        firstBuilder.Append(", ");

                    // 制約リスト
                    var items = new List<string>();
                    var attributes = argument.GenericParameterAttributes;
                    var check = attributes & GenericParameterAttributes.SpecialConstraintMask;

                    // 種類を指定している場合
                    if ((check & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint)
                        items.Add("Class");

                    if ((check & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint)
                        items.Add("NotNullableValueType");

                    if ((check & GenericParameterAttributes.DefaultConstructorConstraint) == GenericParameterAttributes.DefaultConstructorConstraint)
                        items.Add("New");

                    // 何らかのクラス、またはインターフェースを指定している場合
                    if (argument.GetGenericParameterConstraints().Any())
                    {
                        foreach (var constraintsType in argument.GetGenericParameterConstraints())
                            items.Add(constraintsType.Name);
                    }

                    firstBuilder.Append(argument.Name);
                    if (items.Any())
                    {
                        var constraints = string.Join(", ", items.ToArray());
                        firstBuilder.Append($" As {"{"}{constraints}{"}"}");
                    }
                }

                firstBuilder.Insert(0, "(Of ");
                firstBuilder.Append(")");
            }

            return new List<string>() { firstBuilder.ToString(), secondBuilder.ToString() };
        }

        private string GetMethodArguments(MethodBase info)
        {
            return GetMethodArguments(info.GetParameters());
        }

        private string GetMethodArguments(ParameterInfo[] arguments)
        {
            var sb = new StringBuilder();

            foreach (var argument in arguments)
            {
                if (sb.Length != 0)
                    sb.Append(", ");

                var isIn = argument.IsIn;
                var isOut = argument.IsOut;
                var isRef = argument.ParameterType.ToString().EndsWith("&");
                var isOptional = argument.IsOptional;
                var defaultValue = argument.DefaultValue;
                var isParamArray = argument.GetCustomAttributes(true).Any(x => x.GetType().Equals(typeof(ParamArrayAttribute)));
                var parameterName = argument.Name;
                var parameterType = GetVariableTypeName(argument.ParameterType);

                if (_LanguageTypes == LanguageTypes.CSharp)
                {
                    if (isIn) sb.Append("in ");
                    if (isOut) sb.Append("out ");
                    if (isRef) sb.Append("ref ");
                    if (isParamArray) sb.Append("params ");

                    sb.Append($"{parameterType} {parameterName}");

                    if (isOptional) sb.Append($" = {defaultValue}");
                }
                else if (_LanguageTypes == LanguageTypes.VBNET)
                {
                    if (isRef)
                    {
                        parameterType = parameterType.Substring(0, parameterType.LastIndexOf("&"));
                        if (isOptional)
                            sb.Append($"ByRef Optional {parameterName} As {parameterType} = {defaultValue}");
                        else
                            sb.Append($"ByRef {parameterName} As {parameterType}");
                    }
                    else
                    {
                        if (isOptional)
                            sb.Append($"ByVal Optional {parameterName} As {parameterType} = {defaultValue}");
                        else if (isParamArray)
                            sb.Append($"ByVal ParamArray {parameterName} As {parameterType}");
                        else
                            sb.Append($"ByVal {parameterName} As {parameterType}");
                    }
                }
            }

            return $"({sb.ToString()})";
        }


        #endregion


        #region Exception 作成関連


        private void CreateExceptionElement(XElement parent, object instance, Type t)
        {
            var typeName = GetVariableTypeName(t);
            var message = (instance as Exception).Message;

            CreateSingleElement(parent, typeName, message);
        }


        #endregion


        #region DataSet 作成関連


        private void CreateDataSetElement(XElement parent, object instance)
        {
            var ds = instance as DataSet;
            var count = ds.Tables.Count;
            var typeName = GetVariableTypeName(ds, count);

            var element = new XElement("Node");
            element.SetAttributeValue("Type", "Array");
            element.SetAttributeValue("Header", typeName);
            element.SetAttributeValue("FamilyType", "DataSet");
            parent.Add(element);

            if (0 < count)
            {
                foreach (DataTable table in ds.Tables)
                {
                    var t = table.GetType();
                    CreateDataTableElement(element, table, t.Name);
                }
            }
        }

        private void CreateDataViewElement(XElement parent, object instance)
        {
            var typeName = "DataView";
            var view = instance as DataView;
            var table = view.ToTable();

            CreateDataTableElement(parent, table, typeName);
        }

        private void CreateDataTableElement(XElement parent, object instance, string typeName = "DataTable")
        {
            var table = instance as DataTable;
            var count = table.Rows.Count;

            if (count == 1)
                typeName = $"{typeName} (1 item)";
            else
                typeName = $"{typeName} ({count} items)";

            if (10 < count)
            {
                typeName = typeName.Substring(0, typeName.LastIndexOf(")"));
                typeName = $"{typeName}, only first 10 items)";
            }

            var element = new XElement("Node");
            element.SetAttributeValue("Type", "Array");
            element.SetAttributeValue("Header", typeName);
            element.SetAttributeValue("FamilyType", "DataTable");
            parent.Add(element);

            if (0 < count)
            {
                count = -1;
                foreach (DataRow row in table.Rows)
                {
                    // 表示するデータ数を制限する
                    count++;
                    if (10 <= count)
                        break;

                    var t = row.GetType();
                    CreateDataRowElement(element, row, t.Name);
                }
            }
        }

        private void CreateDataRowViewElement(XElement parent, object instance)
        {
            var typeName = "DataRowView";
            var view = instance as DataRowView;
            var row = view.Row;

            CreateDataRowElement(parent, row, typeName);
        }

        private void CreateDataRowElement(XElement parent, object instance, string typeName = "DataRow")
        {
            var row = instance as DataRow;
            var columns = row.Table.Columns;
            var items = new List<Tuple<string, string, object>>();

            foreach (DataColumn column in columns)
            {
                var memberName = column.ColumnName;
                var memberType = column.DataType;
                var memberTypeName = GetVariableTypeName(memberType);
                var memberInstance = row[memberName];

                items.Add(Tuple.Create(memberName, memberTypeName, memberInstance));
            }

            CreateMemberElement(parent, typeName, items);
        }


        #endregion


        #region Entity Framework 作成関連


        // NuGet して参照追加して扱うのが楽だが、Entity Framework の dll を2次配布したくないので、リフレクションで頑張る
        private void CreateDbContextElement(XElement parent, object instance, Type t)
        {
            // DbContext クラスに登録した各DBテーブルメンバー（DbSet<TableClass>）を取得（＝その他管理系のメンバーは除外）
            // DbSet<> は IEnumerable を継承している
            var members = t.GetProperties()
                .Where(x => typeof(IEnumerable).IsAssignableFrom(x.PropertyType))
                .Select(x => x.GetValue(instance, null) as IEnumerable);

            var count = members is null ? 0 : members.Count();
            var typeName = GetVariableTypeName(instance, count);

            var element = new XElement("Node");
            element.SetAttributeValue("Type", "Array");
            element.SetAttributeValue("Header", typeName);
            element.SetAttributeValue("FamilyType", "DbContext");
            parent.Add(element);

            if (0 < count)
            {
                foreach (var member in members)
                {
                    CreateDbSetElement(element, member, member.GetType());
                }
            }
        }

        private void CreateDbSetElement(XElement parent, object instance, Type t)
        {
            CreateCollectionElement(parent, instance, t);
        }


        #endregion


        #region XmlDocument 作成関連


        private void CreateXmlDocumentElement(XElement parent, object instance)
        {
            var doc = instance as XmlDocument;
            var typeName = GetVariableTypeName(doc);

            var element = new XElement("Node");
            element.SetAttributeValue("Type", "Array");
            element.SetAttributeValue("Header", typeName);
            element.SetAttributeValue("FamilyType", "XmlDocument");
            parent.Add(element);

            if (!doc.HasChildNodes)
                return;

            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node is XmlDeclaration)
                {
                    // メンバー数が多すぎるため、指定メンバーのみに絞る
                    var castedElement = node as XmlDeclaration;
                    CreateXmlDeclarationElement(element, castedElement);
                }
                else if (node is XmlComment)
                {
                    var castedElement = node as XmlComment;
                    CreateXmlCommentElement(element, castedElement);
                }
                else if (node is XmlElement)
                {
                    var castedElement = node as XmlElement;
                    CreateXmlElementElement(element, castedElement);
                }
                else
                {
                    // 未対応分
                    var value = node.ToString();
                    Serialize(element, value);
                }
            }
        }

        private void CreateXmlAttributeElement(XElement parent, List<XmlAttribute> attrs)
        {
            var typeName = attrs[0].GetType().Name;
            if (1 < attrs.Count)
                typeName = $"{typeName} ({attrs.Count} items)";

            var valueType = GetVariableTypeName(typeof(string));
            var items = new List<Tuple<string, string, object>>();
            foreach (var attr in attrs)
                items.Add(Tuple.Create(attr.Name, valueType, (object)attr.Value));

            CreateMemberElement(parent, typeName, items);
        }

        private void CreateXmlDeclarationElement(XElement parent, XmlDeclaration dec)
        {
            var typeName = dec.GetType().Name;
            var valueType = GetVariableTypeName(typeof(string));
            object version = string.IsNullOrEmpty(dec.Version) ? string.Empty : dec.Version;
            object encode = string.IsNullOrEmpty(dec.Encoding) ? string.Empty : dec.Encoding;
            object standalone = string.IsNullOrEmpty(dec.Standalone) ? string.Empty : dec.Standalone;

            var items = new List<Tuple<string, string, object>>
            {
                Tuple.Create("Version", valueType, version),
                Tuple.Create("Encoding", valueType, encode),
                Tuple.Create("Standalone", valueType, standalone),
            };

            CreateMemberElement(parent, typeName, items);
        }

        private void CreateXmlCommentElement(XElement parent, XmlComment comment)
        {
            var typeName = comment.GetType().Name;
            var value = comment.Value;

            CreateSingleElement(parent, typeName, value);
        }

        private void CreateXmlElementElement(XElement parent, object instance)
        {
            var target = instance as XmlElement;
            var typeName = GetVariableTypeName(target);

            var result = new XElement("Node");
            result.SetAttributeValue("Type", "Array");
            result.SetAttributeValue("Header", typeName);
            result.SetAttributeValue("FamilyType", "XmlElement");
            parent.Add(result);

            if (!target.HasAttributes && !target.HasChildNodes)
                return;

            if (target.HasAttributes)
            {
                // 謎仕様？子ノードがある時の属性の取得順番について、逆順に取得してしまうので、逆順の逆順に取得する
                var attrs = default(IEnumerable<XmlAttribute>);
                if (target.HasChildNodes)
                    attrs = target.Attributes.Cast<XmlAttribute>().Reverse();
                else
                    attrs = target.Attributes.Cast<XmlAttribute>();

                CreateXmlAttributeElement(result, attrs.ToList());
            }

            if (target.HasChildNodes)
            {
                foreach (XmlNode node in target.ChildNodes)
                {
                    if (node is XmlComment)
                    {
                        var castedElement = node as XmlComment;
                        CreateXmlCommentElement(result, castedElement);
                    }
                    else if (node is XmlText)
                    {
                        var castedElement = node as XmlText;
                        Serialize(result, castedElement.Value);
                    }
                    else if (node is XmlElement)
                    {
                        var castedElement = node as XmlElement;
                        CreateXmlElementElement(result, castedElement);
                    }
                    else
                    {
                        // 未対応分
                        var value = node.ToString();
                        Serialize(result, value);
                    }
                }
            }
        }


        #endregion


        #region XDocument 作成関連


        private void CreateXDocumentElement(XElement parent, object instance)
        {
            var doc = instance as XDocument;
            var typeName = GetVariableTypeName(doc);

            var element = new XElement("Node");
            element.SetAttributeValue("Type", "Array");
            element.SetAttributeValue("Header", typeName);
            element.SetAttributeValue("FamilyType", "XDocument");
            parent.Add(element);

            var dec = doc.Declaration;
            if (!(dec is null))
            {
                CreateXDeclarationElement(element, dec);
            }

            if (!doc.Document.Nodes().Any())
                return;

            foreach (XNode node in doc.Document.Nodes())
            {
                if (node is XComment)
                {
                    var castedElement = node as XComment;
                    CreateXCommentElement(element, castedElement);
                }
                else if (node is XElement)
                {
                    var castedElement = node as XElement;
                    CreateXElementElement(element, castedElement);
                }
                else
                {
                    // 未対応分
                    var value = node.ToString();
                    Serialize(element, value);
                }
            }
        }

        private void CreateXAttributeElement(XElement parent, List<XAttribute> attrs)
        {
            var item = attrs[0];
            var typeName = item.GetType().Name;
            if (1 < attrs.Count)
                typeName = $"{typeName} ({attrs.Count} items)";

            var items = GetNamespaces(item.Document);
            var valueType = GetVariableTypeName(typeof(string));
            var result = new List<Tuple<string, string, object>>();

            foreach (var attr in attrs)
            {
                var nsName = attr.Name.NamespaceName;
                var localName = attr.Name.LocalName;

                localName = ResolveNamespace(items, nsName, localName);
                result.Add(Tuple.Create(localName, valueType, (object)attr.Value));
            }

            CreateMemberElement(parent, typeName, result);
        }

        private void CreateXDeclarationElement(XElement parent, XDeclaration dec)
        {
            var typeName = dec.GetType().Name;
            var valueType = GetVariableTypeName(typeof(string));
            object version = string.IsNullOrEmpty(dec.Version) ? string.Empty : dec.Version;
            object encode = string.IsNullOrEmpty(dec.Encoding) ? string.Empty : dec.Encoding;
            object standalone = string.IsNullOrEmpty(dec.Standalone) ? string.Empty : dec.Standalone;

            var items = new List<Tuple<string, string, object>>
            {
                Tuple.Create("Version", valueType, version),
                Tuple.Create("Encoding", valueType, encode),
                Tuple.Create("Standalone", valueType, standalone),
            };

            CreateMemberElement(parent, typeName, items);
        }

        private void CreateXCommentElement(XElement parent, XComment comment)
        {
            var typeName = comment.GetType().Name;
            var value = comment.Value;

            CreateSingleElement(parent, typeName, value);
        }

        private void CreateXElementElement(XElement parent, object instance)
        {
            var target = instance as XElement;
            var typeName = GetVariableTypeName(target);

            var result = new XElement("Node");
            result.SetAttributeValue("Type", "Array");
            result.SetAttributeValue("Header", typeName);
            result.SetAttributeValue("FamilyType", "XElement");
            parent.Add(result);

            if (!target.HasAttributes && !target.Nodes().Any())
                return;

            if (target.HasAttributes)
            {
                // 謎仕様？子ノードがある時の属性の取得順番について、逆順に取得してしまうので、逆順の逆順に取得する
                var attrs = default(IEnumerable<XAttribute>);
                if (target.Nodes().Any())
                    attrs = target.Attributes().Reverse();
                else
                    attrs = target.Attributes();

                CreateXAttributeElement(result, attrs.ToList());
            }

            if (target.Nodes().Any())
            {
                foreach (XNode node in target.Nodes())
                {
                    if (node is XComment)
                    {
                        var castedElement = node as XComment;
                        CreateXCommentElement(result, castedElement);
                    }
                    else if (node is XText)
                    {
                        var castedElement = node as XText;
                        Serialize(result, castedElement.Value);
                    }
                    else if (node is XElement)
                    {
                        var castedElement = node as XElement;
                        CreateXElementElement(result, castedElement);
                    }
                    else
                    {
                        // 未対応分
                        var value = node.ToString();
                        Serialize(result, value);
                    }
                }
            }
        }


        #endregion


        #region 継承元クラスツリー関連


        // 継承元クラスと継承元インターフェースを階層的に表示する
        public string CreateBaseTypeTree(Type t)
        {
            var className = GetVariableTypeName(t);
            var items = new List<string>();
            items.Add(className);

            var dic = new Dictionary<string, List<string>>();
            var interfaces = t.GetInterfaces();
            if (!(interfaces is null) && 0 < interfaces.Length)
            {
                var values = new List<string>();
                foreach (var interfaceType in interfaces)
                    values.Add(GetVariableTypeName(interfaceType));

                dic[className] = values;
            }

            while (!(t.BaseType is null))
            {
                t = t.BaseType;
                className = GetVariableTypeName(t);
                items.Add(className);

                interfaces = t.GetInterfaces();
                if (!(interfaces is null) && 0 < interfaces.Length)
                {
                    var values = new List<string>();
                    foreach (var interfaceType in interfaces)
                        values.Add(GetVariableTypeName(interfaceType));

                    dic[className] = values;
                }
            }

            // 継承元インターフェースは現在のクラスに集約されるみたいなので、
            // 祖先クラスからさかのぼって、かぶっていたら子クラスのインターフェースは除去する用に調整
            var reserves = new List<string>();
            for (var i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                if (!dic.ContainsKey(item))
                    continue;

                var values = dic[item];
                var newValues = new List<string>();
                foreach (var value in values)
                {
                    if (!reserves.Contains(value))
                    {
                        reserves.Add(value);
                        newValues.Add(value);
                    }
                }
                dic[item] = newValues;
            }

            // 戻り値の作成
            var sb = new StringBuilder();
            sb.AppendLine("BaseTypeTree:");
            
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var columnLayer = new StringBuilder();
                columnLayer.Append(item);
                
                // インターフェースがあれば表示する
                if (dic.ContainsKey(item))
                {
                    var values = dic[item];
                    for (var k = 0; k < values.Count; k++)
                    {
                        columnLayer.Append($"__sep__{values[k]}");
                    }
                }

                sb.AppendLine(columnLayer.ToString());
            }
            
            return sb.ToString();
        }


        #endregion


        #region コールツリー関連


        public string CreateCallTree(StackFrame[] items)
        {
            var result = new List<string>();

            for (var i = 0; i < items.Length; i++)
            {
                var f = items[i];
                var sourceFilePath = f.GetFileName();
                if (string.IsNullOrEmpty(sourceFilePath))
                    continue;

                var method = f.GetMethod();
                var classType = method.ReflectedType;
                var className = classType.Name;
                var methodName = method.Name;

                // このライブラリ内のコールツリーは除外
                if (classType.Namespace == "ObjectVisualization")
                    continue;

                var fi = new FileInfo(sourceFilePath);
                var callInfo = $"{className}.{methodName}() メソッド\r\n{fi.Directory.Name}/{fi.Name}: {f.GetFileLineNumber()} 行目:";
                result.Insert(0, callInfo);
            }

            var sb = new StringBuilder();
            sb.Append("CallTree:__sep__");
            sb.Append(string.Join("__sep__", result.ToArray()));

            return sb.ToString();
        }


        #endregion


        #region 共通ヘルパー関連


        // 「<」「>」「"」は、置換しないと不正になってしまうので相互変換
        // 「&lt;」「&gt;」「""」
        // ただし、もともと「&lt;」「&gt;」「""」を含む文字列は変えたくないため、制御文字列もくっつける
        private string ToEscapeString(string s)
        {
            if (s.Contains("<"))
                s = s.Replace("<", "__lt__");

            if (s.Contains(">"))
                s = s.Replace(">", "__gt__");

            if (s.Contains(@""""))
                s = s.Replace(@"""", @"__dobule_quote__");

            return s;
        }

        private string ToNormalString(string s)
        {
            if (s.Contains("__lt__"))
                s = s.Replace("__lt__", "<");

            if (s.Contains("__gt__"))
                s = s.Replace("__gt__", ">");

            if (s.Contains(@"__dobule_quote__"))
                s = s.Replace(@"__dobule_quote__", @"""");

            return s;
        }

        private string GetVariableTypeName(XmlDocument doc)
        {
            // xml 宣言、コメントは子タグ数に含めない
            var itemsCount = doc.ChildNodes.Count;
            foreach (XmlNode node in doc.ChildNodes)
            {
                if (node is XmlDeclaration)
                    itemsCount--;

                if (node is XmlComment)
                    itemsCount--;
            }

            var name = GetVariableTypeName(doc, itemsCount);
            name = ToXmlItemsCountUnit(name);

            return name;
        }

        private string GetVariableTypeName(XDocument doc)
        {
            // コメントは子タグ数に含めない
            var itemsCount = doc.Document.Nodes().Count();
            foreach (XNode node in doc.Document.Nodes())
            {
                if (node is XComment)
                    itemsCount--;
            }

            var name = GetVariableTypeName(doc, itemsCount);
            name = ToXmlItemsCountUnit(name);

            return name;
        }

        private string GetVariableTypeName(XmlElement element)
        {
            // コメントは子タグ数に含めない
            var itemsCount = element.ChildNodes.Count;
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node is XmlComment)
                    itemsCount--;
            }

            var name = GetVariableTypeName(element, itemsCount, element.Name);
            name = ToXmlItemsCountUnit(name);

            return name;
        }

        private string GetVariableTypeName(XElement element)
        {
            // コメントは子タグ数に含めない
            var itemsCount = element.Nodes().Count();
            foreach (XNode node in element.Nodes())
            {
                if (node is XComment)
                    itemsCount--;
            }

            var tagName = GetXElementName(element);
            var name = GetVariableTypeName(element, itemsCount, tagName);
            name = ToXmlItemsCountUnit(name);

            return name;
        }

        private string GetXElementName(XElement element)
        {
            var items = GetNamespaces(element.Document);
            var nsName = element.Name.NamespaceName;
            var localName = element.Name.LocalName;
            localName = ResolveNamespace(items, nsName, localName);

            return localName;
        }

        private List<Tuple<string, string, string>> GetNamespaces(XDocument doc)
        {
            var items = doc.Descendants()
                .Where(x => x.HasAttributes)
                .SelectMany(x => x.Attributes())
                .Select(x => Tuple.Create(x.Name.NamespaceName, x.Name.LocalName, x.Value))
                .ToList();

            // xmlns プリフェックスの定義を取得できないため、手動置換する
            items.Add(Tuple.Create(string.Empty, "xmlns", "http://www.w3.org/2000/xmlns/"));
            return items;
        }

        private string ResolveNamespace(List<Tuple<string, string, string>> items, string nsName, string localName)
        {
            if (!string.IsNullOrEmpty(nsName))
            {
                var foundName = items.FirstOrDefault(x => x.Item3 == nsName); // Item3: Value
                if (foundName is null)
                {
                    if (nsName.EndsWith("/"))
                        nsName = nsName.Substring(0, nsName.Length - 1);

                    nsName = nsName.Substring(nsName.LastIndexOf("/") + 1);
                }
                else
                {
                    nsName = foundName.Item2; // Item2: LocalName
                }

                localName = $"{nsName}:{localName}";
            }

            return localName;
        }

        private string GetVariableTypeName(object instanceOrType, int itemsCount = -1, string tagName = "")
        {
            var t = default(Type);
            if (instanceOrType is Type)
                t = instanceOrType as Type;
            else
                t = instanceOrType.GetType();

            var name = GetVariableTypeNameCore(t);
            name = ToEscapeString(name);
            name = TrimTitle(name);

            if (-1 < itemsCount)
            {
                if (itemsCount == 1)
                {
                    if (string.IsNullOrEmpty(tagName))
                        name = $"{name} (1 item)";
                    else
                        name = $"{name} <{tagName}> (1 item)";
                }
                else
                {
                    if (string.IsNullOrEmpty(tagName))
                        name = $"{name} ({itemsCount} items)";
                    else
                        name = $"{name} <{tagName}> ({itemsCount} items)";
                }
            }

            return name;
        }

        private string GetVariableTypeNameCore(Type t)
        {
            var name = ToTagetLanguageKeyword(t.Name);
            var check = t.FullName is null ? t.Name : t.FullName;

            if (IsAnonymousType(t))
            {
                name = "匿名型";
            }
            else if (Regex.IsMatch(check, @"(System.Linq.Enumerable\+)(<*)(\w+Iterator)(>*)"))
            {
                name = "匿名型コレクション";
            }
            else if (0 < t.GetGenericArguments().Length)
            {
                // ジェネリック系
                var arguments = t.GetGenericArguments().Select(x => GetVariableTypeName(x)).ToArray();
                name = Regex.Replace(name, @"`\d+", string.Empty);

                if (_LanguageTypes == LanguageTypes.CSharp)
                {
                    name = $"{name}<{string.Join(", ", arguments)}>";
                }
                else if (_LanguageTypes == LanguageTypes.VBNET)
                {
                    name = $"{name}(Of {string.Join(", ", arguments)})";
                }
            }

            if (_LanguageTypes == LanguageTypes.VBNET)
            {
                if (name.Contains("["))
                    name = name.Replace("[", "(");

                if (name.Contains("]"))
                    name = name.Replace("]", ")");
            }

            return name;
        }

        private string GetVariableTypeNameCore(string s)
        {
            // clr 型で以下のような文字列が来るので不要情報を除去したり置換したりする
            // "System.System.Action`2[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"
            // "System.Collections.Generic.Dictionary`2[System.Collections.Generic.Dictionary`2[System.String[],System.Int32],System.Collections.Generic.Dictionary`2[System.String,System.Int32[]]]"

            // 名前空間を除去、アセンブリ情報を除去、ジェネリック定義数を除去、各ジェネリック型の残った不要情報を除去 -> Action[[Int32, mscorlib],[Int32, mscorlib]] ジェネリック型毎のかっことか残情報とか
            s = Regex.Replace(s, @"([\w.]+)[.]", "");
            s = Regex.Replace(s, @"(, Version=[\d\.]+)(, Culture=[\w\.]+)(, PublicKeyToken=[\w\.]+)", "");
            s = Regex.Replace(s, @"`\d+", "");
            s = Regex.Replace(s, @"\[([\w\d]+), [\w\d\.]+\]", "$1");

            // 言語別のジェネリック記述方法への置換、配列の置換
            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                s = Regex.Replace(s, @"\[", "<");
                s = Regex.Replace(s, @"\]", ">");
                s = Regex.Replace(s, @"<>", "[]");
            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                s = Regex.Replace(s, @"\[", "(Of ");
                s = Regex.Replace(s, @"\]", ")");
                s = Regex.Replace(s, @"\(Of \)", "()");
            }
            else
            {
                // nop.
            }

            // ジェネリック型のカンマ後のスペース追加
            s = Regex.Replace(s, @",", ", ");
            s = ToTagetLanguageKeyword(s);
            return s;
        }

        // VBNET / "List(Of String, item) (1 item)" があり得るか？
        private string ToXmlItemsCountUnit(string name)
        {
            if (name.EndsWith(" item)"))
            {
                name = name.Substring(0, name.LastIndexOf(" item)"));
                name = $"{name} child item)";
            }
            else if (name.EndsWith(" items)"))
            {
                name = name.Substring(0, name.LastIndexOf(" items)"));
                name = $"{name} child items)";
            }

            return name;
        }

        private string ToTagetLanguageKeyword(string value)
        {
            if (_LanguageTypes == LanguageTypes.CSharp)
            {
                value = LanguageConverter.ToCSharp(value);
            }
            else if (_LanguageTypes == LanguageTypes.VBNET)
            {
                value = LanguageConverter.ToVBNET(value);
            }
            else
            {
                // nop.
            }

            return value;
        }

        private List<Tuple<string, string, object>> GetFieldAndPropertyMembers(object instance, Type t)
        {
            var fieldTypes = t.GetFields();
            var propertyTypes = t.GetProperties();
            var items = new List<Tuple<string, string, object>>();

            // フィールドとプロパティメンバーをまとめて扱う。同名でかぶってしまうことは無いはず？
            foreach (var info in fieldTypes)
            {
                var memberName = info.Name;
                var memberType = info.FieldType;
                var memberTypeName = GetVariableTypeName(memberType);
                var memberInstance = info.GetValue(instance);
                items.Add(Tuple.Create(memberName, memberTypeName, memberInstance));
            }

            foreach (var info in propertyTypes)
            {
                var memberName = info.Name;
                var memberType = info.PropertyType;
                var memberTypeName = GetVariableTypeName(memberType);
                var memberInstance = default(object);

                // Getter が無い場合に備える
                try
                {
                    memberInstance = info.GetValue(instance, null);
                }
                catch (Exception ex)
                {
                    memberInstance = ex;
                }

                items.Add(Tuple.Create(memberName, memberTypeName, memberInstance));
            }

            return items;
        }

        private string TrimTitle(string value, int overLength = 80)
        {
            if (overLength <= value.Length)
            {
                value = $"{value.Substring(0, overLength)}...";
            }

            return value;
        }

        // C#における「ビットマップ形式の画像データを相互変換」まとめ
        // https://qiita.com/YSRKEN/items/a24bf2173f0129a5825c
        // (WinForms) System.Drawing.Bitmap を、(WPF) System.Windows.Media.Imaging.BitmapSource に変換する
        private BitmapSource ToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            var result = default(BitmapSource);
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                result = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            }

            return result;
        }

        private bool IsDescendantsLimitCheck(XElement element, string propertyName = "", int limitCount = 4)
        {
            // コレクション系の再帰制限、4階層まで
            // <Node Type="Array" Header="List<object> (1 item)">
            //   <Node Type="Array" Header="List<object> (1 item)">
            //     <Node Type="Array" Header="List<object> (1 item)">
            //       <Node Type="Array" Header="List<object> (1 item)">
            //         <Node Type="Array" Header="List<object> (1 item)">
            // 
            // クラス系の再帰制限、4階層まで
            // <Node Type="Member" Header="Person">
            //   <Property Name="Empty" Type="Person">
            //     <Node Type="Member" Header="Person">
            //       <Property Name="Empty" Type="Person">
            //         <Node Type="Member" Header="Person">
            //           <Property Name="Empty" Type="Person">
            //             <Node Type="Member" Header="Person">
            //               <Property Name="Empty" Type="Person">
            //                 <Node Type="Member" Header="Person">
            //                   <Property Name="Empty" Type="Person">

            // どちらもチェックしておいて、どちらか一方が制限に引っ掛かっていたら true, どちらも制限ないなら false
            // ただし、propertyName が空の場合は、コレクション系のチェックのみで判定する
            // また、XmlDocument 系, XDocument 系はすぐに階層が深くなるのだが、全表示させたいため制限の対象外とする

            if (IsXmlOperationClass(element) || IsXmlOperationClass(element.Parent))
                return false;
            
            var dummy = element;
            var arrayCount = 0;
            var propertyCount = 0;

            while (!(dummy.Parent is null))
            {
                dummy = dummy.Parent;

                if (IsArray(dummy))
                    arrayCount++;

                if (!string.IsNullOrEmpty(propertyName))
                {
                    if (IsPropertyAndSameName(dummy, propertyName))
                        propertyCount++;
                }

            }

            if (string.IsNullOrEmpty(propertyName))
            {
                if (limitCount <= arrayCount)
                    return true;
                else
                    return false;
            }
            else
            {
                if (limitCount <= arrayCount || limitCount <= propertyCount)
                    return true;
                else
                    return false;
            }
        }

        private bool IsPropertyAndSameName(XElement element, string propertyName)
        {
            return (element.Name == "Property" && element.Attribute("Name").Value == propertyName);
        }

        private bool IsArray(XElement element)
        {
            return (element.Name == "Node" && element.Attribute("Type").Value == "Array");
        }

        private bool IsXmlOperationClass(XElement element)
        {
            if (element is null)
                return false;

            return (IsXmlDocumentFamily(element) || IsXDocumentFamily(element));
        }

        private bool IsXmlDocumentFamily(XElement element)
        {
            if (element.Name != "Node")
                return false;

            var typeName = element.Attribute("Header").Value;
            return (typeName.StartsWith("XmlDocument") || typeName.StartsWith("XmlDeclaration") || typeName.StartsWith("XmlAttribute") || typeName.StartsWith("XmlComment") || typeName.StartsWith("XmlElement"));
        }

        private bool IsXDocumentFamily(XElement element)
        {
            if (element.Name != "Node")
                return false;

            var typeName = element.Attribute("Header").Value;
            return (typeName.StartsWith("XDocument") || typeName.StartsWith("XDeclaration") || typeName.StartsWith("XAttribute") || typeName.StartsWith("XComment") || typeName.StartsWith("XElement"));
        }
        

        #endregion

    }
}
