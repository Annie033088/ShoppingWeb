using NLog;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Service
{
    public class MemberPointService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MemberPointRepository memberPointRepository;

        public MemberPointService()
        {
            memberPointRepository = new MemberPointRepository();
        }

        public bool ResetMemberPoint()
        {
            try
            {
                return memberPointRepository.ResetMemberPoint();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

    }
}