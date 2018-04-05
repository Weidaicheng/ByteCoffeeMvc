namespace ByteCoffee.Utility.Data
{
    #region 消息模板

    public class SMSTemplateData
    {
        /// <summary>
        /// 会员登录名
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string SecurityCode { get; set; }
    }

    public class SMSTemplate
    {
        /// <summary>
        /// 获取发送短信的文本
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="IsHtml"></param>
        /// <returns></returns>
        public static string GetContent(SMSTemplateData data)
        {
            return string.Empty;//客服电话
        }
    }

    #endregion 消息模板
}