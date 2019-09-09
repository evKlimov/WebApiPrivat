using System;

namespace WebApi.ApplicationLayer.Executors
{
    [Serializable]
    public class RequestClientCommand : ICommand
    {
        public string client_id { get; set; }
        public string departemnt_address { get; set; }
    }
}
