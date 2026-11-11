using dalil.Core.DTOs;
using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using dalil.Core.Repository;
using dalil.Core.Security;
using dalil.Core.Service;
using dalil.Infra.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static dalil.Core.DTOs.Auth;
using static dalil.Core.DTOs.StudentDto;
using static dalil.Core.Exceptions.AppExceptions;

namespace dalil.Infra.Service
{
    public class AuthService(
    IStudentRepository studentRepository,
 IUserRepository users,
 IPasswordHasher hasher,
 ITokenService tokens) : IAuthService
    {
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await users.GetByEmailAsync(dto.Email);
            // Same message for "no such user" and "wrong password" — never reveal which
            if (user is null || !hasher.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedException("Invalid email or password."); // → 401
            var (token, expiresAt) = tokens.CreateToken(user);
            return new LoginResponseDto(token, expiresAt);
        }

        public async Task<LoginResponseDto> RegisterAsync(RegisterDto dto)
        {
            //Check that the email address is not already registered.
            var existingUser = await users.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new ConflictException("Email is already registered.");
          //  Hash passwords using BCrypt
            var passwordHash = hasher.Hash(dto.Password);
          // Automatically assign the `Student` role to newly registered users.
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "Student" 
            };

            await users.CreateAsync(user);

            var student = new Student
            {
                Name = dto.Name, 
                Email = dto.Email,
                UserId = user.Id 
            };

            await studentRepository.CreateAsync(student);

            var (token, expiresAt) = tokens.CreateToken(user);

         //  Return a JWT Token after successful registration.
            return new LoginResponseDto(token, expiresAt);
        }
    
        
    }
}
