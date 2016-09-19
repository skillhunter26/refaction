using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace refactor_me.DataAccess
{
    public abstract class DataMapper<T> : IDataMapper<T>
    {
        public abstract bool Delete(T item);

        public abstract IEnumerable<T> FindAll();

        public abstract T FindById(Guid id);

        public abstract bool Insert(T item);

        public abstract bool Update(T item);

        public abstract bool Upsert(T item);
    }
}