using System;
using System.IO;
using System.Linq;
using System.Web;

namespace ByteCoffee.Utility.Helpers
{
    #region 文件上传的状态

    /// <summary>
    /// 文件上传的状态
    /// </summary>
    public enum UploadFileStatus
    {
        /// <summary>
        /// 上传成功
        /// </summary>
        Succeed,

        /// <summary>
        /// 上传文件为空
        /// </summary>
        NullFile,

        /// <summary>
        /// 后缀名错误
        /// </summary>
        ExtensionError,

        /// <summary>
        /// 上传文件长度错误
        /// </summary>
        FileSizeError,

        /// <summary>
        /// 其他错误
        /// </summary>
        OtherError
    }

    #endregion 文件上传的状态

    public class UploadFileHelper
    {
        #region 上传文件并返回文件名

        ///// <summary>
        ///// 上传文件并返回文件名
        ///// </summary>
        ///// <param name="hifile">HtmlInputFile控件</param>
        ///// <param name="strAbsolutePath">绝对路径.如:Server.MapPath(@"Image/upload")与Server.MapPath(@"Image/upload/")均可,用\符号亦可</param>
        ///// <returns>返回的文件名即上传后的文件名</returns>
        //public static string SaveFile(HttpPostedFileBase file, string strAbsolutePath)
        //{
        //    string strOldFilePath = "", strExtension = "", strNewFileName = "";
        //    strOldFilePath = Path.GetFileName(file.FileName);
        //    //取得上传文件的扩展名
        //    strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf(".")).ToLower();
        //    //文件上传后的命名
        //    strNewFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + strExtension;
        //    //如果路径末尾为\符号，则直接上传文件
        //    var physicalPath = string.Empty;
        //    if (strAbsolutePath.LastIndexOf("\\") == strAbsolutePath.Length)
        //    { physicalPath = Path.Combine(strAbsolutePath, strNewFileName); }
        //    else
        //    { physicalPath = strAbsolutePath + "\\" + strNewFileName; }
        //    file.SaveAs(physicalPath);
        //    return strNewFileName;
        //}

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="UploadFile">The file.</param>
        /// <param name="SavePhysicalPathFolder">保存文件的物理路径(不含文件名)</param>
        /// <param name="AllowExtension">允许的文件后缀名</param>
        /// <param name="MaxLength">最大长度(单位:KB)</param>
        /// <param name="MiniLength">最小长度(单位:KB)</param>
        /// <param name="SaveFileName">返回保存后的文件名(含extension)</param>
        /// <returns>
        /// 上传成功(true);上传失败(false)
        /// </returns>
        public static UploadFileStatus SaveFile(HttpPostedFileBase UploadFile,
                                    string SavePhysicalPathFolder,
                                    string[] AllowExtension,
                                    int MaxLength,
                                    int MiniLength,
                                    out string SaveFileName)
        {
            SaveFileName = string.Empty;
            if (UploadFile != null && UploadFile.ContentLength > 0)
            {
                string SourceFileName = UploadFile.FileName;
                string SourceExtension = string.Empty;
                if (SourceFileName.Contains("."))
                { SourceExtension = SourceFileName.Substring(SourceFileName.LastIndexOf(".")).ToLower(); }

                bool IsAllowExtension = false;
                for (int i = 0; i < AllowExtension.Length; i++)
                {
                    if (string.Compare(SourceExtension, AllowExtension[i], true) == 0)
                    { IsAllowExtension = true; }
                }

                bool IsPassSizeCheck = false;
                if (UploadFile.ContentLength >= MiniLength * 1024 && UploadFile.ContentLength <= MaxLength * 1024)
                { IsPassSizeCheck = true; }

                if (IsAllowExtension)
                {
                    if (IsPassSizeCheck)
                    {
                        SaveFileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + SourceExtension;
                        string SavePath = SavePhysicalPathFolder + SaveFileName;
                        try
                        {
                            UploadFile.SaveAs(SavePath);
                            return UploadFileStatus.Succeed;
                        }
                        catch { return UploadFileStatus.OtherError; }
                    }
                    else { return UploadFileStatus.FileSizeError; }
                }
                else { return UploadFileStatus.ExtensionError; }
            }
            else { return UploadFileStatus.NullFile; }
        }

