using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW.Utils.Models.RefundsCallback
{
    public class RefundsCallbackRespModel
    {
        /// <summary>
        /// 返回状态码,错误码，SUCCESS为清算机构接收成功，其他错误码为失败。
        /// </summary>
        public string code { set; get; } = "SUCCESS";

        /// <summary>
        /// 返回信息，如非空，为错误原因。
        /// </summary>
        public string message { set; get; } = string.Empty;
    }
}
