using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace GA.SuperSocket.MobileApp.Utility
{
    public class XmlSerializeUtility
    {
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

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">序列化类型</typeparam>
        /// <param name="filename">文件路径</param>
        /// <param name="obj">序列化对象</param>
        public static void XmlSerialize<T>(string filename, T obj)
        {
            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                using (var fileStream = new FileStream(filename, FileMode.Create))
                {
                    var formatter = new XmlSerializer(typeof(T));
                    formatter.Serialize(fileStream, obj);
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 从流文件中反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objString"></param>
        /// <returns></returns>
        public static T Deserailize<T>(string objString)
        {
            try
            {
                using (Stream stream = CreateStream(objString))
                {
                    var formatter = new XmlSerializer(typeof(T));
                    T result = (T)formatter.Deserialize(stream);

                    return result;
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(string.Format("反序列化字符串为{0}对象时出现错误。\r\n可能原因:不是有效的XML格式。"
                                                , typeof(T).Name), ex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static Stream CreateStream(string result)
        {
            //=>主要是将unicode编码格式 eg:"\u66fe\u51e1\u9f99" 转换为中文。
            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }
    }
}
