using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Techniqly.Microservices.Abstractions
{
    public interface IRequestConverter<TTarget>
    {
        Task<TTarget> ConvertAsync(HttpRequest request);
    }
}
