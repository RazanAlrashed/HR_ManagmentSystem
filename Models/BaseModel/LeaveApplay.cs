using System.ComponentModel.DataAnnotations;

namespace HR_ManagmentSystem.Models.BaseModel
{
    public class LeaveApplay
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int NumberOfDay { get; set; }
        [Required]
        public string Reason { get; set; }
        public string Status { get; set; }
        [Required]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

    }
}