        /// <summary>
        /// check the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="allowExtension">允许的文件后缀名</param>
        /// <param name="maxLength">最大长度(单位:KB)</param>
        /// <param name="miniLength">最小长度(单位:KB)</param>
        /// <param name="saveFileName">返回保存后的文件名(含extension)</param>
        /// <returns>上传成功(true);上传失败(false)</returns>
        public static UploadFileStatus FileToCheck(HttpPostedFileBase file,
            string[] allowExtension, int maxLength, int miniLength, out string saveFileName)
        {
            saveFileName = string.Empty;
            if (file == null || file.ContentLength <= 0)
            { return UploadFileStatus.NullFile; }
            var ex = Path.GetExtension(file.FileName);
            var isAllowExtension = allowExtension.Any(e => string.Compare(ex, e, true) == 0);
            var isPassSizeCheck = file.ContentLength >= miniLength * 1024 && file.ContentLength <= maxLength * 1024;

            if (!isAllowExtension)
            { return UploadFileStatus.ExtensionError; }
            if (!isPassSizeCheck)
            { return UploadFileStatus.FileSizeError; }

            saveFileName = Guid.NewGuid().ToString("N") + ex;
            return UploadFileStatus.Succeed;
        }

        /// <summary>
        /// check the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="savePhysicalPathFolder"> 保存文件的物理路径(不含文件名)</param>
        /// <param name="allowExtension">允许的文件后缀名</param>
        /// <param name="maxLength">最大长度(单位:KB)</param>
        /// <param name="miniLength">最小长度(单位:KB)</param>
        /// <param name="saveFileName">返回保存后的文件名(含extension)</param>
        /// <returns>上传成功(true);上传失败(false)</returns>
        public static UploadFileStatus FileToCheck(HttpPostedFileBase file, string savePhysicalPathFolder,
            string[] allowExtension, int maxLength, int miniLength, out string saveFileName)
        {
            saveFileName = string.Empty;
            if (file == null || file.ContentLength <= 0)
            { return UploadFileStatus.NullFile; }
            var ex = Path.GetExtension(file.FileName);
            var isAllowExtension = allowExtension.Any(e => string.Compare(ex, e, true) == 0);
            var isPassSizeCheck = file.ContentLength >= miniLength * 1024 && file.ContentLength <= maxLength * 1024;

            if (!isAllowExtension)
            { return UploadFileStatus.ExtensionError; }
            if (!isPassSizeCheck)
            { return UploadFileStatus.FileSizeError; }

            saveFileName = Guid.NewGuid().ToString("N") + ex;
            var savePath = savePhysicalPathFolder + saveFileName;
            try
            {
                file.SaveAs(savePath);
                return UploadFileStatus.Succeed;
            }
            catch { return UploadFileStatus.OtherError; }
        }

        #endregion 上传文件并返回文件名

        #region 上传指定文件名(不含后缀名)的文件,并返回保存后的文件名(含后缀名)

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="UploadFile">The file.</param>
        /// <param name="SavePhysicalPathFolder">保存文件的物理路径(不含文件名)</param>
        /// <param name="SaveFileName">保存的文件名(不含extension)</param>
        /// <param name="AllowExtension">允许的文件后缀名</param>
        /// <param name="MaxLength">最大长度(单位:KB)</param>
        /// <param name="MiniLength">最小长度(单位:KB)</param>
        /// <param name="SaveFullName">返回保存后的文件名(含extension)</param>
        /// <returns>
        /// 上传成功(true);上传失败(false)
        /// </returns>
        public static UploadFileStatus SaveFile(HttpPostedFileBase UploadFile,
                                    string SavePhysicalPathFolder,
                                    string SaveFileName,
                                    string[] AllowExtension,
                                    int MaxLength,
                                    int MiniLength,
                                    out string SaveFullName)
        {
            SaveFullName = string.Empty;
            if (UploadFile != null && UploadFile.ContentLength > 0)
            {
                string SourceFileName = UploadFile.FileName;
                string SourceExtension = string.Empty;
                if (SourceFileName.Contains("."))
                { SourceExtension = SourceFileName.Substring(SourceFileName.LastIndexOf(".")).ToLower(); }

                bool IsAllowExtension = false;
                for (int i = 0; i < AllowExtension.Length; i++)
                {
                    if (string.Compare(SourceExtension, AllowExtension[i], true) == 0)
                    { IsAllowExtension = true; }
                }

                bool IsPassSizeCheck = false;
                if (UploadFile.ContentLength >= MiniLength * 1024 && UploadFile.ContentLength <= MaxLength * 1024)
                { IsPassSizeCheck = true; }

                if (IsAllowExtension)
                {
                    if (IsPassSizeCheck)
                    {
                        SaveFullName = SaveFileName + SourceExtension;
                        string SavePath = SavePhysicalPathFolder + SaveFullName;
                        try
                        {
                            UploadFile.SaveAs(SavePath);
                            return UploadFileStatus.Succeed;
                        }
                        catch { return UploadFileStatus.OtherError; }
                    }
                    else { return UploadFileStatus.FileSizeError; }
                }
                else { return UploadFileStatus.ExtensionError; }
            }
            else { return UploadFileStatus.NullFile; }
        }

        #endregion 上传指定文件名(不含后缀名)的文件,并返回保存后的文件名(含后缀名)
    }
}