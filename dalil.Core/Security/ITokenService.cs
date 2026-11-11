using dalil.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Security
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) CreateToken(User user);

    }
}
