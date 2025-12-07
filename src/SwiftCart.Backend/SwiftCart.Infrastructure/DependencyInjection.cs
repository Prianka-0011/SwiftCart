using System.Text;
using EmotiaMart.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;  
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens; // Required for Token Validation
using SwiftCart.Application.Interfaces;
using SwiftCart.Application.Interfaces.Repositories;
using SwiftCart.Domain.Entities;
using SwiftCart.Infrastructure.Repositories;
using SwiftCart.Infrastructure.Services;


namespace SwiftCart.Infrastructure
{
    public static class DependencyInjection
    {
        private static string dbHost => 
            Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                ? "swift-cart-sql"
                : "localhost";

        private static int dbPort => int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out var port) ? port : 1433;
        private static string dbName => Environment.GetEnvironmentVariable("DB_NAME") ?? "SwiftCartDb";
        private static string dbUser => Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
        private static string dbPass => Environment.GetEnvironmentVariable("DB_PASS") ?? "SwiftCart123!";

        public static string GetConnectionString(string databaseName)
        {
            return $"Server={dbHost},{dbPort};Database={databaseName};User Id={dbUser};Password={dbPass};TrustServerCertificate=True;Encrypt=False;";
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine($"DB Connection String: {GetConnectionString(dbName)}");

            // Add EF Core with retry on transient failures
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    GetConnectionString(dbName),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    )
                );
            });


            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                 options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                };

                 options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Prefer cookie token
                        var token = context.Request?.Cookies["jwt"] ?? context.Request?.Cookies["access_token"];

                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }

                         return Task.CompletedTask;
                    }
                };
            });

           

            

            // Quartz setup
            // services.AddQuartz(q =>
            // {
            //     q.SchedulerName = "EmotiaMart-Scheduler";
            //     q.SchedulerId = "AUTO";

            //     q.UseSimpleTypeLoader();

            //     q.UsePersistentStore(s =>
            //     {
            //         s.UseClustering();
            //         s.UseSqlServer(c =>
            //         {
            //             c.ConnectionString = GetConnectionString(dbName);
            //             c.TablePrefix = "EMOTIA_QRTZ_";
            //         });
            //         s.UseJsonSerializer();
            //     });

            //     q.UseDefaultThreadPool(tp =>
            //     {
            //         tp.MaxConcurrency = 10;
            //     });
            // });

            // services.AddQuartzHostedService(options =>
            // {
            //     options.WaitForJobsToComplete = true;
            // });

            // Register other services, repositories, etc.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IFileService, LocalFileService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IUserContextService,  UserContextService>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            

            return services;
        }
    }
}
