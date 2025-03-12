using System.Net;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;
using Microsoft.Extensions.Hosting;
using NSwag.CodeGeneration.CSharp;
using NSwag;

namespace ApiClientGeneration
{
    /// <summary>
    /// Background service to generate API client
    /// </summary>
    public class ApiClientGenerationService : BackgroundService
    {
        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var wclient = new WebClient())
            {
                var document = await OpenApiDocument.FromJsonAsync(wclient.DownloadString("https://localhost:44349/swagger/v1/swagger.json"));

                var settings = new CSharpClientGeneratorSettings
                {
                    ClassName = "GeneratedApiClient",
                    CSharpGeneratorSettings = { Namespace = "ApiClientGeneration" }
                };

                var generator = new CSharpClientGenerator(document, settings);
                var code = generator.GenerateFile();

                // Optionally, write the generated code to a file
                await File.WriteAllTextAsync("GeneratedApiClient.cs", code);
            }
        }
    }
}