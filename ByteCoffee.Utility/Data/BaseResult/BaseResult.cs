namespace ByteCoffee.Utility.Data
{
    /// <summary>
    /// 结果基类
    /// </summary>
    /// <typeparam name="TResultType"></typeparam>
    public abstract class BaseResult<TResultType> : BaseResult<TResultType, object>
    {
        /// <summary>
        /// 初始化一个<see cref="BaseResult{TResultType}"/>类型的新实例
        /// </summary>
        protected BaseResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="BaseResult{TResultType}"/>类型的新实例
        /// </summary>
        protected BaseResult(TResultType type)
            : this(type, null, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="BaseResult{TResultType}"/>类型的新实例
        /// </summary>
        protected BaseResult(TResultType type, string message)
            : this(type, message, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="BaseResult{TResultType}"/>类型的新实例
        /// </summary>
        protected BaseResult(TResultType type, string message, object data)
        {
            ResultType = type;
            Message = message;
            Data = data;
        }
    }

    /// <summary>
    /// 结果基类
    /// </summary>
    /// <typeparam name="TResultType">结果类型</typeparam>
    /// <typeparam name="TData">结果数据类型</typeparam>
    public abstract class BaseResult<TResultType, TData> : IBaseResult<TResultType, TData>
    {
        protected string _message;

        /// <summary>
        /// 初始化一个<see cref="BaseResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected BaseResult()
            : this(default(TResultType))
        { }

        /// <summary>
        /// 初始化一个<see cref="BaseResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected BaseResult(TResultType type)
            : this(type, null, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="BaseResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected BaseResult(TResultType type, string message)
            : this(type, message, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="BaseResult{TResultType,TData}"/>类型的新实例
        /// </summary>
        protected BaseResult(TResultType type, string message, TData data)
        {
            ResultType = type;
            _message = message;
            Data = data;
        }

        /// <summary>
        /// 获取或设置 结果类型
        /// </summary>
        public TResultType ResultType { get; set; }

        /// <summary>
        /// 获取或设置 返回消息
        /// </summary>
        public virtual string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 获取或设置 结果数据
        /// </summary>
        public TData Data { get; set; }
    }
}