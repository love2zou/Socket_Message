using GA.SuperSocket.Service.Model;
using GA.SuperSocket.Service.Utility;
using ServiceStack.Redis;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace GA.SuperSocket.Service.Core.Strategy
{
    public class RedisQueueMessageStrategy : IQueueMessageStrategy
    {
        public const string SystemQueue_PrefixName = "user.msg.{0}";//队列名称前缀

        /// <summary>
        /// 给某个队列发送消息
        /// </summary>
        /// <param name="queueName">队列名称</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public ResponseResult SendMessage(string queueName, string message)
        {
            ResponseResult result = ResponseResult.Default();
            try
            {
                //NLogHelper.Instance.Info("开始SendPrintMessage=>Redis MQ消息队列......");
                if (string.IsNullOrEmpty(queueName))
                {
                    result = ResponseResult.Faild("queueName不能为空!");
                    return result;
                }
                using (IRedisClient redisClient = RedisManager.GetClient())
                {
                    redisClient.EnqueueItemOnList(queueName, message);
                    result.RequestStatus = (int)ResultFlags.OK;
                    result.Msg = message;
                    NLogHelper.Instance.Info(string.Format("SendPrintMessage=>Redis MQ消息队列:{0}，消息:{1}......", queueName, message));
                }
            }
            catch (System.Exception ex)
            {
                result.RequestStatus = (int)ResponseResultStatus.Exception;
                result.Msg = ex.Message;
                NLogHelper.Instance.Info(string.Format("SendPrintMessage=>Redis MQ消息队列Exception异常:{0}", ex.Message));
            }
            finally
            {
                //NLogHelper.Instance.Info("结束SendPrintMessage=>Redis MQ消息队列......");
            }
            return result;
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public ResponseResult ReceiveMessage(string queueName)
        {
            ResponseResult result = ResponseResult.Default();
            try
            {
                //NLogHelper.Instance.Info("开始ReceiveMessage=>Redis MQ消息队列......");
                if (string.IsNullOrEmpty(queueName))
                {
                    result = ResponseResult.Faild("queueName不能为空!");
                    return result;
                }

                using (IRedisClient redisClient = RedisManager.GetClient())
                {
                    var count = redisClient.GetListCount(queueName);
                    if (count > 0)
                    {
                        result.Msg = redisClient.DequeueItemFromList(queueName);
                        result.RequestStatus = (int)ResultFlags.OK;
                        NLogHelper.Instance.Info(string.Format("ReceiveMessage=>Redis MQ消息队列:{0}，消息:{1}......", queueName, result.Msg));
                    }
                    else
                    {
                        result.Msg = string.Format("当前队列{0}中没有消息队列", queueName);
                        result.RequestStatus = (int)ResultFlags.NotFound;
                    }
                }

            }
            catch (System.Exception ex)
            {
                result.RequestStatus = (int)ResponseResultStatus.Exception;
                result.Msg = ex.Message;
                NLogHelper.Instance.Info(string.Format("ReceiveMessage=>Redis MQ消息队列Exception异常:{0}", ex.Message));
            }
            finally
            {
                //NLogHelper.Instance.Info("结束ReceiveMessage=>Redis MQ消息队列......");
            }
            return result;
        }
    }

    /// <summary>
    ///Redis Manager类
    /// </summary>
    public static class RedisManager
    {
        private static PooledRedisClientManager prcm;
        static RedisManager()
        {
            CreateManager();
        }

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager()
        {
            prcm = new PooledRedisClientManager(new string[] { AppContext.AppServiceConfig.RedisConnectionString.Split('|')[0] },
                                                new string[] { AppContext.AppServiceConfig.RedisConnectionString.Split('|')[1] },
                                                               new RedisClientManagerConfig
                                                               {
                                                                   MaxWritePoolSize = 5000,//设置最大写的连接数
                                                                   MaxReadPoolSize = 5000,//设置最大读的连接数
                                                                   AutoStart = true,
                                                                   DefaultDb = 1//使用索引为1的DB存储空间
                                                               });
            prcm.IdleTimeOutSecs = 60;//设置连接空闲时长超过60秒后自动清理，一定要设置，不然连接数会一直增加
        }

        //从池中获取Redis客户端实例
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient()
        {
            if (prcm == null)
                CreateManager();
            return prcm.GetClient();
        }
    }
}