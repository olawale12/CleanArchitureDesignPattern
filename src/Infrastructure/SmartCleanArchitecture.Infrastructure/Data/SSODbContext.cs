using Microsoft.EntityFrameworkCore;
using SmartCleanArchitecture.Domain.Entities.Dals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCleanArchitecture.Infrastructure.Data
{
    public class SSODbContext: DbContext
    {
        public SSODbContext(DbContextOptions<SSODbContext> dbContextOptions)
            :base(dbContextOptions) 
        {
            
        }

        public DbSet<Users> Users { get; set; }
    }
}
