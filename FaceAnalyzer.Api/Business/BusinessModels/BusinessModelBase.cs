using AutoMapper;
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
}