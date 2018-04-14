using ByteCoffee.Utility.Helpers;
using ByteCoffee.Utility.IO;
using System;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ByteCoffee.Utility.Configs
{
    public static class AppVar
    {
        #region 系统参数

        /// <summary>
        /// 管理员区域的url匹配
        /// </summary>
        public const string LoginAdminRegexStr = @"^.{0,}admin.{0,}$";//todo 需要移除的配置,需要修改业务逻辑代码

        #region 验证码变量名

        /// <summary>
        /// 记录浏览器变量的Cookie Name
        /// </summary>
        public const string BrowserConfigCookieName = "te-bc";

        /// <summary>
        /// 记录语言类型的Cookie Name
        /// </summary>
        public const string LangCookieName = "te-lang";

        /// <summary>
        /// 记录管理端登录验证码的Cookie Name
        /// </summary>
        public const string VCodeCookieName = "te-lvc";

        /// <summary>
        /// 记录会员注册验证码变量名
        /// </summary>
        public const string MemberRegVCodeVarName = "te-rvc";

        /// <summary>
        /// 会员投资验证码变量名
        /// </summary>
        public const string MemberInvestVCodeVarName = "te-ivc";

        /// <summary>
        /// 会员重设登录密码的验证码变量名
        /// </summary>
        public const string MemberResetLoginPwVarName = "te-rlpvc";

        /// <summary>
        /// 会员修改绑定手机验证码变量名
        /// </summary>
        public const string MemberResetBindPhoneVarName = "te-rbpvc";

        /// <summary>
        /// 会员修改邮箱绑定的验证码变量名
        /// </summary>
        public const string MemberResetBindEmailVarName = "te-rbmvc";

        #endregion 验证码变量名

        #region 其他

        /// <summary>
        /// 全局变量AppVar所属Xml文件名
        /// </summary>
        public const string AppVarXmlFileName = "appvar.config";

        public const string Sql_BasContent = "WITH TEMP_BAS AS (select ROW_NUMBER() OVER(PARTITION BY ModuleType ORDER BY IsTop desc,AddOn DESC) AS RowId,ContentID,ModuleType,Title,AddOn,PicPath,CContent FROM Bas_Content WHERE State = 1) SELECT CONVERT(int,RowId) AS RowId,CONVERT(varchar(40),ContentID) AS ID,ModuleType,Title,PicPath,CContent,AddOn AS Time FROM TEMP_BAS WHERE RowId<=8";

        public const string Sql_AuthPermission = "WITH PermIds as (select PermissionID from Sys_User_Role ur inner join Sys_Role_Permission rp on ur.RoleID=rp.RoleID where ur.UserID=@UserID union select PermissionID from Sys_User_Permission where UserID=@UserID),Tree AS(SELECT p.PermissionID,p.FatherPermissionID,p.ModuleUrl as Url,p.ModuleName as Name,p.ShowOrder FROM Sys_Permission p inner join PermIds pid on p.PermissionID=pid.PermissionID WHERE PermissionType!=3 UNION ALL SELECT p.PermissionID,p.FatherPermissionID,p.ModuleUrl as Url,p.ModuleName as Name,p.ShowOrder FROM Sys_Permission p,Tree WHERE Tree.FatherPermissionID = p.PermissionID) SELECT distinct * FROM Tree";

        #endregion 其他

        #endregion 系统参数

        #region 常量设置

        #region 公司相关（含域名、站点名称、客服电话、备案号等）

        /// <summary>
        /// PC端域名
        /// </summary>
        public const string DomianName_WebUI = "http://oa.yicaikongjian.cn";

        /// <summary>
        /// 移动端域名
        /// </summary>
        public const string DomianName_H5 = "http://oa.yicaikongjian.cn";

        /// <summary>
        /// 管理端域名
        /// </summary>
        public const string DomianName_Admin = "http://oa.yicaikongjian.cn";

        /// <summary>
        /// PC端呈现域名
        /// </summary>
        public const string DomianView_WebUI = "oa.yicaikongjian.cn";

        /// <summary>
        /// 站点名称
        /// </summary>
        public const string Site_Name = "艺彩空间学员管理系统";

        /// <summary>
        /// 系统名称
        /// </summary>
        public const string System_Name = "艺彩空间学员管理系统";

        /// <summary>
        /// 公司名称
        /// </summary>
        public const string CorpName = "";

        /// <summary>
        /// 公司地址
        /// </summary>
        public const string CorpAddress = "";

        /// <summary>
        /// 客服电话
        /// </summary>
        public const string ServiceTel = "";

        /// <summary>
        /// 备案号
        /// </summary>
        public const string Site_Record = "";

        #endregion 公司相关（含域名、站点名称、客服电话、备案号等）

        #region OSS基本设置

        /// <summary>
        /// 图片的bucket和endpoint组合
        /// </summary>
        public const string Oss_Path = "http://te-ypw.oss-cn-hangzhou.aliyuncs.com";

        /// <summary>
        /// 缩略图的bucket和endpoint组合
        /// </summary>
        public const string Oss_ImgPath = "http://te-ypw.img-cn-hangzhou.aliyuncs.com";

        /// <summary>
        /// OSS的bucket名称
        /// </summary>
        public const string Oss_BucketName = "te-ypw";

        /// <summary>
        /// 投资项目相关信息
        /// </summary>
        public const string Oss_ObjectKey = "goods/";

        /// <summary>
        /// 图片新闻
        /// </summary>
        public const string Oss_PicObjectKey = "picontent/";

        #endregion OSS基本设置

        #region 分页条数

        /// <summary>
        /// 普通列表型
        /// </summary>
        public const int PageSize_Thum = 10;

        /// <summary>
        /// 图片列表型
        /// </summary>
        public const int PageSize_List = 15;

        #endregion 分页条数

        #region 其他

        /// <summary>
        /// 站长工具推送地址
        /// </summary>
        public const string PostUrl = "http://data.zz.baidu.com/urls?site=www.Uhouse.com&token=*";

        #endregion 其他

        #endregion 常量设置

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

        #region 小票宣传语管理

        /// <summary>
        /// 小票宣传语
        /// </summary>
        public string ReceiptSlogan { get; set; }

        #endregion 小票宣传语管理

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