using Newtonsoft.Json;
using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.Product;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.SelectProduct | UserPermission.CreateProduct | UserPermission.EditProductName | UserPermission.EditProductDescription | UserPermission.EditProductPrice | UserPermission.EditProductQuantity | UserPermission.EditProductCategory | UserPermission.EditProductStatus | UserPermission.DelProduct)]
    public class MainProductController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        MainProductService mainProductService;

        /// <summary>
        /// 初始化參數
        /// </summary>
        public MainProductController()
        {
            mainProductService = new MainProductService();
        }

        /// <summary>
        /// 主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得所有商品
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllProduct(string Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetAllProduct(Page);
                List<MainProductDto> mainProducts = products.Select(product => new MainProductDto(product)).ToList();

                if (products == null)
                {
                    return Json("noProduct", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((mainProducts, totalPage), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        /// <summary>
        /// 根據分類取得商品
        /// </summary>
        /// <param name="LastSelectCategoryId"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProductByCategory(string LastSelectCategoryId, string Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductByCategory(LastSelectCategoryId, Page);
                List<MainProductDto> mainProducts = products.Select(product => new MainProductDto(product)).ToList();

                if (products == null)
                {
                    return Json("noProduct", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((mainProducts, totalPage), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        /// <summary>
        /// 根據商品id取得商品
        /// </summary>
        /// <param name="LastSelectProductId"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProductById(string LastSelectProductId, string Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductById(LastSelectProductId, Page);
                List<MainProductDto> mainProducts = products.Select(product => new MainProductDto(product)).ToList();

                if (products == null)
                {
                    return Json("noProduct", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((mainProducts, totalPage), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        /// <summary>
        /// 根據名稱取得商品
        /// </summary>
        /// <param name="LastSelectProductName"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetProductByName(string LastSelectProductName, string Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductByName(LastSelectProductName, Page);
                List<MainProductDto> mainProducts = products.Select(product => new MainProductDto(product)).ToList();

                if (products == null)
                {
                    return Json("noProduct", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((mainProducts, totalPage), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }

        }

        /// <summary>
        /// 轉到創建商品頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateProduct()
        {
            return View();
        }

        /// <summary>
        /// 提交創建商品
        /// </summary>
        /// <param name="productDetail"></param>
        /// <param name="StyleList"></param>
        /// <param name="ImageList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitCreateProduct(ProductDetail productDetail, List<ProductStyle> StyleList, List<string> ImageList)
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

        /// <summary>
        /// 刪除商品
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
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
                ProductDetailDto productDetail = new ProductDetailDto(product);
                List<ProductStyleDto> productStyle = styles.Select(style => (new ProductStyleDto(style))).ToList();

                if (images == null)
                {
                    string jsonData = JsonConvert.SerializeObject((productDetail, productStyle, "noImage"));
                    ViewBag.JsonData = jsonData;
                }
                else
                {
                    string jsonData = JsonConvert.SerializeObject((productDetail, productStyle, images));
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

        /// <summary>
        /// 提交修改商品
        /// </summary>
        /// <param name="Product"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitEditProduct(ProductDetail Product)
        {
            try
            {
                bool successFlag = mainProductService.EditProduct(Product);
                return Json(successFlag);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改商品圖片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductImage()
        {
            try
            {
                var files = Request.Files;
                string productId = Request.Form["ProductId"];
                DateTime lastEditTime = DateTime.Parse(Request.Form["LastEditTime"]);
                List<ProductImage> delOldImageList = JsonConvert.DeserializeObject<List<ProductImage>>(Request.Form["DelImageList"]);
                bool successFlag = mainProductService.EditProductImage(productId, lastEditTime, delOldImageList, files);

                return Json(successFlag);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改商品的細項
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditProductStyle()
        {
            try
            {
                var files = Request.Files;
                DateTime lastEditTime = DateTime.Parse(Request.Form["LastEditTime"]);
                ProductStyle EditStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["EditStyle"]);
                string ImageType = Request.Form["ImageType"];
                bool success = mainProductService.EditProductStyle(EditStyle, files, ImageType, lastEditTime);
                return Json(success);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 新增商品細項
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddProductStyle()
        {
            try
            {
                var files = Request.Files;
                DateTime lastEditTime = DateTime.Parse(Request.Form["LastEditTime"]);
                ProductStyle AddStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["AddStyle"]);
                string ImageType = Request.Form["ImageType"];
                bool success = mainProductService.AddProductStyle(AddStyle, files, ImageType, lastEditTime);
                return Json(success);
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
        /// <param name="ProductStyleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteProductStyle(int ProductStyleId, Guid ProductId, DateTime LastEditTime)
        {
            try
            {
                bool success = mainProductService.DeleteProductStyle(ProductStyleId, ProductId, LastEditTime);
                return Json(success);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

    }
}