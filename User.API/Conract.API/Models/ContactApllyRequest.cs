using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Models
{
    /// <summary>
    /// 好友申请
    /// </summary>
    public class ContactApllyRequest
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 用户职位
        /// </summary>
        public string Ttile { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 申请人ID
        /// </summary>
        public int Applierid { get; set; }
        /// <summary>
        /// 0 申请通过，1申请为通过
        /// </summary>
        public string Approval { get; set; }
        public DateTime HandleTime { get; set; }
        public DateTime ApplyTime { get; set; }
    }
}
