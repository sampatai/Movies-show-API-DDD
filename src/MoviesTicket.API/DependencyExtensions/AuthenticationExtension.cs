using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MoviesTicket.API.DependencyExtensions
{
    public static class AuthenticationExtension
    {
        internal static IServiceCollection AddAuthenticationWithBearer(
          this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(o =>
                    {
                        o.RequireHttpsMetadata = false;
                        o.Audience = configuration["Authorization:Audience"];
                        o.MetadataAddress = configuration["Authorization:MetaDataAddress"]!;
                        o.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = configuration["Authorization:ValidIssure"]
                        };
                    });
            return services;
        }
    }
}
