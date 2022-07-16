using App.Models;
using App.Models.ViewModels;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace App.Controllers
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
            private readonly RoleManager<ApplicationRole> roleManager;
            private readonly UserManager<ApplicationUser> userManager;
            private readonly SignInManager<ApplicationUser> signInManager;
            private readonly ILogger<AdministrationController> logger;
            public RoleController(RoleManager<ApplicationRole> roleManager,
                                             UserManager<ApplicationUser> userManager,
                                             ILogger<AdministrationController> logger,
                                             SignInManager<ApplicationUser> signInManager)

            {
                this.userManager = userManager;
                this.roleManager = roleManager;
                this.logger = logger;
                this.signInManager = signInManager;
            }

            [Authorize(Roles = "Admin")]
            public IActionResult CreateRole()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
            {
                if (ModelState.IsValid)
                {
                    // We just need to specify a unique role name to create a new role
                    ApplicationRole applicationRole = new ApplicationRole
                    {
                        Name = model.RoleName
                    };

                    // Saves the role in the underlying AspNetRoles table
                    IdentityResult result = await roleManager.CreateAsync(applicationRole);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles", "Role");
                    }

                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return View(model);
            }

            [HttpGet]
            public async Task <IActionResult> ListRoles()
            {
            var roles = await roleManager.Roles.ToListAsync();
            foreach (ApplicationRole role in roles)
            {
                var roleName = role.Name;
                var IsEnabled = role.IsEnabled;
                var roleId = role.Id; 
                
            }
                //var roles = roleManager.Roles;
                return View(roles);
            }


            [HttpPost]
            public async Task<IActionResult> ListRoles(string id, ApplicationRole model)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (id != role.Id)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                role.Name = model.Name;
                role.IsEnabled = model.IsEnabled;
                return RedirectToAction(nameof(ListRoles));
            }
            return View(model);
        }

            
            [Authorize(Roles ="Admin")]
            [HttpGet]
            public async Task<IActionResult> EditRole(string id)
            {
            // var role = await roleManager.FindByIdAsync(id);
            //if (role == null)
            //{
            //  ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
            //return View("NotFound");
            //}

            //var model = new EditRoleViewModel
            //{
            //  RoleId = role.Id,
            // RoleName = role.Name
            //};

            // foreach (var user in userManager.Users)
            //{
            //  if (await userManager.IsInRoleAsync(user, role.Name))
            //{
            // model.Users.Add(user.UserName);
            //}

            //}
            //return View(model);

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            var RoleClaims = await roleManager.GetClaimsAsync(role);
            var UserRoles = await roleManager.GetRoleNameAsync(role);

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Claims = RoleClaims.Select(c => c.Value).ToList(),
            };

            foreach (var user in userManager.Users)
            {
               var Userss = $"{user.FirstName + " " + user.LastName + " " + $"({user})"}";

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(Userss);
                }
            }

            return View(model);
            }


            [HttpPost]
            public async Task<IActionResult> EditRole(EditRoleViewModel model)
            {
                var role = await roleManager.FindByIdAsync(model.Id);
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = model.RoleName;
                    var result = await roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            [Authorize(Roles ="Admin")]
            [HttpGet]
            public async Task<IActionResult> ManageUsersInRole(string roleId)
            {
                ViewBag.roleId = roleId;

                var role = await roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                    return View("NotFound");
                }

                var model = new List<UserRoleViewModel>();

                foreach (var user in userManager.Users)
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = user.Id,
                        FullName = $"{user.FirstName + " " + user.LastName + " " + $"({user.Email})"}"
                    };

                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }

                    model.Add(userRoleViewModel);
                }

                return View(model);
            }

            [HttpPost]
            public async Task<IActionResult> ManageUsersInRole(List<UserRoleViewModel> model, string roleId)
            {
                var role = await roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                    return View("NotFound");
                }

                for (int i = 0; i < model.Count; i++)
                {
                    var user = await userManager.FindByIdAsync(model[i].UserId);

                    IdentityResult result = null;

                    if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    if (result.Succeeded)
                    {
                        if (i < (model.Count - 1))
                            continue;
                        else
                            return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }

                return RedirectToAction("EditRole", new { Id = roleId });
            }

            [Authorize(Roles ="Admin")]
            [HttpGet]
            public async Task<IActionResult> ManageClaimsInRole(string roleId)
            {
                var role = await roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                    return View("NotFound");
                }

                var existingRoleClaims = await roleManager.GetClaimsAsync(role);

                var model = new RoleClaimViewModel
                {
                    RoleId = roleId,
                };

                foreach (Claim claim in ClaimsStore.AllClaims)
                {
                    RoleClaim roleClaim = new RoleClaim
                    {
                        ClaimType = claim.Type
                    };

                    if (existingRoleClaims.Any(c => c.Type == claim.Type))
                    {
                        roleClaim.IsSelected = true;
                    }

                    model.Claims.Add(roleClaim);
                }
                   return View(model);

            }

            [HttpPost]
            public async Task<IActionResult> ManageClaimsInRole(RoleClaimViewModel model)
            {
                var role = await roleManager.FindByIdAsync(model.RoleId);

                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with Id = {model.RoleId} cannot be found";
                    return View("NotFound");
                }

                var claims = await roleManager.GetClaimsAsync(role);

                foreach (var claim in claims)
                {
                    var result = await roleManager.RemoveClaimAsync(role, claim);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Cannot remove claim from exisitng role");
                        return View(model);
                    }
                }
                //Adding all the selected claims to the role
                var data_ = model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType));

                foreach (var data in data_)
                {
                    var result_ = await roleManager.AddClaimAsync(role, data);

                    if (!result_.Succeeded)
                    {
                        ModelState.AddModelError("", "Cannot add selected claims to role");
                        return View(model);
                    }
                }

                return RedirectToAction("EditRole", new { Id = model.RoleId });
            }


    }
}

