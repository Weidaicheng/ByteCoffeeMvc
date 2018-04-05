using System;

namespace ByteCoffee.Utility.Extensions
{
    public static class NumberExtensions
    {
        /// <summary>
        /// 字符串转化为int32
        /// 描述：转化成功则返回转化的值，如果转化失败则返回默认值
        /// </summary>
        /// <param name="txt">字符串</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns></returns>
        public static int ToInt32(this string txt, int defaultVal = 0)
        {
            int ResultVal = 0;
            if (string.IsNullOrEmpty(txt) || !int.TryParse(txt, out ResultVal))
            { return defaultVal; }
            else
            { return ResultVal; }
        }

        /// <summary>
        /// 数字使用成汉语表示(只考虑整百,整千,整万)
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string ToChineseNumber(this decimal d)
        {
            double NumPart = 0;
            string TextPart = "";
            if (d >= 10000)
            {
                int Units = 10000;
                NumPart = Convert.ToDouble(d / Units); //Convert.ToDouble(((d - d % Units) / Units));
                TextPart = "万";
            }
            else if (d >= 1000)
            {
                int Units = 1000;
                NumPart = Convert.ToInt32(((d - d % Units) / Units));
                TextPart = "千";
            }
            else if (d >= 100)
            {
                int Units = 100;
                NumPart = Convert.ToInt32(((d - d % Units) / Units));
                TextPart = "百";
            }
            else if (d >= 10)
            {
                int Units = 10;
                NumPart = Convert.ToInt32(((d - d % Units) / Units));
                TextPart = "十";
            }
            else if (d > 0)
            {
                NumPart = Convert.ToDouble(d);
                TextPart = "元";
            }
            return NumPart.ToString() + TextPart;
        }
    }
}