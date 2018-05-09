using Contact.API.Data;
using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Event
{
    public class UserProfileChangeEventHandle:ICapSubscribe
    {
        private IContactRepository contactRepository;
        public UserProfileChangeEventHandle(IContactRepository _contactRepository)
        {
            contactRepository = _contactRepository;
        }
        [CapSubscribe("userapi.userProfileChangeEvent")]
        public void UpdateUserInfo(Dtos.BaseUserInfo userInfo)
        {
            contactRepository.UpdateContactInfoAsync(userInfo, new System.Threading.CancellationToken());
        }
    }
}
