using System.ComponentModel.DataAnnotations;

namespace HR_ManagmentSystem.Models.BaseModel
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string EmployeeNumber { get; set; } 
        [Required]
        public string EmployeeName { get; set; } 
        [Required]
        public string Email { get; set; } 
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public int Department { get; set; } //forgin key
        [Required]
        public int DesignationName { get; set; } //forgin key
        [Required]
        public decimal Salary { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string BankAccountNumber { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public string Address { get; set; }

    }
}
