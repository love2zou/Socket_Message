using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace GA.SuperSocket.Service.Model
{
    /// <summary>
    /// 统一响应结果类
    /// </summary>
    public class ResponseResult
    {
        public ResponseResult()
        {

        }

        /// <summary>
        /// 状态
        /// </summary>
        public int RequestStatus
        {
            get;
            set;
        }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg
        {
            get;
            set;
        }

        public static ResponseResult Default()
        {
            var result = new ResponseResult();
            result.RequestStatus = (int)ResponseResultStatus.Default;
            result.Msg = "";
            return result;
        }

        public static ResponseResult Success(string message = null)
        {
            var result = new ResponseResult();
            result.RequestStatus = (int)ResponseResultStatus.Succeed;
            result.Msg = message;
            return result;
        }

        public static ResponseResult Exception(string message)
        {
            var result = new ResponseResult();
            result.RequestStatus = (int)ResponseResultStatus.Exception;
            result.Msg = message;
            return result;
        }

        public static ResponseResult Faild(string message)
        {
            var result = new ResponseResult();
            result.RequestStatus = (int)ResponseResultStatus.Faild;
            result.Msg = message;
            return result;
        }

        public static ResponseResult NotAuthorization(string message)
        {
            var result = new ResponseResult();
            result.RequestStatus = (int)ResponseResultStatus.NotAuthorization;
            result.Msg = message;
            return result;
        }

    }

    public class ResponseResult<T> : ResponseResult
        where T : class, new()
    {
        public ResponseResult()
        {
            this.Data = new T();
        }

        public T Data
        {
            get;
            set;
        }

        public static ResponseResult<T> Default()
        {
            var result = new ResponseResult<T>();
            result.Data = null;
            result.RequestStatus = (int)ResponseResultStatus.Default;
            result.Msg = "";
            return result;
        }

        public static ResponseResult<T> Success(T t, string message = null)
        {
            var result = new ResponseResult<T>();
            result.Data = t;
            result.RequestStatus = (int)ResponseResultStatus.Succeed;
            result.Msg = message;
            return result;
        }

        public static ResponseResult<T> Exception(string message)
        {
            var result = new ResponseResult<T>();
            result.Data = null;
            result.RequestStatus = (int)ResponseResultStatus.Exception;
            result.Msg = message;
            return result;
        }

        public static ResponseResult<T> Faild(string message)
        {
            var result = new ResponseResult<T>();
            result.Data = null;
            result.RequestStatus = (int)ResponseResultStatus.Faild;
            result.Msg = message;
            return result;
        }

        public static ResponseResult<T> NotAuthorization(string message)
        {
            var result = new ResponseResult<T>();
            result.Data = null;
            result.RequestStatus = (int)ResponseResultStatus.NotAuthorization;
            result.Msg = message;
            return result;
        }
    }

    public enum ResponseResultStatus
    {
        //默认值
        Default = 0,
        //成功
        Succeed = 100,
        //失败
        Faild = 101,
        //异常
        Exception = 102,
        //验证失败
        NotAuthorization = 403
    }
}