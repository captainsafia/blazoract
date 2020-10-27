using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Events;
using Microsoft.DotNet.Interactive.Commands;
using blazoract.Shared;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace blazoract.Api
{
    public class KernelFunction
    {
        private CompositeKernel _kernel;
        public KernelFunction(CompositeKernel kernel)
        {
            this._kernel = kernel;
        }

        [FunctionName("RunCode")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "code/run")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var cell = JsonConvert.DeserializeObject<ExecuteRequest>(requestBody);

            var request = await _kernel.SendAsync(new SubmitCode(cell.Input), new CancellationToken());
            var result = new ExecuteResult();
            request.KernelEvents.Subscribe(x =>
            {
                Console.WriteLine($"Received event: {x}");
                switch (x)
                {
                    case DisplayEvent displayEvent:
                        result.Output = displayEvent.Value?.ToString();
                        break;
                    case CommandFailed commandFailed:
                        result.CommandFailedMessage = commandFailed.Message;
                        break;
                }
            });

            return new OkObjectResult(result);
        }
    }
}
