using System.Configuration;

namespace GA.SuperSocket.AppClient.Utility
{
    public static class ConfigUtility
    {
        private static readonly Configuration config =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>
        /// 在＊.exe.config文件中appSettings配置节增加一对键、值对
        /// </summary>
        /// <param ></param>
        /// <param ></param>
        /// <param name="newKey"></param>
        /// <param name="newValue"></param>
        public static void UpdateConfig(string newKey, string newValue)
        {
            bool isModified = false;

            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == newKey)
                {
                    isModified = true;
                }
            }

            if (isModified)
            {
                config.AppSettings.Settings.Remove(newKey);
            }

            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Value</returns>
        public static string ReadConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 读取配置信息为int类型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int ReadConfigToInt(string key)
        {
            var value = ReadConfig(key);
            int result = 0;
            int.TryParse(value, out result);

            return result;
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Value</returns>
        public static bool ReadConfigToBool(string key)
        {
            return bool.Parse(ConfigurationManager.AppSettings[key]);
        }
    }
}