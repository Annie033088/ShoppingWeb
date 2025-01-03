using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.AddressDto;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI;

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
        internal (List<MemberAddress> addresses, int totalPage) GetSortedAddress(RequestGetSortedAddressDto getSortedAddressDto)
        {
            try
            {
                bool haveThisColumn = false;

                if (getSortedAddressDto.SortColumn == "AddressId")
                {
                    getSortedAddressDto.SortColumn = "f_addressId";
                    haveThisColumn = true;
                }
                else if (getSortedAddressDto.SortColumn == "MemberId")
                {
                    getSortedAddressDto.SortColumn = "f_memberId";
                    haveThisColumn = true;
                }
                else if (getSortedAddressDto.SortColumn == "MemberAddress")
                {
                    getSortedAddressDto.SortColumn = "f_city";
                    haveThisColumn = true;
                }
                else if (getSortedAddressDto.SortColumn == "PostalCode")
                {
                    getSortedAddressDto.SortColumn = "f_postalCode";
                    haveThisColumn = true;
                }

                if (haveThisColumn)
                {
                    return memberAddressRepository.GetSortedAddress(getSortedAddressDto);
                }

                return (null, 0);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}