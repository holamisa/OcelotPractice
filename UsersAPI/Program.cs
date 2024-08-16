using UsersAPI.Helper;
using Middleware;
using Middleware.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.RegisterServices(builder.Configuration);

builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog ����
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// �⺻ �ΰŸ� Serilog�� ����
builder.Host.UseSerilog();

var app = builder.Build();

app.UseExceptionHandler((_ => { }));

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

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
