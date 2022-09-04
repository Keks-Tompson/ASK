using ASK.BLL.Helper.Setting;
using Quartz;
using System.Threading.Tasks;


namespace ASK.Workers
{
    public class DeleteOld_4_20m : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await GlobalStaticSettingsASK.DeleteOldSensor_4_20m_Async();
        }
    }
}
