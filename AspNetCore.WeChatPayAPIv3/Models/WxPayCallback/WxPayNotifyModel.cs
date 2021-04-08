namespace AspNetCore.WeChatPayAPIv3.Models.WxPayCallback
{
    /// <summary>
    /// 微信支付结果回调通知实体
    /// </summary>
    public class WxPayNotifyModel
    {
        /// <summary>
        /// 通知的唯一ID
        /// </summary>
        public string id { set; get; }

        /// <summary>
        /// 通知创建时间,格式为YYYY-MM-DDTHH:mm:ss+TIMEZONE，YYYY-MM-DD表示年月日，T出现在字符串中，表示time元素的开头，HH:mm:ss.表示时分秒，TIMEZONE表示时区（+08:00表示东八区时间，领先UTC 8小时，即北京时间）。例如：2015-05-20T13:29:35+08:00表示北京时间2015年05月20日13点29分35秒。
        /// </summary>
        public string create_time { set; get; }

        /// <summary>
        /// 通知的类型，支付成功通知的类型为TRANSACTION.SUCCESS
        /// </summary>
        public string event_type { set; get; }

        /// <summary>
        /// 通知的资源数据类型，支付成功通知为encrypt-resource
        /// </summary>
        public string resource_type { set; get; }

        /// <summary>
        /// 通知资源数据,json格式
        /// </summary>
        public WxPayResourceModel resource { set; get; }

        /// <summary>
        /// 回调摘要
        /// </summary>
        public string summary { set; get; }
    }
    
}
