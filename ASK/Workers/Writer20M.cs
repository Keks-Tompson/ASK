
using Quartz;
using System;
using System.Threading.Tasks;

using System.Net.Sockets;
using ASK.BLL.Helper.Setting;

namespace ASK.Workers
{
    public class Writer20M : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {

            //if (DateTime.Now.Minute == 19 || DateTime.Now.Minute == 39 || DateTime.Now.Minute == 59)
            //{
                await GlobalStaticSettingsASK.Add_20M_Async();
            //}

        }
    }
}
