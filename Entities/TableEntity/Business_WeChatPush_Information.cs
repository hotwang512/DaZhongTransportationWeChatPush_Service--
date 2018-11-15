using System;
namespace DaZhongManagementSystem.Entities.TableEntity
{
    public class Business_WeChatPush_Information
    {

        public int PushType { get; set; }

        public string Title { get; set; }

        public int MessageType { get; set; }

        public Boolean Timed { get; set; }

        public DateTime? TimedSendTime { get; set; }

        public Boolean Important { get; set; }

        public string Message { get; set; }

        public string CoverImg { get; set; }

        public string CoverDescption { get; set; }

        public DateTime PeriodOfValidity { get; set; }

        public DateTime? PushDate { get; set; }

        public string PushPeople { get; set; }

        public int Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ChangeUser { get; set; }

        public Guid VGUID { get; set; }

        public Guid ExercisesVGUID { get; set; }//习题Vguid

        public string History { get; set; }   //推送历史

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