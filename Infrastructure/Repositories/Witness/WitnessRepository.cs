using Application.Interfaces.Witness;
using Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Witness
{
    public class WitnessRepository : RepositoryAsync<Domain.Entities.Witness.Witness, long>, IWitnessRepository
    {
        public WitnessRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
