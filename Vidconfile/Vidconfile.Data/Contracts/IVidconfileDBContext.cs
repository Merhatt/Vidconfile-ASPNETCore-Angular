using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vidconfile.Data.Contracts
{
    public interface IVidconfileDBContext : IDisposable
    {
        //IDbSet<Game> Games { get; set; }

        //IDbSet<Category> Categories { get; set; }

        //IDbSet<Platform> Platforms { get; set; }

        int SaveChanges();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
