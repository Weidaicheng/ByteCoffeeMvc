using ByteCoffee.Utility.Data;
using System;
using System.Web;
using System.Web.Caching;

namespace ByteCoffee.Utility.Cache
{
    public class SimpleCacheHelper
    {
        /// <summary>
        /// Gets the cache.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="Key">The key.</param>
        /// <returns></returns>
        public static OperationResult<TData> GetCache<TData>(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            { return new OperationResult<TData>(OperationResultType.Error, "Key值不能为空"); }

            if (HttpRuntime.Cache[Key] == null)
            { return new OperationResult<TData>(OperationResultType.Error, "缓存为空"); }

            TData CacheData = (TData)HttpRuntime.Cache.Get(Key);
            return new OperationResult<TData>(OperationResultType.Success, "获取缓存成功", CacheData);
        }

        /// <summary>
        /// Sets the cache.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="Key">The key.</param>
        /// <param name="CacheExpiration">缓存时长（单位：秒）</param>
        /// <param name="CacheData">The data.</param>
        /// <returns></returns>
        public static OperationResult InsertCache<TData>(string Key, int CacheExpiration, TData CacheData)
        {
            if (string.IsNullOrEmpty(Key))
            { return new OperationResult(OperationResultType.Error, "Key值不能为空"); }

            if (CacheExpiration <= 0)
            { return new OperationResult(OperationResultType.Error, "缓存时长设置错误"); }

            HttpRuntime.Cache.Insert(Key, CacheData, null, DateTime.Now.AddSeconds(CacheExpiration), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            return new OperationResult(OperationResultType.Success, "缓存设置成功");
        }
    }
}