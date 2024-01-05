using Application.Interfaces.Evidence;
using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Evidence
{
    public class EvidenceRepository : RepositoryAsync<Domain.Entities.Evidence.Evidence, long>, IEvidenceRepository
    {
        public EvidenceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
