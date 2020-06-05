using Microsoft.AspNetCore.Mvc;


namespace apiTest1.Controllers
{
    [Route("Home")]
    [ApiController]
    public class FrontEndController : Controller
    {

        public FrontEndController()
        {

        }


        [HttpGet]
        public IActionResult HomePage()
        {

            return Content("SmartNG APi Home");
        }

    }
}