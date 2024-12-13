
using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;

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
        /// <param name="column"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        internal (List<Member>, int) GetSortedMember(string column, string page, string sortOrder)
        {
            try
            {
                if (column == "MemberId") column = "f_memberId";
                else if (column == "Email") column = "f_email";
                else if (column == "Phone") column = "f_phone";
                else if (column == "MemberName") column = "f_memberName";
                else if (column == "Status") column = "f_status";
                else if (column == "Level") column = "f_level";

                return memberRepository.GetSortedMember(column, int.Parse(page), sortOrder);
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
        /// <param name="createMemberViewModel"></param>
        /// <returns></returns>
        public bool CreateMember(CreateMemberViewModel createMemberViewModel)
        {
            try
            {
                Member member = new Member()
                {
                    Acct = createMemberViewModel.Acct,
                    Pwd = createMemberViewModel.Pwd,
                    Email = createMemberViewModel.Email,
                    MemberName = createMemberViewModel.MemberName,
                    Nickname = createMemberViewModel.Nickname == null ? string.Empty : createMemberViewModel.Nickname
                };


                if (createMemberViewModel.CountryCode == null)
                {
                    if (createMemberViewModel.Phone == null)
                    {
                        member.Phone = string.Empty;
                    }
                    else
                    {
                        member.Phone = "886" + " " + createMemberViewModel.Phone;
                    }
                }
                else
                {
                    if (createMemberViewModel.Phone != null)
                    {
                        member.Phone = createMemberViewModel.CountryCode + " " + createMemberViewModel.Phone;
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
        /// <param name="selectColumn"></param>
        /// <param name="value"></param>
        /// <param name="sortColumn"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public (List<Member>, int) SelectMember(string selectColumn, string value, string sortColumn, string page, string sortOrder)
        {
            try
            {
                //以下是目前有的搜尋欄位, 如果要擴充, 需要注意先把欄位level跟status進行轉換判斷, 可以轉換成byte(tinyint)或者bool(bit)
                if (selectColumn == "MemberId")
                {
                    selectColumn = "f_memberId";
                }
                else if (selectColumn == "MemberName")
                {
                    selectColumn = "f_memberName";
                }

                //根據甚麼欄位進行排序
                if (sortColumn == "MemberId") sortColumn = "f_memberId";
                else if (sortColumn == "Email") sortColumn = "f_email";
                else if (sortColumn == "Phone") sortColumn = "f_phone";
                else if (sortColumn == "MemberName") sortColumn = "f_memberName";
                else if (sortColumn == "Status") sortColumn = "f_status";
                else if (sortColumn == "Level") sortColumn = "f_level";

                return memberRepository.GetSelectMember(selectColumn, value, sortColumn, page, sortOrder);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public bool EditMemberlevel(string memberId, string level, string status)
        {
            Member member = new Member();
            member.MemberId = int.Parse(memberId);

            if (status == "0")
            {
                member.Status = false;
            }

            else if (status == "1")
            {
                member.Status = true;
            }

            member.Level = int.Parse(level);
            if (level == "1") member.Points = 0;
            if (level == "2") member.Points = 3000;
            if (level == "3") member.Points = 12000;

            return memberRepository.UpdateMemberLevel(member); 
        }
    }
}