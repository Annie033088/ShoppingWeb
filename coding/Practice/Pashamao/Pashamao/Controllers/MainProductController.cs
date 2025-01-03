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
    [UserRoleAuthFilter(UserPermission.SelectProduct | UserPermission.CreateProduct | UserPermission.EditProduct | UserPermission.DelProduct)]
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
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得所有商品
        /// </summary>
        [HttpPost]
        public ActionResult GetProduct(RequestGetSelectProductDto getSelectProductDto)
        {
            try
            {
                //檢查前端資料
                if (getSelectProductDto.ProductId != null)
                {
                    Guid productId = Guid.NewGuid();
                    if (!Guid.TryParse(getSelectProductDto.ProductId, out productId))
                    {
                        string errorMessage = "錯誤的產品Id";
                        return Json(new { errorMessage });
                    }
                }

                if (getSelectProductDto.ProductName != null)
                {
                    if (getSelectProductDto.ProductName.Length > 30)
                    {
                        string errorMessage = "錯誤的產品名稱";
                        return Json(new { errorMessage });
                    }
                }

                //呼叫服務
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProduct(getSelectProductDto);

                if (products == null)
                {
                    string errorMessage = "沒有商品";
                    return Json(new { errorMessage });
                }
                else
                {
                    List<MainProductDto> mainProducts = products.Select(product => new MainProductDto(product)).ToList();
                    return Json((new { products = mainProducts, totalPage }));
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
            }
        }

        /// <summary>
        /// 根據分類取得商品
        /// </summary>
        [HttpPost]
        public ActionResult GetProductByCategory(int CategoryId, int Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductByCategory(CategoryId, Page);

                if (products == null)
                {
                    string errorMessage = "沒找到商品";
                    return Json(new { errorMessage });
                }
                else
                {
                    List<MainProductDto> mainProducts = products.Select(product => new MainProductDto(product)).ToList();
                    return Json((new { products = mainProducts, totalPage }));
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
            }
        }

        /// <summary>
        /// 根據商品id取得商品
        /// </summary>
        [HttpPost]
        public ActionResult GetProductById(string ProductId, int Page)
        {
            try
            {
                Guid productId = new Guid();
                bool successFlag = Guid.TryParse(ProductId, out productId);

                if (!successFlag)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductById(productId, Page);

                if (products == null)
                {
                    string errorMessage = "沒找到商品";
                    return Json(new { errorMessage });
                }
                else
                {
                    List<MainProductDto> mainProducts = products.Select(product => new MainProductDto(product)).ToList();
                    return Json((new { products = mainProducts, totalPage }));
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
            }
        }

        /// <summary>
        /// 根據名稱取得商品
        /// </summary>
        [HttpPost]
        public ActionResult GetProductByName(string ProductName, int Page)
        {
            try
            {
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProductByName(ProductName, Page);

                if (products == null)
                {
                    string errorMessage = "沒找到商品";
                    return Json(new { errorMessage });
                }
                else
                {
                    List<MainProductDto> mainProducts = products.Select(product => new MainProductDto(product)).ToList();
                    return Json((new { products = mainProducts, totalPage }));
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
            }
        }

        /// <summary>
        /// 轉到創建商品頁面
        /// </summary>
        [UserRoleAuthFilter(UserPermission.CreateProduct)]
        public ActionResult CreateProduct()
        {
            return View();
        }

        /// <summary>
        /// 提交創建商品
        /// </summary>
        [UserRoleAuthFilter(UserPermission.CreateProduct)]
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
        [UserRoleAuthFilter(UserPermission.DelProduct)]
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
        [UserRoleAuthFilter(UserPermission.EditProduct)]
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
        [UserRoleAuthFilter(UserPermission.EditProduct)]
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
        [UserRoleAuthFilter(UserPermission.EditProduct)]
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
        [UserRoleAuthFilter(UserPermission.EditProduct)]
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
        [UserRoleAuthFilter(UserPermission.EditProduct)]
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
        [UserRoleAuthFilter(UserPermission.EditProduct)]
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