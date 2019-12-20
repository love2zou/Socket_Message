
namespace GA.SuperSocket.Service.Core
{
    /// <summary>
    /// 应用程序上下文
    /// </summary>
    public class AppContext
    {
        /// <summary>
        ///  app服务配置
        /// </summary>
        static AppContext()
        {
            AppServiceConfig = new AppServiceConfig();
        }
        public static AppServiceConfig AppServiceConfig { get; set; }
    }
}