using System;

namespace MesGamification.Logger.Api.Entities
{
    public class UserLog : EntityBase
    {
        public string Body { get; set; }
        public Guid UserId { get; set; }        
    }
}