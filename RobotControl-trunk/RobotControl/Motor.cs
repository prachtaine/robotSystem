﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RobotControl
{
    public enum ControlMode { Reserved, Potentiometer, RelativeStep };
    public enum HomingSource { NoLimitSwitch, MinLimitSwitch, MaxLimitSwitch }
    /// <summary>
    /// This is the main class for interacting with particular motors that are located inside a joint.
    /// </summary>
    /// <remarks>
    /// <para>
    /// There are two control modes that a motor can operate in -- <see cref="ControlMode"/>.Potentiometer and <see cref="ControlMode"/>.RelativeStep.
    /// </para>
    /// <para>
    /// In Potentiometer mode, the potentiometer attached to the joint is used to gauge the joint position. The potentiometer can be calibrated by setting
    /// <see cref="MinAngle"/> and <see cref="MaxAngle"/> to their appropriate values. No homing is required for this method.
    /// </para>
    /// <para>
    /// In RelativeStep mode, the motor shaft's Hall effect sensors are used to keep track of the shaft position by counting each click from the sensor.
    /// Generally, there is one click per shaft revolution (this excludes the gearbox that's almost always attached to motor). 
    /// <see cref="MinAngle"/> and <see cref="MaxAngle"/> are used to set the minimum and maximum angle of the joint, while <see cref="MinStepCount"/> and
    /// <see cref="MaxStepCount"/> are used to set the minimum and maximum step counts that correspond to the minimum and maximum angles mentioned previously.
    /// </para>
    /// <para>
    /// Since the Hall effect sensors produce relative step counts only, it is necessary to initialize the current shaft position to an accurate value
    /// based on the actual angle of the joint. This is known as "Homing" and can be done automatically using limit switches, or manually, by associating 
    /// the current shaft position with a measured angle of the joint.
    /// </para>
    /// <para>
    /// To use automated homing, set <see cref="Motor.HomingSource"/> to either <see cref="HomingSource"/>.MinLimitSwitch or <see cref="HomingSource"/>.MaxLimitSwitch
    /// and execute <see cref="Home"/>. This will slowly move the joint toward either minimum or maximum value, and once the limit switch is pressed, both the joint
    /// angle and the current position counter will be updated to their correct values.
    /// </para>
    /// </remarks>
    [Serializable]
    public class Motor
    {
        public Motor()
        {
            encoderClicksPerRevolution = 1;
            kp = 1;
        }

        private ControlMode controlMode;

        /// <summary>
        /// Gets or sets a value that determines if the motor's joint position is controlled using an external potentiometer attached to the control board or
        /// the Hall effect sensors built-in to the motor shaft. 
        /// </summary>
        public ControlMode ControlMode
        {
            get { return controlMode; }
            set {
                if (controlMode != value)
                {
                    controlMode = value;
                    Debug.Write("Updating Control Mode");
                    this.UpdateConfiguration();
                }
            }
        }

        private byte kp;

        public byte Kp
        {
            get { return kp; }
            set { 
                if(value != kp)
                {
                    kp = value;
                    this.UpdateConfiguration();
                }
            }
        }

        public void UpdateConfiguration()
        {
            if (controller != null && controller.Robot != null)
                controller.Robot.SendCommand(JointCommands.Configure, this.controller, new byte[] { (byte)index, (byte)controlMode, kp });
        }

        public void Jog(int speed, bool direction)
        {
            if(controller != null && controller.Robot != null)
                controller.Robot.SendCommand(JointCommands.Jog, this.controller, new byte[] { (byte)index, (byte)speed, (byte)(direction ? 1 : 0) });
        }
        

        private HomingSource homingSource;

        /// <summary>
        /// Gets or sets a value that determines the homing source (if any) for use in RelativeStep homing.
        /// </summary>
        public HomingSource HomingSource
        {
            get { return homingSource; }
            set { homingSource = value; }
        }
        
        private int index;

        /// <summary>
        /// The index of the motor.
        /// </summary>
        /// <remarks>
        /// This is used to direct commands and configuration parameters to the correct motor attached to the joint. The current-generation board
        /// supports two motors with index values 0 and 1, though future boards may have more or fewer motors supported.
        /// </remarks>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private Controller controller;

        public Controller Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        private double angle;

        public double Angle
        {
            get { return angle; }
            set { 
                if(angle != value)
                {
                    angle = value;
                    UpdateSetpointFromAngle();
                }
            }
        }

        public double OffsetAngle { set; get; }

        private double minAngle;
        /// <summary>
        /// Gets or sets the minimum angle of the joint for this motor, in degrees;
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is measured relative to the potentiometer and helps establish the coordinate frame of the robot.
        /// </para>
        /// <para>
        /// This property answers the question "When the potentiometer of the joint measures its minimum value (0 volts), what angle is the joint at?"
        /// </para>
        /// </remarks>
        public double MinAngle
        {
            get { return minAngle; }
            set { minAngle = value; }
        }

        private double maxAngle;

        private double encoderClicksPerRevolution;

        /// <summary>
        /// Gets or sets a value associated with the number of encoder signals that 
        /// corresponds with one revolution of the output shaft.
        /// </summary>
        public double EncoderClicksPerRevolution 
        {
            get { return encoderClicksPerRevolution; }
            set { encoderClicksPerRevolution = value; }
        }

        /// <summary>
        /// Allows the user to manually identify the angle associated with the current position of the motor.
        /// </summary>
        /// <param name="angle"></param>
        /// <remarks>
        /// <para>
        /// This method works regardless of the current control method. In Relative step count control, this method zeros the
        /// internal step counter and maps the angle offset to this value.
        /// </para>
        /// <para>
        /// In potentiometer control, this method sets the potentiometer offset angle.
        /// </para>
        /// </remarks>
        public void CalibrateOffsetAngle(double angle = 0)
        {
            // If we're in relative step mode, reset our counter
            if(controlMode == RobotControl.ControlMode.RelativeStep)
            {
                if (controller != null)
                    controller.Robot.SendCommand(JointCommands.ResetCounters, this.controller, new byte[] { (byte)index, (byte)0x01 });
            }
            OffsetAngle = angle;
        }

        private void UpdateSetpointFromAngle()
        {
            if(controlMode == RobotControl.ControlMode.RelativeStep)
            {
                double setPoint = encoderClicksPerRevolution / (360) * (Angle - OffsetAngle);
                int position = (int)Math.Round(setPoint);
                byte[] motorCmd = new byte[7];
                motorCmd[0] = (byte)Index;
                motorCmd[1] = 0; //unused
                motorCmd[2] = 0; //unused
                Array.Copy(BitConverter.GetBytes(position), 0, motorCmd, 3, 4);

                if (controller != null && controller.Robot != null)
                    controller.Robot.SendCommand(JointCommands.MoveTo, controller, motorCmd);
            } 
            else if(controlMode == RobotControl.ControlMode.Potentiometer)
            {
                
            }

            //double AdcStepsPerDegree = MaxAngle - MinAngle * UInt16.MaxValue;
            //double newPosition = Math.Round(UInt16.MaxValue / 2 + (Angle - MinAngle) * AdcStepsPerDegree);
            //Position = (int)newPosition;
        }

        //private int position;

        //public int Position
        //{
        //    get { return position; }
        //    set {
        //        if (position != value)
        //        {
        //            position = value; 
        //            byte[] motorCmd = new byte[7];
        //            motorCmd[0] = (byte)Index;
        //            motorCmd[1] = 0; //unused
        //            motorCmd[2] = 0; //unused
        //            Array.Copy(BitConverter.GetBytes(position), 0, motorCmd, 3, 4);
                    
        //            joint.Robot.SendCommand(JointCommands.MoveTo, joint, motorCmd);

        //            //Debug.WriteLine("Moving " + this + " to " + this.position);
        //        }
                
                
        //    }
        //}

        public override string ToString()
        {
            return ("Motor " + this.Index.ToString());
        }
    }
}
