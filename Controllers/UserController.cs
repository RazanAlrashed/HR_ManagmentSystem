using HR_ManagmentSystem.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.EntityFrameworkCore;
using HR_ManagmentSystem.Models.BaseModel;
using iText.Commons.Actions.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace HR_ManagmentSystem.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly DatabaseContext _databaseContext; 
        public UserController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Profile()
        {
            var getuserid = User.Identity.Name.Trim(); 
            Console.WriteLine($"Logged-in user (EmployeeNumber): '{getuserid}'");

            // Check if employee exists in the database
            var employee = _databaseContext.Employees
                .FirstOrDefault(e => e.EmployeeNumber == getuserid);

            if (employee == null)
            {
                // Log if employee not found
                Console.WriteLine("Employee not found");

                TempData["msg"] = "Employee information not found.";
                return RedirectToAction(nameof(Login));  // Redirect if employee not found
            }

            return View(employee);
        }

        public async Task<IActionResult> LeaveApplications()
        {
            var getEmployeeId = User.Identity.Name; 

            // Fetching the employee based on the logged-in username
            var employee = await _databaseContext.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == getEmployeeId);

            if (employee == null)
            {
                // If employee not found
                TempData["msg"] = "Employee not found!";
                return RedirectToAction("Display", "Dashboard");
            }

            // Fetching leave applications for the logged-in employee
            var leaveApplications = await _databaseContext.LeaveApplays
                .Where(l => l.EmployeeId == employee.Id)
                .ToListAsync();

            return View(leaveApplications);
        }

        public IActionResult ApplyLeave()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyLeave(LeaveApplay leaveApplication)
        {
            ModelState.Remove("Employee");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(leaveApplication); 
            }
            

            var getEmployeeId = User.Identity.Name;
            var employee = await _databaseContext.Employees
                .FirstOrDefaultAsync(e => e.EmployeeNumber == getEmployeeId);

            if (employee == null)
            {
                TempData["msg"] = "Employee not found!";
                return RedirectToAction(nameof(LeaveApplications));
            }

            leaveApplication.EmployeeId = employee.Id;
            leaveApplication.Status = "Pending";

            try
            {
                _databaseContext.LeaveApplays.Add(leaveApplication);
                await _databaseContext.SaveChangesAsync();

                TempData["msg"] = "Leave application submitted successfully.";
                return RedirectToAction(nameof(LeaveApplications));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving leave application: {ex.Message}");
                TempData["msg"] = "An error occurred while submitting your leave application.";
                return View(leaveApplication);
            }
        }

    }
}
