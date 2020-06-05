using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.DataProfiles
{
    public class UserServicesProfile
    {
        [Required]
        public string ApiKey { get; set; }
        public string ServiceName { get; set; }

    }
}
