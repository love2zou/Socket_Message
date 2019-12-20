using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace GA.SuperSocket.AppClient.Utility
{
    public class DESEncrypt
    {
        private const string KEY = "Globalegrow";

        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Encrypt(string text)
        {
            return Encrypt(text, KEY);
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string text, string sKey = "Globalegrow")
        {
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(text);
            des.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        #endregion ========加密========

        #region ========解密========

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Decrypt(string text)
        {
            return Decrypt(text, KEY);
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string text, string sKey = "Globalegrow")
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var des = new DESCryptoServiceProvider();
            int len;
            len = text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion ========解密========
    }
}