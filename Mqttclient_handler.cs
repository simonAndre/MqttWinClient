using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MQTT
{
 


    public class Mqttclient_handler:IDisposable
    {
        private MqttClient client = null;
        public delegate void onconnectedHandler(Mqttclient_handler sender, EventArgs e);
        public delegate void onDisconnectedHandler(Mqttclient_handler sender, EventArgs e);
        public delegate void MqttMessagePublishHandler(Mqttclient_handler sender, MessPublishEventArgs e);
        public event onconnectedHandler OnConnected;
        public event onDisconnectedHandler OnDisconnected;
        public event MqttMessagePublishHandler OnMessageArrived;


        public Mqttclient_handler()
        {

        }

        public bool MqttConnect(string serveur, string clientid, string username, string password, bool willRetain, byte willQosLevel, bool willFlag, string willTopic, string willMessage, bool cleanSession, ushort keepAlivePeriod)
        {
            // create client instance 
            client = new MqttClient(serveur);

            // register to message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            //string clientId = Guid.NewGuid().ToString();
           var res= client.Connect(clientid,  username,  password,  willRetain,  willQosLevel,  willFlag,  willTopic,  willMessage,  cleanSession,  keepAlivePeriod);
            if (OnConnected != null)
                OnConnected(this, new EventArgs());
            return client.IsConnected ;
        }
        public bool MqttConnect(string serveur, string clientid)
        {
            // create client instance 
            client = new MqttClient(serveur);

            // register to message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            //string clientId = Guid.NewGuid().ToString();
            client.Connect(clientid);
            if (OnConnected != null)
                OnConnected(this, new EventArgs());
            return client.IsConnected;
        }


        private void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            if (OnMessageArrived != null)
                OnMessageArrived(this, new MessPublishEventArgs(e.Topic,e.Message,e.DupFlag,e.QosLevel,e.Retain));
        }

        public bool Disconnect()
        {
            if (this.client != null)
            {
                this.client.Disconnect();
                if (OnDisconnected != null)
                    OnDisconnected(this, new EventArgs());
                return !this.client.IsConnected;
            }
            return false;
        }
        public bool IsConnected { get {
                return client!=null && client.IsConnected;
            }
        }

        public void subscribe(string topic, byte qos = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE)
        {
            client.Subscribe(new string[] { topic }, new byte[] { qos });
        }
        public void UnSubscribe(string topic)
        {
            client.Unsubscribe(new string[] { topic });
        }
        public void publish(string topic, string message,byte qos,bool retain=false)
        {
            client.Publish(topic, Encoding.UTF8.GetBytes(message), qos, retain);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                }
                if (this.client != null)
                {
                    if (this.client.IsConnected)
                        this.client.Disconnect();
                    this.client = null;
                }
                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // TODO: remplacer un finaliseur seulement si la fonction Dispose(bool disposing) ci-dessus a du code pour libérer les ressources non managées.
        // ~Mqtt() {
        //   // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
        //   Dispose(false);
        // }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
    public class MessPublishEventArgs : MqttMsgPublishEventArgs
    {
        public MessPublishEventArgs(string topic, byte[] message, bool dupFlag, byte qosLevel, bool retain) : base(topic, message, dupFlag, qosLevel, retain)
        {

        }


        public string StringMessage
        {
            get
            {
                return Encoding.UTF8.GetString(base.Message);
            }
        }
    }
}
