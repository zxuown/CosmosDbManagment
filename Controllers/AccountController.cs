using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CosmosDbManagment.Models;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;
using CosmosDbManagment.Models.Forms;
using Microsoft.EntityFrameworkCore;

namespace CosmosDbManagment.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<TeamMember> _signInManager;
    private readonly UserManager<TeamMember> _userManager;
    private readonly RoleManager<IdentityRole<string>> _roleManager;

    public AccountController(UserManager<TeamMember> userManager, SignInManager<TeamMember> signInManager
        , RoleManager<IdentityRole<string>> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
	}
	//public async Task CreateRoles()
	//{
	//	string[] roleNames = { "Admin", "User" };
	//	foreach (var roleName in roleNames)
	//	{
	//		var roleExist = await _roleManager.RoleExistsAsync(roleName);
	//		if (!roleExist)
	//		{
	//			IdentityRole<string> role = new IdentityRole<string>
	//			{
	//				Id = Guid.NewGuid().ToString(),
	//				Name = roleName
	//			};
	//			await _roleManager.CreateAsync(role);
	//		}
	//	}
	//}


	public IActionResult Index() 
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Login()
    {
        return View(new LoginForm());
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromForm] LoginForm form)
    {

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var user = await _userManager.FindByEmailAsync(form.Login);
			if (user == null)
        {
            ModelState.AddModelError(nameof(form.Login), "User not exists");
            return View(form);
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, form.Password, true, false);

        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError(nameof(form.Login), "Sign in fail");
            return View(form);
        }

        return Redirect("/");
    }

    [HttpGet]
    public async Task<IActionResult> Register()
    {
        return View(new RegisterForm());
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }

	[HttpPost]
	public async Task<IActionResult> Register([FromForm] RegisterForm form)
	{
		if (!ModelState.IsValid)
		{
			return View(form);
		}

		var user = await _userManager.FindByEmailAsync(form.Login);

		if (user != null)
		{
			ModelState.AddModelError(nameof(form.Login), "User already exists");
			return View(form);
		}

		user = new TeamMember
		{
			Id = Guid.NewGuid().ToString(),
			Email = form.Login,
			UserName = form.Username,
			Surname = form.Surname,
			Role = "User"
		};

		var registerResult = await _userManager.CreateAsync(user, form.Password);

		if (!registerResult.Succeeded)
		{
			ModelState.AddModelError(nameof(form.Login), "Invalid data");
			return View(form);
		}

		//await CreateRoles();

		//await _userManager.AddToRoleAsync(user, "User");

		var signInResult = await _signInManager.PasswordSignInAsync(user, form.Password, true, false);

		if (!signInResult.Succeeded)
		{
			ModelState.AddModelError(nameof(form.Login), "Sign in fail");
			return View(form);
		}

		return Redirect("/");
	}
}
