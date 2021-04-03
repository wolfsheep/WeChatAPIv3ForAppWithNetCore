﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW.Utils.Models.RefundsCallback
{
    public class RefundsCallbackDecryptModel
    {
        /// <summary>
        /// 直连商户的商户号，由微信支付生成并下发。
        /// </summary>
        public string mchid { set; get; }

        /// <summary>
        /// 商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一
        /// 原支付交易对应的商户订单号
        /// </summary>
        public string out_trade_no { set; get; }

        /// <summary>
        /// 微信支付交易订单号。
        /// </summary>
        public string transaction_id { set; get; }

        /// <summary>
        /// 商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。
        /// 示例值：1217752501201407033233368018
        /// </summary>
        public string out_refund_no { set; get; }

        /// <summary>
        /// 微信支付退款号。
        /// </summary>
        public string refund_id { set; get; }

        /// <summary>
        /// 退款状态，枚举值：
        /// SUCCESS：退款成功
        /// CLOSED：退款关闭
        /// ABNORMAL：退款异常，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，可前往【服务商平台—>交易中心】，手动处理此笔退款
        /// </summary>
        public string refund_status { set; get; }

        /// <summary>
        /// 退款成功时间，当退款状态为退款成功时有返回。遵循rfc3339标准格式，格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，HH:mm:ss表示时分秒，TIMEZONE表示时区（+08:00表示东八区时间，领先UTC 8小时，即北京时间）。例如：2015-05-20T13:29:35+08:00表示，北京时间2015年5月20日13点29分35秒
        /// </summary>
        public string success_time { set; get; }

        /// <summary>
        /// 取当前退款单的退款入账方，有以下几种情况：
        /// 1）退回银行卡：{银行名称}{卡类型}{ 卡尾号}
        /// 2）退回支付用户零钱：支付用户零钱
        /// 3）退还商户：商户基本账户商户结算银行账户
        /// 4）退回支付用户零钱通：支付用户零钱通。
        /// </summary>
        public string user_received_account { set; get; }

        /// <summary>
        /// 金额信息
        /// </summary>
        public RefundsCallbackRespAmountModel amount { set; get; }
    }

    /// <summary>
    /// 退款订单金额信息。
    /// </summary>
    public class RefundsCallbackRespAmountModel
    {
        /// <summary>
        /// 原支付交易的订单总金额，币种的最小单位，只能为整数。
        /// </summary>
        public int total { set; get; }

        /// <summary>
        /// 退款金额，币种的最小单位，只能为整数，不能超过原订单支付金额，如果有使用券，后台会按比例退。
        /// </summary>
        public int refund { set; get; }

        /// <summary>
        /// 现金支付金额，单位为分，只能为整数。
        /// </summary>
        public int payer_total { set; get; }

        /// <summary>
        /// 退款给用户的金额，不包含所有优惠券金额。
        /// </summary>
        public int payer_refund { set; get; }
    }
}
