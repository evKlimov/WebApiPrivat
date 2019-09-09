using System;

namespace WebApi.Repository.Models
{
    [Serializable]
    public class Log
    {
        public int Id { get; set; }
        public string MassageLog { get; set; }
    }
}
