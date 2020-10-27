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
using Microsoft.CodeAnalysis.Text;

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
        public async Task<IActionResult> RunCode(
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

        [FunctionName("GetCompletions")]
        public async Task<IActionResult> GetCompletions(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "code/completions")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var completionRequest = JsonConvert.DeserializeObject<GetCompletionsRequest>(requestBody);

            var request = await _kernel.SendAsync(new RequestCompletions(completionRequest.Code, new LinePosition(completionRequest.LineNumber - 1, completionRequest.Column - 1)));
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
            return new OkObjectResult(result);
        }

        // TODO: Avoid duplication by changing MonacoRazor to target 3.0 or moving shared types to seperate package.
        public enum CompletionItemKind : int
        {
            Method = 0,
            Function = 1,
            Constructor = 2,
            Field = 3,
            Variable = 4,
            Class = 5,
            Struct = 6,
            Interface = 7,
            Module = 8,
            Property = 9,
            Event = 10,
            Operator = 11,
            Unit = 12,
            Value = 13,
            Constant = 14,
            Enum = 15,
            EnumMember = 16,
            Keyword = 17,
            Text = 18,
            Color = 19,
            File = 20,
            Reference = 21,
            Customcolor = 22,
            Folder = 23,
            TypeParameter = 24,
            User = 25,
            Issue = 26,
            Snippet = 27
        }

        public class Suggestion
        {
            public string Label { get; set; }
            public string InsertText { get; set; }
            public CompletionItemKind Kind { get; set; }
            public string Documentation { get; set; }
        }
    }
}
