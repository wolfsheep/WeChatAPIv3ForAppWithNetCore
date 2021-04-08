using Org.BouncyCastle.Crypto.Modes;
using System;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace AspNetCore.WeChatPayAPIv3.Helper
{
    /// <summary>
    /// 解密微信通知结果帮助类
    /// 参考资料：https://pay.weixin.qq.com/wiki/doc/apiv3/wechatpay/wechatpay4_2.shtml
    /// .NET5环境使用该代码，需要安装Portable.BouncyCastle组件
    /// </summary>
    public class AesGcmHelper
    {
        private static string ALGORITHM = "AES/GCM/NoPadding";
        private static int TAG_LENGTH_BIT = 128;
        private static int NONCE_LENGTH_BYTE = 12;
        private static string AES_KEY = string.Empty;

        public static string AesGcmDecrypt(string associatedData, string nonce, string ciphertext, string APIV3Key)
        {
            GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
            AeadParameters aeadParameters = new AeadParameters(
                new KeyParameter(Encoding.UTF8.GetBytes(APIV3Key)),
                128,
                Encoding.UTF8.GetBytes(nonce),
                Encoding.UTF8.GetBytes(associatedData));
            gcmBlockCipher.Init(false, aeadParameters);

            byte[] data = Convert.FromBase64String(ciphertext);
            byte[] plaintext = new byte[gcmBlockCipher.GetOutputSize(data.Length)];
            int length = gcmBlockCipher.ProcessBytes(data, 0, data.Length, plaintext, 0);
            gcmBlockCipher.DoFinal(plaintext, length);
            return Encoding.UTF8.GetString(plaintext);
        }
    }
}
