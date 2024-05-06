namespace AdmissioningService.Presentation.Web.RPCHandlers
{
    public static class RPCHandlerExtension
    {
        public static void AddRPCHandlers(this IServiceCollection service)
        {
            service.AddSingleton<ManagerRPCHandler>();
            service.AddSingleton<AdmissionRPCHandler>();
        }

        public static void UseRPCHandlers(this IServiceProvider service)
        {
            service.GetRequiredService<ManagerRPCHandler>().CreateRequestListeners();
            service.GetRequiredService<AdmissionRPCHandler>().CreateRequestListeners();
        }
    }
}
