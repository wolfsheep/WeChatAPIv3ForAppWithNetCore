namespace AspNetCore.WeChatPayAPIv3.Models.ClostOrder
{
    public class ClostOrderRespModel
    {
        /// <summary>
        /// 错误的编码
        /// 参考地址：https://pay.weixin.qq.com/wiki/doc/apiv3/Share/error_code.shtml
        /// </summary>
        public string code { set; get; }

        /// <summary>
        /// 具体的错误信息和原因
        /// </summary>
        public string message { set; get; }

        /// <summary>
        /// http状态码
        /// NoContent  关闭成功
        /// BadRequest 关闭失败
        /// </summary>
        public string StatusCode { set; get; }

        /// <summary>
        /// 订单是否关闭成功
        /// </summary>
        public bool Success => StatusCode == "NoContent" && string.IsNullOrEmpty(code);
    }
}
