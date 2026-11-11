using System;
using System.Collections.Generic;
using System.Text;

namespace dalil.Infra.Security
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public double ExpiresInHours { get; set; }
    }
}
