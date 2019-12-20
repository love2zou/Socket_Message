using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace GA.SuperSocket.AppClient.Utility
{
   public class CookieHelper
    {
        /// <summary>
        /// 通过COM来获取Cookie数据。
        /// </summary>
        /// <param name="url">当前网址。</param>
        /// <param name="cookieName">CookieName.</param>
        /// <param name="cookieData">用于保存Cookie Data的<see cref="StringBuilder"/>实例。</param>
        /// <param name="size">Cookie大小。</param>
        /// <returns>如果成功则返回<c>true</c>,否则返回<c>false</c>。</returns>
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(
          string url, string cookieName,
          StringBuilder cookieData, ref int size);

        /// <summary>
        /// 获取当前<see cref="Uri"/>的<see cref="CookieContainer"/>实例。
        /// </summary>
        /// <param name="uri">当前<see cref="Uri"/>地址。</param>
        /// <returns>当前<see cref="Uri"/>的<see cref="CookieContainer"/>实例。</returns>
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;

            // 定义Cookie数据的大小。
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookie(uri.ToString(), null, cookieData,
              ref datasize))
            {
                if (datasize < 0)
                    return null;

                // 确信有足够大的空间来容纳Cookie数据。
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), null, cookieData,
                  ref datasize))
                    return null;
            }


            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }
    }
}
