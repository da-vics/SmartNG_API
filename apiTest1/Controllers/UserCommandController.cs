﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using apiTest1.Data;
using apiTest1.DataProfiles;
using apiTest1.Handlers;
using apiTest1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SmartNG.DataProfiles;

namespace apiTest1.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserCommandController : ControllerBase
    {
        private readonly SqlCommandRepo _sqlCommandRepo;

        #region Constructor
        public UserCommandController()
        {

            _sqlCommandRepo = new SqlCommandRepo(new apiDBContext());

        }
        #endregion


        #region GetSingleServiceData

        [HttpPost]
        [Route("servicedata/getdata")]
        public async Task<IActionResult> GetUserServiceData(GetUserDataProfile getUserData)
        {
            var result = await _sqlCommandRepo.GetUserServiceData(getUserData);

            if (result == null)
                return StatusHandler.NotFound("No Data", "error");

            return Ok(result);
        }

        #endregion


        #region GetUserServices
        [HttpPost]
        [Route("user/getservices")]
        public async Task<IActionResult> GetUserSerivces(GetUserServicesProfile getUserServices)
        {

            List<GetUserServiceDataProfile> result = null;

            try
            {
                result = await _sqlCommandRepo.GetUserServices(getUserServices);
            }

            catch (ArgumentException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }


            if (result == null || result.Count <= 0)
                return StatusHandler.NotFound("No Services", "error");

            return Ok(result);

        }

        #endregion


        #region GetCollectionServiceData

        [HttpPost]
        [Route("servicedata/getcollection")]
        public async Task<IActionResult> GetCollectionUserServiceData(GetCollectionUserDataProfile getUserData)
        {
            var result = await _sqlCommandRepo.GetUserCollatedServiceData(getUserData);


            if (result == null || result.Count <= 0)
                return StatusHandler.NotFound("Invalid Opeartion", "error");

            return Ok(result);
        }

        #endregion


        #region UpdateUserService
        [HttpPost]
        [Route("updateuserservice")]
        public async Task<IActionResult> UpdateUserService(UserServicesProfile userServices)
        {
            if (string.IsNullOrEmpty(userServices.ServiceName) || userServices == null || userServices.DeviceType == null)
                return StatusHandler.NotFound("null Parameter Detected", "error");

            bool result = false;
            try
            {
                result = await _sqlCommandRepo.UpdateUserService(userServices);
            }

            catch (ArgumentException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            catch (NullReferenceException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            if (result)
                return StatusHandler.okResult($"new { userServices.DeviceId} updated", "success");

            else
                return StatusHandler.NotFound("error contact admin!", "error");  /// fix

        }

        #endregion


        #region UserCreateNewService

        [HttpPost]
        [Route("adduserservice")]
        public async Task<IActionResult> AddUserService(UserServicesProfile userServices)
        {
            if (string.IsNullOrEmpty(userServices.ServiceName) || userServices.DeviceType == null)
                return StatusHandler.NotFound("null Parameter Detected", "error");

            bool result = false;
            try
            {
                result = await _sqlCommandRepo.CreateNewUserService(userServices);
            }

            catch (ArgumentException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            catch (NullReferenceException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }


            if (result)
                return StatusHandler.okResult($"new { userServices.ServiceName} created", "success");

            else
                return StatusHandler.NotFound("error contact admin!", "error");  /// fix
        }
        #endregion


        #region UserLogin
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AdminLogin([FromBody]UserLoginProfile userLogin)
        {
            if (userLogin == null || userLogin.Email == null)
                return StatusHandler.NotFound("Null Parameter Detected", "error");
            var Loginresult = await _sqlCommandRepo.VerifyUser(userLogin);

            if (string.IsNullOrEmpty(Loginresult))
                return StatusHandler.NotFound("User Not Found", "error");

            else
            {
                return new OkObjectResult(new { key = Loginresult });
            }

        }
        #endregion


        #region UserRegister
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> UserRegistration([FromBody]UserRegisterProfile registerUser)
        {
            try
            {
                await _sqlCommandRepo.CreateUser(registerUser);
            }
            catch (ArgumentException args)
            {
                return StatusHandler.NotFound(args.Message, "Error");
            }

            catch (SqlNullValueException)
            {
                return StatusHandler.NotFound("Null Parameter Detected", "Error");
            }

            catch (DbUpdateException args)
            {
                return StatusHandler.NotFound(args.Message, "Error");
            }

            catch (Exception args)
            {
                return StatusHandler.NotFound(args.Message, "Error");
            }


            return StatusHandler.okResult($"user {registerUser.FullName} registered successfully", "success");

        }
        #endregion


    }
}