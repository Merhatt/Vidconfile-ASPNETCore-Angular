using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Vidconfile.Data.Contracts;

namespace Vidconfile.Data
{
    public class VidconfileDBContext : DbContext, IVidconfileDBContext
    {
        public VidconfileDBContext(DbContextOptions<VidconfileDBContext> options) 
            : base(options)
        {
        }
        
        //public static VidconfileDBContext Create()
        //{
        //    return new VidconfileDBContext();
        //}
    }
}