using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web_Asm2.Models;

namespace Web_Asm2.Data
{
    public class Web_Asm2Context : DbContext
    {
        public Web_Asm2Context (DbContextOptions<Web_Asm2Context> options)
            : base(options)
        {
        }

        public DbSet<Web_Asm2.Models.Category> Category { get; set; } = default!;
        public DbSet<Web_Asm2.Models.Product> Product { get; set; } = default!;
        public DbSet<Web_Asm2.Models.Order> Order { get; set; } = default!;
    }
}
