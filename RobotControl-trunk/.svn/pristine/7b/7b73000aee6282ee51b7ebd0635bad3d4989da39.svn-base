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
    public partial class JointLimits : PluginBase
    {

        public override void PostLoadSetup()
        {
            Messenger.Default.Register<Messages.Signal>(this, Inputs["UpperBevelSet"].UniqueID, (message) =>
            {
                if (message.Value > maxShoulderDeg)
                    Outputs["UpperBevelLimit"].Value = maxShoulderDeg;
                else if(message.Value < minShoulderDeg)
                    Outputs["UpperBevelLimit"].Value = minShoulderDeg;
                else
                    Outputs["UpperBevelLimit"].Value = message.Value;
            });

            base.PostLoadSetup();
        }

        public JointLimits()
        {
            this.TypeName = "Joint Limits";
            InitializeComponent();

            Outputs.Add("UpperBevelLimit", new ViewModel.OutputSignalViewModel("UpperBevelLimit"));
            Outputs.Add("LowerBevelLimit", new ViewModel.OutputSignalViewModel("LowerBevelLimit"));
            Outputs.Add("ElbowLimit", new ViewModel.OutputSignalViewModel("ElbowLimit"));

            Inputs.Add("UpperBevelSet", new ViewModel.InputSignalViewModel("UpperBevelSet", this.InstanceName));
            Inputs.Add("LowerBevelSet", new ViewModel.InputSignalViewModel("LowerBevelSet", this.InstanceName));
            Inputs.Add("ElbowSet", new ViewModel.InputSignalViewModel("ElbowSet", this.InstanceName));

            PostLoadSetup();
        }

        /// <summary>
        /// The <see cref="LimitsEnable" /> property's name.
        /// </summary>
        public const string LimitsEnablePropertyName = "LimitsEnable";

        private bool limitsEnable = false;

        /// <summary>
        /// Sets and gets the LimitsEnable property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool LimitsEnable
        {
            get
            {
                return limitsEnable;
            }

            set
            {
                if (limitsEnable == value)
                {
                    return;
                }

                limitsEnable = value;
                RaisePropertyChanged(LimitsEnablePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MinShoulderDeg" /> property's name.
        /// </summary>
        public const string MinShoulderDegPropertyName = "MinShoulderDeg";

        private double minShoulderDeg = -75;

        /// <summary>
        /// Sets and gets the MinShoulderDeg property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MinShoulderDeg
        {
            get
            {
                return minShoulderDeg;
            }

            set
            {
                if (minShoulderDeg == value)
                {
                    return;
                }

                minShoulderDeg = value;
                RaisePropertyChanged(MinShoulderDegPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MaxShoulderDeg" /> property's name.
        /// </summary>
        public const string MaxShoulderDegPropertyName = "MaxShoulderDeg";

        private double maxShoulderDeg = 0;

        /// <summary>
        /// Sets and gets the MaxShoulderDeg property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MaxShoulderDeg
        {
            get
            {
                return maxShoulderDeg;
            }

            set
            {
                if (maxShoulderDeg == value)
                {
                    return;
                }

                maxShoulderDeg = value;
                RaisePropertyChanged(MaxShoulderDegPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MinElbowDeg" /> property's name.
        /// </summary>
        public const string MinElbowDegPropertyName = "MinElbowDeg";

        private double minElbowDeg = 0;

        /// <summary>
        /// Sets and gets the MinElbowDeg property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MinElbowDeg
        {
            get
            {
                return minElbowDeg;
            }

            set
            {
                if (minElbowDeg == value)
                {
                    return;
                }

                minElbowDeg = value;
                RaisePropertyChanged(MinElbowDegPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="MaxElbowDeg" /> property's name.
        /// </summary>
        public const string MaxElbowDegPropertyName = "MaxElbowDeg";

        private double maxElbowDeg = 110;

        /// <summary>
        /// Sets and gets the MaxElbowDeg property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double MaxElbowDeg
        {
            get
            {
                return maxElbowDeg;
            }

            set
            {
                if (maxElbowDeg == value)
                {
                    return;
                }

                maxElbowDeg = value;
                RaisePropertyChanged(MaxElbowDegPropertyName);
            }
        }

    }
}
