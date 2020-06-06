using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.Models
{
    public class UserServicesModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]

        public string ServiceName { get; set; }


        [Required]
        [MaxLength(100)]
        public string ApiKeyId { get; set; }

        public UserRegisterModel Users { get; set; }

        [Required]
        public Guid DeviceId { get; set; }
        public ICollection<UserServiceDataModel> servicesData { get; set; }
    }
}
