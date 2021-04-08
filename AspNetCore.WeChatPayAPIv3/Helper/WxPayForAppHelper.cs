using System;
using System.IO;
using System.Security.Cryptography;
using AspNetCore.WeChatPayAPIv3.Models.GenerateOrder;

namespace AspNetCore.WeChatPayAPIv3.Helper
{
    public class WxPayForAppHelper
    {
        /// <summary>
        /// 为app端生成签名
        /// </summary>
        /// <param name="appid">直连商户申请的公众号或移动应用appid</param>
        /// <param name="prepayId">预支付id</param>
        /// <param name="privateKey">私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----     亦不包括结尾的-----END PRIVATE KEY-----</param>
        public static AppPayModel GetSign(string appid, string prepayId, string privateKey)
        {
            var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            string nonce = Path.GetRandomFileName();
            var message = $"{appid}\n{timestamp}\n{nonce}\n{prepayId}\n";
            var signature = Sign(message, privateKey);
            var viewModel = new AppPayModel
            {
                timestamp = timestamp.ToString(),
                noncestr = nonce,
                prepay_id = prepayId,
                signature = signature
            };
            return viewModel;
        }

        private static string Sign(string message, string privateKey)
        {
            //            // NOTE： 私钥不包括私钥文件起始的-----BEGIN PRIVATE KEY-----
            //            //        亦不包括结尾的-----END PRIVATE KEY-----
            //            string privateKey = @"MIIEvAIBADANBgkqhkiG9w0BAQEFAASCBKYwggSiAgEAAoIBAQDi6V9qSE0cEYG5
            //AIZPveW/boI+H1YuK2Nc9FIzYDLev/YGTF+4jmJ9q+pimLsBVvO/OucNUCAUvgPr
            //9AVaLFvzpksjW0494auQWeQCiz4BoaTtAOD3Dh/ZDVvaYlEv37XXKm2ABs/AlMkh
            //J8TjQIKIJtFBpJXLrGGCQWxPp5YnxeoDeGUWRW0WlZ40gD/pN+5MCT8qF41aVMIP
            //KB2R3A0dTEUaPGU1UZ5b2cJAbzdrPA1uHHZheDQO296VS6XkYdw1J1MRblXYVX7T
            //EF68F02iqRJ6P6t/UVCYb/4PWTftnaELmx38I3MfkG6RbxFxk5MLtg+P+mz3PROH
            //t2D8AF/RAgMBAAECggEAb4aKaf99PsVv/9C9MuY/zJXxF71pKyoSHTbMTAoFFqw8
            //rJ3PEjORPvlHKwysJR1qkdvOgiGWkvtNjCQUmVxiGV/rYEZZL1sCauFziriLUlvz
            //ZrVe3K6pgpHpSm22P5RxmH528UznwVyfeldEkvk67tY9VUkigseH6XBkXsYcrBDS
            //YN+v/g60n1o89Z0rDEEs9jireMAeVnlxiBgFC2FRZYSFjbyGYPmKPeezmwXCPF3g
            //4ON9Zj4WQfthGEKyDL/tSF6W6VkD+bPRSuINWRWo+qsPxrjHh/KUInqaDzAT+eEH
            //6YDgMQ4Fl78ZfGuhz4oesEMg3JqOtGokiqwIoa5v0QKBgQD0f1o8AWLezuD/LtEf
            //GWRvKR0qQVLDdg1iXbN9Zwctme5NqPKEI/G3tyMqiWY7559kETTWikSGhd6ToniK
            //ZT3DrZTCE3JBx0rCgDGlgUf59U/OJjsDAYF7u3CM+g9wUQd6EUJu/qLzKGSF+Ofd
            //jmOEMCfKcQZrXCBxU9/WYKDILQKBgQDtljjPUMXDocC/Kdg6Vg0jiYdRCZRjF8HS
            //hhK98ZsgrUJtdle3C4KK9NdjZ/YpF7LXY6hFlPh/AFt0jJKSChjRY2duuwTRc7qX
            //lZhkuMuLFc26OXwVrX0Umj+AxfxqohIAVRwR1cUthuX1vrUbLVDEvkc7wLkevfSP
            //6qPZDU04tQKBgB40HVGMWkpsBB+CCRNub5nL2m5941uCGeUs7h9vutNHUMkHNe5d
            //Al9PoGiA0MBVvLr/5ScbrZtwri5Ow9VU7Gxf5SCUhmjZ1cJxU7C+Z8XZKCKvVlro
            //tLY0UZMY0Q9u8x2BRFOx4S9orgJe3UXhcSnDSScZD1Hz744QbnZtOW+BAoGANDFF
            //B54J4+Eb06LgomgW+NCUjmPZWi9038eeRGXYG0SUjDFsMYTwx4/j5S2IcRyIqDJ3
            //W9nPAS9V5/Odm0FmlFEO/s9MzqpqujQdxzqlVEeU6y0Hqkdza9w6yRI1UkOKStJn
            //mS9p4od46xRa+r5ouN/IDENZJ3y5RghYuCzo4uUCgYAJXtGuVBKN0e2ok+D4uVI/
            //j6WOmQRw+N32hU2eeUk52uOQuiKbUT9tbqk7kRw3PuVpQUXq3bkLbJTZK5+Sd5wK
            //1XZMvI1IuqDz87+nmf/vB/biuYltfDXUV6EVUB1h0WcgJ35RJsSKyY1WzqE4P6ys
            //lmalRcuCNFuvbTIo9IBI0w==";
            byte[] keyData = Convert.FromBase64String(privateKey);
            using (CngKey cngKey = CngKey.Import(keyData, CngKeyBlobFormat.Pkcs8PrivateBlob))
            using (RSACng rsa = new RSACng(cngKey))
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
                return Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
            }
        }
    }

}
