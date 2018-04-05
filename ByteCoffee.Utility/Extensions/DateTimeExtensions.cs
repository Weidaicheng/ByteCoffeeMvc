// -----------------------------------------------------------------------
//  <copyright file="DateTimeExtensions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2015 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>最后修改人</last-editor>
//  <last-date>2015-05-05 11:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

namespace ByteCoffee.Utility.Extensions
{
    /// <summary>
    /// 时间扩展操作类
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 当前时间是否周末
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns>
        ///   <c>true</c> if the specified date time is weekend; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekend(this DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Saturday, DayOfWeek.Sunday };
            return weeks.Contains(dateTime.DayOfWeek);
        }

        /// <summary>
        /// 当前时间是否工作日
        /// </summary>
        /// <param name="dateTime">时间点</param>
        /// <returns>
        ///   <c>true</c> if the specified date time is weekday; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekday(this DateTime dateTime)
        {
            DayOfWeek[] weeks = { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };
            return weeks.Contains(dateTime.DayOfWeek);
        }

        /// <summary>
        /// 获取时间相对唯一字符串
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="milsec">是否使用毫秒</param>
        /// <returns></returns>
        public static string ToUniqueString(this DateTime dateTime, bool milsec = false)
        {
            int sedonds = dateTime.Hour * 3600 + dateTime.Minute * 60 + dateTime.Second;
            string value = string.Format("{0}{1}{2}", dateTime.ToString("yy"), dateTime.DayOfYear, sedonds);
            return milsec ? value + dateTime.ToString("fff") : value;
        }

        /// <summary>
        /// 判断指定的时间是否在时间段内
        /// </summary>
        /// <param name="targetTime">The target time.</param>
        /// <param name="beginTime">The begin time.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>
        ///   <c>true</c> if [is between date] [the specified begin time]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetweenDate(this DateTime targetTime, DateTime beginTime, DateTime endTime)
        {
            if (DateTime.Compare(beginTime, endTime) >= 0)
            { return false; }
            if (DateTime.Compare(targetTime, beginTime) < 0)
            { return false; }
            if (DateTime.Compare(targetTime, endTime) >= 0)
            { return false; }
            return true;
        }
    }
}