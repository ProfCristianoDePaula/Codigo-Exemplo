using System.Security.Claims;
using ClinicaVet.Data;
using ClinicaVet.Models;
using ClinicaVet.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
namespace ClinicaVet.Controllers;
[Authorize]
public class AuthController(ClinicaContext db) : Controller
{
    [AllowAnonymous]
    public async Task<IActionResult> Index() => await SetupNecessario() ? RedirectToAction(nameof(PrimeiroAcesso)) : RedirectToAction(nameof(Login));
    [AllowAnonymous]
    public async Task<IActionResult> PrimeiroAcesso() { if (!await SetupNecessario()) return RedirectToAction(nameof(Login)); return View(new PrimeiroAcessoViewModel()); }
    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> PrimeiroAcesso(PrimeiroAcessoViewModel model)
    {
        if (!await SetupNecessario()) return RedirectToAction(nameof(Login));
        if (!ModelState.IsValid) return View(model);
        var user = new Usuario { Nome=model.Nome.Trim(), Email=model.Email.Trim().ToLowerInvariant() };
        user.SenhaHash = new PasswordHasher<Usuario>().HashPassword(user, model.Senha);
        db.Usuarios.Add(user); await db.SaveChangesAsync(); await Entrar(user);
        return RedirectToAction("Index", "Home");
    }
    [AllowAnonymous]
    public async Task<IActionResult> Login() { if (await SetupNecessario()) return RedirectToAction(nameof(PrimeiroAcesso)); return View(); }
    [AllowAnonymous]
    public async Task<IActionResult> Cadastro()
    {
        if (await SetupNecessario()) return RedirectToAction(nameof(PrimeiroAcesso));
        return View(new PrimeiroAcessoViewModel());
    }
    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastro(PrimeiroAcessoViewModel model)
    {
        if (await SetupNecessario()) return RedirectToAction(nameof(PrimeiroAcesso));
        if (!ModelState.IsValid) return View(model);

        var email = model.Email.Trim().ToLowerInvariant();
        if (await db.Usuarios.AnyAsync(u => u.Email == email))
        {
            ModelState.AddModelError(nameof(model.Email), "Este e-mail já está cadastrado.");
            return View(model);
        }

        var user = new Usuario { Nome = model.Nome.Trim(), Email = email };
        user.SenhaHash = new PasswordHasher<Usuario>().HashPassword(user, model.Senha);
        db.Usuarios.Add(user);
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is SqliteException { SqliteExtendedErrorCode: 2067 } ||
            ex.InnerException is SqlException { Number: 2601 or 2627 })
        {
            ModelState.AddModelError(nameof(model.Email), "Este e-mail já está cadastrado.");
            return View(model);
        }

        await Entrar(user);
        return RedirectToAction("Index", "Home");
    }
    [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string senha)
    {
        var user = await db.Usuarios.SingleOrDefaultAsync(u => u.Email == email.Trim().ToLower());
        if (user is null || new PasswordHasher<Usuario>().VerifyHashedPassword(user,user.SenhaHash,senha) == PasswordVerificationResult.Failed) { ModelState.AddModelError("", "E-mail ou senha inválidos."); return View(); }
        await Entrar(user); return RedirectToAction("Index", "Home");
    }
    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Sair() { await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); return RedirectToAction(nameof(Login)); }
    private async Task<bool> SetupNecessario() => !await db.Usuarios.AnyAsync();
    private async Task Entrar(Usuario user)
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()), new Claim(ClaimTypes.Name,user.Nome), new Claim(ClaimTypes.Email,user.Email) };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme)),new AuthenticationProperties { IsPersistent=false, ExpiresUtc=DateTimeOffset.UtcNow.AddMinutes(30) });
    }
}
