using MesGamification.Logger.Api.Repositories.Interfaces;

namespace MesGamification.Logger.Api.Repositories
{
    public abstract class UnitOfWorkBase
    {
        protected IUserLogRepository _userLogRepository;
    }
}