using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class APPUser
    {
        public APPUser()
        {
            UserProperties = new List<UserProperty>();
        }
        public int Id { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 头衔
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }


        public string Avatar { get; set; }

        public byte Gender { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
         
        public string Province { get; set; }

        public int CityId { get; set; }
        public string NameCard { get; set; }
        public  List<UserProperty> UserProperties { get; set; }
    }
}
