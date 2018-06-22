using System;
using MesGamification.Logger.Api.Entities;
using MesGamification.Logger.Api.Repositories.Context;
using MesGamification.Logger.Api.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace MesGamification.Logger.Api.Repositories
{
    public class UserLogRepository : RepositoryBase<UserLog>, IUserLogRepository
    {
        public UserLogRepository(MesGamificationContext context) : base(context) { }
    }
}