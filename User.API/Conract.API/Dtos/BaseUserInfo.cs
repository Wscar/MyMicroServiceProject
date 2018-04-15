using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Dtos
{
    public class BaseUserInfo
    {   
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// y用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 用户公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 用户职位
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }
    }
}
