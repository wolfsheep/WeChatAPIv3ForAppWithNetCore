namespace AspNetCore.WeChatPayAPIv3.Models.GenerateOrder
{
    /// <summary>
    /// 微信支付下单实体,都是必填的字段，非必填的字段就没有写
    /// </summary>
    public class GenerateOrderModelForWxPay
    {
        /// <summary>
        /// 直连商户申请的公众号或移动应用appid。
        /// </summary>
        public string appid { set; get; }

        /// <summary>
        /// 直连商户的商户号，由微信支付生成并下发。
        /// </summary>
        public string mchid { set; get; }

        /// <summary>
        ///  商品描述
        /// </summary>
        public string description { set; get; }

        /// <summary>
        /// 商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// </summary>
        public string out_trade_no { set; get; }

        /// <summary>
        /// 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用
        /// </summary>
        public string attach { set; get; }

        /// <summary>
        /// 通知URL必须为直接可访问的URL，不允许携带查询串。格式：URL
        /// </summary>
        public string notify_url { set; get; }

        /// <summary>
        /// 订单金额信息
        /// </summary>
        public WxPayAmountModel amount { set; get; }

    }

    /// <summary>
    /// 微信支付金额实体
    /// </summary>
    public class WxPayAmountModel
    {
        /// <summary>
        /// 订单总金额，单位为分。
        /// </summary>
        public int total { set; get; }

        /// <summary>
        /// 货币类型,CNY：人民币，境内商户号仅支持人民币。
        /// </summary>
        public string currency { set; get; } = "CNY";
    }
}
