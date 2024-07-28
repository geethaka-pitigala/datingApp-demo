using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountsController(DataContext context, ITokenService tokenService): BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> RegisterUser(RegisterUserDto registerUser){
        if(await UserExists(registerUser.Username)) return BadRequest("User already exists");
        
        byte[] salt = RandomNumberGenerator.GetBytes(64);
        Console.WriteLine($"Password Salt: {BitConverter.ToString(salt)}");
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
        Encoding.UTF8.GetBytes(registerUser.Password),
        salt,
        350000,
        HashAlgorithmName.SHA512,
        64);
        Console.WriteLine($"Password Hash: {BitConverter.ToString(hash)}");

        var newUser = new AppUser{
            UserName = registerUser.Username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        context.Users.Add(newUser);
        await context.SaveChangesAsync();
        return newUser;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
        var user = await context.Users.FirstOrDefaultAsync(x=> x.UserName == loginDto.Username);
        if(user == null) return Unauthorized("username invalid.");
        var result = VerifyPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt);
        if(result){
            return new UserDto{
                Username = user.UserName,
                Token = tokenService.GenerateJSONWebToken(user)
            };
        }
        return Unauthorized("invalid password.");
    }

    private async Task<bool> UserExists(string username){
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }

    private static bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, 350000, HashAlgorithmName.SHA512, 64);
        return CryptographicOperations.FixedTimeEquals(hashToCompare, hash);
    }


}
