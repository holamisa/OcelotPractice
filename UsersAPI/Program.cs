using UsersAPI.Helper;
using Middleware;
using Middleware.Logging;
using Serilog;
using UsersAPI.Infrastructures;
using UsersAPI.Repositories;
using Dapper;
using UsersAPI.Services;
using Middleware.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.RegisterServices(builder.Configuration);

// Set Dapper's MatchNamesWithUnderscores to true globally
DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddSingleton<DapperContext>(sp =>
    new DapperContext(builder.Configuration));

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<UserService>();

builder.Services.AddCors();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog 구성
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// 기본 로거를 Serilog로 설정
builder.Host.UseSerilog();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

app.UseMiddleware<RequestResponseLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
