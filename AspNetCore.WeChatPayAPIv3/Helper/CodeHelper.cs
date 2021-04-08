using System;

namespace AspNetCore.WeChatPayAPIv3.Helper
{
    /// <summary>
    /// 随机码帮助类
    /// </summary>
    public class CodeHelper
    {
        /// <summary>
        /// 基础字符串
        /// </summary>
        private const string Basestr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        /// <summary>
        /// 基础数字字符串
        /// </summary>
        private const string BaseNumstr = "0123456789";

        /// <summary>
        /// 可选长度的生成随机码
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        private static string CreateCode(int length)
        {
            var random = new Random();
            var sb = "";
            for (var i = 0; i < length; i++)
            {
                var number = random.Next(Basestr.Length);
                sb += Basestr.Substring(number, 1);
            }
            return sb;
        }

        /// <summary>
        /// 可选长度的生成数字随机码
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public static string CreateNumCode(int length)
        {
            var random = new Random();
            var sb = "";
            for (var i = 0; i < length; i++)
            {
                var number = random.Next(BaseNumstr.Length);
                sb += BaseNumstr.Substring(number, 1);
            }
            return sb;
        }

        /// <summary>
        /// 随机生成不重复邀请码
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="seed">种子</param>
        /// <returns></returns>
        private static string CreateRandStrCode(int length, int seed = 0)
        {
            //Guid的哈希码作为种子值
            var buffer = Guid.NewGuid().ToByteArray();
            var ranInt = BitConverter.ToInt32(buffer, 0) + seed;

            var random = new Random(ranInt);
            var re = "";
            for (var i = 0; i < length; i++)
            {
                var number = random.Next(Basestr.Length);
                re += Basestr.Substring(number, 1);
            }
            return re;
        }

        /// <summary>
        /// 生成用户邀请码
        /// </summary>
        public static string CreateUserCode()
        {
            var str = CreateRandStrCode(8, 0);
            return str;
        }

        /// <summary>
        /// 生成机构邀请码
        /// </summary>
        public static string CreateOrgInviteCode()
        {
            var str = CreateRandStrCode(6, 1);
            return str;
        }

        /// <summary>
        /// 生成邀请码前缀
        /// </summary>
        public static string CreateCodePre()
        {
            var sb = CreateCode(1);

            return sb;
        }
    }
}
