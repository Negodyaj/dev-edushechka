using DevEdu.Business.Configuration;
using DevEdu.Business.Helpers;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.Core;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace DevEdu.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<DatabaseSettings>()
                .Bind(configuration.GetSection(nameof(DatabaseSettings)))
                .ValidateDataAnnotations();
            services.AddOptions<AuthSettings>()
                .Bind(configuration.GetSection(nameof(AuthSettings)))
                .ValidateDataAnnotations();
            services.AddOptions<FilesSettings>()
                .Bind(configuration.GetSection(nameof(FilesSettings)))
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
            services.AddTransient<IMaterialRepository, MaterialRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IStudentHomeworkRepository, StudentHomeworkRepository>();
            services.AddTransient<ICourseRepository, CourseRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<ILessonRepository, LessonRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<ITopicRepository, TopicRepository>();
            services.AddTransient<IRatingRepository, RatingRepository>();
            services.AddTransient<IHomeworkRepository, HomeworkRepository>();

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
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IStudentHomeworkService, StudentHomeworkService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IHomeworkService, HomeworkService>();
            services.AddScoped<IWorkWithFiles, WorkWithFiles>();

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