using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW.Utils.Models
{
    public class WxPayConst
    {
        /// <summary>
        /// 直连商户申请的公众号或移动应用appid。
        /// </summary>
        public static string appid => "";

        /// <summary>
        /// AppSecret，app端加密解密使用
        /// </summary>
        public static string AppSecret => "";

        /// <summary>
        /// 密钥，用商户平台上设置的APIv3密钥【微信商户平台—>账户设置—>API安全—>设置APIv3密钥】，记为key；
        /// </summary>
        public static string APIV3Key => "";

        /// <summary>
        /// 直连商户的商户号，由微信支付生成并下发。
        /// </summary>
        public static string mchid => "";

        /// <summary>
        /// 证书序列号
        /// 查看证书序列号：https://wechatpay-api.gitbook.io/wechatpay-api-v3/chang-jian-wen-ti/zheng-shu-xiang-guan#ru-he-cha-kan-zheng-shu-xu-lie-hao
        /// </summary>
        public static string serialNo => "";

        /// <summary>
        /// key  私钥
        /// 私钥和证书：https://wechatpay-api.gitbook.io/wechatpay-api-v3/ren-zheng/zheng-shu#sheng-ming-suo-shi-yong-de-zheng-shu
        /// </summary>
        public static string privateKey => @"";
    }
}