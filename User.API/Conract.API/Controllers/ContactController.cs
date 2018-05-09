using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contact.API.Models;
using Contact.API.Data;
using Contact.API.Service;
using System.Threading;
using Contact.API.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Contact.API.Controllers
{   
    [Authorize]
    [Produces("application/json")]
    [Route("api/Contact")]
    public class ContactController:BaseController
    {
        private readonly IContactApplyRequestRespository requestRespository;
        private readonly IContactRepository contaclRepository;
        private readonly IUserService userService;
       

        public ContactController(IContactApplyRequestRespository _requestRespository, IUserService _userService, IContactRepository _contaclRepository)
        {
            requestRespository = _requestRespository;
            userService = _userService;
            contaclRepository = _contaclRepository;
        }
       
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var user = UserIdentity;

            var result = await contaclRepository.GetContactsAsync(user.UserId);
            return Ok( result);
        }
        [HttpPut]
        [Route("add-tags")]
        public async Task<IActionResult> AddTagsContact([FromBody] TagContactInputViewModel tags)
        {
            var result= await contaclRepository.TagsContactAsync(UserIdentity.UserId, tags.ContactId,tags.Tags);
            if(result)
            {
                return Ok(result);
            }
            //LOG TBD

            return BadRequest();
        }
        [HttpGet]
        [Route("apply-requsets")]
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
       
        public  async Task<IActionResult> GetApplyRequset(CancellationToken cancellationToken)
        {
            var request = await requestRespository.GetRequestListAsync(UserIdentity.UserId,cancellationToken);
            return Ok(request);
        }
        /// <summary>
        /// 添加好友申请
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add-request")]  
       
        public async Task<IActionResult> AddApplyRequset(int userId, CancellationToken cancellationToken)
        {
            var baseUserInfo = await userService.GetBaseUserInfoAsync(userId);
            if (baseUserInfo == null)
            {
                throw new Exception("用户参数错误");
            }
            var result= await requestRespository.AddRequestAsync(new ContactApllyRequest()
            {
                UserId = baseUserInfo.UserId,
                Applierid = this.UserIdentity.UserId,
                Name = baseUserInfo.Name,
                Company = baseUserInfo.Company,
                Avatar = baseUserInfo.Avatar,
                ApplyTime = DateTime.Now,
                Ttile = baseUserInfo.Title
            },cancellationToken);
            if(!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        /// <summary>
        /// 通过好友申请
        /// </summary>
        /// <param name="applierId"></param>
        /// <param name="isApproval"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approval-request/{applierId}")]
        public async Task<IActionResult> AppApprovalRequset( int applierId, string isApproval, CancellationToken cancellationToken)
        {
            var result =await requestRespository.ApprovalAsync(UserIdentity.UserId,applierId,isApproval ,cancellationToken);
            if (result)
            {    
                //获取申请者的用户信息。
                var applier = await this.userService.GetBaseUserInfoAsync(applierId);
                var userInfo = await this.userService.GetBaseUserInfoAsync(UserIdentity.UserId);
                await this.contaclRepository.AddContactAsync(UserIdentity.UserId, userInfo, cancellationToken);
                        
                return Ok();
            }
            //记录日志
            return BadRequest();
        }
    }
}