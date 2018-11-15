using Common;
using DaZhongManagementSystem.Entities.TableEntity;
using Entities.UserDefinedEntity;
using Infrastructure.DataServer;
using Service.BusinessLogic;
using Service.PushFunction;
using Service.PushFunction.WeChatPush;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using weixin.PublicAccount;
using SyntacticSugar;

namespace Service
{
    public class Program
    {
        static void Main(string[] args)
        {
            Service.MainFunction.PushContentFunction();
            //Service.MainFunction.UpdatePersonData();
            //Service.MainFunction.UpdatePersonStatus();
        }
    }
}
