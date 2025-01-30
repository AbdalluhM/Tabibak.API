using Tabibak.Api.BLL;
using Tabibak.Api.Mapper;

namespace EcommerceApi.DependancyInjection
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region BLL
            foreach (var implementationType in typeof(BaseBLL).Assembly.GetTypes().Where(s => s.IsClass && s.Name.EndsWith("BLL") && !s.IsAbstract))
            {
                foreach (var interfaceType in implementationType.GetInterfaces())
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }
            #endregion


            services.AddAutoMapper(typeof(UserProfile));
            // Add Auto Mapper
            //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



            return services;
        }
    }
}
