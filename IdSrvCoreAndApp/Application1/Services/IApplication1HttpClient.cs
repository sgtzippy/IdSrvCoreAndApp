using System.Net.Http;
using System.Threading.Tasks;

namespace Application1.Services
{
    public interface IApplication1HttpClient
    {
        Task<HttpClient> GetClient();
    }
}
