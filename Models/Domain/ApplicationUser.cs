using Microsoft.AspNetCore.Identity;

namespace HR_ManagmentSystem.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string EmployeeNumber { get; set; }
        public string ? ProfilePicture {  get; set; }
    }
}
