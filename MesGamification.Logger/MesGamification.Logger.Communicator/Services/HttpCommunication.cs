using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MesGamification.Logger.Communicator.Entites;
using MesGamification.Logger.Communicator.Interfaces;
using Newtonsoft.Json;

namespace MesGamification.Logger.Communicator.Services
{
    public class HttpCommunication : ICommunication
    {
        public HttpCommunication() { }

        public async Task<Result<TOut>> GetAsync<TOut>(string url)
        {
            using (var client = Client())
            {
                var response = await client.GetAsync(url);
                return await Result<TOut>.ConvertResponse(response);
            }
        }

        public async Task<Result<TOut>> PostAsync<TOut>(string url, object entity)
        {
            using (var client = Client())
            {
                var content = GetContent(entity);
                var response = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
                return await Result<TOut>.ConvertResponse(response);
            }
        }

        public async Task<Result<TOut>> PutAsync<TOut>(string url, object entity)
        {
            using (var client = Client())
            {
                var content = GetContent(entity);
                var response = await client.PutAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
                return await Result<TOut>.ConvertResponse(response);
            }
        }

        public async Task<Result<TOut>> DeleteAsync<TOut>(string url)
        {
            using (var client = Client())
            {
                var response = await client.DeleteAsync(url);
                return await Result<TOut>.ConvertResponse(response);
            }
        }

        #region Helpers
        private HttpClient Client()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private string GetContent(object entity)
        {
            if (entity is null) return string.Empty;
            return JsonConvert.SerializeObject(entity);
        }
        #endregion
    }
}