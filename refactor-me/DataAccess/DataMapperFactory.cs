using refactor_me.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.DataAccess
{
    public static class DataMapperFactory <T> where T : class
    {
        static Dictionary<Type, Func<T>> map = new Dictionary<Type, Func<T>>()
        {
            { typeof(IProductOptionDataMapper), () => { return new ProductOptionDataMapper() as T; } },
            { typeof(IProductDataMapper), () => { return new ProductDataMapper(new ProductOptionDataMapper()) as T; } }
        };

        public static T GetInstance() 
        {
            if (!map.ContainsKey(typeof(T)))
                throw new NotSupportedException($"Not configured for {typeof(Type).FullName}");

            return map[typeof(T)]();
        }

        public static void SetConstructionMap(Dictionary<Type, Func<T>> mp)
        {
            map = mp;
        }
    }
}