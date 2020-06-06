using System;
using System.ComponentModel.DataAnnotations;

namespace apiTest1.DataProfiles
{
    public class GetUserDataProfile
    {
        [Required]
        public string apikey { get; set; }

        [Required]
        public Guid DeviceId { get; set; }
    }
}
