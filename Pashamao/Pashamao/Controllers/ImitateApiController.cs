using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.ImitateApiDto;
using Pashamao.Models.Dto.MemberDto;
using Pashamao.Service;
using System;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class ImitateApiController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        ImitateApiService imitateApiService;
        public ImitateApiController()
        {
            imitateApiService = new ImitateApiService();
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 創建會員頁面
        /// </summary>
        public ActionResult GetCreateMemberView()
        {
            return View("CreateMember");
        }

        /// <summary>
        /// 提交創建會員表單
        /// </summary>
        [HttpPost]
        public ActionResult CreateMember(RequestCreateMemberDto createMemberDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid || createMemberDto.Account == createMemberDto.Pwd)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                if (createMemberDto.Phone != null)
                {
                    if (createMemberDto.Phone.ToString().Length != 9)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                createMemberDto.Nickname = createMemberDto.Nickname == null ? string.Empty : createMemberDto.Nickname;

                bool successFlag = imitateApiService.CreateMember(createMemberDto);

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
        /// 創建訂單頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCreateOrderView()
        {
            try
            {
                return View("CreateOrder");
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
        /// 提交創建訂單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateOrder(RequestCreateOrderDto createOrderDto)
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

                foreach (var createOrderProductDto in createOrderDto.createOrderProductDtos)
                {
                    if (createOrderProductDto.ProductStyleId < 0)
                    {
                        errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                        return Json(new { errorCode });
                    }
                }

                bool successFlag = imitateApiService.CreateOrder(createOrderDto);

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
    }
}