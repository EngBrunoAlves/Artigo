using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MesGamification.Logger.Api.Entities;
using MesGamification.Logger.Api.Repositories.Interfaces;

namespace MesGamification.Logger.Api.Services
{
    public class UserLogService
    {
        private readonly IUnitOfWork _uow;

        public UserLogService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentException(nameof(uow));
        }

        public async Task AddUserLog(UserLog userLog)
        {
            await _uow.UserLogs.InsertAsync(userLog);
        }

        public async Task UpdateUserLog(string id, UserLog userLog)
        {
            var userLogSaved = _uow.UserLogs.Get(id);
            await _uow.UserLogs.UpdateAsync(userLogSaved, i => i.Body, userLog.Body);
        }

        public async Task DeleteUserLog(string id)
        {
            await _uow.UserLogs.DeleteAsync(id);
        }

        public async Task<UserLog> GetUserLog(string id)
        {
            return await Task.Run(() =>
            {
                return _uow.UserLogs.Get(id);
            });
        }

        public async Task DeleteAllUserLog()
        {
            await _uow.UserLogs.DeleteAllAsync();
        }

        public async Task<IEnumerable<UserLog>> ListUserLog()
        {
            return await Task.Run(() =>
            {
                return _uow.UserLogs.FindAll();
            });
        }
    }
}