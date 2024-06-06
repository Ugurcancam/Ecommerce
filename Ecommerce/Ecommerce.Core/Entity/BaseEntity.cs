using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entity
{
    //Bir nesne örneği alınmaması için abstract olarak işaretledik.
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}