namespace AspNetCore.WeChatPayAPIv3.Models.QueryRefunds
{
    public class QueryRefundsOrderRespModel
    {
        /// <summary>
        /// 微信支付退款号。
        /// </summary>
        public string refund_id { set; get; }

        /// <summary>
        /// 商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。
        /// 示例值：1217752501201407033233368018
        /// </summary>
        public string out_refund_no { set; get; }

        /// <summary>
        /// 微信支付交易订单号。
        /// </summary>
        public string transaction_id { set; get; }

        /// <summary>
        /// 商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// 原支付交易对应的商户订单号
        /// </summary>
        public string out_trade_no { set; get; }

        /// <summary>
        /// 枚举值：
        ///  ORIGINAL：原路退款
        ///  BALANCE：退回到余额
        ///  OTHER_BALANCE：原账户异常退到其他余额账户
        ///  OTHER_BANKCARD：原银行卡异常退到其他银行卡
        /// </summary>
        public string channel { set; get; }

        /// <summary>
        /// 取当前退款单的退款入账方，有以下几种情况：
        /// 1）退回银行卡：{银行名称}{卡类型}{ 卡尾号}
        /// 2）退回支付用户零钱：支付用户零钱
        /// 3）退还商户：商户基本账户商户结算银行账户
        /// 4）退回支付用户零钱通：支付用户零钱通。
        /// </summary>
        public string user_received_account { set; get; }

        /// <summary>
        /// 退款成功时间，当退款状态为退款成功时有返回。遵循rfc3339标准格式，格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，HH:mm:ss表示时分秒，TIMEZONE表示时区（+08:00表示东八区时间，领先UTC 8小时，即北京时间）。例如：2015-05-20T13:29:35+08:00表示，北京时间2015年5月20日13点29分35秒
        /// </summary>
        public string success_time { set; get; }

        /// <summary>
        /// 退款受理时间。 遵循rfc3339标准格式，格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，HH:mm:ss表示时分秒，TIMEZONE表示时区（+08:00表示东八区时间，领先UTC 8小时，即北京时间）。例如：2015-05-20T13:29:35+08:00表示，北京时间2015年5月20日13点29分35秒。
        /// </summary>
        public string create_time { set; get; }

        /// <summary>
        /// 退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，可前往商户平台-交易中心，手动处理此笔退款。
        /// 枚举值：
        /// SUCCESS：退款成功
        /// CLOSED：退款关闭
        /// PROCESSING：退款处理中
        /// ABNORMAL：退款异常
        /// </summary>
        public string status { set; get; }

        /// <summary>
        /// 退款所使用资金对应的资金账户类型。 枚举值：
        /// UNSETTLED : 未结算资金
        /// AVAILABLE : 可用余额
        /// UNAVAILABLE : 不可用余额
        /// OPERATION : 运营户
        /// </summary>
        public string funds_account { set; get; }

        /// <summary>
        /// 金额详细信息。
        /// </summary>
        public QueryRefundsRespAmountModel amount { set; get; }
    }

    /// <summary>
    /// 退款订单金额信息。
    /// </summary>
    public class QueryRefundsRespAmountModel
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
        /// 现金支付金额，单位为分，只能为整数。
        /// </summary>
        public int payer_total { set; get; }

        /// <summary>
        /// 退款给用户的金额，不包含所有优惠券金额。
        /// </summary>
        public int payer_refund { set; get; }

        /// <summary>
        /// 应结订单金额=订单金额-免充值代金券金额，应结订单金额<=订单金额，单位为分
        /// </summary>
        public int settlement_refund { set; get; }

        /// <summary>
        /// 优惠退款金额 <=退款金额，退款金额-代金券或立减优惠退款金额为现金，说明详见代金券或立减优惠，单位为分。
        /// </summary>
        public int discount_refund { set; get; }

        /// <summary>
        /// 符合ISO 4217标准的三位字母代码，目前只支持人民币：CNY。
        /// </summary>
        public string currency { set; get; }
    }
}
