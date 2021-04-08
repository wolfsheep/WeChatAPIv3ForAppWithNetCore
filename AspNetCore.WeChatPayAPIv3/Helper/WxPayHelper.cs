using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.WeChatPayAPIv3.Models.ClostOrder;
using AspNetCore.WeChatPayAPIv3.Models.GenerateOrder;
using AspNetCore.WeChatPayAPIv3.Models.QueryOrder;
using AspNetCore.WeChatPayAPIv3.Models.QueryRefunds;
using AspNetCore.WeChatPayAPIv3.Models.Refunds;

namespace AspNetCore.WeChatPayAPIv3.Helper
{
    public class WxPayHelper
    {
        /// <summary>
        /// 直连商户申请的公众号或移动应用appid。
        /// </summary>
        private string _appid { set; get; }

        /// <summary>
        /// 直连商户的商户号，由微信支付生成并下发。
        /// </summary>
        private string _mchid { set; get; }

        /// <summary>
        /// 证书序列号,查看证书序列号：https://wechatpay-api.gitbook.io/wechatpay-api-v3/chang-jian-wen-ti/zheng-shu-xiang-guan#ru-he-cha-kan-zheng-shu-xu-lie-hao
        /// </summary>
        private string _serialNo;

        /// <summary>
        /// 私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----     亦不包括结尾的-----END PRIVATE KEY-----
        /// </summary>
        private string _privateKey;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appid">直连商户申请的公众号或移动应用appid</param>
        /// <param name="mchid">直连商户的商户号，由微信支付生成并下发</param>
        /// <param name="serialNo">证书序列号,查看证书序列号：https://wechatpay-api.gitbook.io/wechatpay-api-v3/chang-jian-wen-ti/zheng-shu-xiang-guan#ru-he-cha-kan-zheng-shu-xu-lie-hao</param>
        /// <param name="privateKey">私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----     亦不包括结尾的-----END PRIVATE KEY-----</param>
        public WxPayHelper(string appid, string mchid, string serialNo, string privateKey)
        {
            _appid = appid;
            _mchid = mchid;
            _serialNo = serialNo;
            _privateKey = privateKey;
        }

