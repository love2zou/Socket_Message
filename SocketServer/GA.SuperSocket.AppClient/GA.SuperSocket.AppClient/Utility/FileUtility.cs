using System;
using System.IO;
using System.Text;

namespace GA.SuperSocket.AppClient.Utility
{
    public static class FileUtility
    {
        #region 写文件

        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="value">文件内容</param>
        public static bool WriteFile(string path, string value)
        {
            try
            {
                if (File.Exists(path) == false)
                {
                    using (FileStream f = File.Create(path))
                    { }
                }

                using (var streamWriter = new StreamWriter(path, true, Encoding.UTF8))
                {
                    streamWriter.WriteLine(value);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        #region 将内容写入文本文件(如果文件path存在就打开，不存在就新建)
        /// <summary>
        /// 将内容写入文本文件(如果文件path存在就打开，不存在就新建)
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="WriteStr">要写入的内容</param>
        /// <param name="FileModes">写入模式：append 是追加写, CreateNew 是覆盖</param>
        public static void WriteStrToTxtFile(string FilePath, string WriteStr, FileMode FileModes = FileMode.Create)
        {
            FileStream fst = new FileStream(FilePath, FileModes);
            StreamWriter swt = new StreamWriter(fst, System.Text.Encoding.GetEncoding("utf-8"));
            swt.WriteLine(WriteStr);
            swt.Close();
            fst.Close();
        }

        #endregion

        #endregion 写文件

        #region 读文件

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            string result = string.Empty;

            if (File.Exists(path) == false)
            {
                return result;
            }

            try
            {
                using (var streamReader = new StreamReader(path, Encoding.UTF8))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                result = string.Empty;
            }

            return result;
        }

        #endregion 读文件

        #region 删除文件

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        public static bool FileDel(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion 删除文件

        #region 获取指定文件详细属性

        /// <summary>
        /// 获取指定文件详细属性
        /// </summary>
        /// <param name="filePath">文件详细路径</param>
        /// <returns>FileInfo</returns>
        public static FileInfo GetFileAttibe(string filePath)
        {
            try
            {
                return new FileInfo(filePath);
            }
            catch
            {
                return default(FileInfo);
            }
        }

        #endregion 获取指定文件详细属性
    }
}