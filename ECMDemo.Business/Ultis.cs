using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ECMDemo.Business.Common
{
    public class Ultis
    {
        public static T ConvertSameData<T>(object source)
        {
            var result = Activator.CreateInstance<T>();
            foreach (var item in typeof(T).GetProperties())
            {
                if (!item.GetMethod.IsVirtual)
                    if (source.GetType().GetProperty(item.Name) != null)
                        item.SetValue(result, source.GetType().GetProperty(item.Name).GetValue(source) ?? null);
            }
            return result;
        }
        public static object ChangeType(object value, Type type)
        {
            // returns null for non-nullable types
            Type type2 = Nullable.GetUnderlyingType(type);

            if (type2 != null)
            {
                if (value == null)
                {
                    return null;
                }

                type = type2;
            }

            return Convert.ChangeType(value, type);
        }
        /// <summary>
        /// Chuyển giá trị từ B sang A
        /// </summary>
        /// <param name="value1">Đối tượng A</param>
        /// <param name="value2">Đối tượng B</param>
        public static void TransferValues(object value1, object value2)
        {
            foreach (var item in value1.GetType().GetProperties())
            {
                //Check if value2 had the same property
                var info2 = value2.GetType().GetProperty(item.Name);
                if (info2 != null)
                {
                    if (!item.GetMethod.IsVirtual)
                        item.SetValue(value1, info2.GetValue(value2) ?? null);
                }
            }
        }
    }
    
    
}