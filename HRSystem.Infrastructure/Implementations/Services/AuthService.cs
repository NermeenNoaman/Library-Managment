using Azure.Core;
using HRSystem.BaseLibrary.DTOs;
using HRSystem.BaseLibrary.Models;
using HRSystem.Infrastructure.Contracts;
using HRSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HRSystem.Infrastructure.Implementations
{
    public class AuthService: IAuthService
    {
        
            private readonly HRSystemContext _context;
            private readonly ITokenService _tokenService;
            private readonly IConfiguration _configuration;

        public AuthService(HRSystemContext context, ITokenService tokenService ,IConfiguration configuration)
            {
                _context = context;
                _tokenService = tokenService;
                _configuration = configuration;
        }

            public async Task<UserReadDto> RegisterAsync(UserRegisterDto request )
            {
                var existingUser = await _context.USERs
                    .FirstOrDefaultAsync(u => u.email == request.email);

                if (existingUser != null)
                    throw new Exception("email already exists");

              

            var newUser = new USER
            {
                            email = request.email,
                            role = "Member"
            };

                var hashedPassword = new PasswordHasher<USER>()
                        .HashPassword(newUser, request.Password);
                newUser.password = hashedPassword;

                _context.USERs.Add(newUser);
                    await _context.SaveChangesAsync();

                
                return new UserReadDto
                {
                    user_id = newUser.user_id,
                    email = newUser.email,
                    role = newUser.role,
                    Token = null,
                    RefreshToken = null,
                    TokenExpires = DateTime.MinValue
                };
        }

            public async Task<UserReadDto> LoginAsync(UserLoginDto request)
            {
            // 1. Find User by Email
                    var user = await _context.USERs
                    .FirstOrDefaultAsync(u => u.email == request.email);

                if (user == null || !VerifyPassword(user,request.Password, user.password))
                    throw new Exception("Invalid email or password");

                var token = await _tokenService.GenerateJwtTokenAsync(user);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

                var refreshTokenEntity = new REFRESH_TOKEN
                {
                    token = refreshToken,
                    user_id = user.user_id,
                    created = DateTime.UtcNow,
                    expires = DateTime.UtcNow.AddDays(14)
                };

                _context.REFRESH_TOKENs.Add(refreshTokenEntity);
                await _context.SaveChangesAsync();

                 var userReadDto=   new UserReadDto
                {
                     user_id = user.user_id,
                     email = user.email,
                    role = user.role,
                    Token = token,
                    RefreshToken = refreshToken,
                    TokenExpires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]))
                };
                return userReadDto;
            }

            public async Task<UserReadDto> RefreshTokenAsync(string refreshToken)
            {
            
                var storedToken = await _context.REFRESH_TOKENs
                    .Include(rt => rt.user)
                    .FirstOrDefaultAsync(rt => rt.token == refreshToken);

                if (storedToken == null)
                    throw new Exception("Invalid refresh token.");

                
                if (storedToken.expires < DateTime.UtcNow)
                    throw new Exception("Refresh token has expired.");

                if (storedToken.revoked != null)
                    throw new Exception("Refresh token has been revoked.");

               
                var newJwtToken = await _tokenService.GenerateJwtTokenAsync(storedToken.user);
                var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();

                
                storedToken.revoked = DateTime.UtcNow;

                var newTokenEntry = new REFRESH_TOKEN
                {
                    token = newRefreshToken,
                    user_id = storedToken.user_id,
                    created = DateTime.UtcNow,
                    expires = DateTime.UtcNow.AddDays(14) 
                };

                _context.REFRESH_TOKENs.Add(newTokenEntry);
                await _context.SaveChangesAsync();

                
                var NewUserReadDto = new UserReadDto();

                NewUserReadDto.user_id = storedToken.user.user_id;
                    NewUserReadDto.email = storedToken.user.email;
                    NewUserReadDto.role = storedToken.user.role;
                    NewUserReadDto.Token = newJwtToken;
                    NewUserReadDto.RefreshToken = newRefreshToken;
                    NewUserReadDto.TokenExpires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpireMinutes"]));
            
                return NewUserReadDto;
        }

            public async Task<bool> LogoutAsync(string email)
            {
                var user = await _context.USERs
                    .Include(u => u.REFRESH_TOKENs)
                    .FirstOrDefaultAsync(u => u.email == email);

                if (user == null) return false;

                foreach (var token in user.REFRESH_TOKENs.Where(t => t.revoked == null))
                {
                    token.revoked = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return true;
        }

            private bool VerifyPassword(USER existingUser ,string password, string storedHash)
            {
                var hashOfInput = new PasswordHasher<USER>()
                    .VerifyHashedPassword(existingUser,storedHash, password);
                return hashOfInput == PasswordVerificationResult.Success;
            }
        }
}
