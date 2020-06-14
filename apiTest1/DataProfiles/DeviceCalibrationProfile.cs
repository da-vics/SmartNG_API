using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmartNG.DataProfiles
{
    public class DeviceCalibrationProfile
    {
        [Required]
        public string ApiKey { get; set; }

        [Required]
        public short DeviceType { get; set; }
    }
}
