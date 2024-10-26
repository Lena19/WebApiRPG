using Microsoft.AspNetCore.Mvc;
using WebApiRPG.DTOs;
using WebApiRPG.Services;

namespace WebApiRPG.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<string>>> Register(UserDTO userDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var response = await _authService.Register(userDTO);
        if(!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginByUsernameDTO userDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var response = await _authService.Login(userDTO);
        if (!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);  
    }
}
