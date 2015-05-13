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
        bool forceFeedbackEnabled = false;
        double forceOffset_LX = 0;
        double forceOffset_LY = 0;
        double forceOffset_LZ = 0;
        double forceOffset_RX = 0;
        double forceOffset_RY = 0;
        double forceOffset_RZ = 0;

        const string address = "newphantom.dll";

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern int initAndSchedule(string leftOmni, string rightOmni);

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setForce1(double forceX, double forceY, double forceZ);

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setForce2(double forceX, double forceY, double forceZ);

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getpos1();

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getpos2();

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lock1();

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lock2();

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReleaseMemory(IntPtr ptr);


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

            int error = initAndSchedule("Omni_Left", "Omni_Right");
            if (error == 1)
            {
                lock1();
                lock2();
            }
            else
            {
                MessageBox.Show("Omni initialization error. Please check connections and try again.");
            }
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

            //set forces to match remote omnis
            IntPtr ptr = getpos1();
            double[] pos1 = new double[8];
            Marshal.Copy(ptr, pos1, 0, 8);

            ReleaseMemory(ptr);

            IntPtr ptr2 = getpos2();
            double[] pos2 = new double[8];
            Marshal.Copy(ptr2, pos2, 0, 8);

            ReleaseMemory(ptr2);


            if (forceFeedbackEnabled)
            {
                double forceLX = (listenerSockets.ElementAt(i).SocketMessage.XOmniLeft - (pos1[0] - forceOffset_LX)) / 50;
                double forceLY = (listenerSockets.ElementAt(i).SocketMessage.YOmniLeft - (pos1[1] - forceOffset_LY)) / 50;
                double forceLZ = (listenerSockets.ElementAt(i).SocketMessage.ZOmniLeft - (pos1[2] - forceOffset_LZ)) / 50;
                double forceRX = (listenerSockets.ElementAt(i).SocketMessage.XOmniRight - (pos2[0] - forceOffset_RX)) / 50;
                double forceRY = (listenerSockets.ElementAt(i).SocketMessage.YOmniRight - (pos2[1] - forceOffset_RY)) / 50;
                double forceRZ = (listenerSockets.ElementAt(i).SocketMessage.ZOmniRight - (pos2[2] - forceOffset_RZ)) / 50;

                setForce1(forceLX, forceLY, forceLZ);
                setForce2(forceRX, forceRY, forceRZ);
            }
            else
            {
                setForce1(0, 0, 0);
                setForce2(0, 0, 0);
            }
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

        private void cb_ForceEnable_CheckedChanged(object sender, EventArgs e)
        {
            forceFeedbackEnabled = cb_ForceEnable.Checked;
        }

        private void btn_zeroForces_Click(object sender, EventArgs e)
        {
            IntPtr ptr = getpos1();
            double[] pos1 = new double[8];
            Marshal.Copy(ptr, pos1, 0, 8);

            ReleaseMemory(ptr);

            IntPtr ptr2 = getpos2();
            double[] pos2 = new double[8];
            Marshal.Copy(ptr2, pos2, 0, 8);

            ReleaseMemory(ptr2);

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

            //other left omni
            double remotePos_LX = listenerSockets.ElementAt(i).SocketMessage.XOmniLeft;
            double remotePos_LY = listenerSockets.ElementAt(i).SocketMessage.YOmniLeft;
            double remotePos_LZ = listenerSockets.ElementAt(i).SocketMessage.ZOmniLeft;

            //other right omni
            double remotePos_RX = listenerSockets.ElementAt(i).SocketMessage.XOmniRight;
            double remotePos_RY = listenerSockets.ElementAt(i).SocketMessage.YOmniRight;
            double remotePos_RZ = listenerSockets.ElementAt(i).SocketMessage.ZOmniRight;

            forceOffset_LX = pos1[0] - remotePos_LX;
            forceOffset_LY = pos1[1] - remotePos_LY;
            forceOffset_LZ = pos1[2] - remotePos_LZ;
            forceOffset_RX = pos2[0] - remotePos_RX;
            forceOffset_RY = pos2[1] - remotePos_RY;
            forceOffset_RZ = pos2[2] - remotePos_RZ;


        }
    }
}
