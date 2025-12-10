using TaskifyMini.Repositories;
using TaskifyMini.Repositories.Interfaces;
using TaskifyMini.Services;

namespace TaskifyMini.Helpers.Extensions
{
    public static class ServiceExtension
    {

        public static IServiceCollection AddTaskServices(this IServiceCollection services)
        {
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITaskService, TaskService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJwtService, JwtService>();

            return services;
        }
    }
}
