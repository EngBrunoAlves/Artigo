using System;
using MesGamification.Logger.Api.Repositories.Context;
using MesGamification.Logger.Api.Repositories.Interfaces;
using Microsoft.Extensions.Options;

namespace MesGamification.Logger.Api.Repositories
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        private readonly MesGamificationContext _context;

        public UnitOfWork(MesGamificationContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(context));
        }

        public IUserLogRepository UserLogs => _userLogRepository ?? (_userLogRepository = new UserLogRepository(_context));
    }
}