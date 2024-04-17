using Microsoft.Extensions.DependencyInjection;
using Common.Models;
using EasyNetQ;

namespace Common.EasyNetQAutoSubscriber
{
    public abstract class BaseEasyNetQRPCHandler
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IBus _bus;

        public BaseEasyNetQRPCHandler(IServiceProvider serviceProvider, IBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        public abstract void CreateRequestListeners();

        protected async Task<ExecutionResult<TResponse>> ExceptionHandlerAsync<TResponse>(Func<IServiceProvider, Task<ExecutionResult<TResponse>>> operationAsync)
        {
            try
            {
                using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    return await operationAsync(scope.ServiceProvider);
                }
            }
            catch
            {
                return new(keyError: "UnknownError", error: "Unknown error");
            }
        }
    }
}

/*
   // ищем реализации RequestAsync
   // для каждой создаем _bus.Rpc.RespondAsync<TRequest, ExecutionResult<TResponse>>

   var assembly = Assembly.GetExecutingAssembly(); // Получаем сборку, содержащую текущий исполняемый файл
   var types = assembly.GetTypes(); // Получаем все типы в этой сборке

   var implementations = types
       .Where(t => t.GetInterfaces()
                       .Any(i => i.IsGenericType &&
                               i.GetGenericTypeDefinition() == typeof(IRequestAsync<,>)))
       .ToList();


   CancellationToken cancellationToken = new();
   Action<IResponderConfiguration> action = (_) =>
   {

   };

   foreach (var implementation in implementations)
   {
       var interfaceType = implementation.GetInterfaces()
           .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestAsync<,>));

       var typeArguments = interfaceType.GetGenericArguments();
       var responseType = typeArguments[0];
       var requestType = typeArguments[1];

       responseType = typeof(ExecutionResult<>).MakeGenericType(responseType);

       var method = typeof(IRpc)!.GetMethod("RespondAsync")!.MakeGenericMethod(requestType, responseType);



       //Func<GetUpdateStatusesRequest, CancellationToken, Task<ExecutionResult<GetUpdateStatusesResponse>>> f = RequestAsync;

       method.Invoke(_bus.Rpc, [f, action, cancellationToken]);
   }

   // автогенерация _bus.Rpc.RespondAsync<GetUpdateStatusesRequest, ExecutionResult<GetUpdateStatusesResponse>>
   // в каждом _bus.Rpc.RespondAsync<GetUpdateStatusesRequest, ExecutionResult<GetUpdateStatusesResponse>> есть обработчик исключений перед вызовом основной логики
   // перед каждым запросом идет проверка атрибутов валидации 


   public interface IRequestAsync<TResponse, TRequest>
   {
       Task<ExecutionResult<TResponse>> RequestAsync(TRequest request);
   }

   public class A : IRequestAsync<GetUpdateStatusesResponse, GetUpdateStatusesRequest>
   {
       private readonly IUpdateDictionaryService _updateDictionaryService;

       public async Task<ExecutionResult<GetUpdateStatusesResponse>> RequestAsync(GetUpdateStatusesRequest request)
       {
           ExecutionResult<List<UpdateStatusDTO>> updateStatuses = await _updateDictionaryService.GetUpdateStatusesAsync();
           if (!updateStatuses.IsSuccess)
           {
               return new() { Errors = updateStatuses.Errors };
           }

           return new()
           {
               Result = new()
               {
                   UpdateStatuses = updateStatuses.Result!.Select(updateStatus => new UpdateStatusCommonDTO()
                   {
                       Comments = updateStatus.Comments,
                       DictionaryType = updateStatus.DictionaryType,
                       LastUpdate = updateStatus.LastUpdate,
                       Status = updateStatus.Status
                   }).ToList()
               }
           };
       }
   }
   */