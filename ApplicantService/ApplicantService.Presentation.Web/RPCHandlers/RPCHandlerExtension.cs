namespace ApplicantService.Presentation.Web.RPCHandlers
{
    public static class RPCHandlerExtension
    {
        public static void AddRPCHandlers(this IServiceCollection service)
        {
            service.AddSingleton<ApplicantRPCHandler>();
            service.AddSingleton<DocumentRPCHandler>();
        }

        public static void UseRPCHandlers(this IServiceProvider service)
        {
            service.GetRequiredService<ApplicantRPCHandler>().CreateRequestListeners();
            service.GetRequiredService<DocumentRPCHandler>().CreateRequestListeners();
        }
    }
}
