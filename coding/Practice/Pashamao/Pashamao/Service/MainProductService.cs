using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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
            return mainProductRepository.GetAllProduct(int.Parse(page));
        }
        public (List<ProductDetail>, int) GetProductByCategory(string categoryId, string page)
        {
            return mainProductRepository.GetProductByCategory(int.Parse(categoryId), int.Parse(page));
        }

        public (List<ProductDetail>, int) GetProductById(string productId, string page)
        {
            return mainProductRepository.GetProductById(int.Parse(productId), int.Parse(page));
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
                (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images) = mainProductRepository.GetProductDetail(int.Parse(productId));
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
                string folderPath = appDirectory + @"images\productImage\" + product.Name;

                for (int i = 0; i < StyleList.Count; i++)
                {
                    ProductStyle productStyle = new ProductStyle();

                    if (StyleList[i].ImageUrl == " ")
                    {
                        productStyle.ImageUrl = " ";
                    }
                    else
                    {

                        string base64String = StyleList[i].ImageUrl.Substring(StyleList[i].ImageUrl.IndexOf(",") + 1);
                        byte[] imageBytes = Convert.FromBase64String(base64String);
                        string fileName = Path.GetFileName(StyleList[i].ImageName);
                        string relativePath = @"\images\productImage\" + product.Name + @"\" + fileName;
                        string filePath = folderPath + @"\" + fileName;

                        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                        File.WriteAllBytes(filePath, imageBytes);
                        productStyle.ImageUrl = relativePath;

                    }

                    productStyle.Price = StyleList[i].Price;
                    productStyle.Style = StyleList[i].Style;
                    productStyle.StockQuantity = StyleList[i].StockQuantity;
                    productStyle.Status = StyleList[i].Status;
                    productStyles.Add(productStyle);
                }

                if (ImageList[0].ImageUrl == " ")
                {
                    productImageUrl.Add(" ");
                }
                else
                {
                    for (int i = 0; i < ImageList.Count; i++)
                    {
                        string base64String = ImageList[i].ImageUrl.Substring(ImageList[i].ImageUrl.IndexOf(",") + 1);
                        byte[] imageBytes = Convert.FromBase64String(base64String);
                        string fileName = Path.GetFileName(ImageList[i].ImageName);
                        string relativePath = @"\images\productImage\" + product.Name + @"\" + fileName;
                        string filePath = folderPath + @"\" + fileName;
                        File.WriteAllBytes(filePath, imageBytes);

                        productImageUrl.Add(relativePath);
                    }
                }


                bool addFlag = mainProductRepository.AddProduct(product, productStyles, productImageUrl);

                //失敗的話就刪除檔案
                if (!addFlag)
                {
                    for (int i = 0; i < productImageUrl.Count; i++)
                    {
                        string absoluteImagePath = appDirectory + productImageUrl[i];
                        if (File.Exists(absoluteImagePath))
                        {
                            File.Delete(absoluteImagePath);
                        }
                    }

                    for (int i = 0; i < productStyles.Count; i++)
                    {
                        string absoluteImagePath = appDirectory + productStyles[i].ImageUrl;
                        if (File.Exists(absoluteImagePath))
                        {
                            File.Delete(absoluteImagePath);
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
                return mainProductRepository.DeleteProduct(int.Parse(productId));
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}