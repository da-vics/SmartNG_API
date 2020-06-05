using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.DataProfiles
{
    public class UserDataProfile
    {
        [Required]
        public string apikey { get; set; }
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public decimal Userdata { get; set; }
    }
}
