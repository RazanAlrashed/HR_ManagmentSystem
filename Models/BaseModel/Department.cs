using System.ComponentModel.DataAnnotations;

namespace HR_ManagmentSystem.Models.BaseModel
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        public string DepartmentName { get; set; }
    }
}
