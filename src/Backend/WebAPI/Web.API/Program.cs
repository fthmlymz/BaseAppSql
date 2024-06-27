using Application.Common.Exceptions;
using Application.Common.Filters;
using Application.Extensions;
using Application.Keycloak;
using DotNetCore.CAP.Messages;
using IdentityModel.Client;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence.Context;
using Persistence.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Web.API.middleware;



var builder = WebApplication.CreateBuilder(args);


#region Controller Validations
builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new ValidateFilterAttribute());
    opt.Filters.Add(typeof(ValidateJsonModelFilter));
})
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull;
});
#endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Dependency Injection - Application - Infrastructure - Persistence
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddPersistenceLayer(builder.Configuration);
#endregion



#region Eventbus DotnetCore.CAP & RabbitMQ
builder.Services.AddCap(x =>
{
    x.UseEntityFramework<DotnetCapDbContext>();
    x.UseSqlServer(builder.Configuration.GetConnectionString("CapLogSqlServerConnection"));
    x.UseRabbitMQ(options =>
    {
        options.ExchangeName = "MosasWeb.API";
        options.BasicQosOptions = new DotNetCore.CAP.RabbitMQOptions.BasicQos(3);
        options.ConnectionFactoryOptions = opt =>
        {
            opt.HostName = builder.Configuration.GetSection("RabbitMQ:Host").Value;
            opt.UserName = builder.Configuration.GetSection("RabbitMQ:Username").Value;
            opt.Password = builder.Configuration.GetSection("RabbitMQ:Password").Value;
            opt.Port = Convert.ToInt32(builder.Configuration.GetSection("RabbitMQ:Port").Value);
            opt.CreateConnection();
        };
    });
    x.UseDashboard(opt => opt.PathMatch = "/cap-dashboard");
    x.FailedRetryCount = 5;
    x.UseDispatchingPerGroup = true;
    x.FailedThresholdCallback = failed =>
    {
        var logger = failed.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError($@"A message of type {failed.MessageType} failed after executing {x.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
    };
    x.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});
#endregion



#region CORS Settings
var allowedResources = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedResources,
        policy =>
        {
            policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});
#endregion



#region Keycloak JWT
var keycloakSettings = builder.Configuration.GetSection("Keycloak").Get<KeycloakSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.Authority = keycloakSettings?.Authority;
               options.Audience = keycloakSettings?.Audience;
               options.RequireHttpsMetadata = false;
               options.UseSecurityTokenValidators = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateIssuerSigningKey = true,
                   RequireSignedTokens = false,
                   ValidIssuer = keycloakSettings?.Authority, //test eklendi
                   ValidAudience = keycloakSettings?.Audience, //test eklendi
                   SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                   {
                       var jwt = new JwtSecurityToken(token);

                       return jwt;
                   },
                   ValidateLifetime = true,
                   RequireExpirationTime = true,
                   ClockSkew = TimeSpan.Zero,
               };
               options.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       return Task.CompletedTask;
                   },
               };
           });
builder.Services.AddTransient<IClaimsTransformation>(_ => new KeycloakRolesClaimsTransformation("role", keycloakSettings.Audience));
builder.Services.AddAuthorization(options =>
{
    #region KKC
    options.AddPolicy("KKCReadRole", builder => { builder.AddRequirements(new RptRequirement("res:kkc", "scopes:read")); });
    options.AddPolicy("KKCCreateRole", builder => { builder.AddRequirements(new RptRequirement("res:kkc", "scopes:create")); });
    options.AddPolicy("KKCUpdateRole", builder => { builder.AddRequirements(new RptRequirement("res:kkc", "scopes:update")); });
    options.AddPolicy("KKCDeleteRole", builder => { builder.AddRequirements(new RptRequirement("res:kkc", "scopes:delete")); });
    #endregion
});

builder.Services.AddHttpClient<KeycloakService>(client =>
{
    client.BaseAddress = new Uri(keycloakSettings.KeycloakResourceUrl);
});
builder.Services.AddHttpClient<IdentityModel.Client.TokenClient>();
builder.Services.AddSingleton(new ClientCredentialsTokenRequest
{
    Address = keycloakSettings?.ClientCredentialsTokenAddress
});
builder.Services.AddScoped<IAuthorizationHandler, RptRequirementHandler>();
#endregion



// Daha sonra tekrar aktif edilecek
#region Kestrel docker-compose.yml
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    var httpPort = builder.Configuration["ASPNETCORE_HTTP_PORTS"];
//    var httpsPort = builder.Configuration["ASPNETCORE_HTTPS_PORTS"];

//    if (builder.Environment.IsDevelopment() || builder.Environment.IsProduction())
//    {
//        if (!string.IsNullOrEmpty(httpPort))
//        {
//            serverOptions.ListenAnyIP(Convert.ToInt32(httpPort));
//        }

//        if (!string.IsNullOrEmpty(httpsPort))
//        {
//            serverOptions.ListenAnyIP(Convert.ToInt32(httpsPort), listenOptions =>
//            {
//                listenOptions.UseHttps(builder.Configuration.GetSection("ASPNETCORE_Kestrel:Certificates:Default:Path").Value,
//                    builder.Configuration.GetSection("ASPNETCORE_Kestrel:Certificates:Default:Password").Value);
//            });
//        }
//    }
//});
#endregion


#region RateLimit - Ex
//builder.Services.AddRateLimiter(options =>
//{
//    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
//    options.AddPolicy("apikey", httpContext =>
//    {
//        if (httpContext.Request.Query.Keys.Contains("api_key"))
//        {
//            return RateLimitPartition.GetFixedWindowLimiter(
//                httpContext.Request.Query["api_key"].ToString(),
//                fac =>
//                {
//                    return new FixedWindowRateLimiterOptions
//                    {
//                        Window = TimeSpan.FromHours(1),
//                        PermitLimit = 10,
//                    };
//                });
//        }
//        else
//        {
//            return RateLimitPartition.GetNoLimiter("");
//        }
//    });
//});
#endregion

builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

#region Auto migrate
using (var scope = app.Services.CreateScope())
{
    var dotnetCapContext = scope.ServiceProvider.GetRequiredService<DotnetCapDbContext>();
    var apiContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();


    dotnetCapContext.Database.Migrate();
    apiContext.Database.Migrate();
}
#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(allowedResources);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseMiddleware<UseCustomExceptionHandler>();

app.UseMiddleware<RateLimitMiddleware>();

app.Run();