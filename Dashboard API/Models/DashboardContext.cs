using Microsoft.EntityFrameworkCore;

namespace Dashboard_API.Models
{
    public class DashboardContext : DbContext
    {
        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options)
        {
        }
    }
}
