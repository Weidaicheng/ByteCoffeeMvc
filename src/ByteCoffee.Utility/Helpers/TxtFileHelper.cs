using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ByteCoffee.Utility.Helpers
{
    public class TxtFileHelper
    {
        #region 记录文本文件

        /// <summary>
        /// Saves the log.
        /// </summary>
        /// <param name="fsname">The fsname.</param>
        /// <param name="content">The content.</param>
        public static void SaveLog(string fsname, string content)
        {
            var reportDirectory = string.Format("~/Reports/{0}/", DateTime.Now.ToString("yyyy-MM"));
            reportDirectory = System.Web.Hosting.HostingEnvironment.MapPath(reportDirectory);
            if (!Directory.Exists(reportDirectory))
            { Directory.CreateDirectory(reportDirectory); }
            var dailyReportFullPath = string.Format("{0}{1}_{2}.log", reportDirectory, fsname, DateTime.Now.ToString("yyyy.MM"));
            var logContent = string.Format("{0}==>>{1}{2}", DateTime.Now, content, Environment.NewLine);
            File.AppendAllText(dailyReportFullPath, logContent);
        }

        /// <summary>
        /// 描述：保存txt文件日志
        /// </summary>
        /// <param name="fsname">保存文件名（无需后缀，已添加日期信息）</param>
        /// <param name="content">内容</param>
        public static void SaveTxtLog(string fsname, string content)
        {
            StreamWriter fs = null;
            try
            {
                var savePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFile");
                savePath = savePath + "\\" + fsname + DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + ".txt";
                fs = new StreamWriter(savePath, false, System.Text.Encoding.UTF8);
                fs.WriteLine(content);
            }
            catch { }
            finally { fs.Close(); }
        }

        /// <summary>
        /// 异常写入文件
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="CustomTags">UserTags</param>
        /// <param name="RequestUrl">发生错误的页面地址</param>
        /// <param name="SavePhyPath">保存信息的物理地址</param>
        public static void SaveErrorLog(Exception ex, string CustomTags, string RequestUrl, string SavePhyPath)
        {
            try
            {
                string filepath = SavePhyPath;
                string TitleText = "[---Begin---]";
                string FootText = "[----End----]";
                FileInfo fileInfo = new FileInfo(filepath);
                if (!fileInfo.Exists)
                {
                    FileStream cfile = fileInfo.Create();
                    cfile.Close();
                }
                using (StreamWriter stream = fileInfo.AppendText())
                {
                    stream.WriteLine(TitleText);
                    stream.WriteLine("时间：" + DateTime.Now);
                    stream.WriteLine("地址：" + RequestUrl);
                    stream.WriteLine("信息：" + ex.Message);
                    stream.WriteLine("对象：" + ex.Source);
                    stream.WriteLine("方法：" + ex.TargetSite);
                    stream.WriteLine("堆栈：");
                    stream.WriteLine(ex.StackTrace);
                    stream.WriteLine(FootText);
                    stream.WriteLine();
                    stream.Close();
                }
            }
            catch { }
        }

        #endregion 记录文本文件

        #region 清除HTML标记

        public static string DropHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return "";
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            // Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;
        }

        #endregion 清除HTML标记

        #region 清除HTML标记且返回相应的长度

        public static string DropHTML(string Htmlstring, int strLen)
        {
            return CutString(DropHTML(Htmlstring), strLen);
        }

        #endregion 清除HTML标记且返回相应的长度

        #region 截取字符长度

        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutString(string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = DropHTML(inputString);
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }

                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            return tempString;
        }

        #endregion 截取字符长度
    }
}