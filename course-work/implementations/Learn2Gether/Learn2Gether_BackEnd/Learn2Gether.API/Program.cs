using Learn2Gether.Application.Interfaces;
using Learn2Gether.Application.Services;
using Learn2Gether.Domain.Entities;
using Learn2Gether.Infastructure.Data;
using Learn2Gether.Infastructure.Data.Seeders;
using Learn2Gether.Infastructure.Identity;
using Learn2Gether.Infastructure.Identity.Interfaces;
using Learn2Gether.Infastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using System.Xml;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<Learn2GetherDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddIdentity<User, IdentityRole<Guid>>(cfg =>
        {
            ConfigureIdentity(builder, cfg);
        })
        .AddEntityFrameworkStores<Learn2GetherDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("ReactApp",
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "https://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
        });
       
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("JWT Key not found in configuration.")))
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["jwt"];

                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                }
            };
                
        });

        builder.Services.AddAuthorization();

        builder.Services.AddScoped(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        builder.Services.AddScoped<ICourseService, CourseService>();
        builder.Services.AddScoped<IWishlistService, WishlistService>();
        builder.Services.AddScoped<INoteService, NoteService>();
        builder.Services.AddScoped<IQuestionService, QuestionService>();
        builder.Services.AddScoped<IAnswerService, AnswerService>();
        builder.Services.AddScoped<IJWTService, JWTService>();
        builder.Services.AddScoped<IModuleService, ModuleService>();
        builder.Services.AddScoped<ILectureService, LectureService>();
        builder.Services.AddScoped<IInstructorService, InstructorService>();
        builder.Services.AddScoped<IAdminService, AdminService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleSeeder = new RoleSeeder();
                var userSeeder = new UserSeeder();

                roleSeeder.SeedRolesAsync(roleManager).GetAwaiter().GetResult();
                userSeeder.SeedUsersAsync(userManager, roleManager).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding roles: {ex.Message}");
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // Enable the CORS policy so browsers can call the API from the frontend dev server
        app.UseCors("ReactApp");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseSwagger();

        app.Run();
    }

    private static void ConfigureIdentity(WebApplicationBuilder builder, IdentityOptions cfg)
    { 
        cfg.Password.RequireDigit = true;
        cfg.Password.RequireLowercase = true;
        cfg.Password.RequireUppercase = true;
        cfg.Password.RequireNonAlphanumeric = false;
        cfg.Password.RequiredLength = 6;

        cfg.SignIn.RequireConfirmedEmail = false;
        cfg.SignIn.RequireConfirmedPhoneNumber = false;
        cfg.SignIn.RequireConfirmedAccount = false;

        cfg.User.RequireUniqueEmail = true;
    }
}