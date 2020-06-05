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
                return StatusHandler.okResult($"new serviceData Added For {userData.ServiceName}", "success");
            }
        }

        #endregion


        #region UserCreateNewService

        [HttpPost]
        [Route("adduserservice")]
        public async Task<IActionResult> AddUserService(UserServicesProfile userServices)
        {
            if (string.IsNullOrEmpty(userServices.ServiceName))
                return NoContent();

            try
            {
                var userserivces = await _sqlCommandRepo.CreateNewUserService(userServices.ApiKey, userServices.ServiceName);
            }

            catch (ArgumentException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }

            catch (NullReferenceException args)
            {
                return StatusHandler.NotFound(args.Message, "error");
            }


            return StatusHandler.okResult($"new { userServices.ServiceName} created", "success");

        }
        #endregion


        #region UserLogin
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> AdminLogin([FromBody]UserLoginProfile userLogin)
        {
            if (User == null)
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
        public async Task<IActionResult> UserRegistration([FromBody]UserRegisterModel registerUser)
        {
            try
            {
                await _sqlCommandRepo.CreateUser(registerUser);
                await _sqlCommandRepo.SaveChanges();
            }

            catch (SqlNullValueException)
            {
                return StatusHandler.NotFound("Null Parameter Detected", "Error");
            }

            catch (DbUpdateException args)
            {
                return StatusHandler.NotFound(args.Message, "Error");
            }

            catch (Exception)
            {
                return StatusHandler.NotFound("Registration Error Contact Admin", "Error");
            }


            return StatusHandler.okResult($"user {registerUser.FullName} registered successfully", "success");

        }
        #endregion


    }
}