using System;

namespace ByteCoffee.Utility.Data
{
    /// <summary>
    /// 数据计算辅助操作类
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// 获取两个坐标的距离
        /// </summary>
        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }

        /// <summary>
        /// 计算四则表达式的结果
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static double FourSpecies(string exp)
        {
            MathExpression expression = new MathExpression(exp);
            return expression.Compute();
        }
    }
}