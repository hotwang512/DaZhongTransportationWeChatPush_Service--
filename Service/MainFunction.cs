using Common;
using DaZhongManagementSystem.Entities.TableEntity;
using Entities.UserDefinedEntity;
using Entities.ViewEntity;
using Infrastructure.DataServer;
using Service.BusinessLogic;
using Service.PushFunction;
using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceLog;
using System.Threading;

namespace Service
{
    public class MainFunction
    {
        public MainFunction()
        {
        }

        /// <summary>
        /// 推送（短信、微信）
        /// </summary>
        public static void PushContentFunction()
        {
            try
            {
                while (true)
                {
                    PushServer ps = new PushServer();
                    U_AccessToken accessTokenModel = new U_AccessToken();
                    accessTokenModel.UserKey = ConfigSugar.GetAppString("WeChatCorpID");
                    accessTokenModel.UserPassword = ConfigSugar.GetAppString("WeChatSecret");
                    string responseMsg = "";
                    bool result = false;
                    #region 推送图文
                    List<Business_WeChatPush_Information> listImgPush = ps.GetImgPushList();//获取要推送的数据
                    List<U_Content> contentList = new List<U_Content>();
                    foreach (var imgPush in listImgPush)
                    {
                        //主表信息
                        U_Content content = new U_Content();
                        content.Message = imgPush.Message;
                        content.MessageType = imgPush.MessageType;
                        content.Title = imgPush.Title;
                        content.CoverDescption = imgPush.CoverDescption;
                        content.CoverImg = imgPush.CoverImg;
                        content.PushType = imgPush.PushType;
                        content.VGUID = imgPush.VGUID;
                        content.RevenueType = 0;
                        content.PushObject = ps.GetPushUserWeChat(imgPush);//获取推送信息接收者信息列表
                        contentList.Add(content);
                        //子表信息
                        var moreGraphicList = ps.GetMoreGraphicList(imgPush.VGUID);
                        PushFunction.PushFunction pf = PushFunctionFactory.PushFunc(PushMode.WeChat, MsgType.SingleTextAndImg);
                        foreach (var item in moreGraphicList)
                        {
                            U_Content contentGraphic = new U_Content();
                            contentGraphic.Message = item.Message;
                            contentGraphic.Title = item.Title;
                            contentGraphic.CoverDescption = item.CoverDescption;
                            contentGraphic.CoverImg = item.CoverImg;
                            contentGraphic.VGUID = item.VGUID;
                            contentGraphic.RevenueType = item.Ranks;
                            contentList.Add(contentGraphic);
                        }
                        var pushPersonList = contentList[0].PushObject;
                        //如果发送对象超过1000个则需要分批发送
                        if (pushPersonList.Count > 1000)
                        {
                            int pushCount = (pushPersonList.Count / 1000) + 1;//分批发送（发送次数）
                            for (int i = 0; i < pushCount; i++)
                            {
                                contentList[0].PushObject = pushPersonList.Skip(i * 1000).Take(1000).ToList();
                                //定时发送
                                if (contentList[0].TimedSendTime != null)
                                {
                                    if (DateTime.Now >= contentList[0].TimedSendTime)
                                    {
                                        pf.Push(accessTokenModel, contentList);
                                    }
                                }
                                else
                                {
                                    pf.Push(accessTokenModel, contentList);
                                }
                            }

                        }
                        else
                        {
                            //定时发送
                            if (contentList[0].TimedSendTime != null)
                            {
                                if (DateTime.Now >= contentList[0].TimedSendTime)
                                {
                                    pf.Push(accessTokenModel, contentList);
                                }
                            }
                            else
                            {
                                pf.Push(accessTokenModel, contentList);
                            }
                        }
                    }

                    #endregion
                    List<Business_WeChatPush_Information> pushList = ps.GetPushList();//获取要推送的数据
                    //循环推送数据列表（获取要推送人员列表）
                    foreach (var item in pushList)
                    {
                        try
                        {
                            PushFunction.PushFunction pf = PushFunctionFactory.PushFunc((PushMode)item.PushType, (MsgType)item.MessageType);
                            if (item.PushType == 1)//微信
                            {

                                U_Content content = new U_Content();
                                //content.PushObject = "@all";
                                //content.Message = "D:\\567925.png";
                                content.Message = item.Message;
                                content.ExercisesVGUID = item.ExercisesVGUID;
                                content.MessageType = item.MessageType;
                                content.Title = item.Title;
                                content.CoverDescption = item.CoverDescption;
                                content.CoverImg = item.CoverImg;
                                content.PushType = item.PushType;
                                content.VGUID = item.VGUID;
                                content.RevenueType = item.RevenueType;
                                content.RedpacketMoney = item.RedpacketMoney;
                                content.RedpacketType = item.RedpacketType;
                                content.RedpacketMoneyFrom = item.RedpacketMoneyFrom;
                                content.RedpacketMoneyTo = item.RedpacketMoneyTo;
                                List<Business_Personnel_Information> pushPersonList = ps.GetPushUserWeChat(item);//获取推送信息接收者信息列表
                                content.PushObject = pushPersonList;
                                //如果发送对象超过1000个则需要分批发送
                                if (pushPersonList.Count > 1000)
                                {
                                    int pushCount = (pushPersonList.Count / 1000) + 1;//分批发送（发送次数）
                                    for (int i = 0; i < pushCount; i++)
                                    {
                                        content.PushObject = pushPersonList.Skip(i * 1000).Take(1000).ToList();
                                        //定时发送
                                        if (item.TimedSendTime != null)
                                        {
                                            if (DateTime.Now >= item.TimedSendTime)
                                            {
                                                pf.Push(accessTokenModel, content);
                                            }
                                        }
                                        else
                                        {
                                            pf.Push(accessTokenModel, content);
                                        }
                                    }
                                }
                                else
                                {
                                    //定时发送
                                    if (item.TimedSendTime != null)
                                    {
                                        if (DateTime.Now >= item.TimedSendTime)
                                        {
                                            pf.Push(accessTokenModel, content);
                                        }
                                    }
                                    else
                                    {
                                        pf.Push(accessTokenModel, content);
                                    }
                                }

                            }
                            else//短信
                            {
                                List<Business_Personnel_Information> pushPersonList = ps.GetPushUserSms(item);//获取推送信息接收者信息列表
                                List<U_Content> uCountList = new List<U_Content>();
                                foreach (var person in pushPersonList)
                                {
                                    U_Content content = new U_Content();
                                    content.Message = item.Message;
                                    content.MessageType = item.MessageType;
                                    content.Title = item.Title;
                                    content.CoverDescption = item.CoverDescption;
                                    content.CoverImg = item.CoverImg;
                                    content.PushType = item.PushType;
                                    content.VGUID = item.VGUID;
                                    content.PushObject = new List<Business_Personnel_Information>();
                                    content.PushObject.Add(person);
                                    uCountList.Add(content);
                                }
                                //定时发送
                                if (item.TimedSendTime != null)
                                {
                                    if (DateTime.Now >= item.TimedSendTime)
                                    {
                                        pf.Push(accessTokenModel, uCountList);
                                    }
                                }
                                else
                                {
                                    pf.Push(accessTokenModel, uCountList);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogManager.WriteLog(LogFile.Error, ex.ToString());
                        }
                    }
                    Thread.Sleep(5*1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogManager.WriteLog(LogFile.Error, ex.ToString());
            }
        }

        /// <summary>
        /// 同步人员数据
        /// </summary>
        public static void UpdatePersonData()
        {
            while (true)
            {
                bool result = false;
                int time = 0;
                if (DateTime.Now.Hour == 3)//时间为三点的时候time初始化
                {
                    time = 0;
                }
                if (DateTime.Now.Hour == 2 && time == 0)//时间为凌晨两点并且time为0的时候执行同步操作
                {
                    //  LogManager.WriteLog(LogFile.Trace, DateTime.Now.ToString("yyyy-MM-dd") + "开始启动定时同步离职人员服务");
                    try
                    {
                        U_AccessToken accessTokenModel = new U_AccessToken();
                        accessTokenModel.UserKey = ConfigSugar.GetAppString("WeChatCorpID");
                        accessTokenModel.UserPassword = ConfigSugar.GetAppString("WeChatSecret");

                        UpdatePersonLogic _updatePersonLogic = new UpdatePersonLogic();
                        List<V_UserNotexistsmiddata> personList = new List<V_UserNotexistsmiddata>();
                        personList = _updatePersonLogic.GetPeosonList();//获取离职人员列表（存在于Person表而不存在与middata表的人员列表）
                        _updatePersonLogic.UpdateStatus2Focus(personList);  //首先将人员状态改为已关注
                        if (personList.Count >= 10)
                        {
                            result = _updatePersonLogic.UpdatePersonStatus(personList, 3);   //状态更改为未匹配
                        }
                        else if (personList.Count > 0)
                        {
                            result = _updatePersonLogic.DisableWeChatData(accessTokenModel, personList);  //同时将微信禁用
                            if (result)
                            {
                                result = _updatePersonLogic.UpdatePersonStatus(personList, 4);  //状态更改为已离职
                            }
                        }
                        //  result = _updatePersonLogic.DeletePersonData(personList);
                        //if (result)
                        //{
                        //    //删除微信通讯录中人员
                        //  result = _updatePersonLogic.DeleteWeChatData(accessTokenModel, personList);
                        //}
                        //   LogManager.WriteLog(LogFile.Trace, DateTime.Now.ToString("yyyy-MM-dd") + "定时同步离职人员服务启动完成");
                    }
                    catch (Exception exp)
                    {
                        LogManager.WriteLog(LogFile.Error, "同步离职人员数据：" + result + "/n" + exp.ToString());
                    }
                    time++;
                }
            }
        }

        /// <summary>
        /// 从微信后台获取人员的状态，并且将状态同步至数据库中
        /// </summary>
        public static void UpdatePersonStatus()
        {
            bool result = false;
            int time = 0;
            while (true)
            {
                if (DateTime.Now.Hour == 2)
                {
                    time = 0;
                }
                if (DateTime.Now.Hour != 1 || time != 0) continue;  //时间为凌晨1点并且time为0的时候执行同步操作              
                try
                {
                    U_AccessToken accessTokenModel = new U_AccessToken();
                    accessTokenModel.UserKey = ConfigSugar.GetAppString("WeChatCorpID");
                    accessTokenModel.UserPassword = ConfigSugar.GetAppString("WeChatSecret");
                    var accessToken = WeChatTools.GetAccessoken(accessTokenModel);
                    var departments = WeChatTools.GetDepartment(accessToken);     //获取所有的部门
                    UpdatePersonLogic _updatePersonLogic = new UpdatePersonLogic();
                    foreach (var item in departments.department)
                    {
                        var users = WeChatTools.GetUsers(accessToken, item.id.ToString());  //根据部门获取所有的人员
                        result = _updatePersonLogic.UpdatePersonStatus(users.userlist);
                    }
                }
                catch (Exception exp)
                {
                    LogManager.WriteLog(LogFile.Error, "同步人员状态：" + result + "/n" + exp);
                }
                time++;
            }

        }


    }
}
