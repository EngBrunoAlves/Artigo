using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MesGamification.Logger.Communicator.Entites
{
    public class Result<T>
    {
        public async static Task<Result<T>> ConvertResponse(HttpResponseMessage message) =>
            new Result<T>
            {
                HttpStatusCode = message.StatusCode,
                Content = message.Content != null && message.IsSuccessStatusCode ? JsonConvert.DeserializeObject<T>(await message.Content.ReadAsStringAsync()) : default(T),
                RequestMessage = message.RequestMessage.Content != null ? await message.RequestMessage.Content.ReadAsStringAsync() : string.Empty,
                Headers = message.RequestMessage?.Headers,
                IsSuccessStatusCode = message.IsSuccessStatusCode
            };

        public HttpStatusCode HttpStatusCode { get; set; }
        public T Content { get; set; }
        public string RequestMessage { get; set; }
        public HttpRequestHeaders Headers { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
}