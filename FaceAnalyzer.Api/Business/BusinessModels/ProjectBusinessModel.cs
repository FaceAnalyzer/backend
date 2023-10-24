using AutoMapper;
using FaceAnalyzer.Api.Data;

namespace FaceAnalyzer.Api.Business.BusinessModels;

public class ProjectBusinessModel : BusinessModelBase
{
    public ProjectBusinessModel(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
    
}