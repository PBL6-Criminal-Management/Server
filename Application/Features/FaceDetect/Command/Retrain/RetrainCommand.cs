using Domain.Constants;
using Domain.Wrappers;
using MediatR;
using Newtonsoft.Json;

namespace Application.Features.FaceDetect.Command.Retrain
{
    public class RetrainCommand : IRequest<Result<string>>
    {
    }
    internal class RetrainCommandHandler : IRequestHandler<RetrainCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(RetrainCommand request, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(StaticVariable.AI_SERVER_BASE_URL + "/retrain", null);

                    if (response.IsSuccessStatusCode)
                    {
                        // Read and deserialize the JSON content
                        string jsonContent = await response.Content.ReadAsStringAsync();
                        dynamic? result = JsonConvert.DeserializeObject(jsonContent);
                        string message = result != null ? result.message : "";
                        return await Result<string>.SuccessAsync(message);
                    }
                    else
                    {
                        return await Result<string>.FailAsync($"Lỗi: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    return await Result<string>.FailAsync($"Lỗi: {ex.Message}");
                }
            }
        }
    }
}
