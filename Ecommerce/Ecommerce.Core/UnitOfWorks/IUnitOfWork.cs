using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ecommerce.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        //Bu kodlar SaveChangesAsync ve SaveChanges metodlarını kullanarak veritabanına kayıt işlemlerini yapmamızı sağlar.
        Task CommitAsync();
        void Commit();
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
    }
}