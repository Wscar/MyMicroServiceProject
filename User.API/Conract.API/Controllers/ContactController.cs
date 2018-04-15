﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contact.API.Models;
using Contact.API.Data;
using Contact.API.Service;
namespace Contact.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Contact")]
    public class ContactController:BaseController
    {
        private readonly IContactApplyRequestRespository requestRespository;
        private readonly IUserService userService;
        public ContactController(IContactApplyRequestRespository _requestRespository, IUserService _userService)
        {
            requestRespository = _requestRespository;
            userService = _userService;
        }
        [HttpGet]
        [Route("apply-requsets")]
        /// <summary>
        /// 获取好友申请列表
        /// </summary>
        /// <returns></returns>
       
        public  async Task<IActionResult> GetApplyRequset()
        {
            var request = await requestRespository.GetRequestListAsync(UserIdentity.UserId);
            return Ok(request);
        }
        /// <summary>
        /// 添加好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("add-request")]      
        public async Task<IActionResult> AddApplyRequset(int userId)
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
                CreateTime = DateTime.Now,
                Ttile = baseUserInfo.Title
            });
            if(!result)
            {
                return BadRequest();
            }
            return Ok();
        }
        /// <summary>
        /// 通过好友请求
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("approval-request")]
        public async Task<IActionResult> AppApprovalRequset(int applierId)
        {
            var result =await requestRespository.ApprovalAsync(applierId);
            if (result)
            {
                return Ok();
            }
            //记录日志
            return BadRequest();
        }
    }
}