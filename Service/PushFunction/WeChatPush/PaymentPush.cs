using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Common.WxPay;
using Entities.TableEntity;
using Entities.UserDefinedEntity;
using ServiceLog;

namespace Service.PushFunction.WeChatPush
{
    public class PaymentPush : PushWeChat
    {
        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            return null;
        }

        protected override string GetPushJson(U_UploadResult uploadResult, U_Content contentModel)
        {

            string responeJsonStr = "";

            return responeJsonStr;
        }

        /// <summary>
        /// 重写推送方法
        /// </summary>
        /// <param name="accessTokenModel"></param>
        /// <param name="contentModel"></param>
        /// <returns></returns>
        public override string Push(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            //获取AccessToken
            bool isSuccess = SetCorpAccount(accessTokenModel);
            // var isOk = 0;
            if (isSuccess)
            {
                var payments = new List<Business_Enterprisepayment_Information>();
                string postUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/promotion/paywwsptrans2pocket";
                //List<U_WeChatUser> weChatUserList = SearchWeChatUserList();
                //string pushObject = GetPushObject(contentModel, weChatUserList);//13788907365|15618738991|18301914615
                string pushObject = GetPushObject(contentModel);
                string[] pushObjs = pushObject.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);  //推送的人数
                int money = (int)(contentModel.RedpacketMoney * 100);  //总金额(分)
                var redPacket = Enumerable.Repeat(money, pushObjs.Length).ToArray();
                for (int i = 0; i < pushObjs.Length; i++)
                {
                    var payment = new Business_Enterprisepayment_Information();
                    payment.Business_WeChatPushVguid = contentModel.VGUID;
                    WxPayData data = new WxPayData();
                    data.SetValue("appid", WxPayConfig.APPID);//公众账号ID
                    data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
                    data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
                    var outTradeNo = WxPayApi.GenerateOutTradeNo();
                    payment.OrderNumber = outTradeNo;
                    data.SetValue("partner_trade_no", outTradeNo);  //商户订单号
                    string openid = ConvertToOpenidByUserId(_accessToken, pushObjs[i]);
                    var openInfo = openid.JsonToModel<U_OpenInfo>();
                    data.SetValue("openid", openInfo.openid);    //商户appid下，某用户的openid
                    data.SetValue("check_name", "NO_CHECK");    //校验用户姓名选项(NO_CHECK：不校验真实姓名 FORCE_CHECK：强校验真实姓名)
                    //data.SetValue("re_user_name", "刘洋");    //收款用户真实姓名。 如果check_name设置为FORCE_CHECK，则必填用户真实姓名
                    data.SetValue("amount", redPacket[i]);               //金额，单位为分
                    payment.RedpacketMoney = (decimal)(redPacket[i] * 1.0 / 100);  //红包金额
                    data.SetValue("desc", contentModel.Message);//付款说明 
                    data.SetValue("spbill_create_ip", "192.168.0.1");//Ip地址
                    data.SetValue("ww_msg_type", "NORMAL_MSG");    //付款消息类型
                    data.SetValue("act_name", contentModel.Title);   //项目名称
                    data.SetValue("workwx_sign", data.MakeWorkWxSign("payment"));  //企业微信签名
                    data.SetValue("sign", data.MakeSign());            //微信支付签名

                    payment.UserID = pushObjs[i];           //红包接收人的微信号
                    string xml = data.ToXml();
                    string response = PostWebRequest(postUrl, xml, Encoding.UTF8, true);//调用HTTP通信接口提交数据到API
                    WxPayData result = new WxPayData();
                    result.FromXml(response);
                    if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
                    {
                        payment.RedpacketStatus = 1;  //成功
                        var paymentTime = result.GetValue("payment_time").ToString();
                        payment.CreatedDate = DateTime.Parse(paymentTime);
                    }
                    else
                    {
                        payment.RedpacketStatus = 2; //失败
                        payment.Reson = result.GetValue("err_code_des").ToString();
                        LogManager.WriteLog(LogFile.Error, result.GetValue("err_code") + ":" + result.GetValue("err_code_des"));
                    }
                    payment.VGUID = Guid.NewGuid();
                    payment.CreatedDate = DateTime.Now;
                    payment.CreatedUser = "sysAdmin";
                    payments.Add(payment);
                }

                InsertPaymentInfos(payments);
                UpdatePushStatus(contentModel);
                return "推送成功！";
            }
            return "推送失败！";
        }

        public override List<string> Push(U_AccessToken accessTokenModel, List<U_Content> contentModels)
        {
            List<string> rtnList = new List<string>();
            foreach (var contentModel in contentModels)
            {
                string rtn = Push(accessTokenModel, contentModel);
                rtnList.Add(rtn);
            }
            return rtnList;
        }
    }
}