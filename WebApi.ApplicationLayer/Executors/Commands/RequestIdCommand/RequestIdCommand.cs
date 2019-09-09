using System;

namespace WebApi.ApplicationLayer.Executors
{
    [Serializable]
    public class RequestIdCommand : ICommand
    {
        public int request_id { get; set; }
    }
}
