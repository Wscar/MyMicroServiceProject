﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using User.Identity.Service;
namespace User.Identity.Authentication
{
    public class SmaAuthCodeValidaor : IExtensionGrantValidator
    {
        private readonly IAuthCodeService authCodeServce;
        private readonly IUserServices userServices;
        public SmaAuthCodeValidaor(IAuthCodeService _authCodeServce, IUserServices _userServices)
        {
            authCodeServce = _authCodeServce;
            userServices = _userServices;
        }
        public string GrantType => "sms_auth_code";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var phone = context.Request.Raw["phone"];
           var code=  context.Request.AuthorizationCode;
            var auth_code = context.Request.Raw["auth_code"];
            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(auth_code))
            {
                context.Result = errorValidationResult;
                return;
            }
            //验证手机号码
            if (!authCodeServce.Validate(phone, auth_code))
            {
                context.Result = errorValidationResult;
                return;
            }
            //验证用户
            var userInfo = await userServices.CheckOrCreate(phone);
            if (userInfo==null)
            {
                context.Result = errorValidationResult;
                return;
            }
            //返回正确的结果
            var claims = new Claim[]{
                new Claim("Name",userInfo.Name??string.Empty),
                new Claim("Company",userInfo.Company),
                new Claim("Title",userInfo.Title),
                new Claim("Avatar",userInfo.Avatar)
            };
         
            context.Result = new GrantValidationResult(userInfo.Id.ToString(), GrantType,claims );
        }
    }
}
