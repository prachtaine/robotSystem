using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotControl;
using System.Diagnostics;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RobotApp.ViewModel
{
    [DataContract]
    [Serializable]
    public class MotorViewModel : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// The jointItem vm that this motor belongs to.
        /// </summary>
        /// <summary>
        /// The <see cref="Controller" /> property's name.
        /// </summary>
        public const string JointItemPropertyName = "JointItem";

        [DataMember]
        private Controller controller = null;

        /// <summary>
        /// The <see cref="Kp" /> property's name.
        /// </summary>
        public const string KpPropertyName = "Kp";
        [DataMember]
        private byte kp = 10;

        /// <summary>
        /// The <see cref="AngleSetpoint" /> property's name.
        /// </summary>
        public const string AngleSetpointPropertyName = "AngleSetpoint";
        
        private double angleSetpoint = 0;

        /// <summary>
        /// Sets and gets the AngleSetpoint property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double AngleSetpoint
        {
            get
            {
                return angleSetpoint;
            }

            set
            {
                if (angleSetpoint == value)
                {
                    return;
                }

                angleSetpoint = value;
                Motor.Angle = value;
                if(PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(AngleSetpointPropertyName));
            }
        }

        /// <summary>
        /// Sets and gets the Kp property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public byte Kp
        {
            get
            {
                return kp;
            }

            set
            {
                if (kp == value)
                {
                    return;
                }

                kp = value;
                Motor.Kp = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(KpPropertyName));
            }
        }

        /// <summary>
        /// Sets and gets the JointItem property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Controller Controller
        {
            get
            {
                return controller;
            }

            set
            {
                if (controller == value)
                {
                    return;
                }

                controller = value;
                this.Motor.Controller = controller;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(JointItemPropertyName));
            }
        }

        [DataMember]
        public Motor Motor { get; set; }
        
        [DataMember]
        public ObservableDictionary<string, InputSignalViewModel> Sinks { get; set; }
        
        [NonSerialized]
        private RelayCommand jogForwardUp;

        /// <summary>
        /// Gets the JogForwardUp.
        /// </summary>
        public RelayCommand JogForwardUp
        {
            get
            {
                return jogForwardUp
                    ?? (jogForwardUp = new RelayCommand(
                    () =>
                    {
                        JogDirection = true; JogEnabled = false;
                    }));
            }
        }
        [NonSerialized]
        private RelayCommand jogForwardDown;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand JogForwardDown
        {
            get
            {
                return jogForwardDown
                    ?? (jogForwardDown = new RelayCommand(
                    () =>
                    {
                        JogDirection = true; JogEnabled = true;
                    }));
            }
        }
        [NonSerialized]
        private RelayCommand jogReverseUp;

        /// <summary>
        /// Gets the JogReverseUp.
        /// </summary>
        public RelayCommand JogReverseUp
        {
            get
            {
                return jogReverseUp
                    ?? (jogReverseUp = new RelayCommand(
                    () =>
                    {
                        JogDirection = false; JogEnabled = false;
                    }));
            }
        }
        [NonSerialized]
        private RelayCommand jogReverseDown;

        /// <summary>
        /// Gets the JogReverseDown.
        /// </summary>
        public RelayCommand JogReverseDown
        {
            get
            {
                return jogReverseDown
                    ?? (jogReverseDown = new RelayCommand(
                    () =>
                    {
                        JogDirection = false; JogEnabled = true; 
                    }));
            }
        }

        public void CreateInputs()
        {
            Sinks.Add("AngleSetpoint", new InputSignalViewModel("AngleSetpoint", FriendlyName));
            Sinks.Add("JogForward", new InputSignalViewModel("JogForward", FriendlyName));
            Sinks.Add("JogReverse", new InputSignalViewModel("JogReverse", FriendlyName));
            Sinks.Add("JogSpeed", new InputSignalViewModel("JogSpeed", FriendlyName));
        }


        public void SetupMessenger()
        {
            
            Messenger.Default.Register<Messages.Signal>(this, Sinks["AngleSetpoint"].UniqueID, (msg) =>
            {
                AngleSetpoint = msg.Value;
            });

            
            Messenger.Default.Register<Messages.Signal>(this, Sinks["JogForward"].UniqueID, (msg) =>
            {
                if (msg.Value > 0.5)
                {
                    JogDirection = true;
                    JogEnabled = true;
                }

                else
                    JogEnabled = false;
            });

            
            Messenger.Default.Register<Messages.Signal>(this, Sinks["JogReverse"].UniqueID, (msg) =>
            {

                if (msg.Value > 0.5)
                {
                    JogDirection = false;
                    JogEnabled = true;
                }

                else
                    JogEnabled = false;

            });

            
            Messenger.Default.Register<Messages.Signal>(this, Sinks["JogSpeed"].UniqueID, (msg) =>
            {
                int speed = 0;
                if (msg.Value > 0)
                {
                    if (msg.Value < 255.0)
                    {
                        speed = (int)Math.Round(msg.Value);
                    }
                }

                JogSpeed = speed;
            });
        }

        public MotorViewModel()
        {
            Motor = new Motor();
            Sinks = new ObservableDictionary<string, InputSignalViewModel>();
            JogSpeed = 32;

            CreateInputs();
            SetupMessenger();
            

        }


        public string DisplayName
        {
            get
            {
                return "Motor " + Id + " (" + FriendlyName + ")";
            }
        }

        /// <summary>
        /// The <see cref="FriendlyName" /> property's name.
        /// </summary>
        public const string FriendlyNamePropertyName = "FriendlyName";

        [DataMember]
        private string friendlyName = "";

        /// <summary>
        /// Sets and gets the FriendlyName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string FriendlyName
        {
            get
            {
                return friendlyName;
            }

            set
            {
                if (friendlyName == value)
                {
                    return;
                }

                friendlyName = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(FriendlyNamePropertyName));
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("DisplayName"));

                // We have to tell our signal manager what our new name is
                foreach(var sink in Sinks)
                {
                    sink.Value.ParentInstanceName = FriendlyName;
                }
            }
        }

        private void UpdateJogState()
        {
            if(JogEnabled)
            {
                Motor.Jog(JogSpeed, JogDirection);
            }
            else
            {
                Motor.Jog(0, false);
            }
        }
        [DataMember]
        private int jogSpeed;
        public int JogSpeed 
        { 
            get
            {
             return jogSpeed;   
            }
            set
            {
                if (value == jogSpeed)
                    return;
                jogSpeed = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("JogSpeed"));
                UpdateJogState();
            }
        }

        /// <summary>
        /// Sets and gets the EncoderCountsPerRevolution property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        [DataMember]
        private double encoderCountsPerRevolution;

        public double EncoderCountsPerRevolution
        {
            get
            {
                return encoderCountsPerRevolution;
            }

            set
            {
                if (encoderCountsPerRevolution == value)
                {
                    return;
                }
                encoderCountsPerRevolution = value;
                Motor.EncoderClicksPerRevolution = encoderCountsPerRevolution;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("EncoderCountsPerRevolution"));
            }
        }
        
        /// <summary>
        /// The <see cref="JogDirection" /> property's name.
        /// </summary>
        public const string JogDirectionPropertyName = "JogDirection";

        private bool jogDirection = false;

        /// <summary>
        /// Sets and gets the JogDirection property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool JogDirection
        {
            get
            {
                return jogDirection;
            }

            set
            {
                if (jogDirection == value)
                {
                    return;
                }

                jogDirection = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(JogDirectionPropertyName));
                UpdateJogState();
            }
        }
        
        /// <summary>
        /// The <see cref="JogEnabled" /> property's name.
        /// </summary>
        public const string JogEnabledPropertyName = "JogEnabled";

        private bool jogEnabled = false;

        /// <summary>
        /// Sets and gets the JogEnabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool JogEnabled
        {
            get
            {
                return jogEnabled;
            }

            set
            {
                if (jogEnabled == value)
                {
                    return;
                }

                jogEnabled = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(JogEnabledPropertyName));
                UpdateJogState();
            }
        }
        [DataMember]
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value;
            Motor.Index = id;
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }

        [DataMember]
        private int controlModeIndex;

        public int ControlModeIndex
        {
            get
            {
                return controlModeIndex;
            }
            set
            {
                controlModeIndex = value;
                Motor.ControlMode = (ControlMode)(controlModeIndex);
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("ControlModeIndex"));
            }
        }

        public void Dispose()
        {
            // Let the SignalSinkRegistry know that we're going bye-bye!
            foreach(var item in Sinks)
            {
                Messenger.Default.Send<Messages.UnregisterSignalSink>(new Messages.UnregisterSignalSink() { Sink = item.Value });
            }
            
        }
        [field: NonSerializedAttribute()] 
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
