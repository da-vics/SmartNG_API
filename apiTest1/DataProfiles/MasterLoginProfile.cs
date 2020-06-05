
using System.ComponentModel.DataAnnotations;

namespace apiTest1.DataProfiles
{
    public class MasterLoginProfile
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string MasterKey { get; set; }
    }
}
