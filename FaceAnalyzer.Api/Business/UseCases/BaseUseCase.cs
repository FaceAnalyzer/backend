using AutoMapper;
using FaceAnalyzer.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FaceAnalyzer.Api.Business.UseCases;

public abstract class BaseUseCase
{
    
    protected BaseUseCase(IMapper mapper, AppDbContext dbContext)
    {
        Mapper = mapper;
        DbContext = dbContext;
    }
    protected IMapper Mapper { get; set; }
    protected AppDbContext DbContext { get; set; }
}