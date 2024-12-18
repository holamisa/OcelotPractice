using MainGateway.Helper;
using Middleware;
using Middleware.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.RegisterServices(builder.Configuration);
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddCors();

builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

// Serilog 구성
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// 기본 로거를 Serilog로 설정
builder.Host.UseSerilog();

var app = builder.Build();

app.UseExceptionHandler((_ => { }));

if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<RequestResponseLoggingMiddleware>();

await app.UseOcelot();

app.Run();
