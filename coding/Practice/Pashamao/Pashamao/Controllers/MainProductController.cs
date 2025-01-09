using Newtonsoft.Json;
using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.ProductDto;
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
        /// 取得(搜尋)商品
        /// </summary>
        [HttpPost]
        public ActionResult GetProduct(RequestGetSelectProductDto getSelectProductDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                if (getSelectProductDto.ProductId != null)
                {
                    Guid productId = Guid.NewGuid();
                    if (!Guid.TryParse(getSelectProductDto.ProductId, out productId))
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                if (getSelectProductDto.Name != null)
                {
                    if (getSelectProductDto.Name.Length > 30)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                //呼叫服務
                (List<ProductDetail> products, int totalPage) = mainProductService.GetProduct(getSelectProductDto);

                if (products == null)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    List<ResponseMainProductDto> mainProducts = products.Select(product => new ResponseMainProductDto(product)).ToList();
                    errorCode = ErrorCodeDefine.Success;
                    return Json((new { products = mainProducts, totalPage, errorCode }));
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 轉到創建商品頁面
        /// </summary>
        [UserRoleAuthFilter(UserPermission.CreateProduct)]
        public ActionResult GetCreateProductView()
        {
            return View("CreateProduct");
        }

        /// <summary>
        /// 提交創建商品
        /// </summary>
        [UserRoleAuthFilter(UserPermission.CreateProduct)]
        [HttpPost]
        public ActionResult CreateProduct(RequestCreateProductDto createProductDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = mainProductService.CreateProduct(createProductDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.CreateFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        [UserRoleAuthFilter(UserPermission.DelProduct)]
        [HttpPost]
        public ActionResult DeleteProduct(RequestDeleteProductDto productId)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = mainProductService.DeleteProduct(productId.ProductId);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.DeleteFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 取得商品的介紹、圖片跟樣式等等
        /// </summary>
        [UserRoleAuthFilter(UserPermission.EditProduct)]
        public ActionResult ProductDetail(string productId)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //驗證前端資料
                Guid productIdGuid = Guid.NewGuid();

                if (!Guid.TryParse(productId, out productIdGuid))
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images) = mainProductService.GetProductDetail(productIdGuid);

                ResponseProductDetailDto productDetail = new ResponseProductDetailDto
                {
                    SelectProductDetailDto = new ResponseSelectProductDetailDto(product),
                    SelectProductStyleDto = styles.Select(style => (new ResponseSelectProductStyleDto(style))).ToList(),
                    SelectProductImages = images
                };

                string jsonData = JsonConvert.SerializeObject((new { productDetail }));
                ViewBag.JsonData = jsonData;
                return View();
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 提交修改商品
        /// </summary>
        [UserRoleAuthFilter(UserPermission.EditProduct)]
        [HttpPost]
        public ActionResult EditProduct(RequestEditProductDetailDto product)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = mainProductService.EditProduct(product);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
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
                ErrorCodeDefine errorCode = 0;
                //驗證前端資料
                var files = Request.Files;
                Guid productId = new Guid();

                if (!Guid.TryParse(Request.Form["ProductId"], out productId))
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                DateTime lastEditTime = new DateTime();

                if (!DateTime.TryParse(Request.Form["LastEditTime"], out lastEditTime))
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                List<int> delImageIdList = JsonConvert.DeserializeObject<List<int>>(Request.Form["DelImageList"]);

                //先判斷圖片Id的陣列是否為空 再檢查id是不是小於0
                if (delImageIdList.Count != 0)
                {
                    if (delImageIdList.All(id => id < 0))
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                bool successFlag = mainProductService.EditProductImage(productId, lastEditTime, delImageIdList, files);


                errorCode = ErrorCodeDefine.Success;
                return Json(new { errorCode });
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
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
                ErrorCodeDefine errorCode = 0;
                var files = Request.Files;
                DateTime lastEditTime = new DateTime();

                if (!DateTime.TryParse(Request.Form["LastEditTime"], out lastEditTime))
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                if (Request.Form["EditStyle"] == null)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                ProductStyle EditStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["EditStyle"]);
                string ImageType = Request.Form["ImageType"];
                bool successFlag = mainProductService.EditProductStyle(EditStyle, files, ImageType, lastEditTime);
                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
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
                ErrorCodeDefine errorCode = 0;
                var files = Request.Files;
                DateTime lastEditTime = new DateTime();

                if (!DateTime.TryParse(Request.Form["LastEditTime"], out lastEditTime))
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                if (Request.Form["AddStyle"] == null)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                ProductStyle AddStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["AddStyle"]);
                string ImageType = Request.Form["ImageType"];
                bool successFlag = mainProductService.AddProductStyle(AddStyle, files, ImageType, lastEditTime);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.CreateFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 刪除商品細項
        /// </summary>
        [UserRoleAuthFilter(UserPermission.EditProduct)]
        [HttpPost]
        public ActionResult DeleteProductStyle(RequestDeleteProductStyleDto deleteProductStyleDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = mainProductService.DeleteProductStyle(deleteProductStyleDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.DeleteFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }
    }
}