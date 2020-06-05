using apiTest1.DataProfiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiTest1.Controllers
{
    [Route("SmartAdmin/api")]
    [ApiController]
    public class MasterCommandsController : ControllerBase
    {

        public MasterCommandsController()
        {

        }

        [HttpGet]

        public ActionResult SmartAdminTest()
        {
            return Content("Smart Controller");
        }



        [HttpPost]
        [AllowAnonymous]
        [Route("masterlogin")]
        public IActionResult AdminLogin([FromBody]MasterLoginProfile masterLogin)
        {
            return Content("loginPage");
        }

    }
}