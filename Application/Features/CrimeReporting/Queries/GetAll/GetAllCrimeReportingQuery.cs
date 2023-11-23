using Application.Interfaces.CrimeReporting;
using Domain.Helpers;
using Domain.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Application.Features.CrimeReporting.Queries.GetAll
{
    public class GetAllCrimeReportingQuery : GetAllCrimeReportingParameter, IRequest<PaginatedResult<GetAllCrimeReportingResponse>>
    {
    }
    internal class GetAllCrimeReportingQueryHandler : IRequestHandler<GetAllCrimeReportingQuery, PaginatedResult<GetAllCrimeReportingResponse>>
    {
        private readonly ICrimeReportingRepository _crimeReportingRepository;
        public GetAllCrimeReportingQueryHandler(ICrimeReportingRepository crimeReportingRepository)
        {
            _crimeReportingRepository = crimeReportingRepository;
        }
        public async Task<PaginatedResult<GetAllCrimeReportingResponse>> Handle(GetAllCrimeReportingQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Keyword))
                request.Keyword = request.Keyword.Trim();
            var query = _crimeReportingRepository.Entities.AsEnumerable()
                        .Where(cR => !cR.IsDeleted &&
                            (string.IsNullOrEmpty(request.Keyword) || StringHelper.Contains(cR.Content, request.Keyword) || StringHelper.Contains(cR.ReporterAddress, request.Keyword)))
                        .AsQueryable()
                        .Select(cR => new GetAllCrimeReportingResponse
                        {
                            Id = cR.Id,
                            ReporterName = cR.ReporterName,
                            ReporterPhone = cR.ReporterPhone,
                            ReporterAddress = cR.ReporterAddress,
                            ReporterEmail = cR.ReporterEmail,
                            Content = cR.Content,
                            CreatedAt = cR.CreatedAt,
                            Status = cR.Status,
                            SendingTime = DateOnly.FromDateTime(cR.CreatedAt)
                        });
            var data = query.OrderBy(request.OrderBy);
            List<GetAllCrimeReportingResponse> result;

            //Pagination
            if (!request.IsExport)
                result = data.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            else
                result = data.ToList();
            var totalRecord = result.Count();
            return PaginatedResult<GetAllCrimeReportingResponse>.Success(result, totalRecord, request.PageNumber, request.PageSize);
        }
    }
}