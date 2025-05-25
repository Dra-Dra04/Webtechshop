using System.Numerics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Webtechshop.Models;
using Webtechshop.Models.ViewModels;
using Webtechshop.Repository;

namespace Webtechshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUserModel> _userManager;
        private readonly DataContext _dataContext;
        public UserController(UserManager<AppUserModel> userManager, RoleManager<IdentityRole> roleManager,DataContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _dataContext = context;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var usersWithRole = await (from u in _dataContext.Users
                               join ur_join in _dataContext.UserRoles on u.Id.ToString() equals ur_join.UserId into ur_group
                               from ur in ur_group.DefaultIfEmpty() 
                               join r_join in _dataContext.Roles on ur.RoleId equals r_join.Id into r_group
                               from r in r_group.DefaultIfEmpty()  
                               select new UserWithRoleViewModel
                               {
                                            User = u
,
                                            RoleName = r != null ? r.Name :"Chưa có quyền" })
                                .ToListAsync();
            var loggedInUserId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.LoggedInUserId = loggedInUserId;
            return View(model: usersWithRole);
        }
        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(new AppUserModel());
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(AppUserModel user)
        {
            if (ModelState.IsValid)
            {
                var creatUserResult = await _userManager.CreateAsync(user, user.PasswordHash);
                if (creatUserResult.Succeeded)
                {
                    var createUser = await _userManager.FindByEmailAsync(user.Email);//tìm user bằng email
                    var userId = createUser.Id; //Lấy user id
                    var role = _roleManager.FindByIdAsync(user.RoleId); //Lấy role dựa vào id
                    //gán quyền cho user
                    var addToRoleResult = await _userManager.AddToRoleAsync(createUser, role.Result.Name);
                    TempData["success"] = "Thêm người dùng thành công";
                    if(!addToRoleResult.Succeeded)
                    {
                        foreach (var error in addToRoleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in creatUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(user);
                }
            }
            else
            {
                TempData["error"] = "Model có một vài thứ đang lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id, AppUserModel user)
        {
            var existingUser = await _userManager.FindByIdAsync(id);//lấy user dựa vào id
            if (existingUser == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.RoleId = user.RoleId;
                
                var updateResult = await _userManager.UpdateAsync(existingUser);
                if (updateResult.Succeeded)
                {
                    TempData["success"] = "Cập nhật người dùng thành công";
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    AddIdentityErrors(updateResult);
                    return View(existingUser);
                }
            }
            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            
            TempData["error"] = "Model có một vài thứ đang lỗi";
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            string errorMessage = string.Join("\n", errors);
            return View(existingUser);
        }
        private void AddIdentityErrors(IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var deleteResult = await _userManager.DeleteAsync(user);
            if (deleteResult.Succeeded)
            {
                TempData["success"] = "Xoá người dùng thành công";
                return RedirectToAction("Index", "User");
            }
            else
            {
                foreach (var error in deleteResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Index", await _userManager.Users.OrderByDescending(p => p.Id).ToListAsync());
            }
        }

    }
}
