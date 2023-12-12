using Mapster;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Extensions;

public static partial class CoreExtensions
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [GeneratedRegex("^\\w|_\\w")]
    private static partial Regex MyRegex();

    /// <summary>
    /// Convert json to object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T FromJson<T>(this string json)
    {
        T val = JsonSerializer.Deserialize<T>(json, _jsonOptions)!;
        if (val == null)
        {
            DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new(25, 1);
            defaultInterpolatedStringHandler.AppendLiteral("Can't Deserialize object ");
            defaultInterpolatedStringHandler.AppendFormatted(typeof(T));
            throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
        }

        return val;
    }

    /// <summary>
    /// Convert object to json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToJson<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, _jsonOptions);
    }

    public static string ToPascalCase(this string name)
    {
        return MyRegex().Replace(name, (match) => match.Value.Replace("_", "").ToUpper());
    }

    /// <summary>
    /// Base Data Transfer Object for model mapping.
    /// </summary>
    /// <typeparam name="TEntity">The type of the Entity.</typeparam>
    /// <typeparam name="TViewModel">The type of the ViewModel.</typeparam>
    public abstract class BaseDto<TViewModel, TEntity> : IRegister
        where TViewModel : class, new()
        where TEntity : class, new()
    {
        /// <summary>
        /// Configuration of type adapter.
        /// </summary>
        private TypeAdapterConfig? Config { get; set; }

        /// <summary>
        /// Adds custom mappings to the configuration.
        /// </summary>
        public virtual void AddCustomMappings() { }

        /// <summary>
        /// Sets custom mappings for entity to viewmodel.
        /// </summary>
        /// <returns>The type adapter setter.</returns>
        protected TypeAdapterSetter<TViewModel, TEntity> SetCustomMappings() => Config!.ForType<TViewModel, TEntity>();

        /// <summary>
        /// Sets custom mappings for viewmodel to entity.
        /// </summary>
        /// <returns>The type adapter setter.</returns>
        protected TypeAdapterSetter<TEntity, TViewModel> SetCustomMappingsReverse() => Config!.ForType<TEntity, TViewModel>();

        /// <summary>
        /// Registers the type adapter configuration and adds custom mappings.
        /// </summary>
        /// <param name="config">The configuration of the type adapter.</param>
        public virtual void Register(TypeAdapterConfig config)
        {
            Config = config;
            AddCustomMappings();

            // AppUser
            config.NewConfig<AppUser, UserViewModel>().IgnoreNullValues(true);
            config.NewConfig<AppUser, SignUpDto>().IgnoreNullValues(true);

            // AppRole
            config.NewConfig<AppRole, CreateAppRoleDto>().IgnoreNullValues(true);
            config.NewConfig<AppRole, RoleViewModel>().IgnoreNullValues(true);
            config.NewConfig<List<AppRole>, List<CreateAppRoleDto>>().IgnoreNullValues(true);

            // Project
            config.NewConfig<Project, CreateProjectDto>();
            config.NewConfig<Project, ProjectViewModel>().IgnoreNullValues(true);
            config.NewConfig<IReadOnlyCollection<Project>, IReadOnlyCollection<ProjectViewModel>>();
        }

        /// <summary>
        /// Maps Entity to an existing ViewModel.
        /// </summary>
        /// <returns>The Model.</returns>
        public TEntity ToEntity()
        {
            return this.Adapt<TEntity>();
        }

        /// <summary>
        /// Maps Entity to an existing ViewModel.
        /// </summary>
        /// <param name="model">The existing ViewModel instance to be updated.</param>
        /// <returns>The updated Model instance.</returns>
        public TEntity ToEntity(TEntity model)
        {
            return (this as TViewModel).Adapt(model);
        }

        /// <summary>
        /// Maps a ViewModel to a Entity.
        /// </summary>
        /// <param name="model">The ViewModel to be mapped.</param>
        /// <returns>The DTO.</returns>
        public TViewModel ToViewModel(TEntity model)
        {
            return model.Adapt<TViewModel>();
        }
    }
}
