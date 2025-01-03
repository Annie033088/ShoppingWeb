
using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.MemberDto;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;

namespace Pashamao.Service
{
    public class MainMemberService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MemberRepository memberRepository;
        public MainMemberService()
        {
            memberRepository = new MemberRepository();
        }

        /// <summary>
        /// 返回排序後的會員
        /// </summary>
        internal (List<Member> members, int totalPage) GetSortedMember(RequestGetSortedMemberDto getSortedMemberDto)
        {
            try
            {
                bool haveThisColumn = false;

                if (getSortedMemberDto.SortColumn == "MemberId")
                {
                    getSortedMemberDto.SortColumn = "f_memberId";
                    haveThisColumn = true;
                }
                else if (getSortedMemberDto.SortColumn == "Email")
                {
                    getSortedMemberDto.SortColumn = "f_email";
                    haveThisColumn = true;
                }
                else if (getSortedMemberDto.SortColumn == "Phone")
                {
                    getSortedMemberDto.SortColumn = "f_phone";
                    haveThisColumn = true;
                }
                else if (getSortedMemberDto.SortColumn == "MemberName")
                {
                    getSortedMemberDto.SortColumn = "f_memberName";
                    haveThisColumn = true;
                }
                else if (getSortedMemberDto.SortColumn == "Status")
                {
                    getSortedMemberDto.SortColumn = "f_status";
                    haveThisColumn = true;
                }
                else if (getSortedMemberDto.SortColumn == "Level")
                {
                    getSortedMemberDto.SortColumn = "f_level";
                    haveThisColumn = true;
                }

                if (haveThisColumn)
                {
                    return memberRepository.GetSortedMember(getSortedMemberDto);
                }

                return (null, 0);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 創建會員
        /// </summary>
        public bool CreateMember(RequestCreateMemberDto createMemberDto)
        {
            try
            {
                Member member = new Member()
                {
                    Account = createMemberDto.Account,
                    Pwd = createMemberDto.Pwd,
                    Email = createMemberDto.Email,
                    MemberName = createMemberDto.MemberName,
                    Nickname = createMemberDto.Nickname == null ? string.Empty : createMemberDto.Nickname
                };

                if (createMemberDto.CountryCode == null)
                {
                    if (createMemberDto.Phone == null)
                    {
                        member.Phone = string.Empty;
                    }
                    else
                    {
                        member.Phone = "886" + " " + createMemberDto.Phone;
                    }
                }
                else
                {
                    if (createMemberDto.Phone != null)
                    {
                        member.Phone = createMemberDto.CountryCode + " " + createMemberDto.Phone;
                    }
                }

                return memberRepository.Create(member);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 搜尋會員
        /// </summary>
        public (List<Member> members, int totalPage) SelectMember(RequestGetSelectMemberDto getSelectMemberDto)
        {
            try
            {
                //以下是目前有的搜尋欄位, 如果要擴充, 需要注意先把欄位level跟status進行轉換判斷, 可以轉換成byte(tinyint)或者bool(bit)
                if (getSelectMemberDto.SelectColumn == "MemberId")
                {
                    getSelectMemberDto.SelectColumn = "f_memberId";
                }
                else if (getSelectMemberDto.SelectColumn == "MemberName")
                {
                    getSelectMemberDto.SelectColumn = "f_memberName";
                }

                //根據甚麼欄位進行排序
                if (getSelectMemberDto.SortColumn == "MemberId")
                {
                    getSelectMemberDto.SortColumn = "f_memberId";
                }
                else if (getSelectMemberDto.SortColumn == "Email")
                {
                    getSelectMemberDto.SortColumn = "f_email";
                }
                else if (getSelectMemberDto.SortColumn == "Phone")
                {
                    getSelectMemberDto.SortColumn = "f_phone";
                }
                else if (getSelectMemberDto.SortColumn == "MemberName")
                {
                    getSelectMemberDto.SortColumn = "f_memberName";
                }
                else if (getSelectMemberDto.SortColumn == "Status")
                {
                    getSelectMemberDto.SortColumn = "f_status";
                }
                else if (getSelectMemberDto.SortColumn == "Level")
                {
                    getSelectMemberDto.SortColumn = "f_level";
                }

                return memberRepository.GetSelectMember(getSelectMemberDto);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// 修改會員等級及狀態
        /// </summary>
        public bool EditMemberLevelAndStatus(RequestEditMemberLevelAndStatusDto editMemberLevelAndStatusDto)
        {
            Member member = new Member();
            member.MemberId = editMemberLevelAndStatusDto.MemberId;
            member.Status = editMemberLevelAndStatusDto.Status;
            member.Level = editMemberLevelAndStatusDto.Level;
            if (member.Level == 1) member.Points = 0;
            if (member.Level == 2) member.Points = 3000;
            if (member.Level == 3) member.Points = 12000;

            return memberRepository.EditMemberLevelAndStatus(member);
        }
    }
}