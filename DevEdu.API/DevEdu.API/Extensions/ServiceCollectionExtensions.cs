using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;
using DevEdu.Business.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DevEdu.Core;
using Microsoft.Extensions.Configuration;


namespace DevEdu.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddOptions<DatabaseSettings>()
                .Bind(configuration.GetSection(nameof(DatabaseSettings)))
                .ValidateDataAnnotations();
            services.AddOptions<AuthSettings>()
                .Bind(configuration.GetSection(nameof(AuthSettings)))
                .ValidateDataAnnotations();
        }
        public static void AddBearerAuthentication(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var authOptions = provider.GetRequiredService<IAuthOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = authOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = authOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IStudentHomeworkRepository, StudentHomeworkRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IHomeworkRepository, HomeworkRepository>();

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IPaymentService, PaymentService>();
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
            services.AddScoped<IStudentHomeworkService, StudentHomeworkService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IHomeworkService, HomeworkService>();

            return services;
        }

        public static IServiceCollection AddValidationHelpers(this IServiceCollection services)
        {
            services.AddScoped<ICommentValidationHelper, CommentValidationHelper>();
            services.AddScoped<ICourseValidationHelper, CourseValidationHelper>();
            services.AddScoped<IGroupValidationHelper, GroupValidationHelper>();
            services.AddScoped<ILessonValidationHelper, LessonValidationHelper>();
            services.AddScoped<IMaterialValidationHelper, MaterialValidationHelper>();
            services.AddScoped<INotificationValidationHelper, NotificationValidationHelper>();
            services.AddScoped<IPaymentValidationHelper, PaymentValidationHelper>();
            services.AddScoped<IRatingValidationHelper, RatingValidationHelper>();
            services.AddScoped<IStudentHomeworkValidationHelper, StudentHomeworkValidationHelper>();
            services.AddScoped<ITagValidationHelper, TagValidationHelper>();
            services.AddScoped<ITaskValidationHelper, TaskValidationHelper>();
            services.AddScoped<ITopicValidationHelper, TopicValidationHelper>();
            services.AddScoped<IUserValidationHelper, UserValidationHelper>();
            services.AddScoped<IHomeworkValidationHelper, HomeworkValidationHelper>();

            return services;
        }
    }
}