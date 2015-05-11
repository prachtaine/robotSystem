using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RobotControl
{
    public enum JointCommands
    {
        Reserved,
        Ping,
        HomeUp,
        HomeDown,
        MoveTo,
        GetStatus,
        Configure,
        Associate,
        IdentifyLed,
        Jog,
        ResetCounters
    }

    public delegate void OnPingEventHandler(Controller sender);

    [Serializable]
    public class Controller
    {
        #region Events

        public event OnPingEventHandler OnPing;
        
        #endregion

        #region Properties

        private bool identificationLedIsEnabled;

        public bool IdentificationLedIsEnabled
        {
            get { return identificationLedIsEnabled; }
            set {
                if (identificationLedIsEnabled != value)
                {
                    identificationLedIsEnabled = value;
                    if(identificationLedIsEnabled == true)
                        robot.SendCommand(JointCommands.IdentifyLed, this, new byte[] { 1 });
                    else
                        robot.SendCommand(JointCommands.IdentifyLed, this, new byte[] { 0 });
                }
                
            
            }
        }

        public string FriendlyName { get; set; }

        [IgnoreDataMember()]
        [NonSerialized]
        private Robot robot;

        /// <summary>
        /// The <see cref="Robot"/> this joint belongs to
        /// </summary>
        [IgnoreDataMember()]
        public Robot Robot
        {
            get { return robot; }
            set { robot = value; }
        }

        private uint id;

        public uint Id
        {
            get { return id; }
            set { id = value;  }
        }
        #endregion

        #region Commands

        public void UpdateConfiguration()
        {
            //byte[] configuration = new byte[] {
            //    (byte)motor1.ControlMode,
            //    (byte)motor2.ControlMode,
            //    motor1.Kp,
            //    motor2.Kp
            //};
            //Robot.SendCommand(JointCommands.Configure, this, configuration);
        }

        public void Ping()
        {
            robot.SendCommand(JointCommands.Ping, this);
        }

        public async Task GetStatus()
        {
            byte[] response = await robot.RequestData(JointCommands.GetStatus, this);
            byte motor1_controlMode = response[5];
            byte motor2_controlMode = response[6];
            int motor1_shaftCounter = BitConverter.ToInt32(response, 8);
            int motor2_shaftCounter = BitConverter.ToInt32(response, 12);
            ushort motor1_pot = BitConverter.ToUInt16(response, 16);
            ushort motor1_current = BitConverter.ToUInt16(response, 18);
            ushort motor2_pot = BitConverter.ToUInt16(response, 20);
            ushort motor2_current = BitConverter.ToUInt16(response, 22);
        }



        #endregion

        #region Internal methods
        /// <summary>
        /// Call this function with any response data (including the address data) that's received from this joint.
        /// </summary>
        /// <param name="response"></param>
        internal void ParseResponse(byte[] response)
        {
            
        }

        internal void RaiseOnPing()
        {
            if (OnPing != null)
                OnPing(this);
        }

        #endregion
    }
}
