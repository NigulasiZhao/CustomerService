using System.ComponentModel.DataAnnotations;

namespace AfarsoftResourcePlan.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}