﻿using System;
using DataAccess.DatabaseAccess;
using DataManipulation.DatabaseAccess;
using Microsoft.AspNetCore.Mvc;

namespace KPIWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForgotPasswordController : ControllerBase
    {
        private readonly UserDataAccess userDataAccess;
        private readonly EmailManager emailManager;

        public ForgotPasswordController()
        {
            this.userDataAccess = new UserDataAccess();
            this.emailManager = new EmailManager();
        }

        // USED FOR TESTING
        // public ForgotPasswordController(UserDataAccess userDataAccess, EmailManager emailManager)
        // {
        //     this.emailManager = emailManager;
        //     this.userDataAccess = userDataAccess;
        // }

        [HttpPost]
        public IActionResult Post(string email)
        {
            try
            {
                var userInfo = userDataAccess.GetUserInfoByEmail(email);
                var result = emailManager.SendForgotPasswordEmail(userInfo, "");

                if (result == false)
                    return StatusCode(500,
                        "An error occurred while trying to send the password reset email. Please try again later.");
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("User with that email does not exist.");
            }
        }
    }
}
