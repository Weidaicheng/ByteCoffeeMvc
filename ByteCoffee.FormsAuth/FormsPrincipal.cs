using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ByteCoffee.FormsAuth
{
    public enum UserTypeEnum : int
    {
        Admin = 10,
        Manager = 20,
        Member = 30
    }

    public enum ClientTypeEnum : int
    {
        PcWeb = 10,
        WxWeb = 20
    }

    public struct RoleGroups
    {
        /// <summary>
        /// 所有用户都允许
        /// </summary>
        public static string AllowedAll
        {
            get
            {
                return string.Format("{0},{1},{2}", UserTypeEnum.Admin.ToString(), UserTypeEnum.Manager.ToString(), UserTypeEnum.Member.ToString());
            }
        }

        /// <summary>
        /// 返回允许的角色(用户类型)
        /// </summary>
        /// <param name="UserTypes"></param>
        /// <returns></returns>
        public static string Allowed(params UserTypeEnum[] UserTypes)
        {
            StringBuilder userTypes = new StringBuilder(100);
            foreach (var item in UserTypes)
            { userTypes.AppendFormat(",{0}", item.ToString()); }

            string allowedUserTypes = userTypes.ToString();

            return string.IsNullOrEmpty(allowedUserTypes) ? string.Empty : allowedUserTypes.Substring(1);
        }
    }

    public class ByteCoffeePrincipal : IPrincipal, IByteCoffeeUser
    {
        #region IByteCoffeeUser

        public string UserId { get; set; }

        public string UserName { get; set; }

        public UserTypeEnum UserType { get; set; }

        #endregion

        #region custom

        public string GroupId { get; set; }

        public ClientTypeEnum ClientType { get; set; }

        #endregion

        public bool IsInRole(string role)
        {
            var roles = role.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            bool result = (from s in roles
                           where string.Compare(s, UserType.ToString(), true) == 0
                           select s).Any();
            return result;
        }

        [ScriptIgnore]
        public IIdentity Identity { get { throw new NotImplementedException(); } }
    }

    public class FormsPrincipal<TUserData> : IPrincipal where TUserData : class, IPrincipal, new()
    {
        /// <summary>
        /// The _identity
        /// </summary>
        private IIdentity _identity;

        /// <summary>
        /// The _user data
        /// </summary>
        private TUserData _userData;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormsPrincipal{TUserData}"/> class.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="userData">The user data.</param>
        /// <exception cref="System.ArgumentNullException">
        /// ticket
        /// or
        /// userData
        /// </exception>
        public FormsPrincipal(FormsAuthenticationTicket ticket, TUserData userData)
        {
            if (ticket == null)
            { throw new ArgumentNullException("ticket"); }

            if (userData == null)
            { throw new ArgumentNullException("userData"); }

            _identity = new FormsIdentity(ticket);
            _userData = userData;
        }

        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <value>
        /// The user data.
        /// </value>
        public TUserData UserData
        { get { return _userData; } }

        /// <summary>
        /// 获取当前用户的标识。
        /// </summary>
        /// <returns>与当前用户关联的 <see cref="T:System.Security.Principal.IIdentity" /> 对象。</returns>
        public IIdentity Identity
        { get { return _identity; } }

        /// <summary>
        /// 尝试从HttpRequest.Cookies中构造一个MyFormsPrincipal对象
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public static FormsPrincipal<TUserData> ParsePrincipal(HttpRequest request)
        {
            if (request == null)
            { throw new ArgumentNullException("request"); }

            HttpCookie cookie = request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            { return null; }

            try
            {
                TUserData userData = null;
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                { userData = (new JavaScriptSerializer()).Deserialize<TUserData>(ticket.UserData); }

                if (ticket != null && userData != null)
                { return new FormsPrincipal<TUserData>(ticket, userData); }
            }
            catch { /* 有异常也不要抛出，防止攻击者试探。 */ }
            return null;
        }

        /// <summary>
        /// 确定当前用户是否属于指定的角色。
        /// </summary>
        /// <param name="role">要检查其成员资格的角色的名称。</param>
        /// <returns>
        /// 如果当前用户是指定角色的成员，则为 true；否则为 false。
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsInRole(string role)
        {
            IPrincipal principal = _userData as IPrincipal;
            if (principal == null)
            { throw new NotImplementedException(); }
            else
            { return principal.IsInRole(role); }
        }

        /// <summary>
        /// 执行用户登录操作
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="userData">与登录名相关的用户信息</param>
        /// <param name="expiration">登录Cookie的过期时间，单位：分钟。</param>
        /// <exception cref="System.ArgumentNullException">loginName
        /// or
        /// userData</exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void SetAuthCookie(string loginName, TUserData userData, TimeSpan timeSpan)
        {
            if (string.IsNullOrEmpty(loginName))
            { throw new ArgumentNullException("loginName"); }

            if (userData == null)
            { throw new ArgumentNullException("userData"); }

            HttpContext context = HttpContext.Current;
            if (context == null)
            { throw new InvalidOperationException(); }

            string data = null;
            if (userData != null)
            { data = (new JavaScriptSerializer()).Serialize(userData); }

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, loginName, DateTime.Now, DateTime.Now.Add(timeSpan), true, data);
            string cookieValue = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            cookie.Expires = DateTime.Now.Add(timeSpan);

            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 执行用户登录操作
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="userData">与登录名相关的用户信息</param>
        /// <param name="expiration">登录Cookie的过期时间，单位：分钟。</param>
        /// <exception cref="System.ArgumentNullException">loginName
        /// or
        /// userData</exception>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static void SetAuthCookie(string loginName, TUserData userData, int expiration)
        {
            if (expiration <= 0)
            { throw new ArgumentNullException("expiration"); }

            SetAuthCookie(loginName, userData, TimeSpan.FromMinutes(expiration));
        }

        /// <summary>
        /// 用户是否登录
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static bool IsLogin(string roles)
        {
            HttpContext context = HttpContext.Current;
            if (context == null) { return false; }
            if (context.User == null) { return false; }
            if (context.User.Identity == null) { return false; }
            if (context.User.Identity.IsAuthenticated)
            {
                if (context.User.IsInRole(roles))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public static TUserData GetUserData()
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            { throw new ArgumentNullException("context"); }

            HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            { return null; }

            try
            {
                TUserData userData = null;
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                { userData = (new JavaScriptSerializer()).Deserialize<TUserData>(ticket.UserData); }
                return userData;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 根据HttpContext对象设置用户标识对象
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.ArgumentNullException">context</exception>
        public static void SetUserInfo(HttpContext context)
        {
            if (context == null)
            { throw new ArgumentNullException("context"); }

            FormsPrincipal<TUserData> user = ParsePrincipal(context.Request);
            if (user != null)
            { context.User = user; }
        }

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    public interface IByteCoffeeUser
    {
        string UserId { get; set; }

        string UserName { get; set; }

        UserTypeEnum UserType { get; set; }
    }
}