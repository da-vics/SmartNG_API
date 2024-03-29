﻿using apiTest1.Data;
using apiTest1.DataProfiles;
using apiTest1.Handlers;
using apiTest1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartNG.DataProfiles;
using System;
using System.Threading.Tasks;

namespace apiTest1.Controllers
{
    [Route("fieldadmin/api")]
    [ApiController]
    public class FieldCommandsController : ControllerBase
    {
        private readonly SqlCommandRepo _sqlCommandRepo;

        public FieldCommandsController()
        {
            _sqlCommandRepo = new SqlCommandRepo(new apiDBContext());
        }

        [HttpGet]

        public ActionResult SmartAdminTest()
        {
            return Content("Smart Controller");
        }

        #region AddNewDevice
        [HttpPost]
        [Route("RegisterFieldDevice")]
        public async Task<IActionResult> AdminLogin([FromBody]FieldRegisterProfile fieldRegister)
        {
            if (string.IsNullOrEmpty(fieldRegister.MasterKey))
                return StatusHandler.NotFound("null Parameter Detected", "error");

            string result = string.Empty;
            try
            {
                result = await _sqlCommandRepo.CreateFieldDevice(fieldRegister);
            }

            catch (ArgumentException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            catch (NullReferenceException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            if (string.IsNullOrEmpty(result))
                return StatusHandler.NotFound("access denied", "error");

            else
                return new OkObjectResult(new { DeviceID = result, status = "success" });
        }

        #endregion


        #region UserUploadServiceData

        [HttpPost]
        [Route("userdataupload")]
        public async Task<IActionResult> UploadUserServiceData([FromBody] UserDataProfile userData)
        {
            if (userData == null || userData.Userdata == null)
                return StatusHandler.NotFound("Null Parameter Detected", "error");

            bool checkResult = false;
            try
            {
                checkResult = await _sqlCommandRepo.AddUserServiceData(userData);
            }

            catch (NullReferenceException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            catch (DbUpdateException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            if (checkResult == false)
                return StatusHandler.NotFound("Service Error", "error");

            else
            {
                return StatusHandler.okResult($"new serviceData Added For {userData.DeviceId}", "success");
            }
        }

        #endregion


        #region GetAssignedUser

        [HttpPost]
        [Route("getassociateduser")]
        public async Task<IActionResult> AdminGetAssociateUser([FromBody]FiledDeivceProfile deviceid)
        {
            if (deviceid.DeviceId == null || deviceid == null)
                return StatusHandler.NotFound("null parameter detected", "error");

            DeviceCalibrationProfile result = null;
            try
            {
                result = await _sqlCommandRepo.GetFieldUserKey(deviceid);
            }

            catch (ArgumentException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            if (result == null)
                return StatusHandler.NotFound("access denied", "error");

            else
                return new OkObjectResult(result);

        }

        #endregion

    }

}