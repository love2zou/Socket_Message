
namespace GA.SuperSocket.MobileApp
{
    /// <summary>
    /// 应用程序上下文
    /// </summary>
    public class AppContext
    {
        /// <summary>
        ///
        /// </summary>
        static AppContext()
        {
            AppServiceConfig = new AppServiceConfig();
        }

        /// <summary>
        /// 客户端配置文件
        /// </summary>
        public static AppServiceConfig AppServiceConfig { get; set; }
    }
}