using Application.Interfaces.CaseImage;
using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.CaseImage
{
    public class CaseImageRepository : RepositoryAsync<Domain.Entities.CaseImage.CaseImage, long>, ICaseImageRepository
    {
        public CaseImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
