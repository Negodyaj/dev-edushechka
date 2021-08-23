using DevEdu.API.Configuration;
using DevEdu.API.Extensions;
using DevEdu.Business.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag.Generation.Processors.Security;
using System.Text.Json.Serialization;
using DevEdu.Core;

namespace DevEdu.API
{
    public class Startup
    {
        private const string _pathToEnvironment = "ASPNETCORE_ENVIRONMENT";
        
        public Startup(IConfiguration configuration)
        {
            var currentEnvironment = configuration.GetValue<string>(_pathToEnvironment);
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.{currentEnvironment}.json");

            Configuration = builder.Build();
            Configuration.SetEnvironmentVariableForConfiguration();            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container. 
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAppConfiguration(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddRepositories();
            services.AddCustomServices();
            services.AddValidationHelpers();
            services.AddScoped<IAuthOptions, AuthOptions>();

            services.AddControllers();

            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })

                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var exc = new ValidationExceptionResponse(context.ModelState);
                        return new UnprocessableEntityObjectResult(exc);
                    };
                });

            services.AddBearerAuthentication();
            services.AddSwaggerDocument(document =>
            {
                document.DocumentName = "Endpoints for DevEdu";
                document.Title = "DevEdu Education API";
                document.Version = "v8";
                document.Description = "An interface for DevEdushechka.";

                document.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("JWT token", new NSwag.OpenApiSecurityScheme
                    {
                        Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        Description = "Copy 'Bearer ' + valid JWT token into field",
                        In = NSwag.OpenApiSecurityApiKeyLocation.Header
                    }));
                document.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT token"));
            });

            // Add framework services. 
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseMiddleware<ExceptionMiddleware>();

            //This middleware is used to redirects HTTP requests to HTTPS.   
            app.UseHttpsRedirection();

            //This middleware is used to returns static files and short-circuits further request processing.    
            app.UseStaticFiles();

            //This middleware is used to route requests.    
            app.UseRouting();

            //This middleware is used to authorizes a user to access secure resources.   
            app.UseAuthentication();
            app.UseAuthorization();

            //This middleware is used to add Razor Pages endpoints to the request pipeline.    
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}