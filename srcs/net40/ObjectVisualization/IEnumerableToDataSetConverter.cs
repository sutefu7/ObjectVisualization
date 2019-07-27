using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectVisualization
{
    public class IEnumerableToDataSetConverter
    {
        public DataSet Convert(IEnumerable items)
        {
            var table = ToDataTable(items);
            var ds = new DataSet();
            ds.Tables.Add(table);
            return ds;
        }


        private const string FirstColumnName = "Object";

        public DataTable ToDataTable(IEnumerable items)
        {
            // 列は、各データのフィールド、プロパティ名、またはそれ以外
            var table = new DataTable();
            table.Columns.Add(FirstColumnName, typeof(object));

            foreach (var item in items)
            {
                var t = item?.GetType();
                if (item is null || item is DBNull)
                {
                    var row = table.NewRow();
                    row[FirstColumnName] = item;
                    table.Rows.Add(row);
                }
                else if (IsPrimitive(t))
                {
                    var row = table.NewRow();
                    row[FirstColumnName] = item;
                    table.Rows.Add(row);
                }
                else if (t.IsEnum)
                {
                    var value = $"{t.Name}.{item.ToString()}";
                    var row = table.NewRow();
                    row[FirstColumnName] = value;
                    table.Rows.Add(row);
                }
                else if (IsCollectionType(t))
                {
                    // コレクション系のメンバーを取得しないように、１つ前で判定しておく
                    var row = table.NewRow();
                    row[FirstColumnName] = item.ToString();
                    table.Rows.Add(row);
                }
                else if (t.IsClass || IsStructType(t) || IsAnonymousType(t))
                {
                    ToDataTableForMember(item, t, table);
                }
                else if (typeof(DataRowView).IsAssignableFrom(t))
                {
                    var view = item as DataRowView;
                    var row = view.Row;
                    ToDataTableForDataRow(row, table);
                }
                else if (typeof(DataRow).IsAssignableFrom(t))
                {
                    var row = item as DataRow;
                    ToDataTableForDataRow(row, table);
                }
                else
                {
                    var row = table.NewRow();
                    row[FirstColumnName] = item.ToString();
                    table.Rows.Add(row);
                }
            }

            // 最初の列にデータが無い場合、列自体を削除する
            var isDeleteColumn = true;
            foreach (DataRow row in table.Rows)
            {
                if (!(row[FirstColumnName] is DBNull))
                {
                    isDeleteColumn = false;
                    break;
                }
            }

            if (isDeleteColumn)
            {
                var column = table.Columns[FirstColumnName];
                table.Columns.Remove(column);
            }

            // 最初の列が１つだけの場合、列名を消す（List<Primitive> 系を想定）
            if (table.Columns.Count == 1 && table.Columns[0].ColumnName == FirstColumnName)
            {
                table.Columns[0].ColumnName = " ";
            }

            return table;
        }

        private void ToDataTableForDataRow(DataRow row, DataTable table)
        {
            var columns = row.Table.Columns;

            // メンバー数分、列作成
            for (var i = 0; i < columns.Count; i++)
            {
                var memberName = columns[i].ColumnName;
                var memberType = columns[i].DataType;

                if (!table.Columns.Contains(memberName))
                    table.Columns.Add(memberName, typeof(object));
            }

            // １行分（１つ分のデータ）を登録（メンバー名に該当する場合はその値、それ以外は DBNull）
            var newRow = table.NewRow();
            for (var i = 0; i < columns.Count; i++)
            {
                var columnName = columns[i].ColumnName;
                var memberValue = row[columnName];
                if (!(memberValue is null || memberValue is DBNull))
                    memberValue = memberValue.ToString();

                newRow[columnName] = memberValue;
            }
            table.Rows.Add(newRow);
        }

        private void ToDataTableForMember(object item, Type t, DataTable table)
        {
            // クラス、構造体、匿名型
            // メンバー名がかぶった場合まとめて扱う。型が違っていても無視
            var items = GetFieldAndPropertyMembers(item, t);

            // メンバー数分、列作成
            for (var i = 0; i < items.Count; i++)
            {
                var memberName = items[i].Item1;
                var memberType = items[i].Item2;
                if (!table.Columns.Contains(memberName))
                    table.Columns.Add(memberName, typeof(object));
            }

            // １行分（１つ分のデータ）を登録
            var row = table.NewRow();
            for (var i = 0; i < items.Count; i++)
            {
                var memberName = items[i].Item1;
                var memberValue = items[i].Item3;

                if (!(memberValue is null || memberValue is DBNull))
                    memberValue = memberValue.ToString();

                row[memberName] = memberValue;
            }
            table.Rows.Add(row);
        }

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

        // 匿名型かどうか
        private bool IsAnonymousType(Type t)
        {
            return t.Name.Contains("f__AnonymousType");
        }

        // 構造体型かどうか
        private bool IsStructType(Type t)
        {
            return (t.IsValueType && ((t.Attributes & TypeAttributes.SequentialLayout) == TypeAttributes.SequentialLayout));
        }

        // コレクション系かどうか
        private bool IsCollectionType(Type t)
        {
            if (t.Equals(typeof(string))) return false;
            if (typeof(IEnumerable).IsAssignableFrom(t)) return true;
            return false;
        }

        private List<Tuple<string, Type, object>> GetFieldAndPropertyMembers(object instance, Type t)
        {
            var fieldTypes = t.GetFields();
            var propertyTypes = t.GetProperties();
            var items = new List<Tuple<string, Type, object>>();

            // フィールドとプロパティメンバーをまとめて扱う。同名でかぶってしまうことは無いはず？
            foreach (var info in fieldTypes)
            {
                var memberName = info.Name;
                var memberType = info.FieldType;
                var memberInstance = info.GetValue(instance);
                items.Add(Tuple.Create(memberName, memberType, memberInstance));
            }

            foreach (var info in propertyTypes)
            {
                var memberName = info.Name;
                var memberType = info.PropertyType;
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

                items.Add(Tuple.Create(memberName, memberType, memberInstance));
            }

            return items;
        }
    }
}
