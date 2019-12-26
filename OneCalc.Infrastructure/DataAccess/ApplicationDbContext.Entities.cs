using Microsoft.EntityFrameworkCore;
using OneCalc.Domain.Entities;

namespace OneCalc.Infrastructure.DataAccess
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<History> Histories { get; set; }
    }
}
