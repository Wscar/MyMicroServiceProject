using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API
{
    public class UserOperationException:Exception
    {
        public UserOperationException()
        {

        }
        public UserOperationException(string msg) : base(msg)
        {

        }
        public UserOperationException(string msg,Exception innerException):base(msg,innerException)
        {

        }
    }
}
