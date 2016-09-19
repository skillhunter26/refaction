using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactor_me.DataAccess
{
    public interface IDataMapper<T>
    {
        IEnumerable<T> FindAll();
        T FindById(Guid id);        
        bool Upsert(T item);
        bool Update(T item);
        bool Insert(T item);
        bool Delete(T item);
    }
}
