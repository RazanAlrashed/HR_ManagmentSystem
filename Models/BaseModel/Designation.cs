using System.ComponentModel.DataAnnotations;

namespace HR_ManagmentSystem.Models.BaseModel
{
    public class Designation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Designations { get; set; }
    }
}
