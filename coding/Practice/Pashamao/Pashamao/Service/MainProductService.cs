using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.ProductDto;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Pashamao.Service
{
    public class MainProductService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        MainProductRepository mainProductRepository;

        /// <summary>
        /// 初始化參數
        /// </summary>
        public MainProductService()
        {
            mainProductRepository = new MainProductRepository();
        }

        /// <summary>
        /// 取得所有商品
        /// </summary>
        public (List<ProductDetail> products, int totalPage) GetProduct(RequestGetSelectProductDto getSelectProductDto)
        {
            try
            {
                return mainProductRepository.GetProduct(getSelectProductDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 取得商品詳細資訊
        /// </summary>
        public (ProductDetail productDetail, List<ProductStyle> productStyles, List<ProductImage> productImages) GetProductDetail(Guid productId)
        {
            try
            {
                (ProductDetail product, List<ProductStyle> styles, List<ProductImage> images) = mainProductRepository.GetProductDetail(productId);

                return (product, styles, images);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 創建商品
        /// </summary>
        public bool CreateProduct(RequestCreateProductDto createProductDto)
        {
            //把新增的檔案路徑加進來, 如果失敗就可以根據路徑刪除
            List<string> addImageUrlList = new List<string>();

            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Directory.SetCurrentDirectory(appDirectory);
                Guid newGuid = Guid.NewGuid();
                createProductDto.ProductDetailDto.ProductId = newGuid;
                string folderPath = appDirectory + @"images\productImage\" + createProductDto.ProductDetailDto.ProductId;

                if (createProductDto.DisplayImageUrl != null)
                {
                    for (int i = 0; i < createProductDto.DisplayImageUrl.Count; i++)
                    {
                        // 檢查 URL 是否為佔位預設圖片，如果是，則將其替換為空字串。
                        /*if (createProductDto.DisplayImageUrl[i] == "https://localhost:44303/images/productImage/noImage.jpg")
                        {
                            createProductDto.DisplayImageUrl[i] = "";
                        }*/

                        string fileName = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss") + i;

                        //mimeType的取得方式是根據前端傳遞過來的 base64 string進行擷取 (data:image/jpeg;base64, ~) => (image/jpeg)
                        string mimeType = createProductDto.DisplayImageUrl[i].Substring(5, createProductDto.DisplayImageUrl[i].IndexOf(";") - 5);

                        //副檔名
                        string imageType = "";

                        //取得圖片的base64跟byte
                        string base64String = createProductDto.DisplayImageUrl[i].Substring(createProductDto.DisplayImageUrl[i].IndexOf(",") + 1);
                        byte[] imageBytes = Convert.FromBase64String(base64String);

                        switch (mimeType)
                        {
                            case "image/jpeg":
                                imageType = ".jpg";
                                break;
                            case "image/png":
                                imageType = ".png";
                                break;
                            case "image/webp":
                                imageType = ".webp";
                                break;
                            default:
                                return false;
                        }

                        string filePath = folderPath + @"\" + fileName + imageType;

                        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                        File.WriteAllBytes(filePath, imageBytes);
                        createProductDto.DisplayImageUrl[i] = @"\images\productImage\" + createProductDto.ProductDetailDto.ProductId + @"\" + fileName + imageType;
                        addImageUrlList.Add(filePath);
                    }
                }
                else
                {
                    createProductDto.DisplayImageUrl = new List<string>();
                }

                //新增style圖片
                for (int i = 0; i < createProductDto.ProductStyleDto.Count; i++)
                {
                    string fileName = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss") + (i + 10);
                    string a = createProductDto.ProductStyleDto[i].ImageUrl;

                    if (createProductDto.ProductStyleDto[i].ImageUrl != null)
                    {                        
                        // 檢查 URL 是否為佔位預設圖片，如果是，則將其替換為空字串。
                        /*if (createProductDto.ProductStyleDto[i].ImageUrl == "https://localhost:44303/images/productImage/noImage.jpg")
                        {
                            createProductDto.ProductStyleDto[i].ImageUrl = "";
                        }*/
                        //mimeType的取得方式是根據前端傳遞過來的 base64 string進行擷取 (data:image/jpeg;base64, ~) => (image/jpeg)
                        string mimeType = createProductDto.ProductStyleDto[i].ImageUrl.Substring(5, createProductDto.ProductStyleDto[i].ImageUrl.IndexOf(";") - 5);

                        //副檔名
                        string imageType = "";

                        //取得圖片的base64跟byte
                        string base64String = createProductDto.ProductStyleDto[i].ImageUrl.Substring(createProductDto.ProductStyleDto[i].ImageUrl.IndexOf(",") + 1);
                        byte[] imageBytes = Convert.FromBase64String(base64String);

                        switch (mimeType)
                        {
                            case "image/jpeg":
                                imageType = ".jpg";
                                break;
                            case "image/png":
                                imageType = ".png";
                                break;
                            case "image/webp":
                                imageType = ".webp";
                                break;
                            default:
                                return false;
                        }

                        string filePath = folderPath + @"\" + fileName + imageType;

                        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                        File.WriteAllBytes(filePath, imageBytes);
                        createProductDto.ProductStyleDto[i].ImageUrl = @"\images\productImage\" + createProductDto.ProductDetailDto.ProductId + @"\" + fileName + imageType;
                        addImageUrlList.Add(filePath);
                    }
                    else
                    {
                        createProductDto.ProductStyleDto[i].ImageUrl = string.Empty;
                    }
                }

                bool addFlag = mainProductRepository.CreateProduct(createProductDto);

                //失敗的話就刪除圖片檔案

                if (!addFlag)
                {
                    for (int i = 0; i < addImageUrlList.Count; i++)
                    {
                        if (File.Exists(addImageUrlList[i]))
                        {
                            File.Delete(addImageUrlList[i]);
                        }
                    }
                }

                return addFlag;
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
        public bool DeleteProduct(Guid productId)
        {
            try
            {
                bool delSuccess = mainProductRepository.DeleteProduct(productId);

                if (delSuccess)
                {
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string absoluteImagePath = appDirectory + @"\images\productImage\" + productId;

                    if (Directory.Exists(absoluteImagePath))
                    {
                        Directory.Delete(absoluteImagePath, true);
                    }
                }

                return delSuccess;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        public bool EditProduct(RequestEditProductDetailDto product)
        {
            try
            {
                return mainProductRepository.EditProduct(product);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改商品圖片(包括刪除跟新增)
        /// </summary>
        public bool EditProductImage(Guid productId, DateTime lastEditTime, List<int> delImageId, HttpFileCollectionBase files)
        {
            //設置增加的檔案路徑跟刪除的檔案
            List<string> addImageUrl = new List<string>();

            try
            {
                //設置當前(檔案)位置
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Directory.SetCurrentDirectory(appDirectory);
                //下載圖片檔案
                for (int i = 0; i < files.Count; i++)
                {
                    string fileExtension = Path.GetExtension(files[i].FileName).ToLower();
                    string fileName = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss") + i;
                    string folderPath = appDirectory + @"images\productImage\" + productId;

                    if (Directory.Exists(folderPath))
                    {
                        string filePath = folderPath + @"\" + fileName + fileExtension;
                        string relativePath = @"\images\productImage\" + productId + @"\" + fileName + fileExtension;
                        files[i].SaveAs(filePath);
                        addImageUrl.Add(relativePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(folderPath);
                        string filePath = folderPath + @"\" + fileName + fileExtension;
                        string relativePath = @"\images\productImage\" + productId + @"\" + fileName + fileExtension;
                        files[i].SaveAs(filePath);
                        addImageUrl.Add(relativePath);
                    }
                }

                (List<string> delImageUrls, bool EditFlag) = mainProductRepository.EditProductImage(productId, lastEditTime, delImageId, addImageUrl);

                //刪除舊的檔案
                for (int i = 0; i < delImageUrls.Count; i++)
                {
                    string filePath = appDirectory + delImageUrls[i];

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                if (!EditFlag)
                {
                    //資料庫新增失敗的話, 刪除新增的檔案
                    for (int i = 0; i < addImageUrl.Count; i++)
                    {
                        string absoluteImagePath = appDirectory + addImageUrl[i];

                        if (File.Exists(absoluteImagePath))
                        {
                            File.Delete(absoluteImagePath);
                        }
                    }
                }

                return EditFlag;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改商品細項
        /// </summary>
        public bool EditProductStyle(ProductStyle productStyle, HttpFileCollectionBase files, string imageType, DateTime lastEditTime)
        {
            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string folderPath = appDirectory + @"images\productImage\" + productStyle.ProductId;
                string fileName = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");

                if (files.Count > 0)
                {
                    string relativePath = @"\images\productImage\" + productStyle.ProductId + @"\" + fileName + imageType;
                    productStyle.ImageUrl = relativePath;
                    //下載檔案
                    if (Directory.Exists(folderPath))
                    {
                        string filePath = folderPath + @"\" + fileName + imageType;
                        files[0].SaveAs(filePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(folderPath);
                        string filePath = folderPath + @"\" + fileName + imageType;
                        files[0].SaveAs(filePath);
                    }
                }
                else
                {
                    productStyle.ImageUrl = string.Empty;
                }

                (string delImageUrl, bool editSuccessFlag) = mainProductRepository.EditProductStyle(productStyle, lastEditTime);

                //刪除舊的檔案
                if (delImageUrl != string.Empty)
                {
                    string filePath = appDirectory + delImageUrl;

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                //失敗就刪除新增的檔案
                if (!editSuccessFlag)
                {
                    string filePath = folderPath + @"\" + fileName + imageType;
                    File.Delete(filePath);
                }

                return editSuccessFlag;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
        }

        /// <summary>
        /// 新增商品細項
        /// </summary>
        public bool AddProductStyle(ProductStyle productStyle, HttpFileCollectionBase files, string imageType, DateTime lastEditTime)
        {
            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string folderPath = appDirectory + @"images\productImage\" + productStyle.ProductId;
                string fileName = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
                string filePath = folderPath + @"\" + fileName + imageType;

                if (files.Count > 0)
                {
                    string relativePath = @"\images\productImage\" + productStyle.ProductId + @"\" + fileName + imageType;
                    //下載檔案
                    if (Directory.Exists(folderPath))
                    {
                        files[0].SaveAs(filePath);
                    }
                    else
                    {
                        Directory.CreateDirectory(folderPath);
                        files[0].SaveAs(filePath);
                    }
                    productStyle.ImageUrl = relativePath;
                }
                else
                {
                    productStyle.ImageUrl = string.Empty;
                }

                bool addSuccessFlag = mainProductRepository.AddProductStyle(productStyle, lastEditTime);

                //刪除剛剛下載的檔案
                if (!addSuccessFlag)
                {
                    File.Delete(filePath);
                }

                return addSuccessFlag;
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
        public bool DeleteProductStyle(RequestDeleteProductStyleDto deleteProductStyleDto)
        {
            try
            {
                (string delImageUrl, bool delSuccessFlag) = mainProductRepository.DeleteProductStyle(deleteProductStyleDto);

                if (delImageUrl != null)
                {
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    string filePath = appDirectory + delImageUrl;

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                return delSuccessFlag;
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}