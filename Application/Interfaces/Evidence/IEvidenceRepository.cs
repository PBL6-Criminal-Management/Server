using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Evidence
{
    public interface IEvidenceRepository : IRepositoryAsync<Domain.Entities.Evidence.Evidence, long>
    {
    }
}
