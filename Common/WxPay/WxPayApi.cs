using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace Common.WxPay
{
    public class WxPayApi
    {

        protected Hashtable Parameters = new Hashtable();
        /// <summary>
        /// 根据当前系统时间加随机序列来生成订单号
        /// </summary>
        /// <returns>@return 订单号</returns>
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1:yyyyMMddHHmmss}{2}", WxPayConfig.MCHID, DateTime.Now, ran.Next(999));
        }



        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <returns>@return 时间戳</returns>
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 生成随机串，随机串包含字母或数字
        /// </summary>
        /// <returns> @return 随机串</returns>
        public static string GenerateNonceStr()
        {
            //Random random = new Random();
            //return GetMD5(random.Next(1000).ToString(), "GBK");
            return Guid.NewGuid().ToString().Replace("-", "");
        }
        /// <summary>
        /// 获取md5加密字符串
        /// </summary>
        /// <param name="encypStr"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        protected static string GetMD5(string encypStr, string charset)
        {
            byte[] bytes;
            //创建md5对象
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                bytes = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception)
            {
                bytes = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            return BitConverter.ToString(provider.ComputeHash(bytes)).Replace("-", "").ToUpper();
        }
        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        public static string GetSHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }

      
        protected void SetParameter(string parameter, string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                if (this.Parameters.Contains(parameter))
                {
                    this.Parameters.Remove(parameter);
                }
                this.Parameters.Add(parameter, parameterValue);
            }
        }

    }
}
