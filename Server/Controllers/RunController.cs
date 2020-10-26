using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using System.Threading;
using Microsoft.DotNet.Interactive.Events;
using blazoract.Shared;

namespace blazoract.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RunController : ControllerBase
    {
        private readonly ILogger<RunController> _logger;

        private CompositeKernel _kernel;

        public RunController(ILogger<RunController> logger, CompositeKernel kernel)
        {
            _logger = logger;
            _kernel = kernel;
        }

        [HttpPost]
        public async Task<ExecuteResult> PostAsync([FromBody] ExecuteRequest cell)
        {
            var request = await _kernel.SendAsync(new SubmitCode(cell.Input), new CancellationToken());
            var result = new ExecuteResult();
            request.KernelEvents.Subscribe(x =>
            {
                Console.WriteLine($"Received event: {x}");
                if (x is DisplayEvent)
                {

                    result.Output = ((DisplayEvent)x).Value;
                }
            });
            return result;
        }
    }
}
