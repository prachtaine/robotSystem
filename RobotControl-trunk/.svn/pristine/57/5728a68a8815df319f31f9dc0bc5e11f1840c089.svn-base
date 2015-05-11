using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RobotControl;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.IO;
namespace RobotApp.ViewModel
{
    public class ControllerConfigurationViewModel : ViewModelBase
    {
        public RelayCommand DetectControllerCommand { get; set; }

        public MainViewModel MainViewModel { get { return MainViewModel.Instance; } }

        private RelayCommand loadCommand;

        /// <summary>
        /// Gets the LoadCommand.
        /// </summary>
        public RelayCommand LoadCommand
        {
            get
            {
                return loadCommand
                    ?? (loadCommand = new RelayCommand(ExecuteLoadCommand));
            }
        }

        private void ExecuteLoadCommand()
        {
            MainViewModel.LoadData();
        }

        private RelayCommand saveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand
                    ?? (saveCommand = new RelayCommand(ExecuteSaveCommand));
            }
        }

        private void ExecuteSaveCommand()
        {
            MainViewModel.SaveData();
        }

        public ControllerConfigurationViewModel()
        {

            DetectControllerCommand = new RelayCommand(
                () =>
                    {
                        MainViewModel.ScanForControllers();
                    },
                () =>
                    {
                        if (MainViewModel.Robot == null)
                            return false;
                        if (MainViewModel.Robot.Com == null)
                            return false;
                        return true;
                    }
                );

            MainViewModel.Robot.PropertyChanged += Robot_PropertyChanged;
        }


        

        void Robot_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            DetectControllerCommand.RaiseCanExecuteChanged();
        }

    }
}
