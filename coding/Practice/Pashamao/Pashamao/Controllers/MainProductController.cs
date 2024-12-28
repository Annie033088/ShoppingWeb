using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.SelectProduct | UserPermission.CreateProduct | UserPermission.EditProductName | UserPermission.EditProductDescription | UserPermission.EditProductPrice | UserPermission.EditProductQuantity | UserPermission.EditProductCategory | UserPermission.EditProductStatus | UserPermission.DelProduct)]
    public class MainProductController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        MainProductService mainProductService;

        public MainProductController()
        {
            mainProductService = new MainProductService();
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
                (List<ProductDetail> products, int totalPage) = mainProductService.GetAllProduct(Page);
                if (products == null)
                {
                    return Json("noProduct", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((products, totalPage), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        [HttpPost]
        public ActionResult GetProductByCategory(string LastSelectCategoryId, string Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductByCategory(LastSelectCategoryId, Page);

                if (products == null)
                {
                    return Json("noProduct", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((products, totalPage), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        [HttpPost]
        public ActionResult GetProductById(string LastSelectProductId, string Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductById(LastSelectProductId, Page);

                if (products == null)
                {
                    return Json("noProduct", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((products, totalPage), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        [HttpPost]
        public ActionResult GetProductByName(string LastSelectProductName, string Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductByName(LastSelectProductName, Page);

                if (products == null)
                {
                    return Json("noProduct", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((products, totalPage), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        public ActionResult CreateProduct(ProductDetail productDetail, List<CreateProductStyleViewModel> StyleList, List<CreateProductImageViewModel> ImageList)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitCreateProduct(ProductDetail productDetail, List<CreateProductStyleViewModel> StyleList, List<CreateProductImageViewModel> ImageList)
        {
            try
            {
                bool successFlag = mainProductService.CreateProduct(productDetail, StyleList, ImageList);
                return Json(successFlag);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        [HttpPost]
        public ActionResult DeleteProduct(string ProductId)
        {
            try
            {

                bool successFlag = mainProductService.DeleteProduct(ProductId);
                return Json(successFlag, JsonRequestBehavior.AllowGet);
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
        public ActionResult ProductDetail(string ProductId)
        {
            try
            {
                (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images) = mainProductService.GetProductDetail(ProductId);

                if (images == null)
                {
                    string jsonData = JsonConvert.SerializeObject((product, styles, "noImage"));
                    ViewBag.JsonData = jsonData;
                }
                else
                {
                    string jsonData = JsonConvert.SerializeObject((product, styles, images));
                    ViewBag.JsonData = jsonData;
                }

                return View();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        [HttpPost]
        public ActionResult SubmitEditProduct(ProductDetail Product)
        {
            bool successFlag = mainProductService.EditProduct(Product);
            return Json(successFlag);
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
            List<ProductImage> delOldImageList = JsonConvert.DeserializeObject<List<ProductImage>>(Request.Form["DelImageList"]);
            bool successFlag = mainProductService.EditProductImage(productId, delOldImageList, files);

            return Json(successFlag);
        }

        /// <summary>
        /// 修改商品的細項
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductStyle()
        {
            var files = Request.Files;
            ProductStyle EditStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["EditStyle"]);
            bool success = mainProductService.EditProductStyle(EditStyle, files);
            return Json(success);
        }

        /// <summary>
        /// 新增商品細項
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProductStyle()
        {
            var files = Request.Files;
            ProductStyle AddStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["AddStyle"]);
            bool success = mainProductService.AddProductStyle(AddStyle, files);
            return Json(success);
        }

        /// <summary>
        /// 新增商品細項
        /// </summary>
        /// <param name="ProductStyleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProductStyle(int ProductStyleId)
        {
            bool success = mainProductService.DeleteProductStyle(ProductStyleId);
            return Json(success);
        }
    }
}