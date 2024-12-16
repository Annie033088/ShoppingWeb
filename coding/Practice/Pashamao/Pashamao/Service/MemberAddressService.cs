using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Service
{
    public class MemberAddressService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MemberAddressRepository memberAddressRepository;

        public MemberAddressService()
        {
            memberAddressRepository = new MemberAddressRepository();
        }


        /// <summary>
        /// 返回排序後的地址
        /// </summary>
        /// <param name="column"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        internal (List<MemberAddress>, int) GetSortedAddress(string column, string page, string sortOrder)
        {
            try
            {
                if (column == "AddressId") column = "f_addressId";
                else if (column == "MemberId") column = "f_memberId";
                else if (column == "MemberAddress") column = "f_city";
                else if (column == "PostalCode") column = "f_postalCode";

                return memberAddressRepository.GetSortedAddress(column, int.Parse(page), sortOrder);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}