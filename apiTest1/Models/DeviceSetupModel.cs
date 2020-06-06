using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.Models
{
    public class DeviceSetupModel
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class MasterKeys
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string accessKey { get; set; }

    }
}
