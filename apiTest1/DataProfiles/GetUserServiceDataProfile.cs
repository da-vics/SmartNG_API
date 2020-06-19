using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNG.DataProfiles
{
    public class GetUserServiceDataProfile
    {
        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        public string ServiceName { get; set; }
    }
}
