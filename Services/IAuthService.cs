using Microsoft.AspNetCore.Mvc;
using WebApiRPG.DTOs;

namespace WebApiRPG.Services;

public interface IAuthService
{
    public Task<ServiceResponse<string>> Register(UserDTO userDTO);
    public Task<ServiceResponse<string>> Login(UserLoginByUsernameDTO userDTO);
}
