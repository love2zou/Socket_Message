using System;

namespace GA.SuperSocket.Service.Model
{
    /// <summary>
    /// 基类实体
    /// </summary>
    [Serializable]
    public class BaseModel
    {
        public BaseModel()
        {
            this.ResultFlag = (int)ResultFlags.OK;
            this.ResultMsg = string.Empty;
        }

        /// <summary>
        ///
        /// </summary>
        public int ResultFlag { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string ResultMsg { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public enum ResultFlags : int
    {
        /// <summary>
        /// 成功
        /// </summary>
        OK = 100,

        /// <summary>
        /// 不成功
        /// </summary>
        NotOK = 101,

        /// <summary>
        /// 异常
        /// </summary>
        Exception = 102,

        /// <summary>
        /// 接受成功但没发现数据
        /// </summary>
        NotFound = 103,
    }
}