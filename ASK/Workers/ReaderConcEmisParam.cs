
using ASK.BLL.Helper.Setting;
using ASK.BLL.Interfaces;
using ASK.BLL.Services;
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
