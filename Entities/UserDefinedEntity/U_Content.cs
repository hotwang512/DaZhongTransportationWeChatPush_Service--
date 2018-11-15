using DaZhongManagementSystem.Entities.TableEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.UserDefinedEntity
{
    public class U_Content
    {
        /// <summary>
        /// 推送类型
        /// </summary>
        public int PushType { get; set; }

        /// <summary>
        /// 推送标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 推送信息类型
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 是否定时
        /// </summary>
        public Boolean Timed { get; set; }

        /// <summary>
        /// 定时发送时间
        /// </summary>
        public DateTime TimedSendTime { get; set; }

        /// <summary>
        /// 是否重要
        /// </summary>
        public Boolean Important { get; set; }

        /// <summary>
        /// 推送内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImg { get; set; }

        /// <summary>
        /// 封面描述
        /// </summary>
        public string CoverDescption { get; set; }

        /// <summary>
        /// 推送信息有效时间
        /// </summary>
        public DateTime PeriodOfValidity { get; set; }

        /// <summary>
        /// 推送时间
        /// </summary>
        public DateTime? PushDate { get; set; }

        /// <summary>
        /// 推送人
        /// </summary>
        public string PushPeople { get; set; }

        /// <summary>
        /// 信息状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 推送信息Vguid
        /// </summary>
        public Guid VGUID { get; set; }

        /// <summary>
        /// 习题Vguid
        /// </summary>
        public Guid ExercisesVGUID { get; set; }

        /// <summary>
        /// 推送对象
        /// </summary>
        public List<Business_Personnel_Information> PushObject { get; set; }

        /// <summary>
        /// 上传素材ID
        /// </summary>
        public string Media_Id { get; set; }
        /// <summary>
        /// 短信验证码消息ID
        /// </summary>
        public string Msg_ID { get; set; }

        /// <summary>
        /// 营收类型
        /// </summary>
        public int RevenueType { get; set; }

        /// <summary>
        /// 红包类型
        /// </summary>
        public int? RedpacketType { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        public decimal? RedpacketMoney { get; set; }

        /// <summary>
        /// 红包区间从
        /// </summary>
        public decimal? RedpacketMoneyFrom { get; set; }

        /// <summary>
        /// 红包区间到
        /// </summary>
        public decimal? RedpacketMoneyTo { get; set; }

    }
}
