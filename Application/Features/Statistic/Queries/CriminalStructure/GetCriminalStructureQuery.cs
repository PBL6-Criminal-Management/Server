using Application.Interfaces.CaseCriminal;
using Domain.Wrappers;
using MediatR;
using System.Linq.Dynamic.Core;

namespace Application.Features.Statistic.Queries.CriminalStructure
{
    public class GetCriminalStructureQuery : IRequest<Result<List<GetCriminalStructureResponse>>>
    {
    }
    internal class GetCriminalStructureQueryHandler : IRequestHandler<GetCriminalStructureQuery, Result<List<GetCriminalStructureResponse>>>
    {
        private readonly ICaseCriminalRepository _caseCriminalRepository;
        List<string> commonCharge = new List<string>() { "Trộm cắp tài sản", "Ẩu đả, xô xát", "Giết người", "Đánh bạc", "Lừa đảo", "Tội khác" };

        public GetCriminalStructureQueryHandler(ICaseCriminalRepository caseCriminalRepository)
        {
            _caseCriminalRepository = caseCriminalRepository;
        }
        private string Normalize(string charge)
        {
            if (charge.ToLower().Contains("trộm") || charge.ToLower().Contains("cướp"))
                return commonCharge[0];

            if (charge.ToLower().Contains("đánh bạc") || charge.ToLower().Contains("cờ bạc") || charge.ToLower().Contains("bài"))
                return commonCharge[3];

            if (charge.ToLower().Contains("ẩu đả") || charge.ToLower().Contains("xô xát") || charge.ToLower().Contains("đánh") || charge.ToLower().Contains("thương tích"))
                return commonCharge[1];

            if (charge.ToLower().Contains("giết") || charge.ToLower().Contains("chết người"))
                return commonCharge[2];

            if (charge.ToLower().Contains("lừa"))
                return commonCharge[4];

            return commonCharge[5];
        }
        public async Task<Result<List<GetCriminalStructureResponse>>> Handle(GetCriminalStructureQuery request, CancellationToken cancellationToken)
        {
            var criminalStructure = _caseCriminalRepository.Entities.AsEnumerable().Select(c => Normalize(c.Charge)).GroupBy(c => c).Select(gr => new
            {
                Charge = gr.Key,
                ChargeRatio = gr.Count()
            });

            var totalCriminals = _caseCriminalRepository.Entities.Count();

            var query = criminalStructure.Select(cs => new GetCriminalStructureResponse
            {
                Charge = cs.Charge,
                Percent = cs.ChargeRatio*100 / totalCriminals
            });

            var data = query.OrderByDescending(c => c.Percent).ToList();

            return await Result<List<GetCriminalStructureResponse>>.SuccessAsync(data);

        }
    }
}
