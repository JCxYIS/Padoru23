
using Padoru23Server.Services;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<PadoruService>();

var app = builder.Build();

app.UseWebSockets();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();


