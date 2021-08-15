using JMBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Repositories.IRepository
{
    public interface ICoverTypeRepository:IRepository<CoverType>
    {
        void Update(CoverType cover);
    }
}
