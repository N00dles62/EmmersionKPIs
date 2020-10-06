﻿using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;
using DataAccess.DatabaseAccess;
using DataManipulation.DatabaseAccess;
using DataObjects.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KPIWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly UserDataAccess userDataAccess;
        private readonly EmailManager emailManager;
        private const int DuplicateEmail = -1;
        private const int RegistrationError = 0;

        public RegisterController()
        {
            userDataAccess = new UserDataAccess();
            emailManager = new EmailManager();
        }

        // USED FOR TESTING:
        // public RegisterController(UserDataAccess userDataAccess, EmailManager emailManager)
        // {
        //     this.userDataAccess = userDataAccess;
        //     this.emailManager = emailManager;
        // }

        [HttpPost]
        public IActionResult Post(RegisterData data)
        {
            var result = userDataAccess.InsertUserInfo(data.FirstName, data.LastName, data.Email);

            var userInfo = new UserInfo
            {
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName
            };

            if (result != DuplicateEmail && result != RegistrationError)
            {
                emailManager.SendRegistrationEmail(userInfo, data.BaseUrl);
            }

            return result switch
            {
                DuplicateEmail => BadRequest("User with that email already exists."),
                RegistrationError => StatusCode(500, "An error occurred while trying to register. Please try again later."),
                _ => Ok()
            };
        }
    }

    public class RegisterData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string BaseUrl { get; set; }
    }
}
