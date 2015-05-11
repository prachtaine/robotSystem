using GalaSoft.MvvmLight.Command;
using RobotApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using GeomagicTouch;
using GalaSoft.MvvmLight.Messaging;


namespace RobotApp.Views.Plugins
{
    /// <summary>
    /// Interaction logic for HapticWorkspace.xaml
    /// </summary>
    public partial class HapticWorkspace : PluginBase
    {
        private double x, y, z;
        private double setX, setY, setZ;
        private double pauseX, pauseY, pauseZ;
        //private double clutchInX = 0;
        //private double clutchInY = 0; 
        //private double clutchInZ = 0;
        //private double clutchX = 0;
        //private double clutchY = 0;
        //private double clutchZ = 0;
        private double forceX, forceY, forceZ;
        private bool pause = false;
        //private bool clutch = false;

        public override void PostLoadSetup()
        {
            Messenger.Default.Register<Messages.Signal>(this, Inputs["X"].UniqueID, (message) =>
            {
                //if (armSide == 2)
                //    x = -message.Value;
                //else
                    x = message.Value;
                if (armSide != 0)
                {
                    Barriers();
                    UpdateOutput();
                }
            });

            Messenger.Default.Register<Messages.Signal>(this, Inputs["Y"].UniqueID, (message) =>
            {
                y = message.Value;
                if (armSide != 0)
                {
                    Barriers();
                    UpdateOutput();
                }
            });

            Messenger.Default.Register<Messages.Signal>(this, Inputs["Z"].UniqueID, (message) =>
            {
                z = message.Value;
                if (armSide != 0)
                {
                    Barriers();
                    UpdateOutput();
                }
            });

            Messenger.Default.Register<Messages.Signal>(this, Inputs["HapticsPause"].UniqueID, (message) =>
            {
                if ((pause == true) && (message.Value == 1))
                    pause = false;
                else if ((pause == false) && (message.Value == 1))
                {
                    pause = true;
                    pauseX = x;
                    pauseY = y;
                    pauseZ = z;
                }
                if (armSide != 0)
                {
                    Barriers();
                    UpdateOutput();
                }
            });

            //Messenger.Default.Register<Messages.Signal>(this, Inputs["HapticsClutch"].UniqueID, (message) =>
            //{
            //    if ((clutch == true) && (message.Value == 1))
            //    {
            //        clutch = false;
            //        clutchX = clutchInX - x;
            //        clutchY = clutchInY - y;
            //        clutchZ = clutchInZ - z;
            //    }
            //    else if ((clutch == false) && (message.Value == 1))
            //    {
            //        clutch = true;
            //        clutchInX = x;
            //        clutchInY = y;
            //        clutchInZ = z;
            //    }
            //});


            base.PostLoadSetup();
        }

        public HapticWorkspace()
        {
            this.TypeName = "Haptic Workspace";
            InitializeComponent();

            Outputs.Add("SetX", new ViewModel.OutputSignalViewModel("SetX"));
            Outputs.Add("SetY", new ViewModel.OutputSignalViewModel("SetY"));
            Outputs.Add("SetZ", new ViewModel.OutputSignalViewModel("SetZ"));
            Outputs.Add("ForceX", new ViewModel.OutputSignalViewModel("ForceX"));
            Outputs.Add("ForceY", new ViewModel.OutputSignalViewModel("ForceY"));
            Outputs.Add("ForceZ", new ViewModel.OutputSignalViewModel("ForceZ"));
            Outputs.Add("EnableHaptics", new ViewModel.OutputSignalViewModel("EnableHaptics"));

            Inputs.Add("X", new ViewModel.InputSignalViewModel("X", this.InstanceName));
            Inputs.Add("Y", new ViewModel.InputSignalViewModel("Y", this.InstanceName));
            Inputs.Add("Z", new ViewModel.InputSignalViewModel("Z", this.InstanceName));
            Inputs.Add("HapticsPause", new ViewModel.InputSignalViewModel("HapticsPause", this.InstanceName));
            //Inputs.Add("HapticsClutch", new ViewModel.InputSignalViewModel("HapticsClutch", this.InstanceName));

            PostLoadSetup();
        }

        public void UpdateOutput()
        {
            Outputs["SetX"].Value = setX;
            Outputs["SetY"].Value = setY;
            Outputs["SetZ"].Value = setZ;

            Outputs["ForceX"].Value = forceX;
            Outputs["ForceY"].Value = forceY;
            Outputs["ForceZ"].Value = forceZ;
            
            if(hapticsEnabled == true)
                Outputs["EnableHaptics"].Value = 1;
            else
                Outputs["EnableHaptics"].Value = 0;
        }

