using JobService;

var builder = WebApplication.CreateBuilder(args);

Bootstrap.Configure(builder);

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
