using System.ComponentModel.DataAnnotations;

namespace apiTest1.DataProfiles
{
    public class UserServicesProfile
    {
        [Required]
        public string ApiKey { get; set; }
        [Required]
        public string ServiceName { get; set; }

    }
}
