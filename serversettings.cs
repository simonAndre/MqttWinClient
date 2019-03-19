using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT
{
    [Serializable]
    public class serversettings:INotifyPropertyChanged
    {
        public serversettings()
        {
            ServerName = ConfigurationManager.AppSettings["ServerName"];
            ClientName = Environment.MachineName;
            ServerPort = int.Parse(ConfigurationManager.AppSettings["ServerPort"]);
            //willFlag = true;   
            //willMessage = "sortie";
            //willRetain = true;
            //willTopic = "/will/out";
            willQos = 1;
            UserName = ConfigurationManager.AppSettings["UserName"];
            Password =ConfigurationManager.AppSettings["Password"];
            //if (PropertyChanged != null)
            //    PropertyChanged(this, new PropertyChangedEventArgs(null));

        }

        public string ServerName { get; set; }
        public int ServerPort { get; set; }
        public string ClientName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool willRetain { get; set; }
        public byte willQos { get; set; }
        public bool willFlag { get; set; }
        public string willTopic { get; set; }
        public string willMessage { get; set; }
        public bool cleanSession { get; set; }
        public ushort keepAlivePeriod { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
