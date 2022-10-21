
using ASK.BLL.Helper.Setting;
using ASK.BLL.Interfaces;
using ASK.BLL.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System.Threading.Tasks;

namespace ASK.Workers
{
    public class ReaderConcEmisParam :IJob
    {
        //private readonly IServiceScopeFactory _serviceScopeFactory;



        //public ReaderConcEmisParam(IServiceScopeFactory serviceScopeFactory)
        //{
        //    _serviceScopeFactory = serviceScopeFactory;
        //}



        public async Task Execute(IJobExecutionContext context)
        {
            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    var test = scope.ServiceProvider.GetService<IAlarmLog>();
            //    var s = test.CreateAlarmLog(true);

                await GlobalStaticSettingsASK.GetNow_ConcEmisAsync();
            //}
        }
    }
}
