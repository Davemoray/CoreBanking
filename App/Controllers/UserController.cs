using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
            private readonly RoleManager<ApplicationRole> roleManager;
            private readonly UserManager<ApplicationUser> userManager;
            private readonly SignInManager<ApplicationUser> signInManager;
            private readonly ILogger<AdministrationController> logger;
            public UserController(RoleManager<ApplicationRole> roleManager,
                                             UserManager<ApplicationUser> userManager,
                                             ILogger<AdministrationController> logger,
                                             SignInManager<ApplicationUser> signInManager)

            {
                this.userManager = userManager;
                this.roleManager = roleManager;
                this.logger = logger;
                this.signInManager = signInManager;
            }

            [Authorize(Roles ="Admin")]
            [HttpGet]
            public async Task<IActionResult> EditUser(string id)
            {
                var user = await userManager.FindByIdAsync(id);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                    return View("NotFound");
                }

                // GetClaimsAsync retunrs the list of user Claims
                var userClaims = await userManager.GetClaimsAsync(user);
                // GetRolesAsync returns the list of user Roles
                var userRoles = await userManager.GetRolesAsync(user);

                var model = new EditUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.FirstName + "" + user.LastName,
                    Claims = userClaims.Select(c => c.Value).ToList(),
                    Roles = userRoles
                };

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> EditUser(EditUserViewModel model)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListUsers");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }

            [Authorize(Roles = "Admin")]
            [HttpGet]
            public async Task<IActionResult> ManageUserRoles(string userId)
            {
                ViewBag.userId = userId;

                var user = await userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                    return View("NotFound");
                }

                var model = new List<UserRolesViewModel>();

                foreach (var role in roleManager.Roles)
                {
                    var userRolesViewModel = new UserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };

                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRolesViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRolesViewModel.IsSelected = false;
                    }

                    model.Add(userRolesViewModel);
                }

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                    return View("NotFound");
                }

                var roles = await userManager.GetRolesAsync(user);
                var result = await userManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing roles");
                    return View(model);
                }

                result = await userManager.AddToRolesAsync(user,
                    model.Where(x => x.IsSelected).Select(y => y.RoleName));

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected roles to user");
                    return View(model);
                }

                return RedirectToAction("EditUser", new { Id = userId });
            }

            
            [HttpGet]
            public async Task<IActionResult> ManageUserClaims(string userId)
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                    return View("NotFound");
                }

                // UserManager service GetClaimsAsync method gets all the current claims of the user
                var existingUserClaims = await userManager.GetClaimsAsync(user);
           

                var model = new UserClaimsViewModel
                {
                    UserId = userId
                };

                // Loop through each claim we have in our application
                foreach (Claim claim in ClaimsStore.AllClaims)
                {
                    UserClaim userClaim = new UserClaim
                    {
                        ClaimType = claim.Type
                    };

                    // If the user has the claim, set IsSelected property to true, so the checkbox
                    // next to the claim is checked on the UI
                    if (existingUserClaims.Any(c => c.Type == claim.Type))
                    {
                        userClaim.IsSelected = true;
                    }

                    model.Claims.Add(userClaim);
                }

                return View(model);

            }

            [HttpPost]
            public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
            {
                var user = await userManager.FindByIdAsync(model.UserId);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                    return View("NotFound");
                }

                // Get all the user existing claims and delete them
                var claims = await userManager.GetClaimsAsync(user);
                var result = await userManager.RemoveClaimsAsync(user, claims);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing claims");
                    return View(model);
                }

                // Add all the claims that are selected on the UI
                result = await userManager.AddClaimsAsync(user,
                    model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected claims to user");
                    return View(model);
                }

                return RedirectToAction("EditUser", new { Id = model.UserId });

            }

            [HttpGet]
            public async Task<IActionResult> ListUsers()
            {
                var users = await userManager.Users.ToListAsync();
                var userRolesViewModel = new List<EditUserViewModel>();

                foreach (ApplicationUser user in users)
                {
                    var thisViewModel = new EditUserViewModel();
                    thisViewModel.Status = user.IsEnabled;
                    thisViewModel.FirstName = user.FirstName;
                    thisViewModel.LastName = user.LastName;
                    thisViewModel.Id = user.Id;
                    thisViewModel.Email = user.Email;
                    thisViewModel.Roles = await GetUserRoles(user);
                    userRolesViewModel.Add(thisViewModel);
                }
                return View(userRolesViewModel);
            }

            [HttpPost]
            public async Task<IActionResult> ListUsers(string id, EditUserViewModel model)
            {
            var user = await userManager.FindByIdAsync(id);
                
            if (id != user.Id)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            if(ModelState.IsValid)
            {
                user.Id = model.Id;
                user.IsEnabled = model.Status;
                return RedirectToAction(nameof(ListUsers));
            }
            return View(model);
            }


            public async Task<IActionResult> DisableUser(string id)
            {
                var user = await userManager.FindByIdAsync(id); 
                if (user.IsEnabled == false )
                    {
                       await userManager.SetLockoutEnabledAsync(user, true);
                    }
                return View("ListUsers");
            
            }

            private async Task<IList<string>> GetUserRoles(ApplicationUser user)
                {
                    return new List<string>(await userManager.GetRolesAsync(user));
                }
        }

    }


