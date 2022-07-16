using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using App.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AdministrationController> logger;
        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                         UserManager<ApplicationUser> userManager,
                                         ILogger<AdministrationController> logger,
                                         SignInManager<ApplicationUser> signInManager)

        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult RegisterUser()
        {
            return View();
        }

        //[Authorize (Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Copy data from registerviewmodel to the identityuser
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email,
                };

                //Storing user data in aspnetusers database table
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToPage("ListUsers", "Administration");
                }

                //If user is successfully created, signin the user using
                //Signinmanager and rediect to index action of homecontroller

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListRoles");
            }
        }



        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");
            }
        }

        

        //[HttpGet]
        //public IActionResult ListUsers()
        //{

        //    var users = userManager.Users;
        //    return View(users);
        //}

    }

}

    
