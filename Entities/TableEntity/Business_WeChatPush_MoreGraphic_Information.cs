using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.TableEntity
{
    public class Business_WeChatPush_MoreGraphic_Information
    {

        /// <summary>
        /// 推送标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 推送消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Ranks { get; set; }

        /// <summary>
        /// 主信息vguid
        /// </summary>
        public Guid WeChatPushVguid { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImg { get; set; }

        /// <summary>
        /// 封面描述
        /// </summary>
        public string CoverDescption { get; set; }

        public Guid VGUID { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? ChangeDate { get; set; }

        public string ChangeUser { get; set; }

    }
}
