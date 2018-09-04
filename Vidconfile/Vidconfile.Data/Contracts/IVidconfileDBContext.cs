using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using Vidconfile.Data.Models;

namespace Vidconfile.Data.Contracts
{
    public interface IVidconfileDBContext : IDisposable
    {
        DbSet<VidconfileUser> Users { get; set; }

        int SaveChanges();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
