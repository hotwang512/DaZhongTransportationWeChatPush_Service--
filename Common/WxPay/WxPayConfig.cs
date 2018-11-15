using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SyntacticSugar;

namespace Common.WxPay
{
    public class WxPayConfig
    {

        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public static readonly string APPID = ConfigSugar.GetAppString("WeChatCorpID");

        public static readonly string APPSECRET = ConfigSugar.GetAppString("WeChatSecret");

        public static readonly string PAYMENTSECRET = ConfigSugar.GetAppString("PaymentSecret");

        public static readonly string MCHID = ConfigSugar.GetAppString("MCHID");    //商户id号

        public static readonly string KEY = ConfigSugar.GetAppString("APIKey");
        //支付描述
  

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public static readonly string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public static readonly string SSLCERT_PASSWORD = ConfigSugar.GetAppString("MCHID");

        /// <summary>
        /// 客户端IP
        /// </summary>
        /// <param name="hc"></param>
        /// <returns></returns>
        public static string GetIP(HttpContext hc)
        {
            string ip = string.Empty;
            try
            {
                ip = hc.Request.ServerVariables["HTTP_VIA"] != null ? hc.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : hc.Request.ServerVariables["REMOTE_ADDR"];
                if (ip == string.Empty)
                {
                    ip = hc.Request.UserHostAddress;
                }
                return ip;
            }
            catch
            {
                return "8.8.8.8";
            }
        }
    }
}
