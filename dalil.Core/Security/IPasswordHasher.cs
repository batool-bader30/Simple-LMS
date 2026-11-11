using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Core.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hash);
    }
}
