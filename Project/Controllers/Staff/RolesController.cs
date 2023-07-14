using FinalProject.Constants;
using FinalProject.Models;
using FinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FinalProject.Controllers.Staff
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        //[Authorize(Permissions.Staff.View)]
        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            var roleCounts = new Dictionary<string, int>();
            foreach (var role in roles)
            {
                var userCount = await userManager.GetUsersInRoleAsync(role.Name);
                roleCounts.Add(role.Name, userCount.Count);
            }
            ViewBag.userRoleCount = roleCounts;
            return View(roles);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Add(RoleFormViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View("Index", await roleManager.Roles.ToListAsync());

        //    if (await roleManager.RoleExistsAsync(model.Name))
        //    {
        //        ModelState.AddModelError("Name", "Role Already Exists");
        //        return View("Index", await roleManager.Roles.ToListAsync());

        //    }
        //    await roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> ManagePermissions(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
                return NotFound();

            var roleClaims = roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            //var allClaims = Permissions.GenerateAllPermissions();
            var allPermissions = Permissions.GenerateAllPermissions().Select(p => new CheckBoxViewModel { DisplayValue = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(c => c == permission.DisplayValue))
                    permission.IsSelected = true;
            }

            var viewModel = new PermissionsFormViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                RoleCalims = allPermissions
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(PermissionsFormViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
                return NotFound();

            var roleClaims = await roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
                await roleManager.RemoveClaimAsync(role, claim);

            var selectedClaims = model.RoleCalims.Where(c => c.IsSelected).ToList();

            foreach (var claim in selectedClaims)
                await roleManager.AddClaimAsync(role, new Claim("Permission", claim.DisplayValue));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Add()
        {
            var allClaims = Permissions.GenerateAllPermissions().Select(p => new CheckBoxViewModel { DisplayValue = p }).ToList();

            var viewModel = new AddRoleViewModel
            {
                RoleCalims = allClaims
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", await roleManager.Roles.ToListAsync());

            if (await roleManager.RoleExistsAsync(model.RoleName))
            {
                ModelState.AddModelError("RoleName", "Role Already Exists");
                return View(model);

            }
            var selectedClaims = model.RoleCalims.Where(c => c.IsSelected).ToList();

            if (selectedClaims.Count == 0)
            {
                ModelState.AddModelError("RoleName", "Please Select At Least One Permission");
                return View(model);
            }

            var newRole = new IdentityRole
            {
                Name = model.RoleName
            };

            await roleManager.CreateAsync(newRole);

            foreach (var claim in selectedClaims)
                await roleManager.AddClaimAsync(newRole, new Claim("Permission", claim.DisplayValue));

            return RedirectToAction(nameof(Index));
        }

    }
}