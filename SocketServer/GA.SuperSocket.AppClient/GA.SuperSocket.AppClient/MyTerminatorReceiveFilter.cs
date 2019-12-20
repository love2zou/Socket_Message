using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GA.SuperSocket.AppClient
{
    public class MyTerminatorReceiveFilter : TerminatorReceiveFilter<StringPackageInfo>
    {
        public MyTerminatorReceiveFilter()
            : base(Encoding.UTF8.GetBytes("\r\n"))
        {

        }
        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            string cmdFullText = string.Empty;
            string key = string.Empty;
            string body = string.Empty;
            string[] parameters = null;
            cmdFullText = bufferStream.ReadString((int)bufferStream.Length, Encoding.UTF8);
            //1.不间断空格\u00A0,主要用在office中,让一个单词在结尾处不会换行显示,快捷键ctrl+shift+space ;
            //2.半角空格(英文符号)\u0020,代码中常用的;
            //3.全角空格(中文符号)\u3000,中文文章中使用;
            key = Regex.Split(cmdFullText, "\u0020")[0];
            body = string.Join("", cmdFullText.ToArray().Skip(key.ToArray().Length + 1).ToList());
            return new StringPackageInfo(key, body, parameters);
        }
    }
}
