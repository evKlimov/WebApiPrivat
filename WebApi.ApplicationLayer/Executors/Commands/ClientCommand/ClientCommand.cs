using System;
using System.Runtime.Serialization;

namespace WebApi.ApplicationLayer.Executors
{
    [Serializable]
    public class ClientCommand : ICommand
    {
        public string client_id { get; set; }
        public string departemnt_address { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
    }
}
