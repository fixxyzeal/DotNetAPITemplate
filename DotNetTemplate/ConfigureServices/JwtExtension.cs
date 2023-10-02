using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DotNetTemplate.ConfigureServices
{
    public static class JwtExtension
    {
        public static void ConfigJWT(this IServiceCollection services)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("JWTKEY")))
            {
                throw new ArgumentNullException(Environment.GetEnvironmentVariable("JWTKEY"), "Missing JWT Key Please Set in .ENV File or Environment Variable");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWTKEY") ?? string.Empty)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}