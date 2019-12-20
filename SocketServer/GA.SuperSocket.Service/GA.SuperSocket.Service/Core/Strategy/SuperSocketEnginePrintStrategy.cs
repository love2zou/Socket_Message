using GA.SuperSocket.Service.Model;
using GA.SuperSocket.Service.Utility;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace GA.SuperSocket.Service.Core.Strategy
{
    public class SuperSocketEnginePrintStrategy : IFastPrintStrategy
    {
        /// <summary>
        ///  消息队列实例化对象
        /// </summary>
        private static IQueueMessageStrategy redisQueueMessageStrategy = new RedisQueueMessageStrategy();
        private static readonly MyAppServer tcpServerEngine = null;
        //管理在线客户端，即当用户登录后进入此socket会话集合
        private static ConcurrentDictionary<string, MyAppSession> OnlineAppSessionDictionary = new ConcurrentDictionary<string, MyAppSession>();

        static SuperSocketEnginePrintStrategy()
        {
            #region 自定义服务配置并启动服务
            IServerConfig serverConfig = new ServerConfig
            {
                Name = "GA.SuperSocket.Service.Core.Strategy.MyAppServer",//服务器实例的名称
                Ip = "Any",//服务器监听的ip地址。你可以设置具体的地址，也可以设置为下面的值 Any - 所有的IPv4地址
                Mode = SocketMode.Tcp,//Socket服务器运行的模式, Tcp (默认) 或者 Udp;
                Port = int.Parse(ConfigurationManager.AppSettings["MyAppServerpPort"]),//服务器监听的端口;
                SendingQueueSize = 5000,//发送队列最大长度, 默认值为5;
                MaxConnectionNumber = 5000,//可允许连接的最大连接数;
                LogCommand = false,//是否记录命令执行的记录;
                LogBasicSessionActivity = false,// 是否记录session的基本活动，如连接和断开;
                LogAllSocketException = false,// 是否记录所有socket异常活动;
                MaxRequestLength = 5000,//最大允许的请求长度，默认值为1024;
                TextEncoding = "UTF-8",//文本的默认编码，默认值是 ASCII;
                KeepAliveTime = 60,//网络连接正常情况下的keep alive数据的发送间隔, 默认值为 600, 单位为秒;
                KeepAliveInterval = 60,// Keep alive失败之后, keep alive探测包的发送间隔，默认值为 60, 单位为秒;
                ClearIdleSession = false,//true 或 false, 是否定时清空空闲会话，默认值是 false;
                ClearIdleSessionInterval = 60,//清空空闲会话的时间间隔, 默认值是120, 单位为秒;
                SyncSend = true,//是否启用同步发送模式, 默认值: false;
            };
            var rootConfig = new RootConfig()
            {
                MaxWorkingThreads = 5000,//线程池最大工作线程数量;
                MinWorkingThreads = 10,//线程池最小工作线程数量;
                MaxCompletionPortThreads = 5000,//线程池最大完成端口线程数量;
                MinCompletionPortThreads = 10,//线程池最小完成端口线程数量;
                DisablePerformanceDataCollector = true,//是否禁用性能数据采集;
                PerformanceDataCollectInterval = 60,//性能数据采集频率 (单位为秒, 默认值: 60);
                LogFactory = "ConsoleLogFactory",//默认logFactory的名字, 所有可用的 log factories定义在子节点
                Isolation = IsolationMode.AppDomain
            };
            tcpServerEngine = new MyAppServer();
            if (tcpServerEngine.Setup(rootConfig: rootConfig, config: serverConfig))
            {
                if (tcpServerEngine.Start())
                {
                    NLogHelper.Instance.Info("SuperSocket Start Succeed to initialize!");
                    //会话连接
                    tcpServerEngine.NewSessionConnected += tcpServerEngine_NewSessionConnected;
                    //数据接受
                    tcpServerEngine.NewRequestReceived += tcpServerEngine_NewRequestReceived;
                    //会话关闭
                    tcpServerEngine.SessionClosed += tcpServerEngine_SessionClosed;
                }
                else
                {
                    NLogHelper.Instance.Info("SuperSocket Failed to initialize!");
                }
            }
            else
            {
                NLogHelper.Instance.Info("SuperSocketEngine Failed to setup!");
            }
            #endregion

            #region 服务端自动向客户端发送心跳检测
            ThreadPool.QueueUserWorkItem((s) =>
            {
                while (true)
                {
                    try
                    {
                        OnlineAppSessionDictionary.OrderBy(t => t.Key).ToList().ForEach(t =>
                        {
                            string msg = string.Format("heartbeat {0}", "ok" + Environment.NewLine);//一定要加上分隔符
                            byte[] bMsg = Encoding.UTF8.GetBytes(msg);//消息使用UTF-8编码
                            t.Value.Send(new ArraySegment<byte>(bMsg, 0, bMsg.Length));
                            NLogHelper.Instance.Info(string.Format("服务端正向{0} {1} 客户端发送心跳数据包......", t.Key, t.Value.RemoteEndPoint.ToString()));
                        });
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Instance.Info("服务端自动向客户端发送心跳数据包线程出现异常:{0}", ex.Message);
                    }
                    Thread.Sleep(1000 * 60 * 1);//1分钟发送一次
                }
            });
            #endregion

            #region 服务端自动向客户端发送异常打印队列数据,一旦客户端上线，则将异常队列数据推送给指定的客户端，主要是打印离线数据给指定用户
            ThreadPool.QueueUserWorkItem((s) =>
            {
                while (true)
                {
                    try
                    {
                        //循环在线客户端
                        foreach (var onlineClientSessionDict in OnlineAppSessionDictionary)
                        {
                            MyAppSession clientAppSession = GetAppSessionByUserId(onlineClientSessionDict.Key);//key一般就是用户id，获取到该用户的会话
                            if (clientAppSession == null) continue;//判断是否会话存在
                            if (!clientAppSession.Connected) continue;//判断用户是否在线
                            string queueExceptionName = string.Format(RedisQueueMessageStrategy.SystemQueue_PrefixName, onlineClientSessionDict.Key);
                            var responseResult = redisQueueMessageStrategy.ReceiveMessage(queueExceptionName);//获取用户在redis队列中的消息
                            if (responseResult.RequestStatus == (int)ResultFlags.OK)
                            {
                                string msg = string.Format("push {0}", responseResult.Msg + Environment.NewLine);//一定要加上分隔符
                                byte[] bMsg = System.Text.Encoding.UTF8.GetBytes(msg);//消息使用UTF-8编码
                                clientAppSession.Send(new ArraySegment<byte>(bMsg, 0, bMsg.Length));//发送消息
                                NLogHelper.Instance.Info("服务器成功向客户端userId:{0}的发送异常打印队列数据为:{1}", onlineClientSessionDict.Key, responseResult.Msg);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        NLogHelper.Instance.Info("服务端自动向客户端发送异常打印队列数据线程出现异常:{0}", ex.Message);
                    }
                    Thread.Sleep(1000);
                }
            });
            #endregion
        }

        public ConcurrentDictionary<string, MyAppSession> OnlineAppClients
        {
            get
            {
                return OnlineAppSessionDictionary;
            }
        }

        private static void tcpServerEngine_SessionClosed(MyAppSession session, CloseReason value)
        {
            //用户下线要关闭socket会话
            string msg = string.Format("{0}下线", session.RemoteEndPoint.ToString());
            if (OnlineAppSessionDictionary.Values.Contains(session))
            {
                MyAppSession appsession;
                var appOnlineClient = OnlineAppSessionDictionary.FirstOrDefault(s => s.Value == session);
                OnlineAppSessionDictionary.TryRemove(appOnlineClient.Key, out appsession);
            }
            NLogHelper.Instance.Info(msg);
        }

        private static void tcpServerEngine_NewRequestReceived(MyAppSession session, StringRequestInfo requestInfo)
        {
            try
            {
                switch (requestInfo.Key)
                {
                    case "echo":
                        //this.ShowMessage(session.RemoteEndPoint, requestInfo.Body);
                        break;
                    case "heartbeat":
                        //this.ShowMessage(session.RemoteEndPoint, requestInfo.Body);
                        //string msg = string.Format("heartbeat {0}", requestInfo.Body + Environment.NewLine);//一定要加上分隔符
                        //byte[] bMsg = System.Text.Encoding.UTF8.GetBytes(msg);//消息使用UTF-8编码
                        //session.Send(new ArraySegment<byte>(bMsg, 0, bMsg.Length));
                        //break;
                        NLogHelper.Instance.Info(string.Format("接受到客户端{0} {1} 发送的心跳数据包......", GetUserIdByAppSession(session), session.RemoteEndPoint.ToString()));
                        break;
                    case "ClientUniqueID":
                        string userId = requestInfo.Body;
                        if (!string.IsNullOrEmpty(userId))
                        {
                            MyAppSession appsession;
                            if (OnlineAppSessionDictionary.ContainsKey(userId)) 
                                OnlineAppSessionDictionary.TryRemove(userId, out appsession);
                            OnlineAppSessionDictionary.TryAdd(userId, session);//创建新的会话连接
                            NLogHelper.Instance.Info("ClientUniqueID:" + userId);
                        }
                        break;
                    default:
                        //this.ShowMessage(session.RemoteEndPoint, "未知的指令（error unknow command）");
                        break;
                }
            }
            catch (Exception ex) { }
        }

        private static void tcpServerEngine_NewSessionConnected(MyAppSession session)
        {
            string msg = string.Format("{0}上线", session.RemoteEndPoint.ToString());
            NLogHelper.Instance.Info(msg);
        }

        /// <summary>
        /// 根据客户端ClientUniqueID寻找指定的AppSession
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private static MyAppSession GetAppSessionByUserId(string userId)
        {
            var onlineAppSession = OnlineAppSessionDictionary.FirstOrDefault(s => s.Key.ToLower() == userId.ToLower());
            if (!string.IsNullOrEmpty(onlineAppSession.Key))
            {
                return onlineAppSession.Value;
            }
            return null;
        }

        private static string GetUserIdByAppSession(MyAppSession appSession)
        {
            var onlineAppSession = OnlineAppSessionDictionary.FirstOrDefault(s => s.Value == appSession);
            if (!string.IsNullOrEmpty(onlineAppSession.Key))
            {
                return onlineAppSession.Key;
            }
            return null;
        }

        public ResponseResult SendMessage(string userId, string data)
        {
            var result = ResponseResult.Default();
            try
            {
                NLogHelper.Instance.Info("开始SendMessage......");
                NLogHelper.Instance.Info("服务器接受客户端userId:{0}的原始数据为:{1}", userId, data);
                if (string.IsNullOrEmpty(userId))
                {
                    result.RequestStatus = (int)ResponseResultStatus.Faild;
                    result.Msg = "userId参数不能为空!";
                    NLogHelper.Instance.Info(result.Msg);
                    return result;
                }
                if (string.IsNullOrEmpty(data))
                {
                    result.RequestStatus = (int)ResponseResultStatus.Faild;
                    result.Msg = "data参数不能为空!";
                    NLogHelper.Instance.Info(result.Msg);
                    return result;
                }
                MyAppSession clientAppSession = GetAppSessionByUserId(userId);
                if (clientAppSession == null)
                {
                    result.RequestStatus = (int)ResponseResultStatus.Faild;
                    //=>1、此种情况是针对服务端重启后，客户端已断开连接。2、服务器ClientUniqueID根据找不到指定的客户端。
                    //result.Msg = "检查到当前客户端与服务器通信失败，请重新启动PDA智能打印客户端应用程序!";
                    result.Msg = string.Format("与{0}通信服务器失败，请联系技术支持!", userId);
                    NLogHelper.Instance.Info(result.Msg);
                    redisQueueMessageStrategy.SendMessage(string.Format(RedisQueueMessageStrategy.SystemQueue_PrefixName, userId), data);
                    return result;
                }
                if (!clientAppSession.Connected)
                {
                    result.RequestStatus = (int)ResponseResultStatus.Faild;
                    //result.Msg = " 检查到当前客户端与服务器通信失败,请确保PDA智能打印客户端应用程序已打开!";
                    result.Msg = string.Format("与{0}通信服务器失败，请联系技术支持!", userId);
                    NLogHelper.Instance.Info(result.Msg);
                    redisQueueMessageStrategy.SendMessage(string.Format(RedisQueueMessageStrategy.SystemQueue_PrefixName, userId), data);
                    return result;
                }
                //string newData = System.Web.HttpUtility.UrlEncode(data, Encoding.Default);
                NLogHelper.Instance.Info("服务器正准备向客户端userId:{0}的原始数据为:{1}", userId, data);
                //=>服务端向客户端推送数据。
                string msg = string.Format("push {0}", data + Environment.NewLine);//一定要加上分隔符
                byte[] bMsg = System.Text.Encoding.UTF8.GetBytes(msg);//消息使用UTF-8编码
                clientAppSession.Send(new ArraySegment<byte>(bMsg, 0, bMsg.Length));
                result = ResponseResult.Success("发送成功");
            }
            catch (Exception ex)
            {
                result.RequestStatus = (int)ResponseResultStatus.Exception;
                result.Msg = ex.Message;
                NLogHelper.Instance.Info("服务器向客户端userId:{0}发送数据出现异常:{1}", userId, ex.Message);
            }
            finally
            {
                NLogHelper.Instance.Info("结束SendMessage......");
                NLogHelper.Instance.Info(Environment.NewLine);
            }
            return result;
        }
    }
}