namespace TaskManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAzureStorageFileShareService, AzureStorageFileShareService>();

        services.AddScoped<IJWTTokenService, JWTTokenService>();

        services.AddScoped<IEmailSender, EmailSender>();

        services.AddScoped<IPhotoService, PhotoService>();

        services.AddScoped<ITextToImageService, TextToImageService>();

        services.AddScoped<IUploadFileService, UploadFileService>();

        return services;
    }
}
