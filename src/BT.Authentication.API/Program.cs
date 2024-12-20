using Microsoft.EntityFrameworkCore;
using BT.Authentication.API.Data;
using BT.Authentication.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
//  Add swaggerGen support
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("V1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BlazorServerJWTDemo Api", Version = "V1" });
});

//  Register JWT Service
/*builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"]!))
    };
});*/

//  DI
//  See ServiceContainer
/*builder.Services.AddScoped<IJWTUtilities, JWTUtilities>();
builder.Services.AddScoped<IAccount, AccountRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();*/


// Add services to the container.
builder.Services.AddInfrastructureService(builder.Configuration);
var app = builder.Build();





app.UseInfrastructurePolicy();
app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseAntiforgery();

//  Map Swagger
app.UseSwagger();
app.UseSwaggerUI(s => {
    s.SwaggerEndpoint("/swagger/V1/swagger.json", "Your swagger api name");
});

//  Map controllers / user authentication / authorization
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
