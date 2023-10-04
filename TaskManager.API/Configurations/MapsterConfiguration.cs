using Mapster;
using MapsterMapper;
using System.Reflection;
using static TaskManager.Core.Extensions.CoreExtensions;

namespace TaskManager.API.Configurations
{
    public static class MapsterConfiguration
    {
        /// <summary>
        /// Adds Mapster configurations to application.
        /// </summary>
        /// <param name="services">The service collection</param>
        public static void AddMapster(this IServiceCollection services)
        {
            // Get the global type adapter configuration.
            TypeAdapterConfig typeAdapterConfig = TypeAdapterConfig.GlobalSettings;

            // Scan the Application to find models for mapping based on BaseDto.
            Assembly appAssembly = typeof(BaseDto<,>).Assembly;
            typeAdapterConfig.Scan(appAssembly);

            // scans the assembly and gets the IRegister, adding the registration to the TypeAdapterConfig
            typeAdapterConfig.Scan(Assembly.GetExecutingAssembly());
            // register the mapper as Singleton service for my application
            var mapperConfig = new Mapper(typeAdapterConfig);

            services.AddSingleton<IMapper>(mapperConfig);
        }
    }
}
