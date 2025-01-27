using System.ComponentModel.DataAnnotations;

namespace HR_ManagmentSystem.Models.DTO
{
    public class LoginModel
    {
        [Required]
        public string EmployeeNumber { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
