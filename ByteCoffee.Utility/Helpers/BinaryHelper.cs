using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace ByteCoffee.Utility.Helpers
{
    /// <summary>
    /// 序列化辅助操作类
    /// </summary>
    public static class BinaryHelper
    {
        #region 二进制序列化

        /// <summary>
        /// 将数据序列化为二进制数组
        /// </summary>
        public static byte[] ToBinary(object data)
        {
            data.CheckNotNull("data");
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, data);
                ms.Seek(0, 0);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将二进制数组反序列化为强类型数据
        /// </summary>
        public static T FromBinary<T>(byte[] bytes)
        {
            bytes.CheckNotNullOrEmpty("bytes");
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 将数据序列化为二进制数组并写入文件中
        /// </summary>
        public static void ToBinaryFile(object data, string fileName)
        {
            data.CheckNotNull("data");
            fileName.CheckFileExists("fileName");
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }

        /// <summary>
        /// 将指定二进制数据文件还原为强类型数据
        /// </summary>
        public static T FromBinaryFile<T>(string fileName)
        {
            fileName.CheckFileExists("fileName");
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

        #endregion 二进制序列化
    }
}