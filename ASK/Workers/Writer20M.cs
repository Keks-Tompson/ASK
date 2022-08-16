using ASK.Controllers.Add;
using ASK.Controllers.Setting;
using Quartz;
using System;
using System.Threading.Tasks;

using NModbus;
using System.Net.Sockets;

namespace ASK.Workers
{
    public class Writer20M : IJob
    {
      
        public async Task Execute(IJobExecutionContext context)
        {

            if (DateTime.Now.Minute == 19 || DateTime.Now.Minute == 39 || DateTime.Now.Minute == 59)
            {
                await GlobalStaticSettingsASK.Add_20M_Async();
            }

        }
    }
}
