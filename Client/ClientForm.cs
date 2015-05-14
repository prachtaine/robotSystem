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


        ListenerSocket dataListenerSocket = null;
        List<string> slaveIPAddresses = new List<string>();
        bool masterInitialized = false;
        int hsPort = 11000; //handshake port
        int dataPort = 12000;
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
            fillOmniDDL();

            //Set Force Trackbar
            //want force divider between 20 and 220
            //divider = 220 - trackbarValue 
            trb_forceStrength.Minimum = 0;
            trb_forceStrength.Maximum = 200;
            // The TickFrequency property establishes how many positions are between each tick-mark.
            trb_forceStrength.TickFrequency = 20;
            // The LargeChange property sets how many positions to move if the bar is clicked on either side of the slider.
            trb_forceStrength.LargeChange = 2;
            // The SmallChange property sets how many positions to move if the keyboard arrows are used to move the slider.
            trb_forceStrength.SmallChange = 1;
            //set initial value of trackbar
            trb_forceStrength.Value = 170;
        }

        private void InitializeOmnis_Click(object sender, EventArgs e)
        {
            if (cb_noOmnisAttached.Checked)
            {
                cb_noOmnisAttached.Enabled = false;
                btStop.Enabled = false;
                UnderlyingTimer.Enabled = true;
            }
            else if (spRightOmni.SelectedIndex == -1 && spLeftOmni.SelectedIndex == -1)
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
                    masterInitialized = true;
                    //start listening for any clients
                    ListenerSocket newClientListener = new ListenerSocket(hsPort, this);
                    newClientListener.StartListening();
                }
            }
            //do not allow switching between master and slave state once omnis are initialized
            cb_isMaster.Enabled = false;
        }

        private void sendData()
        {
            Coordinate oldPosition = new Coordinate(0, 0, 0, 0, 0, 0);
            double[] pos1 = { 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] pos2 = { 0, 0, 0, 0, 0, 0, 0, 0 };

            if (!cb_noOmnisAttached.Checked)
            {
                IntPtr ptr = getpos1();
                Marshal.Copy(ptr, pos1, 0, 8);
                ReleaseMemory(ptr);

                IntPtr ptr2 = getpos2();
                Marshal.Copy(ptr2, pos2, 0, 8);
                ReleaseMemory(ptr2);

                //home the position
                oldPosition = new Coordinate(pos1[0], pos1[1], pos1[2], pos2[0], pos2[1], pos2[2]);
                //Coordinate homedPosition = homePosition.getPosition(oldPosition);
                //Coordinate leftPositionWithBoundaries = RobotBarriers.GetBoundaryCoordinates(homedPosition);


                //build messageToSend if master has been initialized
                if (masterInitialized)
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

                    messageToSend.XOmniRight = oldPosition.rightX;
                    messageToSend.YOmniRight = oldPosition.rightY;
                    messageToSend.ZOmniRight = oldPosition.rightZ;

                    messageToSend.Gimbal1OmniRight = pos2[3];
                    messageToSend.Gimbal2OmniRight = pos2[4];
                    messageToSend.Gimbal3OmniRight = pos2[5];

                    messageToSend.ButtonsRight = pos2[6];
                    messageToSend.InkwellRight = pos2[7];

                    //finally send the built message to all connected slaves
                    try
                    {
                        foreach (string address in slaveIPAddresses)
                        {
                            TalkerSocket ts = new TalkerSocket(address, dataPort);
                            ts.sendData(messageToSend);
                        }
                    }
                    catch (Exception)
                    {
                        //assume a client has disconnected
                    }
                }
            }

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
        }

        private void setForces()
        {
            double forceLX = 0;
            double forceLY = 0;
            double forceLZ = 0;
            double forceRX = 0;
            double forceRY = 0;
            double forceRZ = 0;

            if (enableForceFeedback && dataListenerSocket != null)
            {
                IntPtr ptr = getpos1();
                double[] pos1 = new double[8];
                Marshal.Copy(ptr, pos1, 0, 8);

                ReleaseMemory(ptr);

                IntPtr ptr2 = getpos2();
                double[] pos2 = new double[8];
                Marshal.Copy(ptr2, pos2, 0, 8);

                ReleaseMemory(ptr2);

                forceLX = (dataListenerSocket.SocketMessage.XOmniLeft - (pos1[0] - forceOffset_LX)) / getForceStrength();
                forceLY = (dataListenerSocket.SocketMessage.YOmniLeft - (pos1[1] - forceOffset_LY)) / getForceStrength();
                forceLZ = (dataListenerSocket.SocketMessage.ZOmniLeft - (pos1[2] - forceOffset_LZ)) / getForceStrength();
                forceRX = (dataListenerSocket.SocketMessage.XOmniRight - (pos2[0] - forceOffset_RX)) / getForceStrength();
                forceRY = (dataListenerSocket.SocketMessage.YOmniRight - (pos2[1] - forceOffset_RY)) / getForceStrength();
                forceRZ = (dataListenerSocket.SocketMessage.ZOmniRight - (pos2[2] - forceOffset_RZ)) / getForceStrength();
            }

            if (cb_noOmnisAttached.Checked)
            {
                if (dataListenerSocket != null)
                { 
                    forceLX = dataListenerSocket.SocketMessage.XOmniLeft;
                    forceLY = dataListenerSocket.SocketMessage.YOmniLeft; 
                    forceLZ = dataListenerSocket.SocketMessage.ZOmniLeft; 
                    forceRX = dataListenerSocket.SocketMessage.XOmniRight;
                    forceRY = dataListenerSocket.SocketMessage.YOmniRight;
                    forceRZ = dataListenerSocket.SocketMessage.ZOmniRight;
                }
                tb_forces.Text = @"Left" + Environment.NewLine + "X = " + forceLX + " Y = " + forceLY + " Z = " + forceLZ + Environment.NewLine + "Right" + Environment.NewLine + "X = " + forceRX + " Y = " + forceRY + " Z = " + forceRZ;
            }
            else
            {
                setForce1(forceLX, forceLY, forceLZ);
                setForce2(forceRX, forceRY, forceRZ);
                tb_forces.Text = @"Left" + Environment.NewLine + "X = " + forceLX + " Y = " + forceLY + " Z = " + forceLZ + Environment.NewLine + "Right" + Environment.NewLine + "X = " + forceRX + " Y = " + forceRY + " Z = " + forceRZ;
            }
        }

        private int getForceStrength()
        {
            return 220 - trb_forceStrength.Value;
        }

        private void UnderlyingTimerTick(object sender, EventArgs e)
        {
            sendData();
            setForces();
        }

        private void ConnectToMasterButtonClick(object sender, EventArgs e)
        {
            //Send IP Address to master
            TalkerSocket ts = new TalkerSocket(tb_ipAddress.Text, hsPort);
            SocketMessage sMessage = new SocketMessage();
            sMessage.MessageType = "Handshake";
            sMessage.IpAddress = GetIP();
            ts.sendData(sMessage);

            //listen for response from master
            ListenerSocket responseListener = new ListenerSocket(hsPort, this);
            responseListener.StartListening();

            ConnectToMasterButton.Enabled = false;
        }

        internal void handshakeReceived(SocketMessage message)
        {
            if (cb_isMaster.Checked)
            {
                //save slave address
                slaveIPAddresses.Add(message.IpAddress);
                //send success message back
                TalkerSocket ts = new TalkerSocket(message.IpAddress, hsPort);
                SocketMessage returnMessage = new SocketMessage();
                returnMessage.MessageType = "Handshake";
                returnMessage.IpAddress = GetIP();
                ts.sendData(returnMessage);
            }
            else
            {
                dataListenerSocket = new ListenerSocket(dataPort, this);
                dataListenerSocket.StartListening();
                
                MessageBox.Show("Connection Successful!");
            }
        }

        private void fillOmniDDL()
        {
            if (!cb_noOmnisAttached.Checked)
            {
                spLeftOmni.DataSource = GetGeomagicDevices();
                spRightOmni.DataSource = GetGeomagicDevices();
            }
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

        private void cb_forceEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_forceEnable.Checked && !cb_isMaster.Checked && ConnectToMasterButton.Enabled)
            {
                MessageBox.Show("There is no connection to a master!");
                cb_forceEnable.Checked = false;
            }
            else
            {
                enableForceFeedback = cb_forceEnable.Checked;
            }
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
            double remotePos_LX = dataListenerSocket.SocketMessage.XOmniLeft;
            double remotePos_LY = dataListenerSocket.SocketMessage.YOmniLeft;
            double remotePos_LZ = dataListenerSocket.SocketMessage.ZOmniLeft;

            //other right omni
            double remotePos_RX = dataListenerSocket.SocketMessage.XOmniRight;
            double remotePos_RY = dataListenerSocket.SocketMessage.YOmniRight;
            double remotePos_RZ = dataListenerSocket.SocketMessage.ZOmniRight;

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
                lbl_myIP.Text = "My IP Address";
                tb_ipAddress.ReadOnly = true;
                tb_ipAddress.Text = GetIP();
                ConnectToMasterButton.Visible = false;
                cb_forceEnable.Visible = false;
                btn_zeroForces.Visible = false;
                tb_forces.Visible = false;
                groupBox3.Visible = false;
                cb_forceEnable.Checked = false;
                enableForceFeedback = false;
                lbl_forceStrength.Visible = false;
                trb_forceStrength.Visible = false;
            }
            else
            {
                lbl_myIP.Text = "Master's IP Address";
                tb_ipAddress.ReadOnly = false;
                tb_ipAddress.Text = "";
                ConnectToMasterButton.Visible = true;
                cb_forceEnable.Visible = true;
                btn_zeroForces.Visible = true;
                tb_forces.Visible = true;
                groupBox3.Visible = true;
                lbl_forceStrength.Visible = true;
                trb_forceStrength.Visible = true;
            }
        }

        private void cb_noOmnisAttached_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_noOmnisAttached.Checked)
            {
                cb_isMaster.Checked = false;
                cb_isMaster.Enabled = false;
                enableForceFeedback = false;
                cb_forceEnable.Enabled = false;
                groupBox3.Text = "Master's Position";
                btn_zeroForces.Enabled = false;
                lbl_myIP.Text = "Master's IP Address";
                tb_ipAddress.ReadOnly = false;
                tb_ipAddress.Text = "";
                ConnectToMasterButton.Visible = true;
                cb_forceEnable.Visible = true;
                btn_zeroForces.Visible = true;
                tb_forces.Visible = true;
                groupBox3.Visible = true;
                lbl_forceStrength.Visible = true;
                trb_forceStrength.Visible = true;
            }
            else
            {
                cb_isMaster.Enabled = true;
                cb_forceEnable.Enabled = true;
                groupBox3.Text = "Forces";
                btn_zeroForces.Enabled = true;
            }
        }

    }
}
