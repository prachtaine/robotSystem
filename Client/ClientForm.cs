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
using Client.Sockets;
using Client.DataModels;
using System.Net;
using Client.Helpers;

namespace Client
{
    public partial class ClientForm : Form
    {
        //absolute path of dll
        const string address = "newphantom.dll";

        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern int initAndSchedule(string leftOmni, string rightOmni);
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern int stopAndDisable();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lock1();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lock2();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void unlock1();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void unlock2();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern double getX1();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern double getY1();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern double getZ1();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setForce1(double forceX, double forceY, double forceZ);
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setForce2(double forceX, double forceY, double forceZ);
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern int setHaptic(int aHaptic);
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern void setViscous(int aViscous);
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getpos1();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getpos2();
        [DllImport(address, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReleaseMemory(IntPtr ptr);

        //sockets
        ListenerSocket socketListener = new ListenerSocket();
        TalkerSocket talkerSocket;
        bool canSend = false;
        int MyUnderlyingListenerPort = 11000;
        int masterListeningPort = 12000;
        bool enableForceFeedback = false;
        double forceOffsetX = 0;
        double forceOffsetY = 0;
        double forceOffsetZ = 0;

        public ClientForm()
        {
            InitializeComponent();
            ClientForm.CheckForIllegalCrossThreadCalls = false;
        }

        private void InitializeOmnis_Click(object sender, EventArgs e)
        {
            if (spRightOmni.SelectedIndex == -1 && spLeftOmni.SelectedIndex == -1)
            {
                MessageBox.Show("Both the Left and Right Omni's need to be selected");
            }
            else if (spRightOmni.SelectedIndex == -1)
            {
                MessageBox.Show("Select a valid Right Omni");
            }
            else if (spLeftOmni.SelectedIndex == -1)
            {
                MessageBox.Show("Select a valid Left Omni");
            }
            else if (spLeftOmni.SelectedIndex == spRightOmni.SelectedIndex)
            {
                MessageBox.Show("Please select two different Omni Devices");
            }
            else
            {
                string Left = spLeftOmni.SelectedItem.ToString();
                string Right = spRightOmni.SelectedItem.ToString();

                int error = initAndSchedule(Left, Right);
                if (error == 1)
                {
                    lock1();
                    lock2();
                    btStop.Enabled = false;
                    _sendFromRealOmnis = true;
                    UnderlyingTimer.Enabled = true;
                    SimulateConnectToMasterButton.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Omni initialization error. Please check connections and try again.");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private double fakeData = 0;
        private bool _sendFromRealOmnis = false;

        private void sendData()
        {
            if (_sendFromRealOmnis)
            {
                SocketMessage messageToSend = new SocketMessage();
                messageToSend.MessageType = "OmniMessage";


                IntPtr ptr = getpos1();
                double[] pos1 = new double[8];
                Marshal.Copy(ptr, pos1, 0, 8);

                ReleaseMemory(ptr);

                IntPtr ptr2 = getpos2();
                double[] pos2 = new double[8];
                Marshal.Copy(ptr2, pos2, 0, 8);

                ReleaseMemory(ptr2);

                //home the position
                Coordinate oldPosition = new Coordinate(pos1[0], pos1[1], pos1[2], pos2[0], pos2[1], pos2[2]);
                //Coordinate homedPosition = homePosition.getPosition(oldPosition);
                //Coordinate leftPositionWithBoundaries = RobotBarriers.GetBoundaryCoordinates(homedPosition);

                lbX1value.Text = "X : " + oldPosition.leftX.ToString();
                lbY1value.Text = "Y : " + oldPosition.leftY.ToString();
                lbZ1value.Text = "Z : " + oldPosition.leftZ.ToString();

                lbGimbal11.Text = "Gimbal 1 : " + pos1[3].ToString();
                lbGimbal21.Text = "Gimbal 2 : " + pos1[4].ToString();
                lbGimbal31.Text = "Gimbal 3 : " + pos1[5].ToString();

                lbButtons1.Text = "Buttons : " + pos1[6].ToString();
                lbInk1.Text = "InkWell : " + pos1[7].ToString();

                lbX2Value.Text = "X : " + oldPosition.rightX.ToString();
                lbY2Value.Text = "Y : " + oldPosition.rightY.ToString();
                lbZ2Value.Text = "Z : " + oldPosition.rightZ.ToString();

                lbGimbal12.Text = "Gimbal 1 : " + pos2[3].ToString();
                lbGimbal22.Text = "Gimbal 2 : " + pos2[4].ToString();
                lbGimbal32.Text = "Gimbal 3 : " + pos2[5].ToString();

                lbButtons2.Text = "Buttons : " + pos2[6].ToString();
                lbInk2.Text = "InkWell : " + pos2[7].ToString();

                if (enableForceFeedback)
                {
                    double forceX = (oldPosition.leftX - (oldPosition.rightX - forceOffsetX))/50;
                    double forceY = (oldPosition.leftY - (oldPosition.rightY - forceOffsetY))/50;
                    double forceZ = (oldPosition.leftZ - (oldPosition.rightZ - forceOffsetZ))/50;

                    tb_forces.Text = @"Sending Force to Right:
                    X = "+forceX+@"
                    Y = "+forceY+@"
                    Z = "+forceZ;

                    setForce2(forceX, forceY, forceZ);
                }
                else
                {
                    setForce2(0, 0, 0);
                }

                //populate socketmessagetoSend if it is to be sent with Left omni position
                if (canSend)
                {
                    messageToSend.XOmniLeft = oldPosition.leftX;
                    messageToSend.YOmniLeft = oldPosition.leftY;
                    messageToSend.ZOmniLeft = oldPosition.leftZ;

                    messageToSend.Gimbal1OmniLeft = pos1[3];
                    messageToSend.Gimbal2OmniLeft = pos1[4];
                    messageToSend.Gimbal3OmniLeft = pos1[5];

                    messageToSend.ButtonsLeft = pos1[6];
                    messageToSend.InkwellLeft = pos1[7];
                }

                //populate socketmessagetoSend if it is to be sent with Right omni position
                if (canSend)
                {
                    messageToSend.XOmniRight = oldPosition.rightX;
                    messageToSend.YOmniRight = oldPosition.rightY;
                    messageToSend.ZOmniRight = oldPosition.rightZ;

                    messageToSend.Gimbal1OmniRight = pos2[3];
                    messageToSend.Gimbal2OmniRight = pos2[4];
                    messageToSend.Gimbal3OmniRight = pos2[5];

                    messageToSend.ButtonsRight = pos2[6];
                    messageToSend.InkwellRight = pos2[7];
                }
                //finally send the built message
                if (canSend)
                {
                    talkerSocket.sendData(messageToSend);
                }
            }
            else
            {
                SocketMessage messageToSend = new SocketMessage();
                messageToSend.MessageType = "OmniMessage";

                fakeData++;
                if (canSend)
                {
                    messageToSend.XOmniLeft = fakeData;
                    messageToSend.YOmniLeft = fakeData;
                    messageToSend.ZOmniLeft = fakeData;
                    messageToSend.Gimbal1OmniLeft = fakeData;
                    messageToSend.Gimbal2OmniLeft = fakeData;
                    messageToSend.Gimbal3OmniLeft = fakeData;
                    messageToSend.ButtonsLeft = fakeData;
                    messageToSend.InkwellLeft = fakeData;

                    messageToSend.XOmniRight = fakeData;
                    messageToSend.YOmniRight = fakeData;
                    messageToSend.ZOmniRight = fakeData;
                    messageToSend.Gimbal1OmniRight = fakeData;
                    messageToSend.Gimbal2OmniRight = fakeData;
                    messageToSend.Gimbal3OmniRight = fakeData;
                    messageToSend.ButtonsRight = fakeData;
                    messageToSend.InkwellRight = fakeData;

                    talkerSocket.sendData(messageToSend);
                }
            }
        }

        private void UnderlyingTimerTick(object sender, EventArgs e)
        {
            sendData();
        }

        private void ConnectToMasterButtonClick(object sender, EventArgs e)
        {
            //if (MasterIPTextBox.Text.Equals(""))
            //{
            //    StatusLabel.Text = "Please enter the IP address of the computer you are connecting to.";
            //}
            //else
            //{
            //    //for testing, leave IP to the IP of this machine
            //    //talkeSocket = new TalkerSocket(MasterIPTextBox);
            //    talkerSocket = new TalkerSocket("129.93.8.92");
            //    canSend = true;
            //    StatusLabel.Text = "---";
            //}
            
            //talkerSocket = new TalkerSocket("137.197.202.93", masterListeningPort);
            //talkerSocket = new TalkerSocket("129.93.23.187", masterListeningPort);
            talkerSocket = new TalkerSocket("129.93.8.92", masterListeningPort);
            canSend = true;
            StatusLabel.Text = "---";
            SocketMessage socketMessage = new SocketMessage();

            //build request that is sent to master console
            socketMessage.MessageType = "PermissionToConnect";
            socketMessage.IpAddress = GetIP();
            if (NameTextBox.Text.Equals(""))
            {
                StatusLabel.Text = "Please enter your name and try connecting again.";
            }
            else
            {
                ListenerSocket listener = new ListenerSocket(MyUnderlyingListenerPort, this);
                listener.StartListening();

                socketMessage.Name = NameTextBox.Text;
                talkerSocket.sendData(socketMessage);
                ConnectToMasterButton.Enabled = false;
                SimulateConnectToMasterButton.Enabled = false;

                StatusLabel.Text = "---";
            }
            
            //socketMessage
            //fakechange

        }

        internal void StartSendingPosition(string port)
        {
            //talkerSocket = new TalkerSocket("137.197.202.93", Int32.Parse(port));
            //talkerSocket = new TalkerSocket("129.93.23.187", Int32.Parse(port));
            talkerSocket = new TalkerSocket("129.93.8.92", Int32.Parse(port));
            canSend = true;
        }

        String GetIP()
        {
            String strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            // Grab the first IP addresses
            String IPStr = "";
            foreach (IPAddress ipaddress in iphostentry.AddressList)
            {
                IPStr = ipaddress.ToString();
                return IPStr;
            }
            return IPStr;
        }

        private void SimulateFakeConnectToMasterClick(object sender, EventArgs e)
        {
            //if (MasterIPTextBox.Text.Equals(""))
            //{
            //    StatusLabel.Text = "Please enter the IP address of the computer you are connecting to.";
            //}
            //else
            //{
            //    //for testing, leave IP to the IP of this machine
            //    //talkeSocket = new TalkerSocket(MasterIPTextBox);
            //    talkerSocket = new TalkerSocket("129.93.8.92");
            //    canSend = true;
            //    StatusLabel.Text = "---";
            //}

            talkerSocket = new TalkerSocket("137.197.202.93", masterListeningPort);
            //talkerSocket = new TalkerSocket("129.93.23.187", masterListeningPort);
            //canSend = true;
            StatusLabel.Text = "---";
            SocketMessage socketMessage = new SocketMessage();

            //build request that is sent to master console
            socketMessage.MessageType = "PermissionToConnect";
            socketMessage.IpAddress = GetIP();
            if (NameTextBox.Text.Equals(""))
            {
                StatusLabel.Text = "Please enter your name and try connecting again.";
            }
            else
            {
                ListenerSocket listener = new ListenerSocket(MyUnderlyingListenerPort, this);
                listener.StartListening();

                socketMessage.Name = NameTextBox.Text;
                talkerSocket.sendData(socketMessage);
                ConnectToMasterButton.Enabled = false;
                SimulateConnectToMasterButton.Enabled = false;
                _sendFromRealOmnis = false;
                UnderlyingTimer.Enabled = true;
                btStop.Enabled = false;

                StatusLabel.Text = "---";
            }

            //socketMessage
            //fakechange
        }
        HomePosition homePosition = new HomePosition();
        private void InvertCheckChanged(object sender, EventArgs e)
        {
            homePosition.InvertX = checkBoxInvertX.Checked;
            homePosition.InvertY = checkBoxInvertY.Checked;
            homePosition.InvertZ = checkBoxInvertZ.Checked;
        }

        private void ResetHomeButtonClick(object sender, EventArgs e)
        {
            IntPtr ptr = getpos1();
            double[] pos1 = new double[8];
            Marshal.Copy(ptr, pos1, 0, 8);

            ReleaseMemory(ptr);

            IntPtr ptr2 = getpos2();
            double[] pos2 = new double[8];
            Marshal.Copy(ptr2, pos2, 0, 8);

            ReleaseMemory(ptr2);
            Coordinate oldxyz = new Coordinate(pos1[0], pos1[1], pos1[2], pos2[0], pos2[1], pos2[2]);
            homePosition.resetHome(oldxyz);
        }

        private void cb_forceEnable_CheckedChanged(object sender, EventArgs e)
        {
            enableForceFeedback = cb_forceEnable.Checked;
        }

        private void btn_zeroForces_Click(object sender, EventArgs e)
        {
            SocketMessage messageToSend = new SocketMessage();
            messageToSend.MessageType = "OmniMessage";


            IntPtr ptr = getpos1();
            double[] pos1 = new double[8];
            Marshal.Copy(ptr, pos1, 0, 8);

            ReleaseMemory(ptr);

            IntPtr ptr2 = getpos2();
            double[] pos2 = new double[8];
            Marshal.Copy(ptr2, pos2, 0, 8);

            ReleaseMemory(ptr2);

            //home the position
            Coordinate oldPosition = new Coordinate(pos1[0], pos1[1], pos1[2], pos2[0], pos2[1], pos2[2]);
            //Coordinate homedPosition = homePosition.getPosition(oldPosition);
            //Coordinate leftPositionWithBoundaries = RobotBarriers.GetBoundaryCoordinates(homedPosition);

            forceOffsetY = oldPosition.rightY - oldPosition.leftY ;
            forceOffsetX = oldPosition.rightX - oldPosition.leftX ;
            forceOffsetZ = oldPosition.rightZ - oldPosition.leftZ ;
        }

    }
}
