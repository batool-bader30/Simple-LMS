using dalil.Core.Entities;
using dalil.Core.Entities.dalil.Core.Entities;
using dalil.Core.Repository;
using dalil.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using static dalil.Core.Exceptions.AppExceptions;

namespace dalil.Infra.Repository
{
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        public async Task<User?> CreateAsync(User user)
        {
         
            try
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();

                return user;
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException
                {
                    SqlState: PostgresErrorCodes.UniqueViolation
                })
            {
                throw new ConflictException(
                    "A user with this email already exists.");
            }
        }
        

        public Task<User?> GetByEmailAsync(string email)
    => db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }
}
