using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GA.SuperSocket.MobileApp.Utility;

namespace GA.SuperSocket.MobileApp
{
    public class AppServiceConfigUtility
    {
        private const string AppServiceConfigFileName = "AppServiceConfig.xml";

        public static AppServiceConfig Load()
        {
            string filePath = Path.Combine(Application.StartupPath, AppServiceConfigFileName);
            return XmlUtil.Deserialize(typeof(AppServiceConfig), FileUtility.ReadFile(filePath)) as AppServiceConfig;
        }
    }
}
