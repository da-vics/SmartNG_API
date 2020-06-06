using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.DataProfiles
{
    public class FiledDeivceProfile
    {
        [Required]
        public Guid DeviceId { get; set; }
    }
}
