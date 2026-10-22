using UserManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<UserStore>();
builder.Services.AddCors(options => options.AddPolicy("ReactDev", policy =>
    policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
          .AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseCors("ReactDev");
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));
app.Run();
