using NLog;
using Pashamao.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Pashamao.Repositories;
using System.Web.UI;
using System.Xml.Linq;
using System.IO;
using Microsoft.Ajax.Utilities;

namespace Pashamao.Service
{
    public class ProductService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ProductRepository productRepository;
        public ProductService()
        {
            productRepository = new ProductRepository();
        }
        public (List<string>, List<string>, List<string>, List<string>, int) GetAllProduct(string page)
        {
            try
            {
                (List<ProductSimple> products, int totalPages) = productRepository.GetAllProduct(int.Parse(page));

                if (products != null)
                {
                    List<string> ProductId = new List<string>();
                    List<string> Name = new List<string>();
                    List<string> Price = new List<string>();
                    List<string> ImageUrl = new List<string>();
                    List<decimal> PriceSmall = new List<decimal>();
                    List<decimal> PriceBig = new List<decimal>();


                    foreach (ProductSimple product in products)
                    {
                        //這個商品的id有記錄過了嗎?預設 無
                        bool haveThisProductIdFlag = false;


                        if (ProductId.Count == 0)
                        {
                            ProductId.Add(product.ProductId.ToString());
                            Name.Add(product.Name);
                            ImageUrl.Add(product.ImageUrl);
                            PriceSmall.Add(product.Price);
                            PriceBig.Add(product.Price);
                        }

                        for (int i = 0; i < ProductId.Count; i++)
                        {
                            if (ProductId[i] == product.ProductId.ToString())
                            {
                                //這個商品id記錄過了=>有
                                haveThisProductIdFlag = true;
                                if (product.Price < PriceSmall[i])
                                {
                                    PriceSmall[i] = products[i].Price;
                                }

                                if (products[i].Price > PriceBig[i])
                                {
                                    PriceBig[i] = products[i].Price;
                                }
                            }
                        }

                        if (haveThisProductIdFlag == false)
                        {
                            ProductId.Add(product.ProductId.ToString());
                            Name.Add(product.Name);
                            ImageUrl.Add(product.ImageUrl);
                            PriceSmall.Add(product.Price);
                            PriceBig.Add(product.Price);
                        }
                    }

                    for (int i = 0; i < PriceSmall.Count; i++)
                    {
                        if (PriceSmall[i] == PriceBig[i])
                        {
                            Price.Add("$" + PriceSmall[i].ToString());
                        }
                        else
                        {
                            Price.Add("$" + PriceSmall[i].ToString() + "~" + PriceBig[i].ToString());
                        }
                    }

                    return (ProductId, Name, Price, ImageUrl, totalPages);
                }


                return (null, null, null, null, 0);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public (ProductDetail, List<ProductStyle>, List<ProductImage>, string) GetProductDetail(string productId)
        {
            (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images) = productRepository.GetProductDetail(int.Parse(productId));
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = appDirectory + "/tableText/categoryTable.txt";
            string categoryName = "其他";

            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    string[] parts = line.Split(',');

                    if (parts.Length == 2)
                    {
                        if (product.CategoryId == int.Parse(parts[0].Trim()))
                        {
                            categoryName = parts[1].Trim();
                        }
                    }
                }
                return (product, styles, images, categoryName);
            }
            catch (Exception e)
            {
                logger.Error (e);
                throw e;
            }
        }

        public bool EditProductImage(string productId, List<ProductImage> delOldImageList, string productName, HttpFileCollectionBase files)
        {
            //設置增加的檔案數跟刪除的檔案
            string addImageUrl = " ";
            string delImageId = " ";

            //設置當前(檔案)位置
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(appDirectory);

            try
            {
                //下載檔案
                for (int i = 0; i < files.Count; i++)
                {
                    string fileName = Path.GetFileName(files[i].FileName);
                    string folderPath = appDirectory + @"images\productImage\" + productName;

                    if (Directory.Exists(folderPath))
                    {
                        string filePath = folderPath + @"\" + fileName;
                        string relativePath = @"\images\productImage\" + productName + @"\" + fileName;
                        files[i].SaveAs(filePath);
                        if (addImageUrl == " ")
                        {
                            addImageUrl = relativePath;
                        }
                        else
                        {
                            addImageUrl = addImageUrl + "," + relativePath;
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(folderPath);
                        string filePath = folderPath + @"\" + fileName;
                        string absoluteFilePath = appDirectory + filePath;
                        files[i].SaveAs(absoluteFilePath);
                    }
                }


                //刪除檔案
                for (int i = 0; i < delOldImageList.Count; i++)
                {
                    string absoluteImagePath = appDirectory + delOldImageList[i].ImageUrl;
                    if (File.Exists(absoluteImagePath))
                    {
                        File.Delete(absoluteImagePath);
                        if (delImageId == " ")
                        {
                            delImageId = delOldImageList[i].ProductImageId.ToString();
                        }
                        else
                        {
                            delImageId = delImageId +  "," + delOldImageList[i].ProductImageId;
                        }
                    }
                }
                return productRepository.EditProductImage(delImageId, int.Parse(productId), addImageUrl); ;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

    }
}