namespace DictionaryService.Presentation.Web.RPCHandlers
{
    public static class RPCHandlerExtension
    {
        public static void AddRPCHandlers(this IServiceCollection service)
        {
            service.AddSingleton<UpdateDictionaryRPCHandler>();
            service.AddSingleton<DictionaryInfoRPCHandler>();
        }

        public static void UseRPCHandlers(this IServiceProvider service)
        {
            service.GetRequiredService<UpdateDictionaryRPCHandler>().CreateRequestListeners();
            service.GetRequiredService<DictionaryInfoRPCHandler>().CreateRequestListeners();
        }
    }
}
