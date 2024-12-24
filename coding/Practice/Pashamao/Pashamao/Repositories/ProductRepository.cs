using NLog;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pashamao.Repositories
{
    public class ProductRepository
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

        /// <summary>
        /// 取得商品的細節
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        internal (ProductDetail, List<ProductStyle>, List<ProductImage>) GetProductDetail(int productId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            ProductDetail product = new ProductDetail();
            List<ProductStyle> styles = new List<ProductStyle>();
            List<ProductImage> images = new List<ProductImage>();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getProductDetail @productId";

                cmd.Parameters.Add("@productId", SqlDbType.Int).Value = productId;

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Connection.Close();

                product.ProductId = productId;
                product.CategoryId = ds.Tables[0].Rows[0].IsNull("f_categoryId") ? 0 : ds.Tables[0].Rows[0].Field<int>("f_categoryId");
                product.Name = ds.Tables[0].Rows[0].IsNull("f_name") ? string.Empty : ds.Tables[0].Rows[0].Field<string>("f_name");
                product.Description = ds.Tables[0].Rows[0].IsNull("f_description") ? string.Empty : ds.Tables[0].Rows[0].Field<string>("f_description");
                product.Introduction = ds.Tables[0].Rows[0].IsNull("f_introduction") ? string.Empty : ds.Tables[0].Rows[0].Field<string>("f_introduction");
                product.Status = ds.Tables[0].Rows[0].IsNull("f_status") ? false : ds.Tables[0].Rows[0].Field<bool>("f_status");
                product.CreateTime = ds.Tables[0].Rows[0].IsNull("f_createTime") ? DateTime.Now : ds.Tables[0].Rows[0].Field<DateTime>("f_createTime");
                product.LastShelveEditTime = ds.Tables[0].Rows[0].IsNull("f_lastShelveEditTime") ? DateTime.Now : ds.Tables[0].Rows[0].Field<DateTime>("f_lastShelveEditTime");

                Console.WriteLine(ds.Tables[1].Rows[0]);

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ProductStyle style = new ProductStyle();
                    style.ProductStyleId = ds.Tables[1].Rows[i].IsNull("f_productStyleId") ? 0 : ds.Tables[1].Rows[i].Field<int>("f_productStyleId");
                    style.Style = ds.Tables[1].Rows[i].IsNull("f_style") ? string.Empty : ds.Tables[1].Rows[i].Field<string>("f_style");
                    style.Price = ds.Tables[1].Rows[i].IsNull("f_price") ? 0 : ds.Tables[1].Rows[i].Field<decimal>("f_price");
                    style.StockQuantity = ds.Tables[1].Rows[i].IsNull("f_stockQuantity") ? 0 : ds.Tables[1].Rows[i].Field<int>("f_stockQuantity");
                    style.ImageUrl = ds.Tables[1].Rows[i].IsNull("f_imageUrl") ? string.Empty : ds.Tables[1].Rows[i].Field<string>("f_imageUrl");
                    style.Status = ds.Tables[1].Rows[i].IsNull("f_status") ? false : ds.Tables[1].Rows[i].Field<bool>("f_status");
                    style.CreateTime = ds.Tables[1].Rows[i].IsNull("f_createTime") ? DateTime.Now : ds.Tables[1].Rows[i].Field<DateTime>("f_createTime");
                    style.LastShelveEditTime = ds.Tables[1].Rows[i].IsNull("f_lastShelveEditTime") ? DateTime.Now : ds.Tables[1].Rows[i].Field<DateTime>("f_lastShelveEditTime");

                    styles.Add(style);
                }

                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ProductImage image = new ProductImage();
                    image.ProductImageId = ds.Tables[2].Rows[i].IsNull("f_productImageId") ? 0 : ds.Tables[2].Rows[i].Field<int>("f_productImageId");
                    image.ImageUrl = ds.Tables[2].Rows[i].IsNull("f_imageUrl") ? string.Empty : ds.Tables[2].Rows[i].Field<string>("f_imageUrl");

                    images.Add(image);
                }


                return (product, styles, images);
            }
            catch (Exception e)
            {
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
        /// 修改商品圖片
        /// </summary>
        /// <param name="delImageId"></param>
        /// <param name="productId"></param>
        /// <param name="addImageUrl"></param>
        /// <returns></returns>
        internal bool EditProductImage(string delImageId, int productId, string addImageUrl)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delAndAddProductImage @delImageId, @productId, @addImageUrl";

                cmd.Parameters.Add("@delImageId", SqlDbType.VarChar).Value = delImageId;
                cmd.Parameters.Add("@productId", SqlDbType.Int).Value = productId;
                cmd.Parameters.Add("@addImageUrl", SqlDbType.NVarChar).Value = addImageUrl;

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

        /// <summary>
        /// 修改商品細項
        /// </summary>
        /// <param name="productStyle"></param>
        /// <returns></returns>
        internal bool EditProductStyle(ProductStyle productStyle)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editProductStyle @productStyleId, @imageUrl, @style, @price, @stockQuantity, @status";
                cmd.Parameters.Add("@productStyleId", SqlDbType.Int).Value = productStyle.ProductStyleId;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = productStyle.ImageUrl;
                cmd.Parameters.Add("@style", SqlDbType.NVarChar).Value = productStyle.Style;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = productStyle.Price;
                cmd.Parameters.Add("@stockQuantity", SqlDbType.Int).Value = productStyle.StockQuantity;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = productStyle.Status;

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

        /// <summary>
        /// 修改商品細項(包括上下架時間)
        /// </summary>
        /// <param name="productStyle"></param>
        /// <returns></returns>
        internal bool EditProductStyleAndShelveTime(ProductStyle productStyle)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editProductStyleAndShelveTime @productStyleId, @imageUrl, @style, @price, @stockQuantity, @status, @lastShelveEditTime";
                cmd.Parameters.Add("@productStyleId", SqlDbType.Int).Value = productStyle.ProductStyleId;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = productStyle.ImageUrl;
                cmd.Parameters.Add("@style", SqlDbType.NVarChar).Value = productStyle.Style;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = productStyle.Price;
                cmd.Parameters.Add("@stockQuantity", SqlDbType.Int).Value = productStyle.StockQuantity;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = productStyle.Status;
                cmd.Parameters.Add("@lastShelveEditTime", SqlDbType.DateTime).Value = productStyle.LastShelveEditTime;

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

        /// <summary>
        /// 新增商品細項
        /// </summary>
        /// <param name="productStyle"></param>
        /// <returns></returns>
        internal bool AddProductStyle(ProductStyle productStyle)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addProductStyle @productId, @imageUrl, @style, @price, @stockQuantity, @status";
                cmd.Parameters.Add("@productId", SqlDbType.Int).Value = productStyle.ProductId;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = productStyle.ImageUrl;
                cmd.Parameters.Add("@style", SqlDbType.NVarChar).Value = productStyle.Style;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = productStyle.Price;
                cmd.Parameters.Add("@stockQuantity", SqlDbType.Int).Value = productStyle.StockQuantity;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = productStyle.Status;

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

        /// <summary>
        /// 刪除商品細項
        /// </summary>
        /// <param name="productStyleId"></param>
        /// <returns></returns>
        internal bool DeleteProductStyle(int productStyleId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delProductStyle @productStyleId";
                cmd.Parameters.Add("@productStyleId", SqlDbType.Int).Value = productStyleId;

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

        /// <summary>
        /// 得知商品是否有某分類
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        internal bool GetExistProductCategory(int categoryId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getExistProductCategory @categoryId";

                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = categoryId;

                cmd.Connection.Open();

                var ExeCnt = cmd.ExecuteScalar();
                if (ExeCnt == null)
                {
                    return false;
                }
                else //代表有商品有這個分類 此分類不能刪除
                {
                    return true;
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

        /// <summary>
        /// 修改商品(介紹, 大綱等等)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        internal bool EditProduct(ProductDetail product)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editProduct @productId, @categoryId, @name, @description, @introduction, @status, @lastShelveEditTime";
                cmd.Parameters.Add("@productId", SqlDbType.Int).Value = product.ProductId;
                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = product.CategoryId;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.Name;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = product.Introduction;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = product.Status;
                cmd.Parameters.Add("@lastShelveEditTime", SqlDbType.DateTime).Value = product.LastShelveEditTime;

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