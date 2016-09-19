using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactor_me.DataAccess
{
    public interface IProductDataMapper : IDataMapper<Product>
    {
        IEnumerable<Product> FindByName(string name);
    }
}
