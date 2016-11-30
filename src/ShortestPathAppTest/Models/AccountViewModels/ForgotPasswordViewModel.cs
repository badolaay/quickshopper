using System.ComponentModel.DataAnnotations;

namespace QuickShopper.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
