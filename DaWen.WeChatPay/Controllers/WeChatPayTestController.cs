using DW.Utils.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DW.Utils.Models;
using DW.Utils.Models.ClostOrder;
using DW.Utils.Models.GenerateOrder;
using DW.Utils.Models.QueryOrder;
using DW.Utils.Models.QueryRefunds;
using DW.Utils.Models.Refunds;
using DW.Utils.Models.RefundsCallback;
using DW.Utils.Models.WxPayCallback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace DaWen.WeChatPay.Controllers
{
    /// <summary>
    /// 测试微信支付的控制器
    /// </summary>
    public class WeChatPayTestController : ControllerBase
    {
        private readonly ILogger<WeChatPayTestController> _logger;
        private readonly IConfiguration _configuration;

        public WeChatPayTestController(ILogger<WeChatPayTestController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// 统一下单接口
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/WeChatPayTest/GenerateOrder")]
        public async Task<AppPayModel> GenerateOrder()
        {
            var orderNumber = $"{DateTime.Now:yyyyMMddHHmmssff}{CodeHelper.CreateNumCode(3)}";
            var helper = new WxPayHelper(WxPayConst.appid, WxPayConst.mchid, WxPayConst.serialNo, WxPayConst.privateKey);
            var notify_url = _configuration["notify_url"]; //这个放在配置文件，从配置文件读取比较灵活，或者写到数据库中
            var payodel = await helper.UnionGenerateOrder("好东西啊", 1, orderNumber, notify_url, "附加信息测试啊");

            #region 为APP生成下单所需的参数，看个人实际需求，也可以APP自己生成所需的参数

            var signModel = WxPayForAppHelper.GetSign(WxPayConst.appid, payodel.prepay_id, WxPayConst.privateKey);

            #endregion

            return signModel;
        }

        /// <summary>
        /// 查询订单接口 -- 测试数据：2021033119240567226
        /// </summary>
        [HttpGet, Route("api/WeChatPayTest/QueryOrder")]
        public async Task<WxPayStatusRespModel> QueryOrder(string orderNumber)
        {
            var helper = new WxPayHelper(WxPayConst.appid, WxPayConst.mchid, WxPayConst.serialNo, WxPayConst.privateKey);
            var payModel = await helper.QueryOrder(orderNumber);
            return payModel;
        }

        /// <summary>
        /// 查询订单接口 -- 测试数据：2021033119240567226
        /// </summary>
        [HttpGet, Route("api/WeChatPayTest/CloseOrder")]
        public async Task<ClostOrderRespModel> CloseOrder(string orderNumber)
        {
            var helper = new WxPayHelper(WxPayConst.appid, WxPayConst.mchid, WxPayConst.serialNo, WxPayConst.privateKey);
            var payModel = await helper.ClostOrder(orderNumber);
            return payModel;
        }

        /// <summary>
        /// 微信支付成功结果回调接口
        /// </summary>
        /// <returns>退款通知http应答码为200且返回状态码为SUCCESS才会当做商户接收成功，否则会重试。注意：重试过多会导致微信支付端积压过多通知而堵塞，影响其他正常通知。</returns>
        [HttpPost, Route("api/WeChatPayTest/WxPayCallback")]
        [AllowAnonymous]
        public async Task<WxPayCallbackRespModel> WxPayCallback()
        {
            #region 获取字符串流

            /**
             * .NET 获取字符串流
             *  System.IO.Stream s = HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
             */

            var buffer = new MemoryStream();
            Request.Body.CopyTo(buffer);

            #endregion
            //我没有使用官方的那种验证数据安全性的方法，我解密出来数据之后，直接拿着订单号再去查询一下订单状态，然后再更新到数据库中。我嫌麻烦……

            var str = Encoding.UTF8.GetString(buffer.GetBuffer());
            var wxPayNotifyModel = str.ToObject<WxPayNotifyModel>();
            var resource = wxPayNotifyModel?.resource ?? new WxPayResourceModel();
            var decryptStr = AesGcmHelper.AesGcmDecrypt(resource.associated_data, resource.nonce, resource.ciphertext, WxPayConst.APIV3Key);
            var decryptModel = decryptStr.ToObject<WxPayResourceDecryptModel>();

            var viewModel = new WxPayCallbackRespModel();
            if (string.IsNullOrEmpty(decryptModel.out_trade_no))
            {
                viewModel.code = "FAIL";
                viewModel.message = "数据解密失败";
            }
            else
            {
                var resp = await QueryOrder(decryptModel.out_trade_no);
                //然后进行数据库更新处理……等等其他操作
            }

            return viewModel;
        }

        /// <summary>
        /// 退款接口 -- 测试数据：2021033119240567226
        /// </summary>
        [HttpGet, Route("api/WeChatPayTest/Refunds")]
        public async Task<RefundsRespModel> Refunds(string orderNumber)
        {
            var helper = new WxPayHelper(WxPayConst.appid, WxPayConst.mchid, WxPayConst.serialNo, WxPayConst.privateKey);
            var refundNumber = $"{DateTime.Now:yyyyMMddHHmmssff}{CodeHelper.CreateNumCode(3)}";
            var payModel = await helper.Refunds(orderNumber, refundNumber, "测试退款行不行", 1, 2, "https://xxxxx.top/api/WeChatPayTest/RefundsCallback");
            return payModel;
        }

        /// <summary>
        /// 退款通知回调接口
        /// </summary>
        /// <returns>退款通知http应答码为200且返回状态码为SUCCESS才会当做商户接收成功，否则会重试。注意：重试过多会导致微信支付端积压过多通知而堵塞，影响其他正常通知。</returns>
        [HttpPost, Route("api/WeChatPayTest/RefundsCallback")]
        [AllowAnonymous]
        public async Task<RefundsCallbackRespModel> RefundsCallback()
        {
            #region 获取字符串流

            /**
             * .NET 获取字符串流
             *  System.IO.Stream s = HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
             */

            var buffer = new MemoryStream();
            Request.Body.CopyTo(buffer);

            #endregion
            //我没有使用官方的那种验证数据安全性的方法，我解密出来数据之后，直接拿着商户退款订单号再去查询一下订单状态，然后再更新到数据库中。我嫌麻烦……

            var str = Encoding.UTF8.GetString(buffer.GetBuffer());
            var wxPayNotifyModel = str.ToObject<RefundsCallbackModel>();
            var resource = wxPayNotifyModel?.resource ?? new RefundsCallbackResourceModel();
            var decryptStr = AesGcmHelper.AesGcmDecrypt(resource.associated_data, resource.nonce, resource.ciphertext, WxPayConst.APIV3Key);
            var decryptModel = decryptStr.ToObject<RefundsCallbackDecryptModel>();

            var viewModel = new RefundsCallbackRespModel();
            if (string.IsNullOrEmpty(decryptModel.out_trade_no))
            {
                viewModel.code = "FAIL";
                viewModel.message = "数据解密失败";
            }
            else
            {
                var resp = await QueryRefunds(decryptModel.out_refund_no);
                //然后进行数据库更新处理……等等其他操作
            }

            return viewModel;
        }

        /// <summary>
        /// 查询退款结果接口
        /// </summary>
        /// <param name="refundNumber">商户系统内部的退款单号，商户系统内部唯一</param>
        /// <returns></returns>
        [HttpGet, Route("api/WeChatPayTest/QueryRefunds")]
        public async Task<QueryRefundsOrderRespModel> QueryRefunds(string refundNumber)
        {
            var helper = new WxPayHelper(WxPayConst.appid, WxPayConst.mchid, WxPayConst.serialNo, WxPayConst.privateKey);
            var payModel = await helper.QueryRefundsOrder(refundNumber);
            return payModel;
        }
    }
}
