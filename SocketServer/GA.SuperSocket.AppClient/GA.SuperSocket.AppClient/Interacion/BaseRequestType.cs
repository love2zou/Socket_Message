using Newtonsoft.Json;

namespace GA.SuperSocket.AppClient.Interacion
{
    public class BaseResponseType
    {
        /// <summary>
        /// 返回状态(100为成功,其余均为失败)
        /// </summary>
        [JsonProperty("status")]
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// 返回信息
        /// </summary>
        [JsonProperty("msg")]
        public string Msg
        {
            get;
            set;
        }
    }
}
