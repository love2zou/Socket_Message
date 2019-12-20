using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GA.SuperSocket.Service.Model
{
    /// <summary>
    /// 在线客户端类
    /// </summary>
    public class OnlineClient
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 用户tcp地址
        /// </summary>
        public string UserAddress { get; set; }
    }
}