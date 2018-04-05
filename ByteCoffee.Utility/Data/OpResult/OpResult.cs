using ByteCoffee.Utility.Extensions;

namespace ByteCoffee.Utility.Data
{
    /// <summary>
    /// 业务操作结果信息类，对操作结果进行封装
    /// </summary>
    public class OpResult : OpResult<object>
    {
        static OpResult()
        {
            Success = new OpResult(OpResultType.Success);
            NoChanged = new OpResult(OpResultType.NoChanged);
        }

        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult()
            : this(OpResultType.NoChanged)
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType)
            : this(resultType, null, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType, string message)
            : this(resultType, message, null)
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType, string message, object data)
            : base(resultType, message, data)
        { }

        /// <summary>
        /// 获取 成功的操作结果
        /// </summary>
        public static OpResult Success { get; private set; }

        /// <summary>
        /// 获取 未变更的操作结果
        /// </summary>
        public new static OpResult NoChanged { get; private set; }
    }

    /// <summary>
    /// 泛型版本的业务操作结果信息类，对操作结果进行封装
    /// </summary>
    /// <typeparam name="TData">返回数据的类型</typeparam>
    public class OpResult<TData> : BaseResult<OpResultType, TData>
    {
        static OpResult()
        {
            NoChanged = new OpResult<TData>(OpResultType.NoChanged);
        }

        /// <summary>
        /// 初始化一个<see cref="OpResult"/>类型的新实例
        /// </summary>
        public OpResult()
            : this(OpResultType.NoChanged)
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult{TData}"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType)
            : this(resultType, null, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult{TData}"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType, string message)
            : this(resultType, message, default(TData))
        { }

        /// <summary>
        /// 初始化一个<see cref="OpResult{TData}"/>类型的新实例
        /// </summary>
        public OpResult(OpResultType resultType, string message, TData data)
            : base(resultType, message, data)
        { }

        /// <summary>
        /// 获取或设置 返回消息
        /// </summary>
        public override string Message
        {
            get { return _message ?? ResultType.ToDescription(); }
            set { _message = value; }
        }

        /// <summary>
        /// 获取 未变更的操作结果
        /// </summary>
        public static OpResult<TData> NoChanged { get; private set; }

        /// <summary>
        /// 获取 是否成功
        /// </summary>
        public bool Successed
        {
            get { return ResultType == OpResultType.Success; }
        }
    }
}