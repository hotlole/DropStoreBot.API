using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using DropStoreBot.Infrastructure.Data;
using DropStoreBot.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --- ������������ ������������ ����� Serilog ---
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// --- ��������� ������� ---
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// --- HealthChecks (���� �����, ����� ������������) ---
// builder.Services.ConfigureHealthChecks(builder.Configuration);

// --- ��������������� API ---
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-API-Version"),
        new QueryStringApiVersionReader("api-version")
    );
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// --- Swagger ---
builder.Services.ConfigureSwagger(); 

// --- ����������� � �� ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information));
builder.Services.AddInfrastructure();
// --- �������������� ������� �������� ---
builder.Services.AddAutoMapper(cfg =>
{
    cfg.ConstructServicesUsing(type => builder.Services.BuildServiceProvider().GetRequiredService(type));
}, AppDomain.CurrentDomain.GetAssemblies());

// --- FluentValidation ---
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

// --- ������������ ������ ��� ������� ��������� ---
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Any())
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        return new BadRequestObjectResult(new
        {
            message = "������ ���������",
            errors
        });
    };
});

// --- ����������� ---
builder.Services.AddAuthorization();

var app = builder.Build();

// --- Middleware ---
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"API {description.GroupName.ToUpperInvariant()}");
        }

        options.HeadContent = """<link rel="stylesheet" href="/css/swagger-custom.css">""";
    });
}

app.MapControllers();
app.Run();
