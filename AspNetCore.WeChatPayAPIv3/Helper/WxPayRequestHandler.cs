using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.WeChatPayAPIv3.Helper
{
    internal class WxPayRequestHandler : DelegatingHandler
    {
        /// <summary>
        /// 直连商户的商户号，由微信支付生成并下发。
        /// </summary>
        private string _merchantId = string.Empty;

        /// <summary>
        /// 证书序列号,查看证书序列号：https://wechatpay-api.gitbook.io/wechatpay-api-v3/chang-jian-wen-ti/zheng-shu-xiang-guan#ru-he-cha-kan-zheng-shu-xu-lie-hao
        /// </summary>
        private string _serialNo = string.Empty;

        /// <summary>
        /// 私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----     亦不包括结尾的-----END PRIVATE KEY-----
        /// </summary>
        private string _privateKey = string.Empty;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="mchid">直连商户的商户号，由微信支付生成并下发。</param>
        /// <param name="serialNo">证书序列号,查看证书序列号：https://wechatpay-api.gitbook.io/wechatpay-api-v3/chang-jian-wen-ti/zheng-shu-xiang-guan#ru-he-cha-kan-zheng-shu-xu-lie-hao</param>
        /// <param name="privateKey">私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----     亦不包括结尾的-----END PRIVATE KEY-----</param>
        public WxPayRequestHandler(string mchid, string serialNo, string privateKey)
        {
            InnerHandler = new HttpClientHandler();
            this._merchantId = mchid;
            this._serialNo = serialNo;
            this._privateKey = privateKey;
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var auth = await BuildAuthAsync(request);
            string value = $"WECHATPAY2-SHA256-RSA2048 {auth}";
            request.Headers.Add("Authorization", value);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> BuildAuthAsync(HttpRequestMessage request)
        {
            var method = request.Method.ToString();
            var body = "";
            if (method == "POST" || method == "PUT" || method == "PATCH")
            {
                var c = request.Content;
                body = await c.ReadAsStringAsync();//这里读取的数据一定要跟传入的参数一致，debug时看到的数据与传入的参数不一致是不可以的
            }

            string uri = request.RequestUri.PathAndQuery;
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            string nonce = Path.GetRandomFileName();
            var message = $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
            var signature = Sign(message);
            return $"mchid=\"{_merchantId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{_serialNo}\",signature=\"{signature}\"";
        }

        private string Sign(string message)
        {
            byte[] keyData = Convert.FromBase64String(_privateKey);
            using (CngKey cngKey = CngKey.Import(keyData, CngKeyBlobFormat.Pkcs8PrivateBlob))
            using (RSACng rsa = new RSACng(cngKey))
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                return Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }
        }
    }
}
