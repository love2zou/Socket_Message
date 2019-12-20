using GA.SuperSocket.Service.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GA.SuperSocket.Service.Core.Strategy
{
    /// <summary>
    /// 快速打印接口定义
    /// </summary>
    public interface IFastPrintStrategy
    {
        /// <summary>
        /// 给指定用户发送消息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        ResponseResult SendMessage(string userId, string data);
        /// <summary>
        /// 所有在线的客户端信息
        /// </summary>
        ConcurrentDictionary<string, MyAppSession> OnlineAppClients { get; }
    }
}