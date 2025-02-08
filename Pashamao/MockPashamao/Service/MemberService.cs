using MockPashamao.Models.Dto.MemberDto;
using MockPashamao.Repositories;
using System;

namespace MockPashamao.Service
{
    public class MemberService
    {
        private MemberRepository memberRepository;

        public MemberService()
        {
            memberRepository = new MemberRepository();
        }

        /// <summary>
        /// 創建會員
        /// </summary>
        public bool CreateMember(RequestCreateMemberDto createMemberDto)
        {
            try
            {
                return memberRepository.CreateMember(createMemberDto);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}