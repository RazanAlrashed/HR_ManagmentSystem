
using HR_ManagmentSystem.Models.Domain;
using HR_ManagmentSystem.Models.DTO;
using HR_ManagmentSystem.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR_ManagmentSystem.Controllers
{
    
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;
        private readonly DatabaseContext _dbContext;

        public UserAuthenticationController(IUserAuthenticationService service, DatabaseContext databaseContext)
        {
            this._service=service;
            this._dbContext=databaseContext;
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid) 
                return View(model);
            model.Role = "user";
            var result = await _service.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _service.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Display","Dashboard");


            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));

            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _service.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        
        public async Task<IActionResult> Reg()
        {
            var model = new RegistrationModel
            {
                EmployeeNumber = "admin1",
                Name = "Razan",
                Email = "doc@gmail.com",
                Password = "Admin@12345#",

            };
            model.Role = "admin";
            var result = await _service.RegistrationAsync(model);
            return Ok(result);

        }
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _service.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }


    }
}
