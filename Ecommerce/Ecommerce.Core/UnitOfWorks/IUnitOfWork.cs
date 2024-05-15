using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        //Bu kodlar SaveChangesAsync ve SaveChanges metodlarını kullanarak veritabanına kayıt işlemlerini yapmamızı sağlar.
        Task CommitAsync();
        void Commit();
    }
}