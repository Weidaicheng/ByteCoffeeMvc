
using ByteCoffee.Utility.Helpers;
using ByteCoffee.Utility.IO;
using System;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ByteCoffee.TmpBLL
{
    public static class AppVar
    {
        #region 系统参数

        #region 管理员用户区域配置

        /// <summary>
        /// 管理员区域的url匹配
        /// </summary>
        public const string AdminRegexStr = @"^.{0,}admin.{0,}$";//todo 需要移除的配置,需要修改业务逻辑代码

        #endregion 管理员用户区域配置

        #region 验证码变量名

        /// <summary>
        /// 浏览器变量的Cookie Name
        /// </summary>
        public const string BrowserConfigCookieName = "BC_BROWSER_CONFIG";

        /// <summary>
        /// 管理端登录验证码的会话名称
        /// </summary>
        public const string ManagerLoginAuthCodeSessionName = "BC_MANAGER_LOGIN_AUTH_CODE";

        /// <summary>
        /// 用户端登录验证码的会话名称
        /// </summary>
        public const string MemberLoginAuthCodeSessionName = "BC_MEMBER_LOGIN_AUTH_CODE";

        /// <summary>
        /// 保存会员注册验证码的会话名称
        /// </summary>
        public const string MemberRegAuthCodeSessionName = "BC_MEMBER_REG_AUTH_CODE";

        /// <summary>
        /// 保存会员重设登录密码验证码的会话名称
        /// </summary>
        public const string MemberResetLoginPwSessionName = "BC_MEMBER_RESET_LOGIN_PASSWORD";

        #endregion 验证码变量名

        #region 系统配置

        /// <summary>
        /// 全局变量AppVar所属Xml文件名
        /// </summary>
        public const string AppVarXmlFileName = "appvar.config";

        #endregion 系统配置

        #endregion 系统参数

        #region 系统动态配置

        private static readonly string OptionsCacheKey = Guid.NewGuid().ToString();

        public static AppVariables Options
        {
            get
            {
                var options = HttpRuntime.Cache[OptionsCacheKey] as AppVariables;
                if (options == null)
                {
                    //var path = Utils.Utils.MapPath(string.Format("/App_Data/{0}", AppVarXmlFileName));
                    var path = WebFileHelper.GetMapPath(string.Format("/App_Data/{0}", AppVarXmlFileName));
                    options = XmlHelper.XmlDeserializeFromFile<AppVariables>(path, Encoding.UTF8);
                    var dep = new CacheDependency(path);
                    HttpRuntime.Cache.Insert(OptionsCacheKey, options, dep);
                }
                return options;
            }
        }

        #endregion 系统动态配置
    }

    public class AppVariables
    {
        #region 系统设置

        public bool IsPushXml { get; set; }

        /// <summary>
        /// 数据读取缓存周期间隔（单位：秒）
        /// </summary>
        public int CacheExpiration { get; set; }

        /// <summary>
        /// 是否推送站长统计
        /// </summary>
        public bool IsPush { get; set; }

        /// <summary>
        /// 短信配置是否是调试状态
        /// </summary>
        public bool SmsCfgIsDebug { get; set; }

        /// <summary>
        /// 试听提醒短信发送时间的字符串设置
        /// </summary>
        public string JRTCfgOfTrialAlert { get; set; }

        /// <summary>
        /// 生日提醒短信发送时间的字符串设置
        /// </summary>
        public string JRTCfgOfBirthdayAlert { get; set; }

        /// <summary>
        /// 学员状态更新执行时间的字符串设置
        /// </summary>
        public string JRTCfgOfCompleteStateUpdate { get; set; }

        /// <summary>
        /// 开课提醒短信发送时间的字符串设置
        /// </summary>
        public string JRTCfgOfStartClassAlert { get; set; }

        #endregion 系统设置

        #region 站点配置

        public string copyright { get; set; }
        public MyCDATA all_menu { get; set; }
        public MyCDATA all_firlink { get; set; }

        public MyCDATA index_1 { get; set; }
        public MyCDATA index_2 { get; set; }
        public MyCDATA index_3 { get; set; }
        public MyCDATA index_4 { get; set; }
        public MyCDATA index_5 { get; set; }
        public MyCDATA lm_1 { get; set; }
        public MyCDATA lm_2 { get; set; }
        public MyCDATA lm_3 { get; set; }
        public MyCDATA lm_4 { get; set; }
        public MyCDATA lm_5 { get; set; }
        public MyCDATA lm_6 { get; set; }
        public MyCDATA lm_7 { get; set; }
        public MyCDATA lm_8 { get; set; }
        public MyCDATA lm_9 { get; set; }
        public MyCDATA lm_10 { get; set; }

        public string descriptionInit { get; set; }
        public string keywordsInit { get; set; }

        #endregion 站点配置

        #region 移动端

        public MyCDATA m_all_navbar { get; set; }
        public MyCDATA m_all_navlogo { get; set; }
        public MyCDATA m_all_menu { get; set; }
        public MyCDATA m_all_firlink { get; set; }
        public MyCDATA m_index_1 { get; set; }
        public MyCDATA m_index_2 { get; set; }
        public MyCDATA m_index_3 { get; set; }
        public MyCDATA m_index_4 { get; set; }
        public MyCDATA m_index_5 { get; set; }
        public MyCDATA m_index_6 { get; set; }
        public MyCDATA m_index_7 { get; set; }
        public MyCDATA m_index_8 { get; set; }

        public MyCDATA m_lm_1 { get; set; }
        public MyCDATA m_lm_2 { get; set; }
        public MyCDATA m_lm_3 { get; set; }
        public MyCDATA m_lm_4 { get; set; }
        public MyCDATA m_lm_5 { get; set; }
        public MyCDATA m_lm_6 { get; set; }
        public MyCDATA m_lm_7 { get; set; }
        public MyCDATA m_lm_8 { get; set; }
        public MyCDATA m_lm_9 { get; set; }
        public MyCDATA m_lm_10 { get; set; }

        #endregion 移动端
    }

    public class MyCDATA : IXmlSerializable
    {
        public MyCDATA()
        {
        }

        public MyCDATA(string value)
        { this.Value = value; }

        public string Value { get; private set; }

        XmlSchema IXmlSerializable.GetSchema()
        { return null; }

        void IXmlSerializable.ReadXml(XmlReader reader)
        { this.Value = reader.ReadElementContentAsString(); }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        { writer.WriteCData(this.Value); }

        public override string ToString()
        { return this.Value; }

        public static implicit operator MyCDATA(string text)
        { return new MyCDATA(text); }
    }
}