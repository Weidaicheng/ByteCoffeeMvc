using System.Collections.Generic;

namespace ByteCoffee.Utility.Data
{
    /// <summary>
    /// UI消息状态
    /// </summary>
    public enum UIMsgStatus
    {
        /// <summary>
        /// 成功 val=1
        /// </summary>
        Succ = 1,

        /// <summary>
        /// 拒绝 val=2
        /// </summary>
        Deny = 2,

        /// <summary>
        /// 失败 val=3
        /// </summary>
        Fail = 3
    }

    /// <summary>
    /// UI消息
    /// </summary>
    public class UIMsg
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UIMsg"/> class.
        /// </summary>
        /// <param name="_Status">消息状态.</param>
        /// <param name="_Msg">消息内容.</param>
        /// <param name="_CustomTag">自定义标记.</param>
        public UIMsg(UIMsgStatus _Status, string _Msg, string _CustomTag = "")
        {
            this.Status = (int)_Status;
            this.Msg = _Msg;
            this.CustomTag = _CustomTag;
        }

        public int Status;
        public string Msg;
        public string CustomTag;
    }

    /// <summary>
    /// 前端UI消息队列处理器
    /// </summary>
    public class UIMsgQueueHandler
    {
        private List<UIMsg> MsgList = new List<UIMsg>();

        /// <summary>
        /// 获取或设置消息队列
        /// </summary>
        /// <value>
        /// 消息队列
        /// </value>
        public List<UIMsg> MsgQueue
        {
            get { return MsgList; }
            set { MsgList = value; }
        }

        /// <summary>
        /// Adds the specified _status.
        /// </summary>
        /// <param name="_status">消息状态.</param>
        /// <param name="_msg">消息内容.</param>
        /// <param name="_customtag">自定义标记.</param>
        public void Add(UIMsgStatus _status, string _msg, string _customtag = "")
        {
            MsgList.Add(new UIMsg(_status, _msg, _customtag));
        }

        /// <summary>
        /// Adds the specified _client MSG.
        /// </summary>
        /// <param name="_clientMsg">消息实例.</param>
        public void Add(UIMsg _clientMsg)
        { MsgList.Add(_clientMsg); }

        /// <summary>
        /// 清空队列中的消息
        /// </summary>
        public void Clear()
        { MsgList.Clear(); }
    }
}