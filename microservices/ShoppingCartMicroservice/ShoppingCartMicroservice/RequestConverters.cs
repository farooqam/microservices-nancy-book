using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Techniqly.Microservices.Abstractions;

namespace Techniqly.Microservices
{
    public class StringArrayRequestConverter : IRequestConverter<string[]>
    {
        public async Task<string[]> ConvertAsync(HttpRequest request)
        {
            var body = await new StreamReader(request.Body).ReadToEndAsync();
            return JsonConvert.DeserializeObject<string[]>(body);
        }
    }
}
