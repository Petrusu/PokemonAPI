using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using PokemonAPi;
using PokemonAPi.Context;
using PokemonAPi.Properties;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<WhrgmhraContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "PokemonAPI",
        Description = "ASP.NET Core 7.0 Web API"
    });
    swagger.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        In=ParameterLocation.Header,
        Description = "Please insert token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserIdPolicy", policy =>
    {
        policy.Requirements.Add(new UserIdRequirement(1)); 
    });
});

// Регистрация обработчика требования
builder.Services.AddSingleton<IAuthorizationHandler, UserIdRequirementHandler>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add JWT Authentication Middleware - This code will intercept HTTP request and validate the JWT.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    opt => {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(policy =>
    policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
);

app.UseHttpsRedirection();
           
//https://referbruv.com/blog/building-custom-responses-for-unauthorized-requests-in-aspnet-core/
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == (int)System.Net.HttpStatusCode.Unauthorized)
    {
        await context.Response.WriteAsync("Token Validation Has Failed. Request Access Denied");
    }
});

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();




