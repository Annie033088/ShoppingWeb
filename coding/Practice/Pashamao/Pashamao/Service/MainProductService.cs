using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Pashamao.Service
{
    public class MainProductService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        MainProductRepository mainProductRepository;
        public MainProductService()
        {
            mainProductRepository = new MainProductRepository();
        }

        public (List<ProductDetail>, int) GetAllProduct(string page)
        {
            try
            {
                return mainProductRepository.GetAllProduct(int.Parse(page));
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
        public (List<ProductDetail>, int) GetProductByCategory(string categoryId, string page)
        {
            try
            {
                return mainProductRepository.GetProductByCategory(int.Parse(categoryId), int.Parse(page));
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
        }

        public (List<ProductDetail>, int) GetProductById(string productId, string page)
        {
            try
            {
                Guid id = new Guid();
                bool success = Guid.TryParse(productId, out id);
                if (success)
                {
                    return mainProductRepository.GetProductById(id, int.Parse(page));
                }
                else
                {
                    return (null, 0);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
        }


        public (List<ProductDetail>, int) GetProductByName(string productName, string page)
        {
            try
            {
                return mainProductRepository.GetProductByName(productName, int.Parse(page));
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
        }

        /// <summary>
        /// 取得商品詳細資訊
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public (ProductDetail, List<ProductStyle>, List<ProductImage>) GetProductDetail(string productId)
        {
            try
            {
                (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images) = mainProductRepository.GetProductDetail(Guid.Parse(productId));
                return (product, styles, images);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public bool CreateProduct(ProductDetail product, List<CreateProductStyleViewModel> StyleList, List<CreateProductImageViewModel> ImageList)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(appDirectory);
            List<ProductStyle> productStyles = new List<ProductStyle>();
            List<string> productImageUrl = new List<string>();

            try
            {
                Guid newGuid = Guid.NewGuid();
                product.ProductId = newGuid;
                string folderPath = appDirectory + @"images\productImage\" + product.ProductId;

                //把style新增到對應的model
                for (int i = 0; i < StyleList.Count; i++)
                {
                    ProductStyle productStyle = new ProductStyle();

                    if (StyleList[i].ImageUrl == " ")
                    {
                        productStyle.ImageUrl = " ";
                    }
                    else
                    {
                        string fileName = Path.GetFileName(StyleList[i].ImageName);
                        string relativePath = @"\images\productImage\" + product.ProductId + @"\" + fileName;
                        productStyle.ImageUrl = relativePath;
                    }

                    productStyle.Price = StyleList[i].Price;
                    productStyle.Style = StyleList[i].Style;
                    productStyle.StockQuantity = StyleList[i].StockQuantity;
                    productStyle.Status = StyleList[i].Status;
                    productStyles.Add(productStyle);
                }

                //把圖片新增到對應model
                if (ImageList[0].ImageUrl == " ")
                {
                    productImageUrl.Add(" ");
                }
                else
                {
                    for (int i = 0; i < ImageList.Count; i++)
                    {
                        string fileName = Path.GetFileName(ImageList[i].ImageName);
                        string relativePath = @"\images\productImage\" + product.ProductId + @"\" + fileName;
                        productImageUrl.Add(relativePath);
                    }
                }

                bool addFlag = mainProductRepository.AddProduct(product, productStyles, productImageUrl);

                //成功的話就新建圖片檔案
                if (addFlag)
                {
                    for (int i = 0; i < productStyles.Count; i++)
                    {
                        if (productStyles[i].ImageUrl != " ")
                        {
                            string base64String = StyleList[i].ImageUrl.Substring(StyleList[i].ImageUrl.IndexOf(",") + 1);
                            byte[] imageBytes = Convert.FromBase64String(base64String);
                            string fileName = Path.GetFileName(StyleList[i].ImageName);
                            string filePath = folderPath + @"\" + fileName;

                            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                            File.WriteAllBytes(filePath, imageBytes);
                        }
                    }

                    if (productImageUrl[0] != " ")
                    {
                        for (int i = 0; i < ImageList.Count; i++)
                        {
                            string base64String = ImageList[i].ImageUrl.Substring(ImageList[i].ImageUrl.IndexOf(",") + 1);
                            byte[] imageBytes = Convert.FromBase64String(base64String);
                            string fileName = Path.GetFileName(ImageList[i].ImageName);
                            string filePath = folderPath + @"\" + fileName;

                            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                            File.WriteAllBytes(filePath, imageBytes);
                        }
                    }
                }

                return addFlag;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public bool DeleteProduct(string productId)
        {
            try
            {
                bool delSuccess = mainProductRepository.DeleteProduct(Guid.Parse(productId));
                if (delSuccess)
                {
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string absoluteImagePath = appDirectory + @"\images\productImage\" + productId;

                    if (Directory.Exists(absoluteImagePath))
                    {
                        Directory.Delete(absoluteImagePath, true);
                    }
                }
                return delSuccess;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public bool EditProduct(ProductDetail product, List<CreateProductStyleViewModel> addStyles, List<EditProductStyleViewModel> editStyles, List<ProductStyle> delStyles, List<CreateProductImageViewModel> images, List<ProductImage> delImages)
        {
            List<int> delStyleId = new List<int>();
            List<string> addImagesUrl = new List<string>();
            List<ProductStyle> addProductStyles = new List<ProductStyle>();
            List<ProductStyle> editProductStyles = new List<ProductStyle>();
            List<int> delImageId = new List<int>();
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            //取得刪除的styleId
            if (delStyles == null)
            {
                delStyleId.Add(0);
            }
            else
            {
                for (int i = 0; i < delStyles.Count; i++)
                {
                    delStyleId.Add(delStyles[i].ProductStyleId);
                }
            }

            //取得新增image的路徑
            if (images.Count == 0)
            {
                addImagesUrl.Add(" ");
            }
            else
            {
                for (int i = 0; i < images.Count; i++)
                {
                    string fileName = Path.GetFileName(images[i].ImageName);
                    string relativePath = @"\images\productImage\" + product.ProductId + @"\" + fileName;
                    addImagesUrl.Add(relativePath);
                }
            }

            //取得刪除的imageId
            if (delImages.Count == 0)
            {
                delImageId.Add(0);
            }
            else
            {
                for (int i = 0; i < delImages.Count; i++)
                {
                    delImageId.Add(delImages[i].ProductImageId);
                }
            }

            //設定新增style時, image的路徑
            for (int i = 0; i < addStyles.Count; i++)
            {
                ProductStyle productStyle = new ProductStyle();

                if (addStyles[i].ImageUrl == " ")
                {
                    productStyle.ImageUrl = " ";
                }
                else
                {
                    string fileName = Path.GetFileName(addStyles[i].ImageName);
                    string relativePath = @"\images\productImage\" + product.ProductId + @"\" + fileName;
                    productStyle.ImageUrl = relativePath;
                }

                productStyle.Price = addStyles[i].Price;
                productStyle.Style = addStyles[i].Style;
                productStyle.StockQuantity = addStyles[i].StockQuantity;
                productStyle.Status = addStyles[i].Status;
                addProductStyles.Add(productStyle);
            }

            //設定修改style時, image的路徑
            for (int i = 0; i < editStyles.Count; i++)
            {
                ProductStyle productStyle = new ProductStyle();

                if (editStyles[i].NewImageUrl == " ")
                {
                    productStyle.ImageUrl = " ";
                }
                else
                {
                    string fileName = Path.GetFileName(editStyles[i].ImageName);
                    string relativePath = @"\images\productImage\" + product.ProductId + @"\" + fileName;
                    productStyle.ImageUrl = relativePath;
                }

                productStyle.ProductStyleId = editStyles[i].ProductStyleId;
                productStyle.Price = editStyles[i].Price;
                productStyle.Style = editStyles[i].Style;
                productStyle.StockQuantity = addStyles[i].StockQuantity;
                productStyle.Status = addStyles[i].Status;
                editProductStyles.Add(productStyle);
            }

            bool editSuccessFlag = mainProductRepository.EditProduct(product, addProductStyles, editProductStyles, delStyleId, addImagesUrl, delImageId);
            string folderPath = appDirectory + @"images\productImage\" + product.ProductId;

            //資料庫修改成功
            if (editSuccessFlag)
            {
                //刪除細項對應圖片
                for (int i = 0; i < delStyles.Count; i++)
                {
                    string absoluteImagePath = appDirectory + delStyles[i].ImageUrl;

                    if (File.Exists(absoluteImagePath))
                    {
                        File.Delete(absoluteImagePath);
                    }
                }

                //刪除展示圖片
                for (int i = 0; i < delImages.Count; i++)
                {
                    string absoluteImagePath = appDirectory + delImages[i].ImageUrl;

                    if (File.Exists(absoluteImagePath))
                    {
                        File.Delete(absoluteImagePath);
                    }
                }

                //修改細項的圖片
                for (int i = 0; i < editStyles.Count; i++)
                {
                    //代表檔案不是預設
                    if (editStyles[i].ImageName != " ")
                    {
                        if (Directory.Exists(folderPath))
                        {
                            //刪除舊檔案
                            string delOldImagePath = appDirectory + editStyles[i].OldImageUrl;

                            if (File.Exists(delOldImagePath))
                            {
                                File.Delete(delOldImagePath);
                            }

                            //新增檔案
                            string base64String = editStyles[i].NewImageUrl.Substring(editStyles[i].NewImageUrl.IndexOf(",") + 1);
                            byte[] imageBytes = Convert.FromBase64String(base64String);
                            string fileName = Path.GetFileName(editStyles[i].ImageName);
                            string addImagePath = folderPath + @"\" + fileName;
                            File.WriteAllBytes(addImagePath, imageBytes);
                        }
                    }
                }

                //新增展示圖片

            }
            return editSuccessFlag;
        }
    }
}