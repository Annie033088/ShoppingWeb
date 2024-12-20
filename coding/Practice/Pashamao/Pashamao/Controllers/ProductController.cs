using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NLog;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;

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
                (List<string> ProductId, List<string> Name, List<string> Price, List<string> ImageUrl, int totalPages) = productService.GetAllProduct(Page);

                var products = new
                {
                    ProductId,
                    Name,
                    Price,
                    ImageUrl
                };

                if (ProductId == null)
                {
                    string noProduct = "noProduct";
                    return Json(noProduct, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((products, totalPages), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

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

        [HttpPost]
        public ActionResult EditProductStyle()
        {
            var files = Request.Files;
            List<EditProductStyleViewModel> afterEditStyle = JsonConvert.DeserializeObject<List<EditProductStyleViewModel>>(Request.Form["AfterEditStyle"]);
            bool success = productService.EditProductStyle(afterEditStyle, files);

            string returnMessage = success.ToString();

            return Json(returnMessage);
        }

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

        [HttpPost]
        public ActionResult DeleteProductStyle(string ProductStyleId)
        {
            bool success = productService.DeleteProductStyle(ProductStyleId);
            string returnMessage = success.ToString();
            return Json(returnMessage);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductDetail productDetail, bool UpdateStatus)
        {
            if (UpdateStatus)
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

        [HttpPost]
        public ActionResult AddProductCategory(string Category)
        {
            bool success = productService.AddProductCategory(Category);
            string returnMessage = success.ToString();
            return Json(returnMessage);
        }

        [HttpPost]
        public ActionResult DeleteProductCategory(string CategoryId)
        {
            bool success = productService.DeleteProductCategory(CategoryId);
            string returnMessage = success.ToString();
            return Json(returnMessage);
        }
    }

}