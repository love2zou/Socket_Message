using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GA.SuperSocket.Service.Utility;
using System.Web;
namespace GA.SuperSocket.Service.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class AppServiceConfigUtility
    {
        /// <summary>
        /// 
        /// </summary>
        private const string AppServiceConfig_FileName = "AppServiceConfig.xml";

        /// <summary>
        /// 加载自定义配置文件
        /// </summary>
        /// <returns></returns>
        public static AppServiceConfig Load()
        {
            string fileContent = string.Empty;
            string filePath = string.Empty;         
            //filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,AppServiceConfig_FileName);
            filePath = Path.Combine(HttpRuntime.BinDirectory, AppServiceConfig_FileName);
            AppServiceConfig _ApplicationConfig = new AppServiceConfig();
            //根据文件所在的路径读取配置
            fileContent = FileUtility.ReadFile(filePath);
            //序列化配置文件中的数据
            object obj = XmlUtil.Deserialize(typeof(AppServiceConfig), fileContent);
            _ApplicationConfig = obj as AppServiceConfig;
            return _ApplicationConfig;
        }
    }
}
