using Microsoft.EntityFrameworkCore;

namespace SockChatBotApi.Models
{
    public class LightContext : DbContext
    {
        public LightContext(DbContextOptions<LightContext> options) : base(options)
        {
        }

        public DbSet<Light> Lights { get; set; }
    }
}