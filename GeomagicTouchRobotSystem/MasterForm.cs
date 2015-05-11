using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;
using MasterController.Sockets;
using MasterController.DataModels;

namespace GeomagicTouchRobotSystem
{
    public partial class MasterForm : Form
    {
        TalkerSocket talkerSocket ;
        List<ListenerSocket> listenerSockets = new List<ListenerSocket>();
        int MyUnderlyingListenerPort = 12000;
        int ClientUnderlyingListenerPort = 11000;
        public MasterForm()
        {
            InitializeComponent();
            MasterForm.CheckForIllegalCrossThreadCalls = false;
        }

        private void StartMasterConsoleButtonClick(object sender, EventArgs e)
        {
            //start the master socket, which is on port 11000
            ListenerSocket tmp = new ListenerSocket(MyUnderlyingListenerPort, this);
            listenerSockets.Add(tmp);
            StartMasterConsoleButton.Enabled = false;
            listenerSockets.ElementAt(0).StartListening();
            timer.Enabled = true;
        }

        private void timerTick(object sender, EventArgs e)
        {
            int i;
            //socketToListenTo
            if (listenerSockets.Count == 1)
            {
                i = 0;
            }
            else
            {
                i = 1;
            }

            //show other Left Omni
            lbX1value.Text = "X : " + listenerSockets.ElementAt(i).SocketMessage.XOmniLeft.ToString();
            lbY1value.Text = "Y : " + listenerSockets.ElementAt(i).SocketMessage.YOmniLeft.ToString();
            lbZ1value.Text = "Z : " + listenerSockets.ElementAt(i).SocketMessage.ZOmniLeft.ToString();

            lbGimbal11.Text = "Gimbal 1 : " + listenerSockets.ElementAt(i).SocketMessage.Gimbal1OmniLeft.ToString();
            lbGimbal21.Text = "Gimbal 2 : " + listenerSockets.ElementAt(i).SocketMessage.Gimbal2OmniLeft.ToString();
            lbGimbal31.Text = "Gimbal 3 : " + listenerSockets.ElementAt(i).SocketMessage.Gimbal3OmniLeft.ToString();

            lbButtons1.Text = "Buttons : " + listenerSockets.ElementAt(i).SocketMessage.ButtonsLeft.ToString();
            lbInk1.Text = "InkWell : " + listenerSockets.ElementAt(i).SocketMessage.InkwellLeft.ToString();


            //show other Right Omni
            lbX2Value.Text = "X : " + listenerSockets.ElementAt(i).SocketMessage.XOmniRight.ToString();
            lbY2Value.Text = "Y : " + listenerSockets.ElementAt(i).SocketMessage.YOmniRight.ToString();
            lbZ2Value.Text = "Z : " + listenerSockets.ElementAt(i).SocketMessage.ZOmniRight.ToString();

            lbGimbal12.Text = "Gimbal 1 : " + listenerSockets.ElementAt(i).SocketMessage.Gimbal1OmniRight.ToString();
            lbGimbal22.Text = "Gimbal 2 : " + listenerSockets.ElementAt(i).SocketMessage.Gimbal2OmniRight.ToString();
            lbGimbal32.Text = "Gimbal 3 : " + listenerSockets.ElementAt(i).SocketMessage.Gimbal3OmniRight.ToString();

            lbButtons2.Text = "Buttons : " + listenerSockets.ElementAt(i).SocketMessage.ButtonsRight.ToString();
            lbInk2.Text = "InkWell : " + listenerSockets.ElementAt(i).SocketMessage.InkwellRight.ToString();
        }

        internal void SomeoneIsConnecting(string IPaddress, string Name)
        {
            User1Label.Text = "User1 : " + Name + " has connected with an IP address of " + IPaddress;
            SocketMessage socketMessage = new SocketMessage();
            socketMessage.Port = (listenerSockets.Count + 12001).ToString();
            socketMessage.MessageType = "PortInformation";
            talkerSocket = new TalkerSocket(IPaddress, ClientUnderlyingListenerPort);
            ListenerSocket newListenSocket = new ListenerSocket(Int32.Parse(socketMessage.Port), this);
            listenerSockets.Add(newListenSocket);
            listenerSockets.ElementAt(listenerSockets.Count - 1).StartListening();
            talkerSocket.sendData(socketMessage);
        }
    }
}
