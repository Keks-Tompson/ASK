using ASK.Controllers.Setting;
using Quartz;
using System.Threading.Tasks;

namespace ASK.Workers
{
    public class ReaderConcEmisParam :IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await GlobalStaticSettingsASK.GetNow_ConcEmisAsync();
        }
    }
}
