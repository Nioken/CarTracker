using CarTracker.Bot;
using CarTracker.Bot.Callbacks;
using CarTracker.Bot.Commands;
using CarTracker.Database;
using CarTracker.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IExpensesService, ExpensesService>();
builder.Services.AddSingleton<IInputService, InputService>();
builder.Services.AddSingleton<IAuthService, AuthCodeService>();

builder.Services.AddScoped<IBotCommand, StartCommand>();
builder.Services.AddScoped<IBotCommand, ProfileCommand>();
builder.Services.AddScoped<IBotCommand, LoginCommand>();

builder.Services.AddScoped<ICallbackHandler, MyCarsCallback>();
builder.Services.AddScoped<ICallbackHandler, AddCarCallback>();
builder.Services.AddScoped<ICallbackHandler, DeleteCarCallback>();
builder.Services.AddScoped<ICallbackHandler, ShowCarInfoCallback>();
builder.Services.AddScoped<ICallbackHandler, ShowExpCallback>();
builder.Services.AddScoped<ICallbackHandler, NewExpenseCallback>();
builder.Services.AddScoped<ICallbackHandler, BackToMainMenuCallback>();
builder.Services.AddScoped<ICallbackHandler, BackToCarList>();
builder.Services.AddScoped<ICallbackHandler, BackToCarCallback>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "CarTracker.Session";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(); 
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5174")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var botToken = builder.Configuration["BotConfiguration:BotToken"];

if (string.IsNullOrEmpty(botToken))
{
    throw new InvalidOperationException("Bot token is missing in appsettings.json");
}

builder.Services.AddSingleton<ITelegramBotClient>(sp => new TelegramBotClient(botToken));
builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddHostedService<BotBackgroundService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

db.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReact");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();