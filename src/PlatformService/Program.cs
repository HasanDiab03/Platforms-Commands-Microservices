using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Options;
using PlatformService.Data;
using PlatformService.Extensions;
using PlatformService.SyncDataServices.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAppServices(builder.Environment, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGrpcService<GrpcPlatformService>();

// this is optional, it is used so the client can recieve the proto file, and know what to send and what is expected to receive
app.MapGet("/protos/platforms.proto", async context =>
{
	await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});

app.MapControllers();

app.PrepareDatabase(app.Environment);

Console.WriteLine($"--> Command Service IP: {builder.Configuration["CommandServiceIP"]}");

app.Run();
