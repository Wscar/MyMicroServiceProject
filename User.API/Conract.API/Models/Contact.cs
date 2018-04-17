using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Models
{
    public class Contact
    {   public Contact()
        {
            Tasg = new List<string>();
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public  int UserId { get; set; }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public  string Name { get; set; }
        /// <summary>
        /// 用户公司
        /// </summary>
        public  string Company { get; set; }

        /// <summary>
        /// 用户职位
        /// </summary>
        public string Ttile { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 用户标签  
        /// </summary>
        public List<String > Tasg { get; set; }
    }
}
