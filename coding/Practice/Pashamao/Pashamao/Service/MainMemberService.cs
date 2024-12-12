
using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;

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

                if (column == "Email") column = "f_email";

                if (column == "Phone") column = "f_phone";

                if (column == "MemberName") column = "f_memberName";

                if (column == "Status") column = "f_status";

                if (column == "Level") column = "f_level";

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

                return memberRepository.Create(member);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public List<Member> SelectMember(string column, string value)
        {
            //以下是目前有的搜尋欄位, 如果要擴充, 需要注意先把欄位level跟status進行轉換判斷, 可以轉換成byte(tinyint)或者bool(bit)
            if (column == "MemberId")
            {
                column = "f_memberId";
            }

            if (column == "MemberName")
            {
                column = "f_memberName";
            }



        }
    }
}