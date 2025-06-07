using AuthService;

var builder = WebApplication.CreateBuilder(args);

Bootstrap.Configure(builder);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
