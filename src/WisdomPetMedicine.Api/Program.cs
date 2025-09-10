using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false);
builder.Services.AddOcelot();

var app = builder.Build();

app.MapWhen(context => context.Request.Path == "/", appBuilder =>
{
    appBuilder.Run(async context =>
    {
        await context.Response.WriteAsync("Wisdom Pet Medicine - API Gateway");
    });
});

await app.UseOcelot();

app.Run();