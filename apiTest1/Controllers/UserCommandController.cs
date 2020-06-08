using System;
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
                return StatusHandler.NotFound("Invalid Opeartion", "error");

            return Ok(result);
        }

        #endregion


        #region GetCollctionServiceData

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


        #region UserUploadServiceData

        [HttpPost]
        [Route("userdataupload")]
        public async Task<IActionResult> UploadUserServiceData([FromBody] UserDataProfile userData)
        {
            if (userData == null)
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


        #region UserCreateNewService

        [HttpPost]
        [Route("adduserservice")]
        public async Task<IActionResult> AddUserService(UserServicesProfile userServices)
        {
            if (string.IsNullOrEmpty(userServices.ServiceName))
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