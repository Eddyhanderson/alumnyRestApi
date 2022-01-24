using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace alumni.Options
{
    public class TokenOptions
    {
        public TokenOptions()
        {
            _genereteSecret();
        }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int LifeTime { get; set; }

        public SecurityKey Key { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public bool ValidateIssuerSigningKey = true;

        public bool ValidateLifetime = true;

        public bool ValidateIssuer = true;

        public bool ValidateAudience = true;
        
        public bool RequireExpirationTime = true;            

        public TimeSpan ClockSkew = TimeSpan.FromSeconds(10);

        private void _genereteSecret()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256);
        }

    }
}
