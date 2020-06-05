using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.Handlers
{
    public static class StatusHandler
    {
        /// <summary>
        /// Custom Error Handler
        /// </summary>
        /// <param name="errormsg"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static NotFoundObjectResult NotFound(string errormsg, string statusCode)
        {

            return new NotFoundObjectResult(new { statusCode = statusCode, Message = errormsg });
        }

        public static OkObjectResult okResult(string message, string status)
        {
            return new OkObjectResult(new { statusCode = status, Message = message });
        }

    }
}
