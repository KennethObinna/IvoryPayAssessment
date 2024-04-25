

 

namespace IvoryPayAssessment.Application.Mappings
{
    public static class MapsterConfig
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            #region   the mappings start here
            //TypeAdapterConfig<RolesView, PermissionView>
            //             .NewConfig()
            //             .Map(dest => dest.Name, src => src.RoleName);

          

            #endregion Mapping ends here



            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        }
    }
}
