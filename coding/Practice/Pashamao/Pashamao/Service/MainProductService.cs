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

        public (List<ProductSimple>, int) GetAllProduct(string page)
        {
            (List<ProductSimple> products, int totalPage) = mainProductRepository.GetAllProduct(int.Parse(page));

            /*List<ProductSimple> productSimples = new List<ProductSimple>();

            foreach (ProductSimple product in products)
            {
                //這個商品的id有記錄過了嗎?預設 無
                bool haveThisProductIdFlag = false;

                //拿來設定此商品的最大金額到最小金額 跟 庫存數量
                for (int i = 0; i < productSimples.Count; i++)
                {
                    if (productSimples[i].ProductId == product.ProductId)
                    {
                        //這個商品id記錄過了=>有
                        haveThisProductIdFlag = true;

                        if (product.Price < PriceSmall[i])
                        {
                            PriceSmall[i] = product.Price;
                        }

                        if (product.Price > PriceBig[i])
                        {
                            PriceBig[i] = product.Price;
                        }

                        if (product.StockQuantity < StockQuantitySmall[i])
                        {
                            StockQuantitySmall[i] = product.StockQuantity;
                        }
                    }
                }

                if (ProductId.Count == 0 || !haveThisProductIdFlag)
                {
                    ProductId.Add(product.ProductId.ToString());
                    Name.Add(product.Name);
                    ImageUrl.Add(product.ImageUrl);
                    CategoryId.Add(product.CategoryId.ToString());
                    StockQuantitySmall.Add(product.StockQuantity);
                    PriceSmall.Add(product.Price);
                    PriceBig.Add(product.Price);
                }
            }*/

            return (null, 0);
        }


        public bool CreateProduct(ProductDetail product, List<CreateProductStyleViewModel> StyleList, List<CreateProductImageViewModel> ImageList)
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(appDirectory);
            List<ProductStyle> productStyles = new List<ProductStyle>();
            List<string> productImageUrl= new List<string>();

            try
            {
                string folderPath = appDirectory + @"images\productImage\" + product.Name;

                for (int i = 0; i < StyleList.Count; i++)
                {
                    ProductStyle productStyle = new ProductStyle();

                    string base64String = StyleList[i].ImageUrl.Substring(StyleList[i].ImageUrl.IndexOf(",") + 1);
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    string fileName = Path.GetFileName(StyleList[i].ImageName);
                    string relativePath = @"\images\productImage\" + product.Name + @"\" + fileName;
                    string filePath = folderPath + @"\" + fileName;
                    File.WriteAllBytes(filePath, imageBytes);
                    productStyle.ImageUrl = relativePath;
                    productStyle.Price = StyleList[i].Price;
                    productStyle.Style = StyleList[i].Style;
                    productStyle.StockQuantity = StyleList[i].StockQuantity;
                    productStyle.Status = StyleList[i].Status;
                    productStyles.Add(productStyle);
                }


                for (int i = 0; i < ImageList.Count; i++)
                {
                    ProductStyle productStyle = new ProductStyle();

                    string base64String = ImageList[i].ImageUrl.Substring(ImageList[i].ImageUrl.IndexOf(",") + 1);
                    byte[] imageBytes = Convert.FromBase64String(base64String);
                    string fileName = Path.GetFileName(ImageList[i].ImageName);
                    string relativePath = @"\images\productImage\" + product.Name + @"\" + fileName;
                    string filePath = folderPath + @"\" + fileName;
                    File.WriteAllBytes(filePath, imageBytes);

                    productImageUrl.Add(relativePath);
                }

                return mainProductRepository.AddProduct(product, productStyles, productImageUrl);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
            return false;
        }
    }
}