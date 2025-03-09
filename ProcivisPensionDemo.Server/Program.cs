using ProcivisPensionDemo.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Lisää SignalR palveluihin
builder.Services.AddSignalR();
builder.Services.AddScoped<QRCodeService>();
var corsPolicy = "_allowVueFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy,
        policy => policy.WithOrigins("https://localhost:5173")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials());
});

var app = builder.Build();

app.UseCors(corsPolicy);

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");
app.MapHub<QrCodeHub>("/qrcodehub").RequireCors(corsPolicy); ;
app.Run();
