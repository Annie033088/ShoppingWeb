using MockPashamao.Models;
using MockPashamao.Models.Dto.MemberDto;
using MockPashamao.Service;
using System;
using System.Web.Mvc;

namespace MockPashamao.Controllers
{
    public class MemberController : Controller
    {
        private MemberService memberService;

        public MemberController()
        {
            memberService = new MemberService();
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

                bool successFlag = memberService.CreateMember(createMemberDto);

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
                throw e;
            }
        }

    }
}