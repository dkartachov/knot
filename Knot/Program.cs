using Knot.Hubs;
using Knot.Options;
using Knot.Services.ContainerRuntime;
using Knot.Services.Containers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBOptions>(builder.Configuration.GetSection(MongoDBOptions.MongoDB));
// MongoDB recommends singleton https://mongodb.github.io/mongo-csharp-driver/2.14/reference/driver/connecting/#re-use
builder.Services.AddSingleton<IContainersService, ContainersServiceMongoDB>();

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddScoped<IContainerRuntime, DockerRuntime>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.MapHub<ContainerLogsHub>("/logs");

app.Run();
