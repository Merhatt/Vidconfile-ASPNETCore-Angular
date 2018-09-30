using System;
using System.Collections.Generic;
using System.Text;

namespace Vidconfile.Data.Contracts
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
