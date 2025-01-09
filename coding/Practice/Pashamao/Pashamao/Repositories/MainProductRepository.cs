using Newtonsoft.Json;
using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.ProductDto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Pashamao.Repositories
{
    public class MainProductRepository
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string ConnStr = ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString;

        /// <summary>
        /// 取得所有商品
        /// </summary>
        internal (List<ProductDetail> products, int totalPage) GetProduct(RequestGetSelectProductDto getSelectProductDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            List<ProductDetail> products = new List<ProductDetail>();
            int totalPages = 0;

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_getProduct @categoryId, @productId, @name, @page, @totalPages OUTPUT";

                if (getSelectProductDto.CategoryId == null)
                {
                    cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = getSelectProductDto.CategoryId.Value;
                }

                if (getSelectProductDto.ProductId == null)
                {
                    cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(getSelectProductDto.ProductId);
                }

                if (getSelectProductDto.Name == null)
                {
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = getSelectProductDto.Name;
                }

                cmd.Parameters.Add("@page", SqlDbType.Int).Value = getSelectProductDto.Page;
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
        internal (ProductDetail productDetail, List<ProductStyle> productStyles, List<ProductImage> productImages) GetProductDetail(Guid productId)
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
                product.LastEditTime = ds.Tables[0].Rows[0].IsNull("f_lastEditTime") ? DateTime.MinValue : ds.Tables[0].Rows[0].Field<DateTime>("f_lastEditTime");

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ProductStyle style = new ProductStyle();
                    style.ProductStyleId = ds.Tables[1].Rows[i].IsNull("f_productStyleId") ? 0 : ds.Tables[1].Rows[i].Field<int>("f_productStyleId");
                    style.Style = ds.Tables[1].Rows[i].IsNull("f_style") ? string.Empty : ds.Tables[1].Rows[i].Field<string>("f_style");
                    style.Price = ds.Tables[1].Rows[i].IsNull("f_price") ? 0 : ds.Tables[1].Rows[i].Field<decimal>("f_price");
                    style.StockQuantity = ds.Tables[1].Rows[i].IsNull("f_stockQuantity") ? 0 : ds.Tables[1].Rows[i].Field<int>("f_stockQuantity");
                    style.ImageUrl = ds.Tables[1].Rows[i].IsNull("f_imageUrl") ? string.Empty : ds.Tables[1].Rows[i].Field<string>("f_imageUrl");
                    style.Status = ds.Tables[1].Rows[i].IsNull("f_status") ? false : ds.Tables[1].Rows[i].Field<bool>("f_status");

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
        internal bool CreateProduct(RequestCreateProductDto createProductDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addProduct @productId, @categoryId, @productName, @description, @introduction, @productStatus, @styles, @addImageUrl";
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = createProductDto.ProductDetailDto.ProductId;
                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = createProductDto.ProductDetailDto.CategoryId;
                cmd.Parameters.Add("@productName", SqlDbType.NVarChar).Value = createProductDto.ProductDetailDto.Name;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = createProductDto.ProductDetailDto.Description;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = createProductDto.ProductDetailDto.Introduction;
                cmd.Parameters.Add("@productStatus", SqlDbType.Bit).Value = createProductDto.ProductDetailDto.Status;
                cmd.Parameters.Add("@addImageUrl", SqlDbType.NVarChar).Value = JsonConvert.SerializeObject(createProductDto.DisplayImageUrl);

                DataTable styleTable = new DataTable();
                styleTable.Columns.Add("f_imageUrl", typeof(string));
                styleTable.Columns.Add("f_style", typeof(string));
                styleTable.Columns.Add("f_price", typeof(decimal));
                styleTable.Columns.Add("f_stockQuantity", typeof(int));
                styleTable.Columns.Add("f_status", typeof(bool));

                foreach (RequestCreateProductStyleDto style in createProductDto.ProductStyleDto)
                {
                    styleTable.Rows.Add(style.ImageUrl, style.Style, style.Price, style.StockQuantity, style.Status);
                }

                var stylesParam = new SqlParameter("@styles", SqlDbType.Structured)
                {
                    TypeName = "dbo.type_pashamao_addProductStyle",
                    Value = styleTable
                };
                cmd.Parameters.Add(stylesParam);

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
        internal bool EditProduct(RequestEditProductDetailDto product)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editProduct @productId, @categoryId, @name, @description, @introduction, @status, @lastEditTime";
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = product.ProductId;
                cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = product.CategoryId;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = product.Name;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = product.Description;
                cmd.Parameters.Add("@introduction", SqlDbType.NVarChar).Value = product.Introduction;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = product.Status;
                cmd.Parameters.Add("@lastEditTime", SqlDbType.DateTime).Value = product.LastEditTime;

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
        internal (List<string> delImageUrls, bool successFlag) EditProductImage(Guid productId, DateTime lastEditTime, List<int> delImageId, List<string> addImageUrl)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delAndAddProductImage @productId, @lastEditTime, @delImageId, @addImageUrl";

                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productId;
                cmd.Parameters.Add("@lastEditTime", SqlDbType.DateTime).Value = lastEditTime;
                cmd.Parameters.Add("@delImageId", SqlDbType.VarChar).Value = JsonConvert.SerializeObject(delImageId);
                cmd.Parameters.Add("@addImageUrl", SqlDbType.NVarChar).Value = JsonConvert.SerializeObject(addImageUrl);

                cmd.Connection.Open();

                da.SelectCommand = cmd;
                da.Fill(dt);

                cmd.Connection.Close();

                List<string> delImageUrls = new List<string>();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        delImageUrls.Add(dt.Rows[i].IsNull("f_imageUrl") ? string.Empty : dt.Rows[i].Field<string>("f_imageUrl"));
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

                if (cmd.Connection.State != ConnectionState.Closed) cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 修改商品細項
        /// </summary>
        internal (string delImageUrl, bool successFlag) EditProductStyle(ProductStyle productStyle, DateTime lastEditTime)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_editProductStyle @productId, @productStyleId, @imageUrl, @style, @price, @stockQuantity, @status, @lastEditTime, @delImageUrl OUTPUT";

                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productStyle.ProductId;
                cmd.Parameters.Add("@productStyleId", SqlDbType.Int).Value = productStyle.ProductStyleId;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = productStyle.ImageUrl;
                cmd.Parameters.Add("@style", SqlDbType.NVarChar).Value = productStyle.Style;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = productStyle.Price;
                cmd.Parameters.Add("@stockQuantity", SqlDbType.Int).Value = productStyle.StockQuantity;
                cmd.Parameters.Add("@status", SqlDbType.Bit).Value = productStyle.Status;
                cmd.Parameters.Add("@lastEditTime", SqlDbType.DateTime).Value = lastEditTime;
                SqlParameter delImageUrlOutput = new SqlParameter("@delImageUrl", SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Output,
                    Size = 100
                };
                cmd.Parameters.Add(delImageUrlOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                string delImageUrl = delImageUrlOutput.Value == DBNull.Value ? null : delImageUrlOutput.Value.ToString();

                if (ExeCnt > 0)
                {
                    return (delImageUrl, true);
                }
                else
                {
                    return (delImageUrl, false);
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
        internal bool AddProductStyle(ProductStyle productStyle, DateTime lastEditTime)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_addProductStyle @productId, @imageUrl, @style, @price, @stockQuantity, @status, @lastEditTime";

                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = productStyle.ProductId;
                cmd.Parameters.Add("@imageUrl", SqlDbType.NVarChar).Value = productStyle.ImageUrl;
                cmd.Parameters.Add("@style", SqlDbType.NVarChar).Value = productStyle.Style;
                cmd.Parameters.Add("@price", SqlDbType.Decimal).Value = productStyle.Price;
                cmd.Parameters.Add("@stockQuantity", SqlDbType.Int).Value = productStyle.StockQuantity;
                cmd.Parameters.Add("@status", SqlDbType.Int).Value = productStyle.Status;
                cmd.Parameters.Add("@lastEditTime", SqlDbType.DateTime).Value = lastEditTime;

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                cmd.Connection.Close();

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

                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 刪除商品細項
        /// </summary>
        internal (string delImageUrl, bool successFlag) DeleteProductStyle(RequestDeleteProductStyleDto deleteProductStyleDto)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = new SqlConnection(this.ConnStr);

            try
            {
                cmd.CommandText = "EXEC pro_pashamao_delProductStyle @productStyleId, @productId, @lastEditTime, @delImageUrl OUTPUT";
                cmd.Parameters.Add("@productStyleId", SqlDbType.Int).Value = deleteProductStyleDto.ProductStyleId;
                cmd.Parameters.Add("@productId", SqlDbType.UniqueIdentifier).Value = deleteProductStyleDto.ProductId;
                cmd.Parameters.Add("@lastEditTime", SqlDbType.DateTime).Value = deleteProductStyleDto.LastEditTime;
                SqlParameter delImageUrlOutput = new SqlParameter("@delImageUrl", SqlDbType.NVarChar)
                {
                    Direction = ParameterDirection.Output,
                    Size = 100
                };
                cmd.Parameters.Add(delImageUrlOutput);

                cmd.Connection.Open();

                int ExeCnt = cmd.ExecuteNonQuery();

                string delImageUrl = delImageUrlOutput.Value == DBNull.Value ? null : delImageUrlOutput.Value.ToString();

                if (ExeCnt > 0)
                {
                    return (delImageUrl, true);
                }
                else
                {
                    return (delImageUrl, false);
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