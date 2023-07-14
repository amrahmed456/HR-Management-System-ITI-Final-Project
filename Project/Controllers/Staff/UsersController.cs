using FinalProject.Constants;
using FinalProject.Enums;
using FinalProject.Models;
using FinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers.Staff
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        //[Authorize(Permissions.Staff.View)]
        public async Task<IActionResult> Index()
        {
            
            var users = await userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Roles = userManager.GetRolesAsync(user).Result
            }).ToListAsync();
            string responseData = TempData["ResponseData"] as string;
            if (!string.IsNullOrEmpty(responseData))
            {
                ViewData["Response"] = responseData;
            }
            return View(users);
        }

        public async Task<IActionResult> Add()
        {
            var roles = await roleManager.Roles.Select(role => new CheckBoxViewModel { DisplayValue = role.Name }).ToListAsync();

            var viewModel = new AddUserViewModel
            {
                Roles = roles
            };

            return View(viewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            if (!model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Must Select Atleast One Role");
                return View(model);
            }
            if (await userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email Is Already Exists");
                return View(model);
            }
            if (await userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "User Name Is Already Exists");
                return View(model);
            }

            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                PasswordHash = model.Password
            };
            var result = await userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);

            }
            await userManager.AddToRolesAsync(newUser, model.Roles.Where(r => r.IsSelected == true).Select(r => r.DisplayValue));


            TempData["ResponseData"] = Responses.success.ToString();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();
            

            var roles = await roleManager.Roles.ToListAsync();
            var viewModel = new EditUserViewModel
            {
              Id = userId,
              Name = user.Name,
              UserName = user.UserName,
              Email = user.Email,
              Roles = roles.Select(role => new CheckBoxViewModel
                {
                    DisplayValue = role.Name,
                    IsSelected = userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();

            var userWithSameEmail = await userManager.FindByEmailAsync(model.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "This Email Already Assigned To Another User");
                return View(model);
            }
            var userWithSameUserName = await userManager.FindByNameAsync(model.UserName);
            if (userWithSameUserName != null && userWithSameUserName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "This User Name Already Assigned To Another User");
                return View(model);
            }
            user.Name = model.Name;
            user.UserName = model.UserName;
            user.Email = model.Email;

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.DisplayValue) && !role.IsSelected)
                {
                    await userManager.RemoveFromRoleAsync(user, role.DisplayValue);
                }

                if (!userRoles.Any(r => r == role.DisplayValue) && role.IsSelected)
                {
                    await userManager.AddToRoleAsync(user, role.DisplayValue);
                }
            }

            await userManager.UpdateAsync(user);
            TempData["ResponseData"] = Responses.success.ToString();
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()?.Description);
            }

            return RedirectToAction("Index");
        }














        //public async Task<IActionResult> ManageRoles(string userId)
        //{
        //    var user = await userManager.FindByIdAsync(userId);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    var roles = await roleManager.Roles.ToListAsync();
        //    var viewModel = new UserRolesViewModel
        //    {
        //        UserId = user.Id,
        //        UserName = user.UserName,
        //        Roles = roles.Select(role => new CheckBoxViewModel
        //        {
        //            DisplayValue = role.Name,
        //            IsSelected = userManager.IsInRoleAsync(user, role.Name).Result
        //        }).ToList()
        //    };
        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        //{
        //    var user = await userManager.FindByIdAsync(model.UserId);

        //    if (user == null)
        //        return NotFound();
        //    var userRoles = await userManager.GetRolesAsync(user);

        //    foreach (var role in model.Roles)
        //    {
        //        if (userRoles.Any(r => r == role.DisplayValue) && !role.IsSelected)
        //        {
        //            await userManager.RemoveFromRoleAsync(user, role.DisplayValue);
        //        }

        //        if (!userRoles.Any(r => r == role.DisplayValue) && role.IsSelected)
        //        {
        //            await userManager.AddToRoleAsync(user, role.DisplayValue);
        //        }
        //    }

        //    return RedirectToAction(nameof(Index));

        //}


    }
}
