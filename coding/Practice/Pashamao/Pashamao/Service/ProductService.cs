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

        public (ProductDetail, List<ProductStyle>, List<ProductImage>) GetProductDetail(string productId)
        {
            return productRepository.GetProductDetail(int.Parse(productId));
        }
    }
}