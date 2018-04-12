using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class UserProperty
    {
        int? _requserHasCode;
        public int APPUserId { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserProperty))
                return false;
            UserProperty item = obj as UserProperty;
            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.Key == this.Value && item.Value == this.Value;
            }
          
        }
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requserHasCode.HasValue)
                {
                    _requserHasCode = (this.Value + this.Key).GetHashCode();
                    return _requserHasCode.Value;
                }
            }
            return base.GetHashCode();
        }
        public static bool operator == (UserProperty left,UserProperty right)
        {
            if (Object.Equals(left, null))
            {
                return (Object.Equals(right, null)) ? true : false;
            }
            else
            {
                return left.Equals(right);
            }
        }
        public static bool operator !=(UserProperty left,UserProperty right)
        {
            if (left.Key == right.Key && left.Value == right.Value)
            {
                return false;
            }
            return true;
        }
        public bool IsTransient()
        {
            return string.IsNullOrEmpty(this.Key) || string.IsNullOrEmpty(this.Value);
        }
    }
    public class UserPropertiesComparer : IEqualityComparer<UserProperty>
    {
        public bool Equals(UserProperty x, UserProperty y)
        {
            return x.Key == y.Key && x.Value == y.Value;
        }

        public int GetHashCode(UserProperty obj)
        {
            return (obj.Value + obj.Key).GetHashCode();
        }
    }
}
