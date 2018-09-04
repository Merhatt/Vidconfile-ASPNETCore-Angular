using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vidconfile.Data.Contracts;
using Vidconfile.Data.Models;

namespace Vidconfile.Data
{
    public class VidconfileDBContext : DbContext, IVidconfileDBContext
    {
        public VidconfileDBContext(DbContextOptions<VidconfileDBContext> options) 
            : base(options)
        {
        }

        public DbSet<VidconfileUser> Users { get; set; }
    }
}