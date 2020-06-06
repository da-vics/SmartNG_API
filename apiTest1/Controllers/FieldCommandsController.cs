using apiTest1.Data;
using apiTest1.DataProfiles;
using apiTest1.Handlers;
using apiTest1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Route("getassociateduser")]
        public async Task<IActionResult> AdminGetAssociateUser([FromBody]FiledDeivceProfile deviceid)
        {
            if (deviceid.DeviceId == null || deviceid == null)
                return StatusHandler.NotFound("null parameter detected", "error");

            var result = await _sqlCommandRepo.GetFieldUserKey(deviceid);

            if (string.IsNullOrEmpty(result))
                return StatusHandler.NotFound("access denied", "error");

            else
                return new OkObjectResult(new { Key = result, status = "success" });

        }

    }
}