        public void Barriers()
        {
            double Lmax = upperLength + foreLength;
            double minShoulderRadians = minShoulderTheta * Math.PI / 180;
            double maxShoulderRadians = maxShoulderTheta * Math.PI / 180;
            double minElbowRadians = minElbowTheta * Math.PI / 180;
            double maxElbowRadians = maxElbowTheta * Math.PI / 180;
            double radius;
            double setScale;
            double c = upperLength * Math.Sin(maxShoulderRadians);
            double xShiftOne = upperLength * Math.Cos(maxShoulderRadians);
            double torusOneTheta = Math.Atan(y / z);
            double yShiftOne = c * Math.Sin(torusOneTheta);
            double zShiftOne = c * Math.Cos(torusOneTheta);
            double torusTwoTheta = Math.Atan(y / x);
            double xShiftTwo = upperLength * Math.Cos(torusTwoTheta);
            double yShiftTwo = upperLength * Math.Sin(torusTwoTheta);
            double a = foreLength;
            double torusOneR, torusTwoR;
            double xMin = upperLength * Math.Sin(maxElbowRadians) - foreLength;
            double sphereOneLimit = Lmax * Math.Sin(maxShoulderRadians);
            double sphereThreeX = upperLength + Math.Cos(maxElbowRadians) * foreLength;
            double sphereThreeY = Math.Sin(maxElbowRadians) * foreLength;
            double sphereTwoR = Math.Sqrt(Math.Pow(sphereThreeX, 2) + Math.Pow(sphereThreeY, 2));
            double phi = Math.Tan(z / x);

            // Setpoints start on input coordinates
            setX = x;
            setY = y;
            setZ = z;

            radius = Math.Sqrt(Math.Pow(setX, 2) + Math.Pow(setY, 2) + Math.Pow(setZ, 2));
            torusOneR = Math.Sqrt(Math.Pow(c - Math.Sqrt(Math.Pow(setY, 2) + Math.Pow(setZ, 2)), 2) + Math.Pow((setX - xShiftOne), 2));
            torusTwoR = Math.Sqrt(Math.Pow(upperLength - Math.Sqrt(Math.Pow(setX, 2) + Math.Pow(setY, 2)), 2) + Math.Pow(setZ, 2));
            // Outside barriers
            if ((radius > Lmax) && (x <= sphereOneLimit))   // Sphere 1 
            {
                setScale = Lmax / radius;
                setX = setX * setScale;
                setY = setY * setScale;
                setZ = setZ * setScale;
            }
            if ((torusOneR > a) && (x > sphereOneLimit) && (radius > sphereTwoR))  // Torus 1
            {
                setScale = a / torusOneR;
                setX = ((setX + xShiftOne) * setScale) - xShiftOne;
                setY = ((setY - yShiftOne) * setScale) + yShiftOne;
                setZ = ((setZ - zShiftOne) * setScale) + zShiftOne;
            }
            //if ((torusTwoR < a) && (x < 0) && (x > -25))
            //    setX = 0;
            if ((torusTwoR < a) && (x <= -25))  // Torus 2
            {
                setScale = a / torusTwoR;
                setX = ((setX + xShiftTwo) * setScale) - xShiftTwo;
                setY = ((setY + yShiftTwo) * setScale) - yShiftTwo;
                setZ = setZ * setScale;
            }
            // Sphere 3 force inside outward
            radius = Math.Sqrt(Math.Pow(x, 2) + Math.Pow((y + upperLength), 2) + Math.Pow(z, 2));
            if ((radius < foreLength) && (y < -35.56) && (x > -25))
            {
                setScale = foreLength / radius;
                setX = x * setScale;
                setY = ((y + upperLength) * setScale) - upperLength;
                setZ = z * setScale;
            }
            // Shpere 2, force inside outward
            radius = Math.Sqrt(Math.Pow(setX, 2) + Math.Pow(setY, 2) + Math.Pow(setZ, 2));
            if ((radius < sphereTwoR) && (y >= -35.56) && (x > -25))
            {
                setScale = sphereTwoR / radius;
                setX = x * setScale;
                setY = y * setScale;
                setZ = z * setScale;
            }
            // Right Wall
            //if (x > Lmax)
            //    setX = Lmax;
            // Left Wall
            //if (setX < (xShift + foreLength))
            //    setX = xShift + foreLength;
            // Ceiling
            if (setY > 0)
                setY = 0;
            // Back Wall
            if (setZ < 0)
                setZ = 0;

            if(pause == true)
            {
                setX = pauseX;
                setY = pauseY;
                setZ = pauseZ;
            }
            //else if(clutch == true)
            //{
            //    setX = clutchInX;
            //    setY = clutchInY;
            //    setZ = clutchInZ;
            //}

            //forceX = Math.Pow((setX - x), 3) * forceGain;
            //forceY = Math.Pow((setY - y), 3) * forceGain;
            //forceZ = Math.Pow((z - setZ), 3) * forceGain;
            if (armSide == 2)
            {
               forceX = (setX - x) * forceGain;
            }
            else
                forceX = (setX - x) * -forceGain;
            forceY = (setY - y) * forceGain;
            forceZ = (setZ - z) * -forceGain;

            //if(clutch == true)
            //{
            //    forceX = 0;
            //    forceY = 0;
            //    forceZ = 0;
            //}
        }

