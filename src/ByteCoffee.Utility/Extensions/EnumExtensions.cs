using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace ByteCoffee.Utility.Extensions
{
    /// <summary>
    /// 枚举<see cref="Enum"/>的扩展辅助操作方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举项上的<see cref="DescriptionAttribute"/>特性的文字描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).FirstOrDefault();
            return member != null ? member.ToDescription() : value.ToString();
        }

        ///// <summary>
        ///// 扩展方法，获得枚举的Description
        ///// </summary>
        ///// <param name="value">枚举值</param>
        ///// <param name="nameInstead">当枚举值没有定义DescriptionAttribute，是否使用枚举名代替，默认是使用</param>
        ///// <returns>
        ///// 枚举的Description
        ///// </returns>
        //public static string GetDescription(this Enum value, Boolean nameInstead = true)
        //{
        //    Type type = value.GetType();
        //    string name = Enum.GetName(type, value);
        //    if (name == null)
        //    { return null; }

        //    FieldInfo field = type.GetField(name);
        //    DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        //    if (attribute == null && nameInstead == true)
        //    { return name; }
        //    return attribute == null ? null : attribute.Description;
        //}

        /// <summary>
        /// 将枚举转换为键值对集合
        /// </summary>
        /// <param name="enumType">枚举的类型</param>
        /// <param name="getText">获取值的委托</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">传入的参数必须是枚举类型;enumType</exception>
        public static Dictionary<Int32, String> ToDictionary(this Type enumType, Func<Enum, String> getText)
        {
            if (!enumType.IsEnum)
            { throw new ArgumentException("传入的参数必须是枚举类型", "enumType"); }
            var enumValues = Enum.GetValues(enumType);
            return enumValues.Cast<Enum>().ToDictionary(Convert.ToInt32, getText);
        }

        /// <summary>
        /// 返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="eEnum">enum?类型</param>
        /// <returns>description描述</returns>
        public static string GetDescription(this object eEnum)
        {
            try
            {
                if (eEnum != null && eEnum.GetType().IsEnum)
                {
                    var t = eEnum.GetType();
                    FieldInfo oFieldInfo = t.GetField(eEnum.ToString());
                    var attributes = (DescriptionAttribute[])oFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return (attributes.Length > 0) ? attributes[0].Description : eEnum.ToString();
                }
                return eEnum == null ? "" : eEnum.ToString();
            }
            catch
            {
                return "UNKNOWN";
            }
        }
    }
}