using System.Text;
using api.Data;
using api.JWT;
using api.Models;
using api.Repository;
using api.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddTransient<ICommonRespository<Employee>, CommonRespository<Employee>>();

builder.Services.AddDbContext<DataContext>(options =>
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        string connStr;

        // Depending on if in development or production, use either Heroku-provided
        // connection string, or development connection string from env var.
        if (env == "Development")
        {
            // Use connection string from file.
            connStr = builder.Configuration.GetConnectionString("SQLConnection")!;
        }
        else
        {
            // Use connection string provided at runtime by Heroku.
            var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL")!;

            // Parse connection URL to connection string for Npgsql
            connUrl = connUrl.Replace("postgres://", string.Empty);
            var pgUserPass = connUrl.Split("@")[0];
            var pgHostPortDb = connUrl.Split("@")[1];
            var pgHostPort = pgHostPortDb.Split("/")[0];
            var pgDb = pgHostPortDb.Split("/")[1];
            var pgUser = pgUserPass.Split(":")[0];
            var pgPass = pgUserPass.Split(":")[1];
            var pgHost = pgHostPort.Split(":")[0];
            var pgPort = pgHostPort.Split(":")[1];

            connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True; Connection Timeout=90";
        }

        // Whether the connection string came from the local development configuration file
        // or from the environment variable from Heroku, use it to set up your DbContext.
        options.UseNpgsql(connStr);
    }
);

builder.Services.AddTransient<ICommonRepository<Employee>, CommonRepository<Employee>>();
builder.Services.AddTransient<ICommonRepository<Event>, CommonRepository<Event>>();
builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddScoped<ITokenManager, TokenManager>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Authentication Middleware
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["HubJWT:Secret"] ?? "")),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(options =>
{
	options.AddPolicy(
		name: "PublicPolicy",
		policy =>
		{
			policy.AllowAnyHeader();
			policy.AllowAnyMethod();
			policy.AllowAnyOrigin();
		}
	);
});
 
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Authentication Token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JsonWebToken",
		Scheme = "Bearer"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
			new string[]{}
		}
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
