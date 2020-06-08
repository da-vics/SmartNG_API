using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNG.DataProfiles
{
    public class UserRegisterProfile
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string PassWordHash { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string HomeAddress { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
