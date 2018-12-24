using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTool.Model
{
    public class ApiResult
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public DoResult Code = 0;
        /// <summary>
        /// 展示给用户的消息
        /// </summary>
        public string Msg = "";
        /// <summary>
        /// 开发者关注错误信息
        /// </summary>
        public Object DebugInfo { get; set; }
    }


    /// <summary>
    /// 通用输出参数
    /// </summary>
    public class BaseDTO : ApiResult
    {

    }

    /// <summary>
    /// 通用输出参数,不允许继承
    /// </summary>
    public sealed class BaseReturnDTO : BaseDTO
    {
        /// <summary>   
        /// 输出参数
        /// </summary>
        public Object Data { get; set; }
    }


    public enum DoResult
    {
        Success,
        Failed,

    }


}
