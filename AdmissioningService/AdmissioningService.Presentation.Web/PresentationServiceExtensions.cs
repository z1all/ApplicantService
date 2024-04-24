﻿using Common.API.Configurations;

namespace AdmissioningService.Presentation.Web
{
    public static class PresentationServiceExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddSwaggerConfigure();
            services.AddModalStateConfigure();

            return services;
        }
    }
}