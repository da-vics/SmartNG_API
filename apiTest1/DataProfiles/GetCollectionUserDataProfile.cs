using System;
using System.ComponentModel.DataAnnotations;

namespace apiTest1.DataProfiles
{
    public class GetCollectionUserDataProfile
    {
        [Required]
        public string apikey { get; set; }

        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        public string setRecord { get; set; }
    }
}
