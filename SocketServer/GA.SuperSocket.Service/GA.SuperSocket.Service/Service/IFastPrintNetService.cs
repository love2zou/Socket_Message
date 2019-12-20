using System.Collections.Generic;
using GA.SuperSocket.Service.Model;

namespace GA.SuperSocket.Service
{
    public interface IFastPrintNetService
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        ResponseResult SendMessage(string userId, string data);
        /// <summary>
        /// 获取在线客户端信息
        /// </summary>
        /// <returns></returns>
        List<OnlineClient> GetOnlineClientsInfo();
        /// <summary>
        /// 获取在线客户端总数
        /// </summary>
        /// <returns></returns>
        string GetOnlineClientsTotalCount();
    }
}