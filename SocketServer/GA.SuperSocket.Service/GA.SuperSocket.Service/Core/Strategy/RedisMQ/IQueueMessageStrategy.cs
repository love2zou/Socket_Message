using GA.SuperSocket.Service.Model;
using SuperSocket.SocketBase;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GA.SuperSocket.Service.Core.Strategy
{
    public interface IQueueMessageStrategy
    {
        /// <summary>
        /// 给某个队列发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        ResponseResult SendMessage(string queueName, string message);
        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        ResponseResult ReceiveMessage(string queueName);
    }
}