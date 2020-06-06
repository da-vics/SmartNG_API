
using System.ComponentModel.DataAnnotations;

namespace apiTest1.DataProfiles
{
    public class FieldRegisterProfile
    {
        [Required]
        public string MasterKey { get; set; }
    }
}
