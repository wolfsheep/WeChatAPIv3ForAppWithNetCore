namespace AspNetCore.WeChatPayAPIv3.Models.Refunds
{
    public class RefundsRequestModel
    {
        /// <summary>
        /// 商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// 原支付交易对应的商户订单号
        /// </summary>
        public string out_trade_no { set; get; }

        /// <summary>
        /// 商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。
        /// 示例值：1217752501201407033233368018
        /// </summary>
        public string out_refund_no { set; get; }

        /// <summary>
        /// 若商户传入，会在下发给用户的退款消息中体现退款原因。
        /// 示例值：商品已售完
        /// 非必填
        /// </summary>
        public string reason { set; get; }

        /// <summary>
        /// 异步接收微信支付退款结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。 如果参数中传了notify_url，则商户平台上配置的回调地址将不会生效，优先回调当前传的这个地址。
        /// 示例值：https://weixin.qq.com， 必须是https的网址
        /// </summary>
        public string notify_url { set; get; }

        /// <summary>
        /// 退款金额实体
        /// </summary>
        public RefundsAmountModel amount { set; get; }
    }

    /// <summary>
    /// 退款订单金额信息。
    /// </summary>
    public class RefundsAmountModel
    {
        /// <summary>
        /// 退款金额，币种的最小单位，只能为整数，不能超过原订单支付金额。
        /// </summary>
        public int refund { set; get; }

        /// <summary>
        /// 原支付交易的订单总金额，币种的最小单位，只能为整数。
        /// </summary>
        public int total { set; get; }

        /// <summary>
        /// 符合ISO 4217标准的三位字母代码，目前只支持人民币：CNY。
        /// </summary>
        public string currency { set; get; }
    }
}
