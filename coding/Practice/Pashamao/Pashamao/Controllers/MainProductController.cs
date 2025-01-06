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
        /// 取得所有商品
        /// </summary>
        [HttpPost]
        public ActionResult GetProduct(RequestGetSelectProductDto getSelectProductDto)
        {
            try
            {
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                if (getSelectProductDto.ProductId != null)
                {
                    Guid productId = Guid.NewGuid();
                    if (!Guid.TryParse(getSelectProductDto.ProductId, out productId))
                    {
                        string errorMessage = "錯誤的產品Id";
                        return Json(new { errorMessage });
                    }
                }

                if (getSelectProductDto.Name != null)
                {
                    if (getSelectProductDto.Name.Length > 30)
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
                    List<ResponseMainProductDto> mainProducts = products.Select(product => new ResponseMainProductDto(product)).ToList();
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
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                bool successFlag = mainProductService.CreateProduct(createProductDto);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "修改失敗";
                    return Json(new { errorMessage });
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
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
                if (!ModelState.IsValid)
                {
                    // 輸出錯誤訊息，查看錯誤的詳細內容
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                bool successFlag = mainProductService.DeleteProduct(productId.ProductId);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "修改失敗";
                    return Json(new { errorMessage });
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
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
                //驗證前端資料
                Guid productIdGuid = Guid.NewGuid();

                if (!Guid.TryParse(productId, out productIdGuid))
                {
                    string errorMessage = "錯誤的產品Id";
                    return Json(new { errorMessage });
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
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
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
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                bool successFlag = mainProductService.EditProduct(product);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "修改失敗";
                    return Json(new { errorMessage });
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
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
                //驗證前端資料
                var files = Request.Files;

                Guid productId = new Guid();

                if (!Guid.TryParse(Request.Form["ProductId"], out productId))
                {
                    string errorMessage = "錯誤的產品Id";
                    return Json(new { errorMessage });
                }

                DateTime lastEditTime = new DateTime();

                if (!DateTime.TryParse(Request.Form["LastEditTime"], out lastEditTime))
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                List<int> delImageIdList = JsonConvert.DeserializeObject<List<int>>(Request.Form["DelImageList"]);

                //先判斷圖片Id的陣列是否為空 再檢查id是不是小於0
                if (delImageIdList.Count != 0)
                {
                    if(delImageIdList.All(id => id < 0))
                    {
                        string errorMessage = "無效的輸入";
                        return Json(new { errorMessage });
                    }
                }

                bool successFlag = mainProductService.EditProductImage(productId, lastEditTime, delImageIdList, files);

                return Json(new { successFlag });
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
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
                DateTime lastEditTime = new DateTime();

                if (!DateTime.TryParse(Request.Form["LastEditTime"], out lastEditTime))
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                if (Request.Form["EditStyle"] == null)
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                ProductStyle EditStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["EditStyle"]);
                string ImageType = Request.Form["ImageType"];
                bool successFlag = mainProductService.EditProductStyle(EditStyle, files, ImageType, lastEditTime);
                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "修改失敗";
                    return Json(new { errorMessage });
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
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
                DateTime lastEditTime = new DateTime();

                if (!DateTime.TryParse(Request.Form["LastEditTime"], out lastEditTime))
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                if (Request.Form["AddStyle"] == null)
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                ProductStyle AddStyle = JsonConvert.DeserializeObject<ProductStyle>(Request.Form["AddStyle"]);
                string ImageType = Request.Form["ImageType"];
                bool successFlag = mainProductService.AddProductStyle(AddStyle, files, ImageType, lastEditTime);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "新增失敗";
                    return Json(new { errorMessage });
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
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
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                bool successFlag = mainProductService.DeleteProductStyle(deleteProductStyleDto);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "刪除失敗";
                    return Json(new { errorMessage });
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
            }
        }
    }
}