        /// <summary>
        /// The <see cref="ArmSide" /> property's name.
        /// </summary>
        public const string ArmSidePropertyName = "ArmSide";

        private int armSide = 0;

        /// <summary>
        /// Sets and gets the ArmSide property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int ArmSide
        {
            get
            {
                return armSide;
            }

            set
            {
                if (armSide == value)
                {
                    return;
                }

                armSide = value;
                RaisePropertyChanged(ArmSidePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="UpperLength" /> property's name.
        /// </summary>
        public const string UpperLengthPropertyName = "UpperLength";

        private double upperLength = 68.58;

        /// <summary>
        /// Sets and gets the UpperLength property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double UpperLength
        {
            get
            {
                return upperLength;
            }

            set
            {
                if (upperLength == value)
                {
                    return;
                }

                upperLength = value;
                RaisePropertyChanged(UpperLengthPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ForeLength" /> property's name.
        /// </summary>
        public const string ForeLengthPropertyName = "ForeLength";

        private double foreLength = 96.393;

        /// <summary>
        /// Sets and gets the ForeLength property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ForeLength
        {
            get
            {
                return foreLength;
            }

            set
            {
                if (foreLength == value)
                {
                    return;
                }

                foreLength = value;
                RaisePropertyChanged(ForeLengthPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MinShoulderTheta" /> property's name.
        /// </summary>
        public const string MinShoulderThetaPropertyName = "MinShoulderTheta";

        private double minShoulderTheta = 0;

        /// <summary>
        /// Sets and gets the MinShoulderTheta property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MinShoulderTheta
        {
            get
            {
                return minShoulderTheta;
            }

            set
            {
                if (minShoulderTheta == value)
                {
                    return;
                }

                minShoulderTheta = value;
                RaisePropertyChanged(MinShoulderThetaPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MaxShoulderTheta" /> property's name.
        /// </summary>
        public const string MaxShoulderThetaPropertyName = "MaxShoulderTheta";

        private double maxShoulderTheta = 90;

        /// <summary>
        /// Sets and gets the MaxShoulderTheta property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MaxShoulderTheta
        {
            get
            {
                return maxShoulderTheta;
            }

            set
            {
                if (maxShoulderTheta == value)
                {
                    return;
                }

                maxShoulderTheta = value;
                RaisePropertyChanged(MaxShoulderThetaPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MinElbowTheta" /> property's name.
        /// </summary>
        public const string MinElbowThetaPropertyName = "MinElbowTheta";

        private double minElbowTheta = 0;

        /// <summary>
        /// Sets and gets the MinElbowTheta property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MinElbowTheta
        {
            get
            {
                return minElbowTheta;
            }

            set
            {
                if (minElbowTheta == value)
                {
                    return;
                }

                minElbowTheta = value;
                RaisePropertyChanged(MinElbowThetaPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MaxElbowTheta" /> property's name.
        /// </summary>
        public const string MaxElbowThetaPropertyName = "MaxElbowTheta";

        private double maxElbowTheta = 100;

        /// <summary>
        /// Sets and gets the MaxElbowTheta property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MaxElbowTheta
        {
            get
            {
                return maxElbowTheta;
            }

            set
            {
                if (maxElbowTheta == value)
                {
                    return;
                }

                maxElbowTheta = value;
                RaisePropertyChanged(MaxElbowThetaPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="HapticsEnabled" /> property's name.
        /// </summary>
        public const string HapticsEnabledPropertyName = "HapticsEnabled";

        private bool hapticsEnabled = false;

        /// <summary>
        /// Sets and gets the HapticsEnabled property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool HapticsEnabled
        {
            get
            {
                return hapticsEnabled;
            }

            set
            {
                if (hapticsEnabled == value)
                {
                    return;
                }

                hapticsEnabled = value;
                RaisePropertyChanged(HapticsEnabledPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ForceGain" /> property's name.
        /// </summary>
        public const string ForceGainPropertyName = "ForceGain";

        private double forceGain = 0.1;

        /// <summary>
        /// Sets and gets the ForceGain property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double ForceGain
        {
            get
            {
                return forceGain;
            }

            set
            {
                if (forceGain == value)
                {
                    return;
                }

                forceGain = value;
                RaisePropertyChanged(ForceGainPropertyName);
            }
        }

    }
}
