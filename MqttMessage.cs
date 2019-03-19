using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT
{
    [Serializable]
    public class MqttMessage
    {
        public MqttMessage()
        {
            date = DateTime.Now;
        }
        public string topic { get; set; }
        public bool retained{ get; set; }
        public byte qos { get; set; }
        public string message{ get; set; }
        public DateTime date{ get; set; }
    }

}
