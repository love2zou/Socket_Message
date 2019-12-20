using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketEngine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//引用包：using SuperSocket.SocketBase; SuperSocket.SocketEngine
namespace GA.SuperSocket.Service.Core.Strategy
{
    /// <summary>
    /// AppServer 代表了监听客户端连接，承载TCP连接的服务器实例。
    /// </summary>
    /// <remarks>
    /// 现在 MyAppSession 将可以用在 MyAppServer 的会话中，也有很多方法可以重载
    /// </remarks>
    public class MyAppServer : AppServer<MyAppSession>
    {
        protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
        {
            // 对配置文件进行相应的修改。
            return base.Setup(rootConfig, config);
        }

        protected override void OnStarted()
        {
            // 服务器启动的逻辑部分
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            // 停止服务器的逻辑部分
            base.OnStopped();
        }
    }
    /// <summary>
    /// AppSession 代表一个和客户端的逻辑连接，基于连接的操作应该定义于在该类之中。
    /// 你可以用该类的实例发送数据到客户端，接收客户端发送的数据或者关闭连接。同时可以保存客户端所关联的数据。
    /// </summary>
    /// <remarks>
    /// 在下面的代码中，当一个新的连接连接上时，服务器端立即向客户端发送欢迎信息。 
    /// 这段代码还重写了其它AppSession的方法用以实现自己的业务逻辑。
    /// </remarks>
    public class MyAppSession : AppSession<MyAppSession>
    {
        // 重载OnSessionStarted函数，赞同于appServer.NewSessionConnected += NewSessionConnected
        protected override void OnSessionStarted()
        {
            // 会话链接成功后的逻辑部分。
            this.Send("Welcome to SuperSocket Telnet Server\r\n");
        }

        protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
        {
            // 收到未知请求的逻辑部分
            this.Send("Unknow request\r\n");
        }

        protected override void HandleException(Exception e)
        {
            // 处理异常的逻辑代码
            this.Send("Application error: {0}\r\n", e.Message);
        }

        protected override void OnSessionClosed(CloseReason reason)
        {
            // 会话关闭后的逻辑代码
            base.OnSessionClosed(reason);
        }
    }
}
