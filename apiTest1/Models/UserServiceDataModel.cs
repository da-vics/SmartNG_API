using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.Models
{
    public class UserServiceDataModel
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string ServiceName { get; set; }

        public virtual UserServicesModel services { get; set; }


        [Required]
        public decimal ServiceData { get; set; }

        [Required]
        public DateTime DataInsertDat { get; set; }


    }
}
