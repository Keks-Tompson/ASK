using ASK.Controllers.Setting;
using Quartz;
using System;
using System.Threading.Tasks;

namespace ASK.Workers
{
    public class Writer20M : IJob
    {
      
        public async Task Execute(IJobExecutionContext context)
        {
            await GlobalStaticSettingsASK.Add_20M_Async();
            //await Console.Out.WriteLineAsync("Greetings from HelloJob!");
        }
    }
}
