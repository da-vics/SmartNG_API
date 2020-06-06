using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.Models
{
    public class UserRegisterModel
    {
        [Key]

        public string Email { get; set; }

        [Required]
        [MaxLength(100)]

        public string PassWordHash { get; set; }

        [Required]
        [MaxLength(100)]
        [DefaultValue(false)]
        public string ApiKeyId { get; set; }


        public ICollection<UserServicesModel> services { get; set; }


        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(300)]
        public string HomeAddress { get; set; }

        [Required]
        [MaxLength(30)]
        public string PhoneNumber { get; set; }

    }
}
