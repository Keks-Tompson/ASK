using ASK.BLL.Helper.Setting;
using Quartz;
using System.Threading.Tasks;

namespace ASK.Workers
{
    public class WriterPDZ : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await GlobalStaticSettingsASK.Add_PDZ_Async();
        }
    }
}
