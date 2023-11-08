using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Witness
{
    public interface IWitnessRepository : IRepositoryAsync<Domain.Entities.Witness.Witness,long>
    {
    }
}
