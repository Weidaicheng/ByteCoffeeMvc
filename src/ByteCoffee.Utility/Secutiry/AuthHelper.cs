namespace ByteCoffee.Utility.Secutiry
{
    public class AuthHelper
    {
        #region 密码加密

        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="_Password">加密前</param>
        /// <returns></returns>
        public static string EncryptedPassword(string _Password)
        {
            return HashHelper.GetMd5(HashHelper.GetMd5(_Password).ToUpper()).ToUpper();
        }

        #endregion 密码加密
    }
}