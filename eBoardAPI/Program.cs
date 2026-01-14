using dotenv.net;
using eBoardAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

builder.Services.AddControllers();

builder.Services
    .AddOpenApi()
    .AddSwagger()
    .AddDatabase()
    .AddRepositories()
    .AddServices()
    .AddAutoMapper();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "eBoard API V1");
});

app.MapControllers();

app.Run();