using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

            return Content("HomePage");
        }

    }
}