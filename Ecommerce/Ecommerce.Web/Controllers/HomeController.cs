using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Web.Models;
using Ecommerce.Core.Entity;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Ecommerce.Web.Controllers;

public class HomeController : Controller
{

    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;

    public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(UserViewModel model)
    {
        //Login with email and password
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Admin");
        }
        return View();
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(UserViewModel model)
    {
        var user = new AppUser()
        {
            Name = model.Name,
            Surname = model.Surname,
            Email = model.Email,
            UserName = model.Email
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Login", "Account");
        }

        foreach (var error in result.Errors)
        {
            Console.WriteLine(error.Description);
        }

        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        System.Console.WriteLine("User logged out");
        return RedirectToAction("Login", "Account");
    }
}
