using System;
using Newtonsoft.Json;

namespace MesGamification.Logger.Catch.Entities
{
    public class CatchDto
    {
        public string Serialize() => JsonConvert.SerializeObject(this);

        public DateTime EventTime { get; set; }
        public string Body { get; set; }
        public string Method { get; set; }
        public string PathBase { get; set; }
    }
}