var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://localhost:8008");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

var app = builder.Build();

// Seed the database with teams and agents if needed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    ChatQueueSystem.API.DatabaseSeeder.Seed(db);
}


app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatQueueSystem API V1");
    c.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();