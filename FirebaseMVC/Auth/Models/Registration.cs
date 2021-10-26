using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CanYouEvenWin.Auth.Models
{
    public class Registration
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ImageLocation { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [Compare(nameof(Password))] //make sure both values are the same
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
