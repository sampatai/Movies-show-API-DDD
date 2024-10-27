using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace MoviesTicket.Infrastructure
{
    public class SecurityDbContext : IdentityDbContext<IdentityUser>
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) :
        base(options)
        {
        }
    }
}
