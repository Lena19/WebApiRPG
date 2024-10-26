using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiRPG.Data;
using WebApiRPG.DTOs;
using WebApiRPG.Models;

namespace WebApiRPG.Services;

public class AuthService : IAuthService
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(DataContext context, IConfiguration configuration, IMapper mapper)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<ServiceResponse<string>> Login(UserLoginByUsernameDTO userDTO)
    {
        var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == userDTO.Username);
        if (user == null)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "User does not exist."
            };
        }

        if(!BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Message = "Wrong password."
            };
        }

        string token = CreateToken(user);
        return new ServiceResponse<string>()
        {
            Data = token
        };
    }

    public async Task<ServiceResponse<string>> Register(UserDTO userDTO)
    {
        if(await _context.Users.AnyAsync(u => u.Username == userDTO.Username))
        {
            return new ServiceResponse<string>()
            {
                Success = false,
                Message = "User with this username already exists."
            };
        }
        if(await _context.Users.AnyAsync(u => u.EmailAddress == userDTO.EmailAddress))
        {
            return new ServiceResponse<string>()
            {
                Success = false,
                Message = "User with this email already exists."
            };
        }
        try
        {
            var user = new User()
            {
                Username = userDTO.Username,
                EmailAddress = userDTO.EmailAddress,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password)
            };
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return new ServiceResponse<string>()
            {
                Data = "User registered successfully."
            };
        }
        catch(Exception ex)
        {
            return new ServiceResponse<string>()
            {
                Success = false,
                Message = "Registration failed."
            };
        }
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration.GetSection("JwtSettings:SecretKey").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                issuer: _configuration.GetSection("JwtSettings:Issuer").Value,
                audience: _configuration.GetSection("JwtSettings:Audience").Value,
                expires: DateTime.UtcNow.AddDays(1)
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

}
