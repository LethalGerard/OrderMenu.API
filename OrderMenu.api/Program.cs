using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using OrderMenu.api;

var builder = WebApplication.CreateBuilder(args);
var kernelBuilder = Kernel.CreateBuilder();

kernelBuilder.AddOpenAIChatCompletion(builder.Configuration["Model"] ?? throw new ArgumentNullException("no Mode found"), builder.Configuration["Key"] ?? throw new ArgumentNullException("no Key found"));


// Add services to the container.
builder.Services.AddSingleton<Kernel>(service => kernelBuilder.Build());
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddControllers();
builder.Services.AddSingleton<ChatService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => {
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
