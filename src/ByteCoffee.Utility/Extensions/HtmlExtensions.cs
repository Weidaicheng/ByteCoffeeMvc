using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ByteCoffee.Utility.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// 截取字符长度
        /// </summary>
        /// <param name="inputString">字符</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string CutHtmlString(this string inputString, int len)
        {
            if (string.IsNullOrEmpty(inputString))
                return "";
            inputString = RemoveHtmlTags(inputString);
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

        /// <summary>
        /// Removes the html tags.
        /// </summary>
        /// <param name="Htmlstring">The htmlstring.</param>
        /// <returns></returns>
        public static string RemoveHtmlTags(this string Htmlstring)
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

        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把TXT代码转换成HTML格式
        public static String ToHtml(this string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&", "&amp;");
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            sb.Replace("\r\n", "<br />");
            sb.Replace("\n", "<br />");
            sb.Replace("\t", " ");
            //sb.Replace(" ", "&nbsp;");
            return sb.ToString();
        }

        /// <summary>
        /// 字符串字符处理
        /// </summary>
        /// <param name="chr">等待处理的字符串</param>
        /// <returns>处理后的字符串</returns>
        /// //把HTML代码转换成TXT格式
        public static String ToTxt(this string Input)
        {
            StringBuilder sb = new StringBuilder(Input);
            sb.Replace("&nbsp;", " ");
            sb.Replace("<br>", "\r\n");
            sb.Replace("<br>", "\n");
            sb.Replace("<br />", "\n");
            sb.Replace("<br />", "\r\n");
            sb.Replace("&lt;", "<");
            sb.Replace("&gt;", ">");
            sb.Replace("&amp;", "&");
            return sb.ToString();
        }

        /// <summary>
        /// Html标签转义表示(显示标签,而不是表达标签)
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string ToHtmlTxt(string Input)
        {
            if (Input != string.Empty && Input != null)
            {
                string ihtml = Input.ToLower();
                ihtml = ihtml.Replace("<script", "&lt;script");
                ihtml = ihtml.Replace("script>", "script&gt;");
                ihtml = ihtml.Replace("<%", "&lt;%");
                ihtml = ihtml.Replace("%>", "%&gt;");
                ihtml = ihtml.Replace("<$", "&lt;$");
                ihtml = ihtml.Replace("$>", "$&gt;");
                return ihtml;
            }
            else
            { return string.Empty; }
        }

        /// <summary>
        /// 获取html中图片地址
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetImgUrl(this string html)
        {
            string regstr = @"<IMG[^>]+src=s*(?:'(?<src>[^']+)'|""(?<src>[^""]+)""|(?<src>[^>s]+))s*[^>]*>";
            string keyname = "src";
            ArrayList resultStr = new ArrayList();
            Regex r = new Regex(regstr, RegexOptions.IgnoreCase);
            MatchCollection mc = r.Matches(html);
            foreach (Match m in mc)
            {
                resultStr.Add(m.Groups[keyname].Value.ToLower());
            }

            if (resultStr.Count > 0)
            {
                return resultStr[0].ToString();
            }
            else
            {
                //没有地址的时候返回空字符
                //resultStr.Add("");
                return "";
            }
        }

        /// <summary>
        /// Gets the ut f8 URL encode.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string GetUTF8UrlEncode(this string text)
        { return string.IsNullOrEmpty(text) ? "" : HttpUtility.UrlEncode(text.Trim(), Encoding.UTF8); }

        /// <summary>
        /// Gets the utf8 URL decode.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string GetUTF8UrlDecode(this string text)
        { return string.IsNullOrEmpty(text) ? "" : HttpUtility.UrlDecode(text.Trim(), Encoding.UTF8); }
    }
}