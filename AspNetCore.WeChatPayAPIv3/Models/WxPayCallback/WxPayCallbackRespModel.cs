namespace AspNetCore.WeChatPayAPIv3.Models.WxPayCallback
{
    /// <summary>
    /// 返回给微信支付成功回调的参数
    /// </summary>
    public class WxPayCallbackRespModel
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
