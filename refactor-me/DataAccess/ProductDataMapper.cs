using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using refactor_me.Models;
using System.Data.SqlClient;

namespace refactor_me.DataAccess
{
    public sealed class ProductDataMapper : DataMapper<Product>, IProductDataMapper
    {
        private readonly IProductOptionDataMapper productOptionMapper;

        public ProductDataMapper()
        {
        }

        public ProductDataMapper(IProductOptionDataMapper productOptionMapper)
        {
            this.productOptionMapper = productOptionMapper;
        }

        public override bool Delete(Product item)
        {           
            foreach (var opt in productOptionMapper.FindByProductId(item.Id))
            {
                productOptionMapper.Delete(opt);
            }

            using (var conn = DataConnection.NewConnection())
            {
                conn.Open();
                var cmd = new SqlCommand($"delete from product where id = @id", conn);
                cmd.Parameters.AddWithValue("@id", item.Id);
                var count = cmd.ExecuteNonQuery();

                return count > 0;
            }
        }

        public override IEnumerable<Product> FindAll()
        {
            return FindBy(null);
        }

        public override Product FindById(Guid id)
        {
            return FindBy("where id = @id", new List<Tuple<string, object>>
            {
                new Tuple<string, object>("@id", id),
            }).FirstOrDefault();
        }

        public IEnumerable<Product> FindByName(string name)
        {
            return FindBy("where lower(name) like '%' + @name + '%'", new List<Tuple<string, object>>
            {
                new Tuple<string, object>("@name", name),
            });
        }

        public override bool Insert(Product item)
        {
            using (var conn = DataConnection.NewConnection())
            {
                var cmd = new SqlCommand($"insert into product (id, name, description, price, deliveryprice) " +
                                        "values (@Id, @Name, @Description, @Price, @DeliveryPrice)", conn);
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Description", item.Description);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@DeliveryPrice", item.DeliveryPrice);
                conn.Open();
                var count = cmd.ExecuteNonQuery();

                return count == 1;
            }
        }

        public override bool Update(Product item)
        {
            using (var conn = DataConnection.NewConnection())
            {
                var cmd = new SqlCommand($"update product set name = @Name, description = @Description, " +
                                        "price = @Price, deliveryprice = @DeliveryPrice where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Description", item.Description);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@DeliveryPrice", item.DeliveryPrice);
                conn.Open();
                var count = cmd.ExecuteNonQuery();

                return count == 1;
            }
        }

        public override bool Upsert(Product item)
        {
            return item.IsNew ? Insert(item) : Update(item);
        }

        private IEnumerable<Product> FindBy(string where, IEnumerable<Tuple<string, object>> parameters = null)
        {
            using (var conn = DataConnection.NewConnection())
            {
                var cmd = new SqlCommand($"select * from product {where}", conn);
                if (parameters != null)
                {
                    foreach (var p in parameters)
                    {
                        cmd.Parameters.AddWithValue(p.Item1, p.Item2);
                    }
                }
                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    var items = new List<Product>();
                    while (rdr.Read())
                    {
                        var id = Guid.Parse(rdr["id"].ToString());
                        items.Add(new Product(id, false)
                        {
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                            Price = decimal.Parse(rdr["Price"].ToString()),
                            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
                        });
                    }
                    return items;
                }
            }
        }
    }
}