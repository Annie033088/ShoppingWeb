using NLog;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Web.Optimization;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace Pashamao.Repositories
{
    public class MainProductRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得所有商品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        internal (List<ProductDetail>, int) GetAllProduct(int page)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ProductDetail> products = new List<ProductDetail>();
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
                        ProductDetail product = new ProductDetail();
                        product.ProductId = dt.Rows[i].IsNull("f_productId") ? Guid.Empty : dt.Rows[i].Field<Guid>("f_productId");
                        product.CategoryId = dt.Rows[i].IsNull("f_categoryId") ? 0 : dt.Rows[i].Field<int>("f_categoryId");
                        product.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        product.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
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
        /// 取得分類下的商品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        internal (List<ProductDetail>, int) GetProductByCategory(int categoryId, int page)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ProductDetail> products = new List<ProductDetail>();
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
                        ProductDetail product = new ProductDetail();
                        product.ProductId = dt.Rows[i].IsNull("f_productId") ? Guid.Empty : dt.Rows[i].Field<Guid>("f_productId");
                        product.CategoryId = dt.Rows[i].IsNull("f_categoryId") ? 0 : dt.Rows[i].Field<int>("f_categoryId");
                        product.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        product.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
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
        /// 取得id對應的商品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        internal (List<ProductDetail>, int) GetProductById(Guid productId, int page)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ProductDetail> products = new List<ProductDetail>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getProductById @productId, @page, @totalPages OUTPUT";

                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productId;
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
                        ProductDetail product = new ProductDetail();
                        product.ProductId = dt.Rows[i].IsNull("f_productId") ? Guid.Empty : dt.Rows[i].Field<Guid>("f_productId");
                        product.CategoryId = dt.Rows[i].IsNull("f_categoryId") ? 0 : dt.Rows[i].Field<int>("f_categoryId");
                        product.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        product.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
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
        /// 取得對應名稱的商品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        internal (List<ProductDetail>, int) GetProductByName(string productName, int page)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ProductDetail> products = new List<ProductDetail>();
            int totalPages = 0;
            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getProductByName @name, @page, @totalPages OUTPUT";

                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = productName;
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
                        ProductDetail product = new ProductDetail();
                        product.ProductId = dt.Rows[i].IsNull("f_productId") ? Guid.Empty : dt.Rows[i].Field<Guid>("f_productId");
                        product.CategoryId = dt.Rows[i].IsNull("f_categoryId") ? 0 : dt.Rows[i].Field<int>("f_categoryId");
                        product.Name = dt.Rows[i].IsNull("f_name") ? string.Empty : dt.Rows[i].Field<string>("f_name");
                        product.Status = dt.Rows[i].IsNull("f_status") ? false : dt.Rows[i].Field<bool>("f_status");
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
        internal (ProductDetail, List<ProductStyle>, List<ProductImage>) GetProductDetail(Guid productId)
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

                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productId;

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
        /// 新增商品
        /// </summary>
        /// <param name="product"></param>
        /// <param name="styles"></param>
        /// <param name="images"></param>
        /// <returns></returns>
        internal bool AddProduct(ProductDetail product, List<ProductStyle> styles, List<string> images)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addProduct @productId, @categoryId, @productName, @description, @introduction, @productStatus, @styles, @images";
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = product.ProductId;
                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = product.CategoryId;
                cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = product.Name;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = product.Introduction;
                cmd.Parameters.Add("@productStatus", SqlDbType.Bit).Value = product.Status;

                DataTable styleTable = new DataTable();
                styleTable.Columns.Add("f_imageUrl", typeof(string));
                styleTable.Columns.Add("f_style", typeof(string));
                styleTable.Columns.Add("f_price", typeof(decimal));
                styleTable.Columns.Add("f_stockQuantity", typeof(int));
                styleTable.Columns.Add("f_status", typeof(bool));

                foreach (ProductStyle style in styles)
                {
                    styleTable.Rows.Add(style.ImageUrl, style.Style, style.Price, style.StockQuantity, style.Status);
                }

                var stylesParam = new SqlParameter("@styles", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_addProductStyle",
                    Value = styleTable
                };
                cmd.Parameters.Add(stylesParam);

                DataTable imageTable = new DataTable();
                imageTable.Columns.Add("f_imageUrl", typeof(string));

                foreach (string image in images)
                {
                    imageTable.Rows.Add(image);
                }

                var imagesParam = new SqlParameter("@images", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_addProductImage",
                    Value = imageTable
                };
                cmd.Parameters.Add(imagesParam);

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
        /// 刪除商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        internal bool DeleteProduct(Guid productId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delProduct @productId";
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productId;

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
        /// 修改商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        internal bool EditProduct(ProductDetail product)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editProduct @productId, @categoryId, @name, @description, @introduction, @status";
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = product.ProductId;
                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = product.CategoryId;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.Name;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = product.Introduction;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = product.Status;

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
        /// 修改商品圖片(包含新增及刪除)
        /// </summary>
        /// <param name="delImageId"></param>
        /// <param name="productId"></param>
        /// <param name="addImageUrl"></param>
        /// <returns></returns>
        internal bool EditProductImage(List<int> delImageId, Guid productId, List<string> addImageUrl)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delAndAddProductImage @delImageId, @productId, @addImageUrl";

                cmd.Parameters.Add("@delImageId", SqlDbType.VarChar).Value = JsonConvert.SerializeObject(delImageId);
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productId;
                cmd.Parameters.Add("@addImageUrl", SqlDbType.NVarChar).Value = JsonConvert.SerializeObject(addImageUrl);

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

        internal (List<string>, bool) EditProductStyle(Guid productId, List<ProductStyle> addStyles, List<ProductStyle> editStyleWithImage, List<ProductStyle> editStyleWithoutImage, List<string> delStyleId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editProductStyle @productId, @addStyles, @editStyleWithImage, @editStyleWithoutImage, @delStyleId";
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productId;
                if (delStyleId == null)
                {
                    cmd.Parameters.Add("@delStyleId", SqlDbType.NVarChar).Value = JsonConvert.SerializeObject(" ");
                }
                else
                {
                    cmd.Parameters.Add("@delStyleId", SqlDbType.NVarChar).Value = JsonConvert.SerializeObject(delStyleId);
                }

                //加入新增style的table
                DataTable addStyleTable = new DataTable();
                addStyleTable.Columns.Add("f_imageUrl", typeof(string));
                addStyleTable.Columns.Add("f_style", typeof(string));
                addStyleTable.Columns.Add("f_price", typeof(decimal));
                addStyleTable.Columns.Add("f_stockQuantity", typeof(int));
                addStyleTable.Columns.Add("f_status", typeof(bool));

                foreach (ProductStyle style in addStyles)
                {
                    addStyleTable.Rows.Add(style.ImageUrl, style.Style, style.Price, style.StockQuantity, style.Status);
                }

                var addStylesParam = new SqlParameter("@addStyles", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_addProductStyle",
                    Value = addStyleTable
                };
                cmd.Parameters.Add(addStylesParam);

                //加入修改style的table(有改圖片)
                DataTable editStyleWithImageTable = new DataTable();
                editStyleWithImageTable.Columns.Add("f_productStyleId", typeof(int));
                editStyleWithImageTable.Columns.Add("f_imageUrl", typeof(string));
                editStyleWithImageTable.Columns.Add("f_style", typeof(string));
                editStyleWithImageTable.Columns.Add("f_price", typeof(decimal));
                editStyleWithImageTable.Columns.Add("f_stockQuantity", typeof(int));
                editStyleWithImageTable.Columns.Add("f_status", typeof(bool));

                foreach (ProductStyle style in editStyleWithImage)
                {
                    editStyleWithImageTable.Rows.Add(style.ProductStyleId, style.ImageUrl, style.Style, style.Price, style.StockQuantity, style.Status);
                }

                var editStylesWithImageParam = new SqlParameter("@editStyleWithImage", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_editProductStyle",
                    Value = editStyleWithImageTable
                };
                cmd.Parameters.Add(editStylesWithImageParam);

                //加入修改style的table(沒改圖片)
                DataTable editStyleWithoutImageTable = new DataTable();
                editStyleWithoutImageTable.Columns.Add("f_productStyleId", typeof(int));
                editStyleWithoutImageTable.Columns.Add("f_imageUrl", typeof(string));
                editStyleWithoutImageTable.Columns.Add("f_style", typeof(string));
                editStyleWithoutImageTable.Columns.Add("f_price", typeof(decimal));
                editStyleWithoutImageTable.Columns.Add("f_stockQuantity", typeof(int));
                editStyleWithoutImageTable.Columns.Add("f_status", typeof(bool));

                foreach (ProductStyle style in editStyleWithoutImage)
                {
                    editStyleWithoutImageTable.Rows.Add(style.ProductStyleId, style.ImageUrl, style.Style, style.Price, style.StockQuantity, style.Status);
                }

                var editStylesWithoutImageParam = new SqlParameter("@editStyleWithoutImage", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_editProductStyle",
                    Value = editStyleWithoutImageTable
                };
                cmd.Parameters.Add(editStylesWithoutImageParam);

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                List<string> delImageUrls = new List<string>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string delImageUrl = dt.Rows[i].IsNull("f_imageUrl") ? string.Empty : dt.Rows[i].Field<string>("f_imageUrl");
                        delImageUrls.Add(delImageUrl);
                    }
                }
                return (delImageUrls, true);
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

        internal  bool EditProductStyleTest(Guid productId, List<ProductStyle> addStyles, List<ProductStyle> editStyleWithImage)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editProductStyleTest2 @productId, @addStyles, @editStyleWithImage";
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productId;

                //加入新增style的table
                DataTable addStyleTable = new DataTable();
                addStyleTable.Columns.Add("f_imageUrl", typeof(string));
                addStyleTable.Columns.Add("f_style", typeof(string));
                addStyleTable.Columns.Add("f_price", typeof(decimal));
                addStyleTable.Columns.Add("f_stockQuantity", typeof(int));
                addStyleTable.Columns.Add("f_status", typeof(bool));

                foreach (ProductStyle style in addStyles)
                {
                    addStyleTable.Rows.Add(style.ImageUrl, style.Style, style.Price, style.StockQuantity, style.Status);
                }

                var addStylesParam = new SqlParameter("@addStyles", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_addProductStyle",
                    Value = addStyleTable
                };
                cmd.Parameters.Add(addStylesParam);

                //加入修改style的table(有改圖片)
                DataTable editStyleWithImageTable = new DataTable();
                editStyleWithImageTable.Columns.Add("f_productStyleId", typeof(int));
                editStyleWithImageTable.Columns.Add("f_imageUrl", typeof(string));
                editStyleWithImageTable.Columns.Add("f_style", typeof(string));
                editStyleWithImageTable.Columns.Add("f_price", typeof(decimal));
                editStyleWithImageTable.Columns.Add("f_stockQuantity", typeof(int));
                editStyleWithImageTable.Columns.Add("f_status", typeof(bool));

                foreach (ProductStyle style in editStyleWithImage)
                {
                    editStyleWithImageTable.Rows.Add(style.ProductStyleId, style.ImageUrl, style.Style, style.Price, style.StockQuantity, style.Status);
                }

                var editStylesWithImageParam = new SqlParameter("@editStyleWithImage", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_editProductStyle",
                    Value = editStyleWithImageTable
                };
                cmd.Parameters.Add(editStylesWithImageParam);

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