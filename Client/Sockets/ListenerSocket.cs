using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Threading;
using Client.DataModels;

namespace Client.Sockets
{
    public class ListenerSocket
    {
        IPEndPoint localEndPoint;
        UdpClient udpListener = new UdpClient();
        SocketMessage _socketMessage = new SocketMessage();
        ClientForm _clientForm;
        int _port;

        public ListenerSocket()
        {

        }
        public ListenerSocket(int port, ClientForm clientForm)
        {
            this._socketMessage = new SocketMessage();
            this._clientForm = clientForm;
            this._port = port;
        }
        public SocketMessage SocketMessage
        {
            private set { this._socketMessage = value; }
            get { return this._socketMessage; }
        }


        public void StartListening()
        {
            localEndPoint = new IPEndPoint(IPAddress.Parse(GetIP()), _port);
            udpListener = new UdpClient(localEndPoint);
            Thread Listener = new Thread(new ThreadStart(Listen));
            Listener.Start();
        }

        void Listen()
        {
            while (true)
            {
                var remoteEP = new IPEndPoint(IPAddress.Any, _port);
                var data = udpListener.Receive(ref remoteEP); // listen on port 11000
                //gets ip of sender
                //Console.WriteLine(remoteEP.Address.ToString());
                ProcessSocketMessage(System.Text.Encoding.ASCII.GetString(data));

                //return null SocketMessage for now
            }
        }


        //feed this message processor a clean message that has the below function run on it
        //this.jsonIncomingMessage.Text = message;
        //string tmp = jsonIncomingMessage.Text;
        void ProcessSocketMessage(string message)
        {
            SocketMessage recievedObject = JsonConvert.DeserializeObject<SocketMessage>(message);
            if (recievedObject.MessageType == "OmniMessage")
            {
                //update omni
                this.SocketMessage = recievedObject;
            }
        }


        String GetIP()
        {
            string ipAddress = "";
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip.ToString();
                }
            }
            return ipAddress;
        }
    }

}
