using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;


namespace CleanArchitecture.Common.Core.Utils
{
    public static class Extension
    {

        public static DataTable MapListToDataTable<T>(this List<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static T MapToExcept<T>(this object obj, params string[] excepts)
        {

            var value = (T)Activator.CreateInstance(typeof(T));

            return obj.MapTo(value, onlyColumns: null, exceptColumns: excepts, afterMap: null);

        }
        public static T MapToOnly<T>(this object obj, params string[] onlyColumns)
        {
            var value = (T)Activator.CreateInstance(typeof(T));
            return obj.MapTo(value, onlyColumns: onlyColumns, exceptColumns: null, afterMap: null);

        }

        public static T MapTo<T>(this object obj, Func<T, T> afterMap)
        {
            var value = (T)Activator.CreateInstance(typeof(T));
            return obj.MapTo(value, onlyColumns: null, exceptColumns: null, afterMap);
        }



        public static List<TO> MapToList<FROM , TO>(this IEnumerable<FROM> list, Func<TO, TO> afterMap)
        {

            return list.Select(m => m.MapTo(afterMap)).ToList(); ;
        }
        public static List<TO> MapToList<FROM, TO>(this IEnumerable<FROM> list)
        {

            return list.Select(m => m.MapTo<TO>()).ToList(); ;
        }


        public static T MapTo<T>(this object obj, T value, string[] onlyColumns, string[] exceptColumns, Func<T, T> afterMap)
        {
            var propName = "";
            try
            {
                var props = value.GetType().GetProperties();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    propName = prop.Name;

                    if (onlyColumns != null && !onlyColumns.Contains(prop.Name))
                    {
                        continue;
                    }

                    if (exceptColumns != null && exceptColumns.Contains(prop.Name))
                    {
                        continue;
                    }

                    var p = props.Where(m => m.Name == prop.Name).FirstOrDefault();
                    if (p != null &&
                        p.CanWrite &&
                        p.GetSetMethod(true).IsPublic)
                    {
                        if (p.PropertyType == prop.PropertyType)
                            p.SetValue(value, prop.GetValue(obj));
                        else
                        {
                            if (isNullable(p) && getNullType(p) == prop.PropertyType)
                            {
                                var val = prop.GetValue(obj, null);
                                var nullable = Nullable.GetUnderlyingType(p.PropertyType);
                                var temp = Activator.CreateInstance(nullable);
                                temp = val;
                                p.SetValue(obj, temp);
                            }
                            if (isNullable(prop) && getNullType(prop) == p.PropertyType)
                            {
                                var val = prop.GetValue(obj, null);
                                if (val == null)
                                {

                                    if (prop.PropertyType.IsValueType)
                                    {
                                        p.SetValue(value, getDefaultValue(prop));
                                    }

                                }
                                else
                                {
                                    p.SetValue(value, prop.GetValue(obj, null));
                                }

                            }
                        }
                    }
                }
                if (afterMap != null)
                    value = afterMap(value);
                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static T MapTo<T>(this object obj, T value)
        {
            try
            {
                return obj.MapTo(value, null, null, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static object getDefaultValue(System.Reflection.PropertyInfo prop)
        {
            return Activator.CreateInstance(prop.PropertyType);
        }
        private static object getDefaultValue(Type prop)
        {
            return Activator.CreateInstance(prop);
        }
        private static bool isNullable(System.Reflection.PropertyInfo p)
        {
            return p.PropertyType.IsGenericType &&
                                            p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool isNullable(Type type)
        {
            return type.IsGenericType &&
                                            type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
        private static Type getNullType(System.Reflection.PropertyInfo p)
        {
            return Nullable.GetUnderlyingType(p.PropertyType);
        }
        private static Type getNullType(Type p)
        {
            return Nullable.GetUnderlyingType(p);
        }
        public static T MapTo<T>(this object obj)
        {
            return obj.MapTo<T>(null);
        }

        public static T MapTo<T>(this object obj, string[] onlyList, string[] exceptionList)
        {

            var value = Activator.CreateInstance<T>();
            return obj.MapTo(value, onlyList, exceptionList, null);
        }

        public static DateTime ResetTime(this DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, 0);
        }
        public static DateTime SetCurrentTime(this DateTime time)
        {
            var dt = DateTime.Now;
            return new DateTime(time.Year, time.Month, time.Day, dt.Hour, dt.Minute, dt.Second, dt.Minute);
        }
        public static DateTime SetTime(this DateTime time, int hour, int minute, int second, int milliSeccond)
        {
            return new DateTime(time.Year, time.Month, time.Day, hour, minute, second, milliSeccond);
        }
        public static long ToUnixMillisecond(this DateTime dt)
        {
            return new DateTimeOffset(dt).ToUnixTimeMilliseconds();
        }

        public static DateTime ToDatetime(this long unixMillisecond)
        {

            return DateTimeOffset.FromUnixTimeSeconds(unixMillisecond / 1000).Date;
        }


        public static string ToLocaleString(this decimal value)
        {
            return value.ToString("###,###.#########").Replace(".", "/");
        }


        public static T MapToObject<T>(this DataRow row, string[] excepts)
        {
            string propName = "";
            try
            {
                T value = Activator.CreateInstance<T>();
                var props = value.GetType().GetProperties();
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (excepts != null && excepts.Contains(column.ColumnName))
                    {
                        continue;
                    }
                    var destProp = props.Where(m => m.Name == column.ColumnName).FirstOrDefault();
                    if (destProp != null && destProp.CanWrite && destProp.GetSetMethod().IsPublic)
                    {
                        propName = destProp.Name;
                        if (destProp.PropertyType == column.DataType)
                        {
                            object sourceValue = row[column.ColumnName];
                            if (sourceValue is DBNull)
                                sourceValue = null;
                            destProp.SetValue(value, sourceValue);
                        }
                        else
                        {

                            if (isNullable(destProp) && getNullType(destProp) == column.DataType)
                            {
                                object sourceValue = row[column.ColumnName];
                                if (sourceValue is DBNull)
                                    sourceValue = null;

                                var nullable = Nullable.GetUnderlyingType(destProp.PropertyType);

                                destProp.SetValue(value, sourceValue);
                            }
                            if (isNullable(column.DataType) && getNullType(column.DataType) == destProp.PropertyType)
                            {
                                object sourceValue = row[column.ColumnName];
                                if (sourceValue is DBNull)
                                    sourceValue = null;
                                if (sourceValue == null)
                                {
                                    if (column.DataType.IsValueType)
                                    {
                                        destProp.SetValue(value, getDefaultValue(column.DataType));
                                    }
                                }
                                else
                                {
                                    destProp.SetValue(value, sourceValue);
                                }

                            }
                        }

                    }

                }
                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public class ReaderColumn
        {

            public string ColumnName { get; set; }
            public string DataTypeName { get; set; }
            public Type DataType
            {
                get
                {
                    return Type.GetType(DataTypeName);
                }
            }
        }

        public static T MapToObject<T>(this IDataReader reader, string[] excepts)
        {
            string propName = "";
            try
            {
                T value = Activator.CreateInstance<T>();
                var props = value.GetType().GetProperties();
                var columnList = reader.GetSchemaTable().AsEnumerable().Select(m => new ReaderColumn()
                {
                    ColumnName = m.Field<string>("ColumnName"),
                    DataTypeName = m.Field<string>("DataTypeName")
                });
                foreach (ReaderColumn column in columnList)
                {
                    if (excepts != null && excepts.Contains(column.ColumnName))
                    {
                        continue;
                    }
                    var destProp = props.Where(m => m.Name == column.ColumnName).FirstOrDefault();
                    if (destProp != null && destProp.CanWrite && destProp.GetSetMethod().IsPublic)
                    {
                        propName = destProp.Name;
                        if (destProp.PropertyType == column.DataType)
                        {
                            object sourceValue = reader[column.ColumnName];
                            if (sourceValue is DBNull)
                                sourceValue = null;
                            destProp.SetValue(value, sourceValue);
                        }
                        else
                        {

                            if (isNullable(destProp) && getNullType(destProp) == column.DataType)
                            {
                                object sourceValue = reader[column.ColumnName];
                                if (sourceValue is DBNull)
                                    sourceValue = null;

                                var nullable = Nullable.GetUnderlyingType(destProp.PropertyType);

                                destProp.SetValue(value, sourceValue);
                            }
                            if (isNullable(column.DataType) && getNullType(column.DataType) == destProp.PropertyType)
                            {
                                object sourceValue = reader[column.ColumnName];
                                if (sourceValue is DBNull)
                                    sourceValue = null;
                                if (sourceValue == null)
                                {
                                    if (column.DataType.IsValueType)
                                    {
                                        destProp.SetValue(value, getDefaultValue(column.DataType));
                                    }
                                }
                                else
                                {
                                    destProp.SetValue(value, sourceValue);
                                }

                            }
                        }

                    }

                }
                return value;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<T> MapToList<T>(this DataTable table, string[] excepts)
        {
            var list = new List<T>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    list.Add(row.MapToObject<T>(excepts));
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static IEnumerable<T> MapToEnumerable<T>(this IDataReader reader, string[] excepts)
        {
            while (reader.Read())
            {
                yield return reader.MapToObject<T>(excepts);
            }
        }

        public static List<T> MapToList<T>(this IDataReader reader, string[] excepts)
        {
            var list = new List<T>();
            try
            {
                while (reader.Read())
                {
                    list.Add(reader.MapToObject<T>(excepts));
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
