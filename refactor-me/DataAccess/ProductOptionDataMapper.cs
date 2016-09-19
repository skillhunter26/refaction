using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using refactor_me.Models;
using System.Data.SqlClient;

namespace refactor_me.DataAccess
{
    public sealed class ProductOptionDataMapper : DataMapper<ProductOption>, IProductOptionDataMapper
    {
        public override bool Delete(ProductOption item)
        {
            using (var conn = DataConnection.NewConnection())
            {
                conn.Open();
                var cmd = new SqlCommand($"delete from productoption where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", item.Id);
                var count = cmd.ExecuteNonQuery();

                return count > 0;
            }
        }

        public override IEnumerable<ProductOption> FindAll()
        {
            return FindBy(null);
        }

        public override ProductOption FindById(Guid id)
        {
            return FindBy($"where id = @id", new List<Tuple<string, object>>()
            {
                new Tuple<string, object>("@id", id)
            }).FirstOrDefault();
        }

        public IEnumerable<ProductOption> FindByProductId(Guid productId)
        {
            return FindBy($"where productid = @productId", new List<Tuple<string, object>>()
            {
                new Tuple<string, object>("@productId", productId)
            });
        }

        public override bool Insert(ProductOption item)
        {
            using (var conn = DataConnection.NewConnection())
            {
                var cmd = new SqlCommand($"insert into productoption (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)", conn);
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@ProductId", item.ProductId);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Description", item.Description);

                conn.Open();
                var count = cmd.ExecuteNonQuery();

                return count == 1;
            }
        }

        public override bool Update(ProductOption item)
        {
            using (var conn = DataConnection.NewConnection())
            {
                var cmd = new SqlCommand($"update productoption set name = @Name, description = @Description, ProductId = @ProductId where id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Description", item.Description);
                cmd.Parameters.AddWithValue("@ProductId", item.ProductId);

                conn.Open();
                var count = cmd.ExecuteNonQuery();

                return count == 1;
            }
        }

        public override bool Upsert(ProductOption item)
        {
            return item.IsNew ? Insert(item) : Update(item);
        }

        private IEnumerable<ProductOption> FindBy(string where, IEnumerable<Tuple<string, object>> parameters = null)
        {
            using (var conn = DataConnection.NewConnection())
            {
                var cmd = new SqlCommand($"select * from productoption {where}", conn);
                foreach (var p in parameters)
                {
                    cmd.Parameters.AddWithValue(p.Item1, p.Item2);
                }
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    var items = new List<ProductOption>();
                    while (rdr.Read())
                    {
                        var id = Guid.Parse(rdr["id"].ToString());
                        items.Add(new ProductOption(id, false)
                        {
                            ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                        });
                    }
                    return items;
                }
            }
        }
    }
}