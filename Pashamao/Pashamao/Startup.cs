using System.Configuration;
using Hangfire;
using Owin;
using Pashamao.Utility;

namespace Pashamao
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 配置 Hangfire 使用 SQL Server 存储
            GlobalConfiguration.Configuration.UseSqlServerStorage(ConfigurationManager.ConnectionStrings["ConnStr"].ToString()); // 替换为你的连接字符串

            // 启动 Hangfire 服务器
            app.UseHangfireServer();

            // 启动 Hangfire Dashboard
            app.UseHangfireDashboard();

            //設定每日00:00運行排成
            RecurringJob.AddOrUpdate<ScheduledJobHandler>(
                "EditOrderStateFromPackageArriveToFinish",
                service => service.AutoEditOrderStateFromPackageArriveToFinish(),
                 Cron.Daily());

            //系統啟動時無條件運行一次
            BackgroundJob.Enqueue<ScheduledJobHandler>(service => service.AutoEditOrderStateFromPackageArriveToFinish());

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
            {
                Attempts = 6,   // 設定重試次數
                DelaysInSeconds = new[] { 10, 30, 60, 120, 240, 300 }  // 設定重試延遲時間
            });
        }
    }
}