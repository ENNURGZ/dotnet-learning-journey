using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers;

[Authorize]
[Route("Account")]
public class AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
{
    private readonly UserManager<IdentityUser> userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    private readonly SignInManager<IdentityUser> signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));

    [HttpGet]
    [Route("Login")]
    [AllowAnonymous]
    public ViewResult Login(string returnUrl = "/")
    {
        return this.View(new LoginViewModel
        {
            ReturnUrl = returnUrl
        });
    }

    [HttpPost]
    [Route("Login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (this.ModelState.IsValid)
        {
            IdentityUser? user = await this.userManager.FindByNameAsync(loginViewModel.Name ?? string.Empty);
            if (user != null)
            {
                await this.signInManager.SignOutAsync();
                if ((await this.signInManager.PasswordSignInAsync(user, loginViewModel.Password ?? string.Empty, false, false)).Succeeded)
                {
                    return this.Redirect(loginViewModel.ReturnUrl);
                }
            }
            this.ModelState.AddModelError(string.Empty, "Invalid name or password.");
        }
        return this.View(loginViewModel);
    }

    [HttpGet]
    [Route("Logout")]
    public async Task<IActionResult> Logout(string returnUrl = "/")
    {
        await this.signInManager.SignOutAsync();
        return this.Redirect(returnUrl);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.userManager.Dispose();
        }
        base.Dispose(disposing);
    }
}
