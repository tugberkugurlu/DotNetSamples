using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorldJwt
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://pfelix.wordpress.com/2012/11/27/json-web-tokens-and-the-new-jwtsecuritytokenhandler-class/

            // JWT with X.509 cert
            // http://social.msdn.microsoft.com/forums/windowsazure/ru-ru/730dedff-6ff6-4705-9834-ae92c2339b45/unable-to-use-json-web-token-handler-ga-with-windows-store-and-rest-scenario
            // https://github.com/thinktecture/Thinktecture.IdentityModel.45/blob/59f7f2cc43d1b1a577e42db520e037afc3dcb713/IdentityModel/Thinktecture.IdentityModel/Tokens/Http/AuthenticationConfigurationExtensionsCore.cs
            // http://stackoverflow.com/questions/18032038/securing-the-jwt-with-a-x509certificate2-jwtsecuritytokenhandler
            // http://msdn.microsoft.com/en-us/library/windowsazure/dn195592.aspx
            //  - When a service receives a token that is issued by a Windows Azure AD tenant, the signature of the token must be validated with a signing key that is published in the federation metadata document.
            //  - The federation metadata includes the public portion of the certificates that the tenants use for token signing. The certificate raw bytes appear in the KeyDescriptor element. The token signing certificate is valid for signing only when the value of the use attribute is signing.

            // X.509 certs
            X509Store store = new X509Store("My");
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 signingCert = store.Certificates[0];
            store.Close();

            string token = CreateTokenWithX509SigningCredentials(signingCert);
            ClaimsPrincipal principal = ValidateTokenWithX509SecurityToken(new X509RawDataKeyIdentifierClause(signingCert.RawData), token);

            string token2 = CreateTokenWithInMemorySymmetricSecurityKey();
        }

        static string CreateTokenWithInMemorySymmetricSecurityKey()
        {
            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = new RandomBufferGenerator(256 / 8).GenerateBufferFromSeed(256 / 8);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "Tugberk"),
                            new Claim(ClaimTypes.Role, "Sales"), 
                        }),
                TokenIssuerName = "self",
                AppliesToAddress = "http://www.example.com",
                Lifetime = new Lifetime(now, now.AddMinutes(2)),
                SigningCredentials = new SigningCredentials(
                        new InMemorySymmetricSecurityKey(symmetricKey),
                        "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                        "http://www.w3.org/2001/04/xmlenc#sha256")
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        static string CreateTokenWithX509SigningCredentials(X509Certificate2 signingCert)
        {
            // JwtSecurityTokenHandler sample
            var now = DateTime.UtcNow;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "Tugberk"),
                            new Claim(ClaimTypes.Role, "Sales")
                        }),
                TokenIssuerName = "self",
                AppliesToAddress = "http://www.example.com",
                Lifetime = new Lifetime(now, now.AddMinutes(2)),
                SigningCredentials = new X509SigningCredentials(signingCert)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        static ClaimsPrincipal ValidateTokenWithX509SecurityToken(X509RawDataKeyIdentifierClause x509DataClause, string token)
        {
            var validatingCertificate = new X509Certificate2(x509DataClause.GetX509RawData());
            var tokenHandler = new JwtSecurityTokenHandler();
            var x509SecurityToken = new X509SecurityToken(validatingCertificate);
            var validationParameters = new TokenValidationParameters()
            {
                AllowedAudience = "http://www.example.com",
                SigningToken = x509SecurityToken,
                ValidIssuer = "self",
            };

            ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(new JwtSecurityToken(token), validationParameters);
            return claimsPrincipal;
        }
    }

    public class RandomBufferGenerator
    {
        private readonly Random _random = new Random();
        private readonly byte[] _seedBuffer;

        public RandomBufferGenerator(int maxBufferSize)
        {
            _seedBuffer = new byte[maxBufferSize];

            _random.NextBytes(_seedBuffer);
        }

        public byte[] GenerateBufferFromSeed(int size)
        {
            int randomWindow = _random.Next(0, size);

            byte[] buffer = new byte[size];

            Buffer.BlockCopy(_seedBuffer, randomWindow, buffer, 0, size - randomWindow);
            Buffer.BlockCopy(_seedBuffer, 0, buffer, size - randomWindow, randomWindow);

            return buffer;
        }
    }
}
