namespace AspNetCore.WeChatPayAPIv3.Models.QueryOrder
{
    public class WxPayStatusRespModel
    {
        /// <summary>
        ///  商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        ///  微信支付系统生成的订单号。
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        ///  交易类型，枚举值：
        ///  JSAPI：公众号支付
        ///  NATIVE：扫码支付
        ///  APP：APP支付
        ///  MICROPAY：付款码支付
        ///  MWEB：H5支付
        ///  FACEPAY：刷脸支付
        /// </summary>
        public string trade_type { get; set; }

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
        /// 附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用
        /// </summary>
        public string attach { set; get; }

        /// <summary>
        /// 支付者信息
        /// </summary>
        public WxPayerModel payer { set; get; }

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
        /// 订单查询是否成功
        /// </summary>
        public bool success_query_order
        {
            get
            {
                if (!string.IsNullOrEmpty(code))
                {
                    return false;
                }
                return true;
            }
        }
    }

    /// <summary>
    /// 支付用户信息实体
    /// </summary>
    public class WxPayerModel
    {
        /// <summary>
        /// 用户在直连商户appid下的唯一标识。
        /// </summary>
        public string openid { get; set; }
    }
}
