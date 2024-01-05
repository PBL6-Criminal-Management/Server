using Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.CaseImage
{
    public interface ICaseImageRepository : IRepositoryAsync<Domain.Entities.CaseImage.CaseImage, long>
    {
    }
}
