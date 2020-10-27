using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using System.Threading;
using Microsoft.DotNet.Interactive.Events;
using blazoract.Shared;
using MonacoRazor;
using Microsoft.CodeAnalysis.Text;
using System.Linq;

namespace blazoract.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
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
        public async Task<ExecuteResult> EvaluateCellAsync([FromBody] ExecuteRequest cell)
        {
            var request = await _kernel.SendAsync(new SubmitCode(cell.Input), new CancellationToken());
            var result = new ExecuteResult();
            request.KernelEvents.Subscribe(x =>
            {
                Console.WriteLine($"Received event: {x}");
                switch (x)
                {
                    case DisplayEvent displayEvent:
                        result.Output = displayEvent.Value;
                        break;
                    case CommandFailed commandFailed:
                        result.CommandFailedMessage = commandFailed.Message;
                        break;
                }
            });
            return result;
        }

        [HttpPost]
        public async Task<Suggestion[]> GetCompletionsAsync([FromBody] GetCompletionsRequest req)
        {
            var request = await _kernel.SendAsync(new RequestCompletions(req.Code, new LinePosition(req.LineNumber - 1, req.Column - 1)));
            var result = Array.Empty<Suggestion>();
            request.KernelEvents.Subscribe(x =>
            {
                if (x is CompletionsProduced completions)
                {
                    result = completions.Completions.Select(c => new Suggestion
                    {
                        Label = c.DisplayText,
                        InsertText = c.InsertText,
                        Kind = Enum.TryParse<CompletionItemKind>(c.Kind, out var parsedKind) ? parsedKind : CompletionItemKind.Property,
                        Documentation = c.Documentation,
                    }).ToArray();
                }
            });
            return result;
        }
    }
}
