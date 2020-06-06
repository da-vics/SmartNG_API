using System;
using System.ComponentModel.DataAnnotations;

namespace apiTest1.DataProfiles
{
    public class UserDataProfileConsumption
    {
        [Required]
        public decimal Userdata { get; set; }

        [Required]
        public string DateInserted { get; set; }
    }
}
