using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Common;
using Common.WxPay;
using DaZhongManagementSystem.Entities.TableEntity;
using Entities.UserDefinedEntity;
using Newtonsoft.Json;
using ServiceLog;
using SyntacticSugar;

namespace Service.PushFunction.WeChatPush
{
    public class RedPacketPush : PushWeChat
    {

        protected override U_UploadResult UpLoadSource(U_AccessToken accessTokenModel, U_Content contentModel)
        {
            return null;
        }

        protected override string GetPushJson(U_UploadResult uploadResult, U_Content contentModel)
        {

            return string.Empty;
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
            if (isSuccess)
            {
                var listRedPacket = new List<Business_Redpacket_Push_Information>();
                //List<U_WeChatUser> weChatUserList = SearchWeChatUserList();
                //string pushObject = GetPushObject(contentModel, weChatUserList);//13788907365|15618738991|18301914615
                string pushObject = GetPushObject(contentModel);
                string[] pushObjs = pushObject.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);  //推送的人数
                int[] redPacket = new int[pushObjs.Length - 1];
                try
                {
                    //固定金额红包推送 总金额固定，按照人数计算每个红包金额（每个红包金额需一致）
                    // 注：如果无法平均分配，则最后一人多出（例：10元分给3人，则金额为3.3、3.3、3.4）
                    if (contentModel.RedpacketType == 1)
                    {
                        int money = (int)(contentModel.RedpacketMoney * 100);  //总金额(分)
                        redPacket = GetAvgRedPacket(pushObjs.Length, money);
                    }
                    else if (contentModel.RedpacketType == 2) //红包总金额（RMB),推送给每人的金额随机。
                    {
                        int money = (int)(contentModel.RedpacketMoney * 100);  //总金额(分)
                        redPacket = GetRandomRedPacket(pushObjs.Length, money);
                    }
                    else if (contentModel.RedpacketType == 3)  //输入单个红包随机金额区间（RMB），发给每人红包的金额在此区间内
                    {
                        int moneyFrom = (int)(contentModel.RedpacketMoneyFrom * 100);
                        int moneyTo = (int)(contentModel.RedpacketMoneyTo * 100);
                        redPacket = GetSingleRandomRedPacket(pushObjs.Length, moneyFrom, moneyTo);
                    }
                    for (int i = 0; i < pushObjs.Length; i++)
                    {
                        WxPayData data = new WxPayData();
                        Business_Redpacket_Push_Information redpacketPushInfo = new Business_Redpacket_Push_Information();
                        redpacketPushInfo.Business_WeChatPushVguid = contentModel.VGUID;    //推送主键
                        data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
                        var outTradeNo = WxPayApi.GenerateOutTradeNo();
                        redpacketPushInfo.OrderNumber = outTradeNo;
                        data.SetValue("mch_billno", outTradeNo); //商户订单号
                        data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
                        data.SetValue("wxappid", WxPayConfig.APPID);//公众账号ID
                        data.SetValue("sender_name", "大众交通出租分公司");      //商户名称
                        //string filePath = AppDomain.CurrentDomain.BaseDirectory + "logo.png";
                        //string mediaid = UploadTempResource(filePath);
                        data.SetValue("sender_header_media_id", "1G6nrLmr5EC3MMb_-zK1dDdzmd0p7cNliYu9V5w7o8K0"); //发送者头像，此id为微信默认的头像
                        string openid = ConvertToOpenidByUserId(_accessToken, pushObjs[i]);
                        var openInfo = JsonHelper.JsonToModel<U_OpenInfo>(openid);
                        JsonConvert.DeserializeObject<U_OpenInfo>(openid);
                        data.SetValue("re_openid", openInfo.openid);  //用户openid   
                        data.SetValue("total_amount", redPacket[i]);     //付款金额，单位分
                        redpacketPushInfo.RedpacketMoney = (decimal)(redPacket[i] * 1.0 / 100);  //红包金额
                        data.SetValue("wishing", contentModel.Message);       //红包祝福语
                        data.SetValue("act_name", contentModel.Title);      //活动名称
                        data.SetValue("remark", "快来抢");  //备注
                        data.SetValue("scene_id", "PRODUCT_4");           //场景(金额大于200元时必填)
                        data.SetValue("workwx_sign", data.MakeWorkWxSign("redPacket"));  //企业微信签名
                        data.SetValue("sign", data.MakeSign());                   //微信支付签名
                        redpacketPushInfo.UserID = pushObjs[i];           //红包接收人的微信号
                        string xml = data.ToXml();
                        const string postUrl = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendworkwxredpack";  //发送企业红包接口地址
                        string response = PostWebRequest(postUrl, xml, Encoding.UTF8, true);//调用HTTP通信接口提交数据到API
                        WxPayData result = new WxPayData();
                        result.FromXml(response);
                        if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
                        {
                            redpacketPushInfo.RedpacketStatus = 2; //已发送待领取
                        }
                        else
                        {
                            redpacketPushInfo.RedpacketStatus = 3; //发送失败
                            redpacketPushInfo.Reson = result.GetValue("err_code_des").ToString();
                            LogManager.WriteLog(LogFile.Error, result.GetValue("err_code") + ":" + result.GetValue("err_code_des"));
                        }
                        redpacketPushInfo.VGUID = Guid.NewGuid();
                        redpacketPushInfo.CreatedDate = DateTime.Now;
                        redpacketPushInfo.CreatedUser = "sysAdmin";
                        listRedPacket.Add(redpacketPushInfo);
                    }
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogFile.Error, ex.ToString());
                    return "推送失败!";
                }
                UpdatePushStatus(contentModel);
                InsertRedPacketInfo(listRedPacket);
                return "推送成功！";
            }
            return "推送失败！";

        }


        //利用Guid产生随机数
        public static int GetRandomNumber(int min, int max)
        {
            int rtn = 0;
            Random r = new Random();
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int iSeed = BitConverter.ToInt32(buffer, 0);
            r = new Random(iSeed);
            rtn = r.Next(min, max + 1);
            return rtn;
        }
        /// <summary>
        /// 固定金额红包推送 总金额固定，按照人数计算每个红包金额（每个红包金额需一致）
        /// </summary>
        /// <param name="totalNum">总人数</param>
        /// <param name="money">每个人的红包金额</param>
        /// <returns></returns>
        public int[] GetAvgRedPacket(int totalNum, int money)
        {

            #region MyRegion
            // int avg = totalMoney / totalNum;  //平均值
            //if (avg < 100)
            //{
            //    throw new Exception("每个人的金额不足1元");
            //}
            //int[] arr = Enumerable.Repeat(avg, totalNum).ToArray();
            //arr[arr.Length - 1] = totalMoney - avg * (totalNum - 1);  //最后一个的值等于总钱数减去其他值的总和
            //return arr; 
            #endregion

            return Enumerable.Repeat(money, totalNum).ToArray();
        }

        /// <summary>
        ///红包总金额（RMB),推送给每人的金额随机。
        /// </summary>
        /// <param name="totalNum">总人数</param>
        /// <param name="totalMoney">总钱数</param>
        /// <returns></returns>
        public int[] GetRandomRedPacket(int totalNum, int totalMoney)
        {
            if (totalMoney / totalNum < 100)
            {
                throw new Exception("每个人的金额不足1元");
            }
            int[] arr = new int[totalNum];
            for (int i = 0; i < totalNum; i++)
            {
                arr[i] = GetRandomNumber(100, totalMoney); //因为红包最低是1元钱
            }
            int dif = arr.Sum() - totalMoney;
            if (dif == 0) return arr;
            int d = dif > 0 ? -1 : 1;
            while (true)
            {
                var index = GetRandomNumber(0, totalNum - 1);
                var v = arr[index] + d;
                if (v < 100 || v > totalMoney) continue;
                arr[index] = v;
                if (arr.Sum() == totalMoney) break;
            }
            return arr;
        }

        /// <summary>
        ///单个红包随机金额区间（RMB），发给每人红包的金额在此区间内
        /// </summary>
        /// <param name="totalNum">总人数</param>
        /// <param name="moneyFrom">钱数从</param>
        /// <param name="moneyTo">钱数至</param>
        /// <returns></returns>
        public int[] GetSingleRandomRedPacket(int totalNum, int moneyFrom, int moneyTo)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            var arr = Enumerable.Range(moneyFrom, totalNum).Select(i => rand.Next(moneyFrom, moneyTo + 1)).ToArray();
            return arr;
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