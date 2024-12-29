using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// Ideally, you will want this name to come from a config file, constants file, etc.
var serviceName = "TodoApiWithOpenTelemetry";

var builder = WebApplication.CreateBuilder(args);

// Configure OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
	  .ConfigureResource(resource => resource.AddService(serviceName))
	  .WithTracing(tracing => tracing
		  .AddAspNetCoreInstrumentation() // Trace incoming HTTP requests
		  .AddHttpClientInstrumentation() // Trace outgoing HTTP requests
		  .AddConsoleExporter())          // Export traces to the console
	  .WithMetrics(metrics => metrics
		  .AddAspNetCoreInstrumentation()
		  .AddHttpClientInstrumentation()
		  .AddConsoleExporter());

builder.Logging.AddOpenTelemetry(options =>
{
	options
		.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
		.AddConsoleExporter();
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();
