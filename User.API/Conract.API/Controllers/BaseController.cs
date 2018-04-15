﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Dtos;
namespace Contact.API.Controllers
{
    public class BaseController:Controller
    {
        protected UserIdentity UserIdentity => new UserIdentity() { UserId = 1 };
    }
}