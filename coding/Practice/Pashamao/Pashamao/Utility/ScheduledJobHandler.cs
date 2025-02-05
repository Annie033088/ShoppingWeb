using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using NLog;
using Pashamao.Repositories;

namespace Pashamao.Utility
{
    public class ScheduledJobHandler
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private OrderRepository orderRepository;

        public ScheduledJobHandler()
        {
            orderRepository = new OrderRepository();
        }

        /// <summary>
        /// 於每日凌晨12點檢查"已到貨"訂單的鑑賞日期, 將超過鑑賞期線的訂單狀態變更為完成
        /// </summary>
        public void AutoEditOrderStateFromPackageArriveToFinish()
        {
            try
            {
                orderRepository.EditOrderStateFromPackageArriveToFinish();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}