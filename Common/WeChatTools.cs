using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Entities.UserDefinedEntity;
using ServiceLog;

namespace Common
{
    public class WeChatTools
    {
        /// <summary>
        /// 获取公司AccessToken
        /// </summary>
        /// <returns></returns>
        public static string GetAccessoken(U_AccessToken accessToken)
        {
            //获取AccessTokenUrl
            string tokenUrl = "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}";
            string respText = "";
            //获取josn数据
            string url = string.Format(tokenUrl, accessToken.UserKey, accessToken.UserPassword);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.Default);
                respText = reader.ReadToEnd();
                resStream.Close();
                reader.Close();
            }
            try
            {
                JavaScriptSerializer Jss = new JavaScriptSerializer();
                Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
                //通过键access_token获取值
                return respDic["access_token"].ToString();
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Error, ex.Message);
                return "";
            }
        }

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <param name="accessToken"></param>
        public static U_Deparment GetDepartment(string accessToken)
        {
            var departmentUrl = "https://qyapi.weixin.qq.com/cgi-bin/department/list?access_token={0}";
            var url = string.Format(departmentUrl, accessToken);
            HttpClient client = new HttpClient();
            var responseMesage = client.GetAsync(url);
            responseMesage.Wait();
            var taskStr = responseMesage.Result.Content.ReadAsStringAsync();
            taskStr.Wait();
            var department = taskStr.Result.JsonToModel<U_Deparment>();
            return department;
        }


        /// <summary>
        /// 获取部门成员详情
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="departmentId"></param>
        public static U_User GetUsers(string accessToken, string departmentId)
        {
            var userUrl = "https://qyapi.weixin.qq.com/cgi-bin/user/list?access_token={0}&department_id={1}";
            var url = string.Format(userUrl, accessToken, departmentId);
            HttpClient client = new HttpClient();
            var responseMesage = client.GetAsync(url);
            responseMesage.Wait();
            var taskStr = responseMesage.Result.Content.ReadAsStringAsync();
            taskStr.Wait();
            var user = taskStr.Result.JsonToModel<U_User>();
            return user;
        }
    }
}