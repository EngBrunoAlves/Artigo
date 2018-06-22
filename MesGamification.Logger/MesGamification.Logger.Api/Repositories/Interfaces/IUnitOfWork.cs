using System;

namespace MesGamification.Logger.Api.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUserLogRepository UserLogs { get; }
    }
}