using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.ApplicationLayer.Rabbit
{
    [Serializable]
    public class RabbitMessage
    {
        public byte[] Data { get; set; }
    }
}
