using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Domain.Enum;
using DictionaryService.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DictionaryService.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController(IUpdateDictionaryService _updateDictionaryService) : ControllerBase
    {
        // В класс нужно пропихнуть IServiceProvider
        // Класс регаем как синглтон
        // в этом классе bus.Rpc.Respond<MyRequest, MyResponse>() передаем лямбду, в которой через IServiceProvider получаем нужный сервис 


        [HttpPost]
        public async Task<ActionResult> Update(DictionaryType dictionaryType)
        {
            return Ok(await _updateDictionaryService.UpdateDictionaryAsync(dictionaryType));
        }

        [HttpPost("all")]
        public async Task UpdateAll()
        {
            await _updateDictionaryService.UpdateAllDictionaryAsync();
        }

        [HttpGet]
        public async Task<List<UpdateStatusDTO>> GetStatuses()
        {
            return (await _updateDictionaryService.GetUpdateStatusesAsync()).Result!;
        }

        //[HttpGet]
        //public async Task<bool> Get() 
        //{
            

        //    using (IDbContextTransaction scope = await appDbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable))
        //    {

                
        //        //Lock the table during this transaction
        //        await appDbContext.Database.ExecuteSqlRawAsync("LOCK TABLE \"Faculties\" IN ACCESS EXCLUSIVE MODE");

        //        var a = await appDbContext.Faculties.FirstOrDefaultAsync();

        //        Console.WriteLine(a.Name + " 11111111111111");

        //        var f = appDbContext.Faculties.ToList();
        //        f.ForEach(x => x.Name = x.Name + "666");
        //        appDbContext.UpdateRange(f);
        //        await appDbContext.SaveChangesAsync();

        //        Thread.Sleep(10000);
        //        //Do your work with the locked table here...

        //        //Complete the scope here to commit, otherwise it will rollback
        //        //The table lock will be released after we exit the TransactionScope block
        //        scope.Commit();
        //    }

        //    await appDbContext.Faculties.AddAsync(new()
        //    {
        //        Name = "123",
        //        Deprecated = true,
        //    });

        //    Console.WriteLine("SAD11");
        //    await appDbContext.SaveChangesAsync();
        //    Console.WriteLine("SAD22");

        //    return true;
        //}


        //[HttpGet("sad")]
        //public async Task<List<Faculty>> Get2()
        //{
        //    var a = await appDbContext.Faculties.FirstOrDefaultAsync();

        //    Console.WriteLine(a.Name + " 2222222222333333333");

        //    return appDbContext.Faculties.ToList();
        //}
    }
}
