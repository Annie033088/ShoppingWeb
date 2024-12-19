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
            (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images, string categoryName) = productService.GetProductDetail(ProductId);


            if (images == null)
            {
                string jsonData = JsonConvert.SerializeObject((product, styles, "noImage", categoryName));
                ViewBag.JsonData = jsonData;
            }
            else
            {
                string jsonData = JsonConvert.SerializeObject((product, styles, images, categoryName));
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



            productService.EditProductImage(productId, delOldImageList, productName, files);
            string returnMessage = "failure";

            return Json(returnMessage);
        }

    }
}