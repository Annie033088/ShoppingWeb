using Newtonsoft.Json;
using NLog;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Pashamao.Controllers
{
    public class ProductController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ProductService productService;

        public ProductController()
        {
            this.productService = new ProductService();
        }

        /// <summary>
        /// 商品主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得商品主頁
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllProduct(string Page)
        {
            try
            {
                (List<string> ProductId, List<string> Name, List<string> Price, List<string> ImageUrl, List<string> CategoryId, List<int> StockQuantity, List<ProductCategory> categories, int totalPages) = productService.GetAllProduct(Page);

                var products = new
                {
                    ProductId,
                    Name,
                    Price,
                    ImageUrl,
                    CategoryId,
                    StockQuantity
                };

                if (ProductId == null)
                {
                    string noProduct = "noProduct";
                    return Json((noProduct, categories), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((products, categories, totalPages), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 按分類取得商品主頁
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProductByCategory(string LastSelectCategoryId, string Page)
        {
            try
            {
                (List<string> ProductId, List<string> Name, List<string> Price, List<string> ImageUrl, List<string> CategoryId, List<int> StockQuantity, int totalPages) = productService.GetProductByCategory(LastSelectCategoryId, Page);

               
                if (ProductId == null)
                {
                    string noProduct = "noProduct";
                    return Json(noProduct, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var products = new
                    {
                        ProductId,
                        Name,
                        Price,
                        ImageUrl,
                        CategoryId,
                        StockQuantity
                    };
                    return Json((products, totalPages), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 取得商品的介紹、圖片跟樣式等等
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public ActionResult GetProductDetail(string ProductId)
        {
            (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images, List<ProductCategory> categories) = productService.GetProductDetail(ProductId);


            if (images == null)
            {
                string jsonData = JsonConvert.SerializeObject((product, styles, "noImage", categories));
                ViewBag.JsonData = jsonData;
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject((product, styles, images, categories));
                ViewBag.JsonData = jsonData;
            }

            return View();
        }

        /// <summary>
        /// 修改商品圖片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductImage()
        {
            var files = Request.Files;
            string productId = Request.Form["ProductId"];
            string productName = Request.Form["ProductName"];
            List<ProductImage> delOldImageList = JsonConvert.DeserializeObject<List<ProductImage>>(Request.Form["DelOldImageList"]);
            bool success = productService.EditProductImage(productId, delOldImageList, productName, files);
            string returnMessage = success.ToString();

            return Json(returnMessage);
        }

        /// <summary>
        /// 修改商品的細項
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductStyle()
        {
            var files = Request.Files;
            EditProductStyleViewModel afterEditStyle = JsonConvert.DeserializeObject<EditProductStyleViewModel>(Request.Form["AfterEditStyle"]);
            bool success = productService.EditProductStyle(afterEditStyle, files);
            string returnMessage = success.ToString();

            return Json(returnMessage);
        }

        /// <summary>
        /// 新增商品細項
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProductStyle()
        {
            var files = Request.Files;
            string productName = Request.Form["ProductName"];
            ProductStyle AddStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["AddStyle"]);
            bool success = productService.AddProductStyle(AddStyle, productName, files);

            string returnMessage = success.ToString();

            return Json(returnMessage);
        }

        /// <summary>
        /// 新增商品細項
        /// </summary>
        /// <param name="ProductStyleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProductStyle(string ProductStyleId)
        {
            bool success = productService.DeleteProductStyle(ProductStyleId);
            string returnMessage = success.ToString();
            return Json(returnMessage);
        }

        /// <summary>
        /// 修改商品主要信息(包含名稱, 介紹等等)
        /// </summary>
        /// <param name="productDetail"></param>
        /// <param name="UpdateStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProduct(ProductDetail productDetail, bool UpdateStatusFlag)
        {
            if (UpdateStatusFlag)
            {
                productDetail.LastShelveEditTime = DateTime.Now;
            }
            else
            {
                string minTime = "1753-01-01 00:00:00.000";
                productDetail.LastShelveEditTime = DateTime.Parse(minTime);
            }

            bool success = productService.EditProduct(productDetail);
            string returnMessage = success.ToString();
            return Json(returnMessage);
        }

        /// <summary>
        /// 新增分類
        /// </summary>
        /// <param name="Category"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProductCategory(string Category)
        {
            bool success = productService.AddProductCategory(Category);
            string returnMessage = success.ToString();
            return Json(returnMessage);
        }

        /// <summary>
        /// 刪除商品分類
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProductCategory(string CategoryId)
        {
            bool success = productService.DeleteProductCategory(CategoryId);
            string returnMessage = success.ToString();
            return Json(returnMessage);
        }

        public ActionResult CreateProduct() {
            string jsonData = JsonConvert.SerializeObject(productService.GetProductCategory());
            ViewBag.JsonData = jsonData;
            return View("CreateProduct");
        }


    }
}