        /// <summary>
        /// 统一下单接口，只对接了必填的字段
        /// </summary>
        /// <param name="description">商品描述</param>
        /// <param name="price">订单总金额，单位为分</param>
        /// <param name="out_trade_no">商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一</param>
        /// <param name="notify_url">通知URL必须为直接可访问的URL，不允许携带查询串。格式：URL</param>
        /// <param name="attach">附加数据，在查询API和支付通知中原样返回，可作为自定义参数使用</param>
        /// <param name="currency">货币类型,CNY：人民币，境内商户号仅支持人民币</param>
        /// <returns></returns>
        public async Task<WxPayRespModel> UnionGenerateOrder(string description, int price, string out_trade_no, string notify_url, string attach = "", string currency = "CNY")
        {
            var url = "https://api.mch.weixin.qq.com/v3/pay/transactions/app";
            var req = new GenerateOrderModelForWxPay
            {
                appid = this._appid,
                mchid = this._mchid,
                description = description,
                amount = new WxPayAmountModel
                {
                    total = price,
                    currency = currency
                },
                out_trade_no = out_trade_no,
                attach = attach,
                notify_url = notify_url
            };

            var client = new HttpClient(new WxPayRequestHandler(_mchid, _serialNo, _privateKey));
            var bodyJson = new StringContent(req.ToJson(), Encoding.UTF8, "application/json");

            var resp = await client.PostAsync(url, bodyJson);
            // 注意！！！ 这个resp只是http的结果，需要把接口具体返回的值读取出来，如果接口报错的话，这地方可以看到具体的错误信息，我就是在这里入坑的。
            var respStr = await resp.Content.ReadAsStringAsync();

            // 如果下单成功，就解析返回的结果，把prepay_id解析出来
            var viewModel = respStr.ToObject<WxPayRespModel>();

            return viewModel;
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="orderNumber">商户自己的订单号,不是微信生成的订单号</param>
        public async Task<WxPayStatusRespModel> QueryOrder(string orderNumber)
        {
            var url = $"https://api.mch.weixin.qq.com/v3/pay/transactions/out-trade-no/{orderNumber}?mchid={_mchid}";
            var client = new HttpClient(new WxPayRequestHandler(_mchid, _serialNo, _privateKey));
            var resp = await client.GetAsync(url);
            var respStr = await resp.Content.ReadAsStringAsync();
            var payModel = respStr.ToObject<WxPayStatusRespModel>();
            return payModel;
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="orderNumber">商户自己的订单号,不是微信生成的订单号</param>
        public async Task<ClostOrderRespModel> ClostOrder(string orderNumber)
        {
            var url = $"https://api.mch.weixin.qq.com/v3/pay/transactions/out-trade-no/{orderNumber}/close";
            var client = new HttpClient(new WxPayRequestHandler(_mchid, _serialNo, _privateKey));
            var bodyJson = new StringContent(new { mchid = this._mchid }.ToJson(), Encoding.UTF8, "application/json");
            var resp = await client.PostAsync(url, bodyJson);
            var respStr = await resp.Content.ReadAsStringAsync();
            var closeModel = new ClostOrderRespModel();
            if (!string.IsNullOrEmpty(respStr))
                closeModel = respStr.ToObject<ClostOrderRespModel>();
            closeModel.StatusCode = resp.StatusCode.ToString();
            return closeModel;
        }


        /// <summary>
        /// 微信支付申请退款接口
        /// </summary>
        /// <param name="out_trade_no">商户系统内部订单号，只能是数字、大小写字母_-*且在同一个商户号下唯一;原支付交易对应的商户订单号。</param>
        /// <param name="out_refund_no">商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。</param>
        /// <param name="reason">若商户传入，会在下发给用户的退款消息中体现退款原因。</param>
        /// <param name="refund">退款金额，币种的最小单位，只能为整数，不能超过原订单支付金额。</param>
        /// <param name="total">原支付交易的订单总金额，币种的最小单位，只能为整数。</param>
        /// <param name="notify_url">异步接收微信支付退款结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。 如果参数中传了notify_url，则商户平台上配置的回调地址将不会生效，优先回调当前传的这个地址。示例值：https://weixin.qq.com ,必须是https的网址</param>
        /// <param name="currency">符合ISO 4217标准的三位字母代码，目前只支持人民币：CNY。</param>
        /// <returns></returns>
        public async Task<RefundsRespModel> Refunds(string out_trade_no, string out_refund_no, string reason, int refund, int total, string notify_url = "", string currency = "CNY")
        {
            var req = new RefundsRequestModel
            {
                out_refund_no = out_refund_no,
                out_trade_no = out_trade_no,
                reason = reason,
                notify_url = notify_url,
                amount = new RefundsAmountModel
                {
                    refund = refund,
                    total = total,
                    currency = currency
                }
            };

            var url = $"https://api.mch.weixin.qq.com/v3/refund/domestic/refunds";
            var client = new HttpClient(new WxPayRequestHandler(_mchid, _serialNo, _privateKey));
            var bodyJson = new StringContent(req.ToJson(), Encoding.UTF8, "application/json");

            var resp = await client.PostAsync(url, bodyJson);
            var respStr = await resp.Content.ReadAsStringAsync();

            var payModel = respStr.ToObject<RefundsRespModel>();
            return payModel;
        }

        /// <summary>
        /// 查询退款结果
        /// </summary>
        /// <param name="out_refund_no">商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。</param>
        public async Task<QueryRefundsOrderRespModel> QueryRefundsOrder(string out_refund_no)
        {
            var url = $"https://api.mch.weixin.qq.com/v3/refund/domestic/refunds/{out_refund_no}";
            var client = new HttpClient(new WxPayRequestHandler(_mchid, _serialNo, _privateKey));
            var resp = await client.GetAsync(url);
            var respStr = await resp.Content.ReadAsStringAsync();
            var payModel = respStr.ToObject<QueryRefundsOrderRespModel>();
            return payModel;
        }
    }
}
