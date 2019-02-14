using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace System
{
    public static class EnumHelper<T>
    {
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static List<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        public static string GetDisplayValue(T value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DisplayAttribute descriptionAttribute = fieldInfo?.GetCustomAttributes<DisplayAttribute>().FirstOrDefault();

            if (descriptionAttribute == null)
            {
                return string.Empty;
            }

            return descriptionAttribute.Name;
        }
    }
}
