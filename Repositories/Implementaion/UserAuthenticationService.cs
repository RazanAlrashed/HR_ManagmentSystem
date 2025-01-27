using System.Security.Claims;
using HR_ManagmentSystem.Models.Domain;
using HR_ManagmentSystem.Models.DTO;
using HR_ManagmentSystem.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;

namespace HR_ManagmentSystem.Repositories.Implementaion
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserAuthenticationService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<Status> RegistrationAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.EmployeeNumber);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exists";
                return status;
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                Name = model.Name,
                EmployeeNumber = model.EmployeeNumber,
                EmailConfirmed = true,

            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed ";
                return status;
            }

            // role managment
            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }


            status.StatusCode = 1;
            status.Message = "User has registered successfully";
            return status;

        }


        public async Task<Status> LoginAsync(LoginModel model)
        {
            var status =new Status();
            var user = await userManager.FindByNameAsync(model.EmployeeNumber);
            if (user == null) 
            {
                status.StatusCode = 0;
                status.Message = "Invalid username";
                return status;
            }
            // match password
            if (!await userManager.CheckPasswordAsync(user , model.Password))
            {
                status.StatusCode = 0;
                status.Message = "Invalid password";
                return status;
            }

            var SignInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (SignInResult.Succeeded)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.EmployeeNumber)
                };
                foreach (var userRole in userRoles)
                { 
                    authClaims.Add(new Claim(ClaimTypes.Role,userRole));
                }
                status.StatusCode = 1;
                status.Message = "logged in succesfully";
            }
            else if (SignInResult.IsLockedOut)
            {
                status.StatusCode = 0;
                status.Message = "User Locked out";
            }
            else
            {
                status.StatusCode = 0;
                status.Message = "Error on loggin in";
            }
            return status;

        }

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username)
        {
            var status = new Status();

            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;

        }


        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

    }
}
