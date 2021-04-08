namespace AspNetCore.WeChatPayAPIv3.Models.WxPayCallback
{
    /// <summary>
    /// 微信支付回调通知结果解密实体
    /// </summary>
    public class WxPayResourceDecryptModel
    {
        /// <summary>
        /// 直连商户申请的公众号或移动应用appid
        /// </summary>
        public string appid { set; get; }

        /// <summary>
        /// 商户的商户号，由微信支付生成并下发。
        /// </summary>
        public string mchid { set; get; }

        /// <summary>
        /// 商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一。特殊规则：最小字符长度为6
        /// </summary>
        public string out_trade_no { set; get; }

        /// <summary>
        /// 微信支付系统生成的订单号。
        /// </summary>
        public string transaction_id { set; get; }

        /// <summary>
        /// 交易状态，枚举值：
        /// SUCCESS：支付成功
        /// REFUND：转入退款
        /// NOTPAY：未支付
        /// CLOSED：已关闭
        /// REVOKED：已撤销（付款码支付）
        /// USERPAYING：用户支付中（付款码支付）
        /// PAYERROR：支付失败(其他原因，如银行返回失败)
        /// ACCEPT：已接收，等待扣款
        /// </summary>
        public string trade_state { get; set; }

        /// <summary>
        /// 交易状态描述
        /// </summary>
        public string trade_state_desc { get; set; }

        /// <summary>
        /// 支付者信息
        /// </summary>
        public WxPayerResourceDecryptModel payer { set; get; }

    }

    /// <summary>
    /// 支付用户信息实体
    /// </summary>
    public class WxPayerResourceDecryptModel
    {
        /// <summary>
        /// 用户在直连商户appid下的唯一标识。
        /// </summary>
        public string openid { get; set; }
    }
}
