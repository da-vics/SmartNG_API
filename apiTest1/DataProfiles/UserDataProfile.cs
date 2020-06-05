using System.ComponentModel.DataAnnotations;

namespace apiTest1.DataProfiles
{
    public class UserDataProfile
    {
        [Required]
        public string apikey { get; set; }
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public decimal Userdata { get; set; }
    }
}
