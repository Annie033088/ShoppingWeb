using NLog;
using Pashamao.Models.Dto.ImitateApiDto;
using Pashamao.Models.Dto.MemberDto;
using Pashamao.Repositories;
using System;

namespace Pashamao.Service
{
    public class ImitateApiService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ImitateApiRepository imitateApiRepository;

        public ImitateApiService()
        {
            imitateApiRepository = new ImitateApiRepository();
        }

        /// <summary>
        /// 創建會員
        /// </summary>
        public bool CreateMember(RequestCreateMemberDto createMemberDto)
        {
            try
            {
                return imitateApiRepository.CreateMember(createMemberDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        public bool CreateOrder(RequestCreateOrderDto createOrderDto)
        {
            try
            {
                string date = DateTime.Now.ToString("yyMMdd");
                string second = ((int)(DateTime.Now - DateTime.Today).TotalSeconds).ToString("D5"); //取得午夜至現在過的秒數並轉成5位

                string orderNumberStr = date + second + createOrderDto.MemberId.ToString("D7");
                long orderNumberLong = long.Parse(orderNumberStr);

                return imitateApiRepository.CreateOrder(createOrderDto, orderNumberLong);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}