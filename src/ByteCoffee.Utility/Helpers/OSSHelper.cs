using Aliyun.OSS;
using Aliyun.OSS.Common;
using ByteCoffee.Utility.Data;
using System;
using System.IO;
using System.Web;

namespace ByteCoffee.Utility.Helpers
{
    /// <summary>
    /// Oss配置
    /// </summary>
    public class OssConfig
    {
        /// <summary>
        /// Gets the access identifier.
        /// </summary>
        /// <value>
        /// The access identifier.
        /// </value>
        public string AccessId { get; private set; }

        /// <summary>
        /// Gets the access key.
        /// </summary>
        /// <value>
        /// The access key.
        /// </value>
        public string AccessKey { get; private set; }

        /// <summary>
        /// Gets the endpoint.
        /// </summary>
        /// <value>
        /// The endpoint.
        /// </value>
        public string Endpoint { get; private set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="IsDebug">if set to <c>true</c> [is debug].</param>
        /// <returns></returns>
        public static OssConfig GetConfig(bool IsDebug)
        {
            var config = new OssConfig();
            if (IsDebug)
            {
                config.AccessId = "kvVXCoEgEwcg53IO";
                config.AccessKey = "YykpGV4qOnUB4IFWym7q5yHzN0cFuE";
                config.Endpoint = "http://oss-cn-hangzhou.aliyuncs.com";
            }
            else
            {
                config.AccessId = "kvVXCoEgEwcg53IO";
                config.AccessKey = "YykpGV4qOnUB4IFWym7q5yHzN0cFuE";
                config.Endpoint = "http://oss-cn-hangzhou.aliyuncs.com";//http://oss-cn-hzjbp-a-internal.aliyuncs.com
            }
            return config;
        }
    }

    /// <summary>
    /// Oss服务
    /// </summary>
    public class OssHelper
    {
        private static readonly OssConfig OssConfig = OssConfig.GetConfig(false);
        private static readonly OssClient OssClient = new OssClient(OssConfig.Endpoint, OssConfig.AccessId, OssConfig.AccessKey);

        /// <summary>
        /// 描述：根据bucket和key创建Object
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="key">OSS存储路径及名称</param>
        /// <param name="file">上传文件</param>
        /// <param name="etag">资源关联的记号</param>
        /// <returns></returns>
        public static OperationResult PutObject(string bucketName, string key, HttpPostedFileBase file, out string etag)
        {
            etag = string.Empty;
            try
            {
                var metadata = new ObjectMetadata { CacheControl = "No-Cache", ContentLength = file.ContentLength, ContentType = file.ContentType };
                etag = OssClient.PutObject(bucketName, key, file.InputStream, metadata).ETag;
                //var accs = OssClient.GetBucketAcl(bucketName);
                //var fileUrl = !accs.Grants.Any() ? OssClient.GeneratePresignedUri(bucketName, key, DateTime.Now.AddMinutes(5)).AbsoluteUri : string.Format("http://{0}.oss-cn-hzjbp-a-internal.aliyuncs.com/{1}", bucketName, key);
                var fileUrl = string.Format("http://{0}.oss-cn-hangzhou.aliyuncs.com/{1}", bucketName, key);
                return new OperationResult(OperationResultType.Success, string.Empty, fileUrl);
            }
            catch (OssException ex)
            { return new OperationResult(OperationResultType.Error, string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId)); }
            catch (Exception ex)
            { return new OperationResult(OperationResultType.Error, string.Format("Failed with error info: {0}", ex.Message)); }
        }

        public static OperationResult PutObject(string bucketName, string key, HttpPostedFileBase file)
        {
            try
            {
                var metadata = new ObjectMetadata { CacheControl = "No-Cache", ContentLength = file.ContentLength, ContentType = file.ContentType };
                OssClient.PutObject(bucketName, key, file.InputStream, metadata);
                //var accs = OssClient.GetBucketAcl(bucketName);
                //var fileUrl = !accs.Grants.Any() ? OssClient.GeneratePresignedUri(bucketName, key, DateTime.Now.AddMinutes(5)).AbsoluteUri : string.Format("http://{0}.oss-cn-hzjbp-a-internal.aliyuncs.com/{1}", bucketName, key);
                var fileUrl = string.Format("http://{0}.oss-cn-hangzhou.aliyuncs.com/{1}", bucketName, key);
                return new OperationResult(OperationResultType.Success, string.Empty, fileUrl);
            }
            catch (OssException ex)
            { return new OperationResult(OperationResultType.Error, string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId)); }
            catch (Exception ex)
            { return new OperationResult(OperationResultType.Error, string.Format("Failed with error info: {0}", ex.Message)); }
        }

        public static OperationResult PutObject(string bucketName, string key, string filePath)
        {
            try
            {
                OssClient.PutObject(bucketName, key, filePath);
                //var accs = OssClient.GetBucketAcl(bucketName);
                //var fileUrl = !accs.Grants.Any() ? OssClient.GeneratePresignedUri(bucketName, key, DateTime.Now.AddMinutes(5)).AbsoluteUri : string.Format("http://{0}.oss-cn-hzjbp-a-internal.aliyuncs.com/{1}", bucketName, key);
                var fileUrl = string.Format("http://{0}.oss-cn-hangzhou.aliyuncs.com/{1}", bucketName, key);
                return new OperationResult(OperationResultType.Success, string.Empty, fileUrl);
            }
            catch (OssException ex)
            { return new OperationResult(OperationResultType.Error, string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}", ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId)); }
            catch (Exception ex)
            { return new OperationResult(OperationResultType.Error, string.Format("Failed with error info: {0}", ex.Message)); }
        }

        /// <summary>
        /// 描述：根据bucket和key读取Object
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="key">OSS存储路径及名称</param>
        /// <param name="fileToDownload">下载路径及文件</param>
        /// <param name="mime">mime类型值</param>
        /// <returns></returns>
        public static OperationResult GetObject(String bucketName, string key, string fileToDownload, out string mime)
        {
            var o = OssClient.GetObject(bucketName, key);
            mime = o.Metadata.ContentType;

            using (var requestStream = o.Content)
            {
                var buf = new byte[1024];
                var fs = File.Open(fileToDownload, FileMode.OpenOrCreate);
                int len;
                while ((len = requestStream.Read(buf, 0, 1024)) != 0)
                { fs.Write(buf, 0, len); }
                fs.Close();
                return new OperationResult(OperationResultType.Success);
            }
        }

        /// <summary>
        /// 描述：根据bucket和key删除Object
        /// </summary>
        /// <param name="bucketName">bucket名称</param>
        /// <param name="key">OSS存储路径及名称</param>
        public static void DeleteObject(string bucketName, string key)
        { OssClient.DeleteObject(bucketName, key); }
    }
}