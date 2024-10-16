using HolaHousing_BE.Interfaces;
using HolaHousing_BE.Models;
using HolaHousing_BE.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<EXE201Context>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPropertyInterface, PropertyRepositories>();
builder.Services.AddScoped<IPropertyImageInterface, PropertyImageRepositories>();
builder.Services.AddScoped<IPostPriceInterface, PostPriceRepositories>();
builder.Services.AddScoped<IUserInterface, UserRepositories>();
builder.Services.AddScoped<INewInterface, NewRepositories>();
builder.Services.AddScoped<ITagInterface, TagRepositories>();
builder.Services.AddScoped<IPartContentInterface, PartContentRepositories>();
builder.Services.AddScoped<IPostTypeInterface, PostTypeRepositories>();
builder.Services.AddScoped<IAmentityInterface, AmentityRepositories>();
builder.Services.AddScoped<IDeclineReasonInterface, DeclineReasonRepositories>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("CorsPolicy");
app.Run();
