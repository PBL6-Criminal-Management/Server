using Application.Interfaces.CriminalImage;
using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.CriminalImage
{
    public class CriminalImageRepository : RepositoryAsync<Domain.Entities.CriminalImage.CriminalImage, long>, ICriminalImageRepository
    {
        public CriminalImageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
