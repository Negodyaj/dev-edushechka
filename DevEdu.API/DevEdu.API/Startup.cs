using DevEdu.Business.Services;
using DevEdu.Business.Servicies;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IMaterialService, MaterialService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICourseService, CourseService>(); 
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IMaterialService, MaterialService>();

            services.AddControllers();

            services.AddSwaggerDocument(settings => {
                settings.Title = "DevEdu Education API";
                settings.Version = "v8";
            });
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

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}