using ByteCoffee.Utility.Configs;
using Newtonsoft.Json;
using System;
using System.Web;

namespace ByteCoffee.Utility.Data
{
    #region 用户浏览器个性化参数

    public class BrowserConfig
    {
        /// <summary>
        /// 列表的单页数据条数
        /// </summary>
        public int GlobalPageSize;
    }

    #endregion 用户浏览器个性化参数

    public class UserBrowserConfig
    {
        #region 设置和获取用户配置信息

        /// <summary>
        /// Sets the user configuration.
        /// </summary>
        /// <param name="data">The data.</param>
        public static void SetBrowserConfig(BrowserConfig data)
        {
            if (HttpContext.Current == null)
            { return; }
            string val = JsonConvert.SerializeObject(data);
            HttpCookie cookie = new HttpCookie(AppVar.BrowserConfigCookieName);
            cookie.HttpOnly = false;
            cookie.Value = val;
            cookie.Expires = DateTime.Now.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Gets the user configuration.
        /// </summary>
        /// <returns></returns>
        public static BrowserConfig GetBrowserConfig()
        {
            string cookiename = AppVar.BrowserConfigCookieName;
            if (HttpContext.Current == null
                || HttpContext.Current.Request == null
                || HttpContext.Current.Request.Cookies[cookiename] == null)
            {
                BrowserConfig obj = GetDefaultBrowserConfig();
                SetBrowserConfig(obj);
                return obj;
            }
            try
            {
                string cookieval = HttpContext.Current.Request.Cookies[cookiename].Value;
                return JsonConvert.DeserializeObject<BrowserConfig>(cookieval);
            }
            catch
            {
                BrowserConfig obj = GetDefaultBrowserConfig();
                SetBrowserConfig(obj);
                return obj;
            }
        }

        /// <summary>
        /// Gets the default configuration.
        /// </summary>
        /// <returns></returns>
        public static BrowserConfig GetDefaultBrowserConfig()
        { return new BrowserConfig { GlobalPageSize = 20 }; }

        #endregion 设置和获取用户配置信息
    }
}