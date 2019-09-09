using System;

namespace WebApi.Repository
{
    [Serializable]
    public class Client
    {
        public int Id { get; set; }
        public string client_id { get; set; }
        public string departemnt_address { get; set; }
        public int applicationStatus { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
    }
}
