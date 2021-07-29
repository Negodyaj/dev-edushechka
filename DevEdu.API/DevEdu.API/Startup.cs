using DevEdu.API.Configuration;
using DevEdu.Business.Configuration;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag.Generation.Processors.Security;

namespace DevEdu.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IStudentAnswerOnTaskRepository, StudentAnswerOnTaskRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IStudentAnswerOnTaskService, StudentAnswerOnTaskService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<ICommentValidationHelper, CommentValidationHelper>();
            services.AddScoped<ICourseValidationHelper, CourseValidationHelper>();
            services.AddScoped<IGroupValidationHelper, GroupValidationHelper>();
            services.AddScoped<ILessonValidationHelper, LessonValidationHelper>();
            services.AddScoped<IMaterialValidationHelper, MaterialValidationHelper>();
            services.AddScoped<INotificationValidationHelper, NotificationValidationHelper>();
            services.AddScoped<IPaymentValidationHelper, PaymentValidationHelper>();
            services.AddScoped<IRatingValidationHelper, RatingValidationHelper>();
            services.AddScoped<ITagValidationHelper, TagValidationHelper>();
            services.AddScoped<ITaskValidationHelper, TaskValidationHelper>();
            services.AddScoped<ITopicValidationHelper, TopicValidationHelper>();
            services.AddScoped<IUserValidationHelper, UserValidationHelper>();

            services.AddControllers();

            services.AddMvc();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

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
                app.UseOpenApi();
                app.UseSwaggerUi3();
            }

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