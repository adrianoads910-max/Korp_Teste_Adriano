using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FaturamentoService.Models.Auth; // ✅ importante: namespace onde estão os DTOs

namespace FaturamentoService.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest dto)  // ✅ FromBody
    {
        var user = new IdentityUser
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await userManager.CreateAsync(user, dto.Senha);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { message = "Usuário registrado com sucesso!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest dto) // ✅ FromBody
    {
        var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Senha, false, false);

        if (!result.Succeeded)
            return Unauthorized(new { message = "Credenciais inválidas" });

        return Ok(new { authenticated = true, message = "Login realizado com sucesso!" });
    }
}
