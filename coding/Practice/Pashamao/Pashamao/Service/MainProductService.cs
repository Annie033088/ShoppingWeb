using Newtonsoft.Json;
using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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

        public bool EditProduct(ProductDetail product)
        {
            try
            {
                return mainProductRepository.EditProduct(product);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改商品圖片(包括刪除跟新增)
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="delOldImageList"></param>
        /// <param name="productName"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool EditProductImage(string productId, List<ProductImage> delOldImageList, HttpFileCollectionBase files)
        {
            //設置增加的檔案數跟刪除的檔案
            List<string> addImageUrl = new List<string>();
            List<int> delImageId = new List<int>();

            //設置當前(檔案)位置
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(appDirectory);

            try
            {
                //下載圖片檔案
                for (int i = 0; i < files.Count; i++)
                {
                    string fileName = Path.GetFileName(files[i].FileName);
                    string folderPath = appDirectory + @"images\productImage\" + productId;

                    if (Directory.Exists(folderPath))
                    {
                        string filePath = folderPath + @"\" + fileName;
                        string relativePath = @"\images\productImage\" + productId + @"\" + fileName;
                        files[i].SaveAs(filePath);
                        addImageUrl.Add(relativePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(folderPath);
                        string filePath = folderPath + @"\" + fileName;
                        string relativePath = @"\images\productImage\" + productId + @"\" + fileName;
                        files[i].SaveAs(filePath);
                        addImageUrl.Add(relativePath);
                    }
                }

                //紀錄刪除的圖片id
                for (int i = 0; i < delOldImageList.Count; i++)
                {
                    string absoluteImagePath = appDirectory + delOldImageList[i].ImageUrl;
                    if (File.Exists(absoluteImagePath))
                    {
                        delImageId.Add(delOldImageList[i].ProductImageId);
                    }
                }

                bool EditFlag = mainProductRepository.EditProductImage(delImageId, Guid.Parse(productId), addImageUrl);

                if (EditFlag)
                {
                    //刪除圖片檔案
                    for (int i = 0; i < delOldImageList.Count; i++)
                    {
                        string absoluteImagePath = appDirectory + delOldImageList[i].ImageUrl;
                        if (File.Exists(absoluteImagePath))
                        {
                            File.Delete(absoluteImagePath);
                        }
                    }
                }
                else
                {
                    //資料庫新增失敗的話, 刪除新增的檔案
                    for (int i = 0; i < addImageUrl.Count; i++)
                    {
                        string absoluteImagePath = appDirectory + addImageUrl[i];
                        if (File.Exists(absoluteImagePath))
                        {
                            File.Delete(absoluteImagePath);
                        }
                    }
                }

                return EditFlag;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改商品細項
        /// </summary>
        /// <param name="afterEditStyle"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool EditProductStyle(ProductStyle productStyle, HttpFileCollectionBase files)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = appDirectory + @"images\productImage\" + productStyle.ProductId;
            string fileName = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

            if (files.Count > 0)
            {
                string relativePath = @"\images\productImage\" + productStyle.ProductId + @"\" + fileName ;
                productStyle.ImageUrl = relativePath;
                //下載檔案
                if (Directory.Exists(folderPath))
                {
                    string filePath = folderPath + @"\" + fileName;
                    files[0].SaveAs(filePath);
                }
                else
                {
                    Directory.CreateDirectory(folderPath);
                    string filePath = folderPath + @"\" + fileName;
                    files[0].SaveAs(filePath);
                }
            }
            else
            {
                productStyle.ImageUrl = string.Empty;
            }

            (string delImageUrl, bool editSuccessFlag) = mainProductRepository.EditProductStyle(productStyle);

            //刪除舊的檔案
            if (delImageUrl != string.Empty)
            {
                string filePath = appDirectory + delImageUrl;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            //失敗就刪除新增的檔案
            if (!editSuccessFlag)
            {
                string filePath = folderPath + @"\" + fileName;
                File.Delete(filePath);
            }

            return editSuccessFlag;
        }

        /// <summary>
        /// 新增商品細項
        /// </summary>
        /// <param name="productStyle"></param>
        /// <param name="productName"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool AddProductStyle(ProductStyle productStyle, HttpFileCollectionBase files)
        {
            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string folderPath = appDirectory + @"images\productImage\" + productStyle.ProductId;
                string fileName = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                string filePath = folderPath + @"\" + fileName;

                if (files.Count > 0)
                {
                    string mimeType = files[0].ContentType.ToLower();
                    string fileExtension = string.Empty;

                    Console.WriteLine(mimeType);

                    switch (mimeType)
                    {
                        case "image/jpeg":
                            fileExtension = ".jpg";
                            break;
                        case "image/png":
                            fileExtension = ".png";
                            break;
                        case "image/webp":
                            fileExtension = ".webp";
                            break;
                        default:
                            fileExtension = string.Empty;
                            break;
                    }

                    filePath = filePath + fileExtension;
                    string relativePath = @"\images\productImage\" + productStyle.ProductId + @"\" + fileName + fileExtension;
                    //下載檔案
                    if (Directory.Exists(folderPath))
                    {
                        files[0].SaveAs(filePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(folderPath);
                        files[0].SaveAs(filePath);
                    }
                    productStyle.ImageUrl = relativePath;
                }
                else
                {
                    productStyle.ImageUrl = string.Empty;
                }

                bool addSuccessFlag = mainProductRepository.AddProductStyle(productStyle);

                //刪除剛剛下載的檔案
                if (!addSuccessFlag)
                {
                    File.Delete(filePath);
                }

                return addSuccessFlag;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 刪除商品細項
        /// </summary>
        /// <param name="productStyleId"></param>
        /// <returns></returns>
        public bool DeleteProductStyle(int productStyleId)
        {
            (string delImageUrl, bool delSuccessFlag) = mainProductRepository.DeleteProductStyle(productStyleId);

            if (delImageUrl != null)
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = appDirectory + delImageUrl;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            return delSuccessFlag;
        }
    }
}