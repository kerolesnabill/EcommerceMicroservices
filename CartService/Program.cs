using CartService.Extensions;
using BuildingBlocks.Middlewares;
using BuildingBlocks.Extensions.ServiceCollection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServiceExtensions(builder.Configuration);
builder.Services.AddAuthenticationService(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapCarter();
app.UseAuthentication();
app.UseAuthorization();
app.Run();
