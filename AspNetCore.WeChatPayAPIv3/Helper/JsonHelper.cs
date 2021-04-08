using Newtonsoft.Json;

namespace AspNetCore.WeChatPayAPIv3.Helper
{
    public static class JsonHelper
    {
        /// <summary>
        /// 转化为json字符串，默认的时间格式
        /// </summary>
        /// <param name="obj">要被转化的对象</param>
        /// <returns>json字符串</returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, DateFormatString = "yyyy-MM-dd HH:mm:ss" });
        }
        /// <summary>
        /// json字符串转化为相应的类型
        /// </summary>
        /// <typeparam name="T">转化后的类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns>转化后的类型</returns>
        public static T ToObject<T>(this string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }
    }
}
