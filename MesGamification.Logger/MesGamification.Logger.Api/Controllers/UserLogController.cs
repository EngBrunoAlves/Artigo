using System;
using System.Threading.Tasks;
using MesGamification.Logger.Api.Entities;
using MesGamification.Logger.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace MesGamification.Logger.Api.Controllers
{
    public class UserLogController : ControllerBase
    {
        private readonly UserLogService _service;

        public UserLogController(UserLogService service)
        {
            _service = service ?? throw new ArgumentException(nameof(service));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserLog userLog)
        {
            await _service.AddUserLog(userLog);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UserLog userLog)
        {
            await _service.UpdateUserLog(id, userLog);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteUserLog(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _service.GetUserLog(id));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _service.DeleteAllUserLog();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.ListUserLog());
        }
    }
}