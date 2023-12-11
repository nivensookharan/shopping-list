using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.OpenApi.Models;
using Minio;
using ShoppingList.Contracts;
using ShoppingList.Core.Mapping;
using ShoppingList.Dal;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Database Connection
builder.Services.AddDbContext<Entities>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Database Repositories
builder.Services.AddSingleton<RepositoryFactories, RepositoryFactories>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepositoryProvider, RepositoryProvider>();

// AutoMappers
builder.Services.AddAutoMapper(typeof(UserMapping));
builder.Services.AddAutoMapper(typeof(ShoppingListMapping));
builder.Services.AddAutoMapper(typeof(ShoppingListItemMapping));


// Keycloak
var keycloakConfig = builder.Configuration.GetSection("Keycloak");
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddOpenIdConnect(options =>
    {
        options.Authority = keycloakConfig["Instance"];
        options.ClientId = keycloakConfig["ClientId"];
        options.ClientSecret = keycloakConfig["ClientSecret"];
        options.CallbackPath = keycloakConfig["CallbackPath"];
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.SaveTokens = true;
    });

// Minio
var minioConfig = builder.Configuration.GetSection("Minio");
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioConfig["Endpoint"])
    .WithCredentials(minioConfig["AccessKey"], minioConfig["SecretKey"])
    .WithSSL(true)
    .Build()
);

builder.Services.AddCors();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            NamingStrategy = new Newtonsoft.Json.Serialization.DefaultNamingStrategy()
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Games Global Swagger", Version = "v1" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {

                AuthorizationUrl = new Uri(keycloakConfig["AuthorizationUrl"]),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID Connect" },
                    { "profile", "User profile" },
                    { "email", "User Email" },
                }
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "openid", "profile", "email" }
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(_swaggerUiOptions =>
    {
        _swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Games Global .Net Core Web API");
        _swaggerUiOptions.OAuthClientId(keycloakConfig["ClientId"]);
        //_swaggerUiOptions.EnablePersistAuthorization();
        _swaggerUiOptions.OAuthAppName("Games Global");
        _swaggerUiOptions.EnableTryItOutByDefault();
        _swaggerUiOptions.OAuthUsePkce();
        
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
