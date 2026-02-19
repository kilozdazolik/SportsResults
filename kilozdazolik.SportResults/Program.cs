using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using kilozdazolik.SportResutls;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<ScraperService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddHostedService<WorkerService>();

var host = builder.Build();
await host.RunAsync();