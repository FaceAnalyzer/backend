using AutoMapper;
using FaceAnalyzer.Api.Business.UseCases;
using FaceAnalyzer.Api.Data;

namespace FaceAnalyzer.Api.Business.BusinessModels;

public abstract class BusinessModelBase
{
    protected BusinessModelBase(IMapper mapper, AppDbContext dbContext)
    {
        Mapper = mapper;
        DbContext = dbContext;
    }

    protected IMapper Mapper { get; set; }
    protected AppDbContext DbContext { get; set; }
    // protected T Create<T, TIn, TOut>() where T :  BaseUseCase<TIn, TOut>, new ()
    // {
    //     var instance = new T();
    //     instance.AddServices(Mapper, DbContext);
    //     return instance;
    // }
}