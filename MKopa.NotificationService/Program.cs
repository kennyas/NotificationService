using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using MKopaMessageBox.Domain.DTOs;
using MKopaMessageBox.Extensions;
using MKopaMessageBox.Extenstions.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region CUSTOM SERVICES AND DI

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection(nameof(EmailConfig)));

#region Serilog configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console(formatter: new Serilog.Formatting.Json.JsonFormatter(), restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    .CreateLogger();
#endregion

builder.Services.ConfigureServicesAndDI(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MKopa Notification APIs",
        Version = "1.0.0",
        Contact = new OpenApiContact { Email = "applicationdev@accessbank.com", Name = "MKopa MessageBox" }
    });
    c.AddSecurityDefinition("XApiKey", new OpenApiSecurityScheme
    {
        Name = "XApiKey",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "XApiKey",
        Description = "The access key required to access resources on this service. Example: {XApiKey, SGE35HWE5EW5363256HERH }"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id= "XApiKey",
                    Type= ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});


#endregion

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MKopa Messagebox  v1");
        c.DefaultModelsExpandDepth(-1);
    });
}

app.Use((context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { (string)context.Request.Headers["Origin"] });
        context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type,contentType, Accept, Authorization, XApiKey,session,Session" });
        context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
        context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
    }
    return next.Invoke();
});
app.UseCors(builder => builder.WithOrigins("*"));

bool acitvateMiddleware = builder.Configuration.GetValue<bool>("AppSettings:ActivateMiddleware");
if (acitvateMiddleware)
{
    app.UseMiddleware<KeyAccessMiddleware>();
}

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
