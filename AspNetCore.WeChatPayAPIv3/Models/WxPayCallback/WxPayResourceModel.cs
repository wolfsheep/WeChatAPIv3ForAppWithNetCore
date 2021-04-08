namespace AspNetCore.WeChatPayAPIv3.Models.WxPayCallback
{
    /// <summary>
    /// 微信支付回调通知结果resource实体
    /// </summary>
    public class WxPayResourceModel
    {
        /// <summary>
        /// 对开启结果数据进行加密的加密算法，目前只支持AEAD_AES_256_GCM
        /// </summary>
        public string algorithm { set; get; }

        /// <summary>
        /// Base64编码后的开启/停用结果数据密文
        /// </summary>
        public string ciphertext { set; get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public string associated_data { set; get; }

        /// <summary>
        /// 原始回调类型，为transaction
        /// </summary>
        public string original_type { set; get; }

        /// <summary>
        /// 加密使用的随机串
        /// </summary>
        public string nonce { set; get; }
    }

}
