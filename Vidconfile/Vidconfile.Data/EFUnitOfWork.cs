using System;
using System.Collections.Generic;
using System.Text;
using Vidconfile.Data.Contracts;

namespace Vidconfile.Data
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private IVidconfileDBContext context;

        public EFUnitOfWork(IVidconfileDBContext context)
        {
            this.context = context ?? throw new NullReferenceException("context cannot be null");
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }
    }
}
