using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using System.Threading;
using blazoract.Shared;
using Microsoft.DotNet.Interactive.Events;

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
            Console.WriteLine(cell.Input);
            var result = new ExecuteResult();
            request.KernelEvents.Subscribe(x =>
            {
                Console.WriteLine(x);
                if (x is DisplayEvent)
                {
                    Console.WriteLine(x);
                    result.Output = ((DisplayEvent)x).Value;
                }
            });
            return result;
        }
    }
}
