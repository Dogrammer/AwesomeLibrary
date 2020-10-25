using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Core.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save();
    }
}
