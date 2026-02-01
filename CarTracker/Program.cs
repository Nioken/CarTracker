using CarTracker.Bot;
using CarTracker.Bot.Callbacks;
using CarTracker.Bot.Commands;
using CarTracker.Database;
using CarTracker.Services;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IExpensesService, ExpensesService>();
builder.Services.AddSingleton<IInputService, InputService>();

builder.Services.AddScoped<IBotCommand, StartCommand>();
builder.Services.AddScoped<IBotCommand, ProfileCommand>();

builder.Services.AddScoped<ICallbackHandler, MyCarsCallback>();
builder.Services.AddScoped<ICallbackHandler, AddCarCallback>();
builder.Services.AddScoped<ICallbackHandler, DeleteCarCallback>();
builder.Services.AddScoped<ICallbackHandler, ShowCarInfoCallback>();
builder.Services.AddScoped<ICallbackHandler, ShowExpCallback>();
builder.Services.AddScoped<ICallbackHandler, NewExpenseCallback>();

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

app.UseHttpsRedirection();

app.Run();