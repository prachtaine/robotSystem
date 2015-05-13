using System;
using System.IO;
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
using System.Net.Sockets;

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

        TalkerSocket talkerSocket = null;
        ListenerSocket listenerSocket = null;
        bool canSend = false;
        int port = 12000;
        bool enableForceFeedback = false;

        double forceOffset_LX = 0;
        double forceOffset_LY = 0;
        double forceOffset_LZ = 0;
        double forceOffset_RX = 0;
        double forceOffset_RY = 0;
        double forceOffset_RZ = 0;

        public ClientForm()
        {
            InitializeComponent();
            ClientForm.CheckForIllegalCrossThreadCalls = false;
            tb_ipAddress.Text = GetIP();
            fillOmniDDL();
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
                    UnderlyingTimer.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Omni initialization error. Please check connections and try again.");
                }

                if (cb_isMaster.Checked && !tb_ipAddress.Text.Equals(""))
                {
                    //start sending omni info
                    talkerSocket = new TalkerSocket(tb_ipAddress.Text, port);
                    canSend = true;
                }
                else if (cb_isMaster.Checked && tb_ipAddress.Text.Equals(""))
                {
                    MessageBox.Show("Please enter an IP address to send to.");
                }
            }
        }

        private void sendData()
        {
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

            //populate socketmessagetoSend if it is to be sent with Left omni position
            if (canSend)
            {
                SocketMessage messageToSend = new SocketMessage();
                messageToSend.MessageType = "OmniMessage";

                messageToSend.XOmniLeft = oldPosition.leftX;
                messageToSend.YOmniLeft = oldPosition.leftY;
                messageToSend.ZOmniLeft = oldPosition.leftZ;

                messageToSend.Gimbal1OmniLeft = pos1[3];
                messageToSend.Gimbal2OmniLeft = pos1[4];
                messageToSend.Gimbal3OmniLeft = pos1[5];

                messageToSend.ButtonsLeft = pos1[6];
                messageToSend.InkwellLeft = pos1[7];

                //populate socketmessagetoSend with Right omni position
                messageToSend.XOmniRight = oldPosition.rightX;
                messageToSend.YOmniRight = oldPosition.rightY;
                messageToSend.ZOmniRight = oldPosition.rightZ;

                messageToSend.Gimbal1OmniRight = pos2[3];
                messageToSend.Gimbal2OmniRight = pos2[4];
                messageToSend.Gimbal3OmniRight = pos2[5];

                messageToSend.ButtonsRight = pos2[6];
                messageToSend.InkwellRight = pos2[7];

                //finally send the built message
                talkerSocket.sendData(messageToSend);
            }
        }

        private void setForces()
        {
            if (enableForceFeedback)
            {
                IntPtr ptr = getpos1();
                double[] pos1 = new double[8];
                Marshal.Copy(ptr, pos1, 0, 8);

                ReleaseMemory(ptr);

                IntPtr ptr2 = getpos2();
                double[] pos2 = new double[8];
                Marshal.Copy(ptr2, pos2, 0, 8);

                ReleaseMemory(ptr2);

                double forceLX = (listenerSocket.SocketMessage.XOmniLeft - (pos1[0] - forceOffset_LX)) / getForceStrength();
                double forceLY = (listenerSocket.SocketMessage.YOmniLeft - (pos1[1] - forceOffset_LY)) / getForceStrength();
                double forceLZ = (listenerSocket.SocketMessage.ZOmniLeft - (pos1[2] - forceOffset_LZ)) / getForceStrength();
                double forceRX = (listenerSocket.SocketMessage.XOmniRight - (pos2[0] - forceOffset_RX)) / getForceStrength();
                double forceRY = (listenerSocket.SocketMessage.YOmniRight - (pos2[1] - forceOffset_RY)) / getForceStrength();
                double forceRZ = (listenerSocket.SocketMessage.ZOmniRight - (pos2[2] - forceOffset_RZ)) / getForceStrength();

                setForce1(forceLX, forceLY, forceLZ);
                setForce2(forceRX, forceRY, forceRZ);
            }
            else
            {
                setForce1(0, 0, 0);
                setForce2(0, 0, 0);
            }
        }

        private int getForceStrength()
        {
            //want force divider between 20 and 220
            //divider = 220 - trackbarValue 

            trb_forceStrength.Minimum = 0;
            trb_forceStrength.Maximum = 200;

            // The TickFrequency property establishes how many positions 
            // are between each tick-mark.
            trb_forceStrength.TickFrequency = 20;

            // The LargeChange property sets how many positions to move 
            // if the bar is clicked on either side of the slider.
            trb_forceStrength.LargeChange = 2;

            // The SmallChange property sets how many positions to move 
            // if the keyboard arrows are used to move the slider.
            trb_forceStrength.SmallChange = 1;

            return 220 - trb_forceStrength.Value;
        }

        private void UnderlyingTimerTick(object sender, EventArgs e)
        {
            sendData();
            setForces();
        }

        private void ConnectToMasterButtonClick(object sender, EventArgs e)
        {
            //listen for master
            listenerSocket = new ListenerSocket(port, this);
            listenerSocket.StartListening();
            ConnectToMasterButton.Enabled = false;
        }

        private void fillOmniDDL()
        {
            spLeftOmni.DataSource = GetGeomagicDevices();
            spRightOmni.DataSource = GetGeomagicDevices();
        }

        string[] GetGeomagicDevices()
        {
            string[] fileNames = new string[1];
            try
            {
                fileNames = Directory.GetFiles(@"C:\Users\Public\Documents\SensAble\", "*.config");
                for (int i = 0; i < fileNames.Length; i++)
                {
                    fileNames[i] = Path.GetFileNameWithoutExtension(fileNames[i]);
                }
            }
            catch { }
            return fileNames;
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
 
        HomePosition homePosition = new HomePosition();

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
            IntPtr ptr = getpos1();
            double[] pos1 = new double[8];
            Marshal.Copy(ptr, pos1, 0, 8);

            ReleaseMemory(ptr);

            IntPtr ptr2 = getpos2();
            double[] pos2 = new double[8];
            Marshal.Copy(ptr2, pos2, 0, 8);

            ReleaseMemory(ptr2);

            //other left omni
            double remotePos_LX = listenerSocket.SocketMessage.XOmniLeft;
            double remotePos_LY = listenerSocket.SocketMessage.YOmniLeft;
            double remotePos_LZ = listenerSocket.SocketMessage.ZOmniLeft;

            //other right omni
            double remotePos_RX = listenerSocket.SocketMessage.XOmniRight;
            double remotePos_RY = listenerSocket.SocketMessage.YOmniRight;
            double remotePos_RZ = listenerSocket.SocketMessage.ZOmniRight;

            forceOffset_LX = pos1[0] - remotePos_LX;
            forceOffset_LY = pos1[1] - remotePos_LY;
            forceOffset_LZ = pos1[2] - remotePos_LZ;
            forceOffset_RX = pos2[0] - remotePos_RX;
            forceOffset_RY = pos2[1] - remotePos_RY;
            forceOffset_RZ = pos2[2] - remotePos_RZ;
        }

        private void cb_isMaster_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_isMaster.Checked)
            {
                //user wants to be master
                lbl_myIP.Text = "Send to IP Address";
                tb_ipAddress.ReadOnly = false;
                tb_ipAddress.Text = "";
                ConnectToMasterButton.Visible = false;
                cb_forceEnable.Visible = false;
                btn_zeroForces.Visible = false;
                tb_forces.Visible = false;
                groupBox3.Visible = false;
                cb_forceEnable.Checked = false;
                enableForceFeedback = false;
            }
            else
            {
                lbl_myIP.Text = "My IP Address";
                tb_ipAddress.ReadOnly = true;
                tb_ipAddress.Text = GetIP();
                ConnectToMasterButton.Visible = true;
                cb_forceEnable.Visible = true;
                btn_zeroForces.Visible = true;
                tb_forces.Visible = true;
                groupBox3.Visible = true;
            }
        }

    }
}
