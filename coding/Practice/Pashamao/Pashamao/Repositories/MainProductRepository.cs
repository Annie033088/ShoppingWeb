using NLog;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Pashamao.Repositories
{
    public class MainProductRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得所有商品(大綱)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        internal (List<ProductSimple>, int) GetAllProduct(int page)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ProductSimple> products = new List<ProductSimple>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getAllProduct @page, @totalPages OUTPUT";

                cmd.Parameters.Add("@page", SqlDbType.Int).Value = page;
                SqlParameter totalPagesOutput = new SqlParameter("@totalPages", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(totalPagesOutput);

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);
                totalPages = (int)totalPagesOutput.Value;

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ProductSimple product = new ProductSimple();
                        product.ProductId = dt.Rows[i].IsNull("f_productId") ? 0 : dt.Rows[i].Field<int>("f_productId");
                        product.CategoryId = dt.Rows[i].IsNull("f_categoryId") ? 0 : dt.Rows[i].Field<int>("f_categoryId");
                        product.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        product.Price = dt.Rows[i].IsNull("f_price") ? 0 : dt.Rows[i].Field<decimal>("f_price");
                        product.ImageUrl = dt.Rows[i].IsNull("f_imageUrl") ? string.Empty : dt.Rows[i].Field<string>("f_imageUrl");
                        product.StockQuantity = dt.Rows[i].IsNull("f_stockQuantity") ? 0 : dt.Rows[i].Field<int>("f_stockQuantity");
                        products.Add(product);
                    }
                    return (products, totalPages);
                }
                else
                {
                    return (null, 0);
                }

            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 取得分類下的商品(大綱)
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        internal (List<ProductSimple>, int) GetProductByCategory(int categoryId, int page)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ProductSimple> products = new List<ProductSimple>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getProductByCategory @categoryId, @page, @totalPages OUTPUT";

                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = categoryId;
                cmd.Parameters.Add("@page", SqlDbType.Int).Value = page;
                SqlParameter totalPagesOutput = new SqlParameter("@totalPages", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(totalPagesOutput);

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);
                totalPages = (int)totalPagesOutput.Value;

                cmd.Connection.Close();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ProductSimple product = new ProductSimple();
                        product.ProductId = dt.Rows[i].IsNull("f_productId") ? 0 : dt.Rows[i].Field<int>("f_productId");
                        product.CategoryId = dt.Rows[i].IsNull("f_categoryId") ? 0 : dt.Rows[i].Field<int>("f_categoryId");
                        product.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        product.Price = dt.Rows[i].IsNull("f_price") ? 0 : dt.Rows[i].Field<decimal>("f_price");
                        product.ImageUrl = dt.Rows[i].IsNull("f_imageUrl") ? string.Empty : dt.Rows[i].Field<string>("f_imageUrl");
                        product.StockQuantity = dt.Rows[i].IsNull("f_stockQuantity") ? 0 : dt.Rows[i].Field<int>("f_stockQuantity");
                        products.Add(product);
                    }
                    return (products, totalPages);
                }
                else
                {
                    return (null, 0);
                }

            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        internal bool AddProduct(ProductDetail product, List<ProductStyle> styles, List<string> images)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addProduct @categoryId, @productName, @description, @introduction, @productStatus, @style, @price, @stockQuantity, @styleStatus, @imageUrl";
                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = product.CategoryId;
                cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = product.ProductName;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = product.Introduction;
                cmd.Parameters.Add("@productStatus", SqlDbType.Bit).Value = product.ProductStatus;
                cmd.Parameters.Add("@style", SqlDbType.NVarChar).Value = product.Style;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = product.Price;
                cmd.Parameters.Add("@stockQuantity", SqlDbType.Int).Value = product.StockQuantity;
                cmd.Parameters.Add("@styleStatus", SqlDbType.Bit).Value = product.StyleStatus;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = product.ImageUrl;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                if (ExeCnt > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            finally
            {
                cmd.Parameters.Clear();
                cmd.Connection.Close();
            }
        }
    }
}