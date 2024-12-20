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
using System.Web.Optimization;

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

        public (ProductDetail, List<ProductStyle>, List<ProductImage>, List<ProductCategory>) GetProductDetail(string productId)
        {
            (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images) = productRepository.GetProductDetail(int.Parse(productId));
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = appDirectory + "/tableText/categoryTable.txt";
            List<ProductCategory> categories = new List<ProductCategory>();

            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    ProductCategory category = new ProductCategory();
                    string[] parts = line.Split(',');

                    if (parts.Length == 2)
                    {
                        category.CategoryId = int.Parse(parts[0].Trim());
                        category.Name = parts[1].Trim();
                    }
                    categories.Add(category);
                }
                return (product, styles, images, categories);
            }
            catch (Exception e)
            {
                logger.Error(e);
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
                            delImageId = delImageId + "," + delOldImageList[i].ProductImageId;
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

        public bool EditProductStyle(List<EditProductStyleViewModel> afterEditStyle, HttpFileCollectionBase files)
        {
            ProductStyle productStyle = new ProductStyle();
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(appDirectory);

            if (files.Count > 0)
            {
                string fileName = Path.GetFileName(files[0].FileName);
                string folderPath = appDirectory + @"images\productImage\" + afterEditStyle[0].ProductName;
                string relativePath = @"\images\productImage\" + afterEditStyle[0].ProductName + @"\" + fileName;
                //下載檔案
                if (Directory.Exists(folderPath))
                {
                    string filePath = folderPath + @"\" + fileName;
                    files[0].SaveAs(filePath);
                    productStyle.ImageUrl = relativePath;
                }
                else
                {
                    Directory.CreateDirectory(folderPath);
                    string filePath = folderPath + @"\" + fileName;
                    string absoluteFilePath = appDirectory + filePath;
                    files[0].SaveAs(absoluteFilePath);
                    productStyle.ImageUrl = relativePath;
                }

                //刪除檔案
                string absoluteImagePath = appDirectory + afterEditStyle[0].OldImageUrl;
                if (File.Exists(absoluteImagePath))
                {
                    File.Delete(absoluteImagePath);
                }
            }
            else
            {
                productStyle.ImageUrl = afterEditStyle[0].OldImageUrl;
            }

            if (afterEditStyle[0].UpdateStyleStatus)
            {
                productStyle.LastShelveEditTime = DateTime.Now;
            }
            else
            {
                productStyle.LastShelveEditTime = afterEditStyle[0].LastShelveEditTime;
            }

            productStyle.ProductStyleId = afterEditStyle[0].ProductStyleId;
            productStyle.Status = afterEditStyle[0].Status;
            productStyle.StockQuantity = afterEditStyle[0].StockQuantity;
            productStyle.Price = afterEditStyle[0].Price;
            productStyle.Style = afterEditStyle[0].Style;


            return productRepository.EditProductStyle(productStyle);
        }

        public bool AddProductStyle(ProductStyle productStyle, string productName, HttpFileCollectionBase files)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(appDirectory);
            if (files.Count > 0)
            {
                string fileName = Path.GetFileName(files[0].FileName);
                string folderPath = appDirectory + @"images\productImage\" + productName;
                string relativePath = @"\images\productImage\" + productName + @"\" + fileName;
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
                    string absoluteFilePath = appDirectory + filePath;
                    files[0].SaveAs(absoluteFilePath);
                }
                productStyle.ImageUrl = relativePath;
            }
            else
            {
                string sourceFilePath = appDirectory + @"\images\productImage\noImage.jpg";
                string destinationFilePath = appDirectory + @"images\productImage\" + productName + @"\noImage.jpg";
                File.Copy(sourceFilePath, destinationFilePath, overwrite: true);
                string relativePath = @"\images\productImage\" + productName + @"\noImage.jpg";
                productStyle.ImageUrl = relativePath;
            }
            return productRepository.AddProductStyle(productStyle);
        }

        public bool DeleteProductStyle(string productStyleId)
        {
            return productRepository.DeleteProductStyle(int.Parse(productStyleId));
        }

        public bool AddProductCategory(string categoryName)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = appDirectory + "/tableText/categoryTable.txt";
            List<int> categoriesId = new List<int>();

            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    ProductCategory category = new ProductCategory();
                    string[] parts = line.Split(',');

                    if (parts.Length == 2)
                    {
                        categoriesId.Add(int.Parse(parts[0].Trim()));
                        if (parts[1].Trim() == categoryName)
                        {
                            return false;
                        }
                    }
                }

                for (int i = 0; i < categoriesId.Count + 1; i++)
                {
                    int id = i + 1;
                    if (id != categoriesId.Count + 1)
                    {
                        bool repeatId = false;

                        for (int j = 0; j < categoriesId.Count; j++)
                        {
                            if (id == categoriesId[j])
                            {
                                repeatId = true;
                                break;
                            }
                        }

                        if (!repeatId)
                        {
                            string categoryLine = id.ToString() + "," + categoryName;
                            File.AppendAllText(filePath, categoryLine + Environment.NewLine);
                            return true;
                        }
                    }
                    else
                    {
                        string categoryLine = id.ToString() + "," + categoryName;
                        File.AppendAllText(filePath, categoryLine + Environment.NewLine);
                        return true;
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public bool DeleteProductCategory(string categoryId)
        {

            try
            {
                //沒有商品擁有這個分類才可以刪除
                if (!productRepository.GetExistProductCategory(int.Parse(categoryId)))
                {
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = appDirectory + "/tableText/categoryTable.txt";

                    List<string> lines = File.ReadAllLines(filePath).ToList();
                    List<string> newLines = new List<string>();

                    foreach (string line in lines)
                    {
                        if (!line.StartsWith(categoryId.ToString() + ","))
                        {
                            newLines.Add(line);
                        }
                    }
                    File.WriteAllLines(filePath, newLines);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        public bool EditProduct(ProductDetail product)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = appDirectory + "/tableText/categoryTable.txt";
            bool haveThisCategory = false;

            try
            {
                foreach (var line in File.ReadLines(filePath))
                {
                    string[] parts = line.Split(',');

                    if (parts.Length == 2)
                    {
                        if (int.Parse(parts[0].Trim()) == product.CategoryId)
                        {
                            haveThisCategory = true;
                            break;
                        }
                    }
                }

                if (haveThisCategory)
                {
                    return productRepository.EditProduct(product);
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
        }
    }
}