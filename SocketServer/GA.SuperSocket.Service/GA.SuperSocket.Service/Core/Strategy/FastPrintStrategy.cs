using System;
using System.Configuration;
using GA.SuperSocket.Service.Utility;

namespace GA.SuperSocket.Service.Core.Strategy
{
    public class FastPrintStrategy
    {
        private static IFastPrintStrategy _iprintstrategy = null;
        //快速打印策略类申明
        private static readonly string fastPrintStrategyClass =
            ConfigurationManager.AppSettings["FastPrintStrategyAssembly"];

        static FastPrintStrategy()
        {
            try
            {
                string fullName = string.Empty;
                string assemblyName = string.Empty;
                fullName = fastPrintStrategyClass.Split(new char[] { ',' })[0];//命名空间.类型名
                assemblyName = fastPrintStrategyClass.Split(new char[] { ',' })[1];//程序集名称
                //接口实例化
                _iprintstrategy = ReflectionHelper.CreateInstance<IFastPrintStrategy>(fullName, assemblyName);
            }
            catch (Exception ex)
            {
                throw new Exception("创建'策略对象'失败,可能存在的原因:未将'策略程序集'添加到bin目录中");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static IFastPrintStrategy Instance
        {
            get { return _iprintstrategy; }
        }
    }
}