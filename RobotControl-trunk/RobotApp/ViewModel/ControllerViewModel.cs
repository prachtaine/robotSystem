using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotControl;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Media;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ComponentModel;
namespace RobotApp.ViewModel
{
    [DataContract]
    [Serializable]
    public class ControllerViewModel : INotifyPropertyChanged
    {
        [DataMember]
        public Controller Controller {get; set;}

        [DataMember]
        public ObservableCollection<MotorViewModel> Motors { get; set; }

        int counter = 0;

        public ControllerViewModel()
        {
            
        }

        public ControllerViewModel(Controller controller)
        {
            Controller = controller;
            Motors = new ObservableCollection<MotorViewModel>();
        }

        [IgnoreDataMember]
        [NonSerialized]
        private RelayCommand deleteMotorCommand;

        /// <summary>
        /// Gets the DeleteMotorCommand.
        /// </summary>
        [IgnoreDataMember]
        public RelayCommand DeleteMotorCommand
        {
            get
            {
                return deleteMotorCommand
                    ?? (deleteMotorCommand = new RelayCommand(
                    () =>
                    {
                        if (!DeleteMotorCommand.CanExecute(null))
                        {
                            return;
                        }

                        Motors.Last().Dispose();
                        Motors.Remove(Motors.Last());
                        counter--;
                    },
                    () => true));
            }
        }
        [IgnoreDataMember]
        [NonSerialized]
        private RelayCommand addMotorCommand;

        /// <summary>
        /// Gets the AddMotorCommand.
        /// </summary>
        [IgnoreDataMember]
        public RelayCommand AddMotorCommand
        {
            get
            {
                return addMotorCommand
                    ?? (addMotorCommand = new RelayCommand(
                    () =>
                    {
                        if (!AddMotorCommand.CanExecute(null))
                        {
                            return;
                        }

                        Debug.Print("Add new motor");
                        var mvm = new MotorViewModel() { Controller = this.Controller };
                        mvm.Id = counter++;
                        Motors.Add(mvm);
                    },
                    () => true));
            }
        }
        [IgnoreDataMember]
        [NonSerialized]
        private RelayCommand deleteCommand;

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        [IgnoreDataMember]
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand
                    ?? (deleteCommand = new RelayCommand(
                    () =>
                    {
                        if (!DeleteCommand.CanExecute(null))
                        {
                            return;
                        }
                        Messenger.Default.Send<Messages.RemoveController>(new Messages.RemoveController() { ControllerToRemove = this });

                    },
                    () => true));
            }
        }


        public uint Id { get { return Controller.Id; } }

        public string IdString { get { return "Address: " + Controller.Id.ToString(); } }

        [DataMember]
        public bool LedIsEnabled
        {
            get { return Controller.IdentificationLedIsEnabled; }
            set { Controller.IdentificationLedIsEnabled = value; }
        }


        [DataMember]
        public string FriendlyName
        {
            get 
            {
                if (Controller.FriendlyName != null && Controller.FriendlyName.Length > 0)
                    return Controller.FriendlyName;
                else
                    return "<No name>";
            }
            set 
            {
                Controller.FriendlyName = value;
                if(PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("FriendlyName"));
            }
        }



        [field: NonSerializedAttribute()] 
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
