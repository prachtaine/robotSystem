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


namespace RobotApp.Views.Plugins
{
    /// <summary>
    /// Interaction logic for HapticWorkspace.xaml
    /// </summary>
    public partial class SphereBarrier : PluginBase
    {
        private double x, y, z;
        private double setX, setY, setZ;
        private double forceX, forceY, forceZ;


        public SphereBarrier()
        {
            this.TypeName = "Sphere Barrier";
            InitializeComponent();

            Outputs.Add("SetX", new ViewModel.OutputSignalViewModel("SetX"));
            Outputs.Add("SetY", new ViewModel.OutputSignalViewModel("SetY"));
            Outputs.Add("SetZ", new ViewModel.OutputSignalViewModel("SetZ"));
            Outputs.Add("EnableHaptics", new ViewModel.OutputSignalViewModel("EnableHaptics"));

            //Inputs.Add("X", new ViewModel.InputSignalViewModel("X", this.InstanceName,
            //    (value) =>
            //    {
            //        x = value;
            //        Barriers();
            //        UpdateOutput();
            //    }));

            //Inputs.Add("Y", new ViewModel.InputSignalViewModel("Y", this.InstanceName,
            //    (value) =>
            //    {
            //        y = value;
            //        Barriers();
            //        UpdateOutput();
            //    }));

            //Inputs.Add("Z", new ViewModel.InputSignalViewModel("Z", this.InstanceName,
            //    (value) =>
            //    {
            //        z = value;
            //        Barriers();
            //        UpdateOutput();
            //    }));
        }

        public void UpdateOutput()
        {
                Outputs["SetX"].Value = setX;
                Outputs["SetY"].Value = setY;
                Outputs["SetZ"].Value = setZ;
            if (hapticsEnabled == true)
                Outputs["EnableHaptics"].Value = 1;
            else
                Outputs["EnableHaptics"].Value = 0;
        }

        public void Barriers()
        {
            double Lmax = 50;
            double radius;
            double setScale;
//            double gain = -.1;

            // Setpoints start on input coordinates
            setX = x;
            setY = y;
            setZ = z;

            radius = Math.Sqrt(Math.Pow(setX, 2) + Math.Pow(setY, 2) + Math.Pow(setZ, 2));
            // Outside barriers
            if (radius > Lmax)   // Sphere 1
            {
                setScale = Lmax / radius;
                setX = setX * setScale;
                setY = setY * setScale;
                setZ = setZ * setScale;
            }

//            if (setY > 0)
//                setY = 0;
            
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

    }
}
