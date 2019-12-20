using GA.SuperSocket.Service.Core;
using GA.SuperSocket.Service.Core.Strategy;
using GA.SuperSocket.Service.Utility;
using System;
using System.Web;

namespace GA.SuperSocket.Service
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //加载redis配置
            AppContext.AppServiceConfig = AppServiceConfigUtility.Load();
            NLogHelper.Instance.Info("GA.SuperSocket.Service服务应用程序正在启动......");
            InitLoadFastPrintStrategy();
        }

        /// <summary>
        /// 站点第一次启动启动预热加载Socket服务
        /// </summary>
        private static void InitLoadFastPrintStrategy()
        {
            var instance = FastPrintStrategy.Instance;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            NLogHelper.Instance.Info("GA.SuperSocket.Service服务应用程序正在关闭......");
        }
    }
}