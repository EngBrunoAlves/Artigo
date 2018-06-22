using System.Threading.Tasks;
using MesGamification.Logger.Communicator.Entites;

namespace MesGamification.Logger.Communicator.Interfaces
{
    public interface ICommunication
    {
        Task<Result<TOut>> GetAsync<TOut>(string url);
        Task<Result<TOut>> PostAsync<TOut>(string url, object entity);
        Task<Result<TOut>> PutAsync<TOut>(string url, object entity);
        Task<Result<TOut>> DeleteAsync<TOut>(string url);
    }
}