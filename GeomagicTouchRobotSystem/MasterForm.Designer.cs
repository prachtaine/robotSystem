namespace GeomagicTouchRobotSystem
{
    partial class MasterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartMasterConsoleButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbInk2 = new System.Windows.Forms.Label();
            this.lbButtons2 = new System.Windows.Forms.Label();
            this.lbGimbal32 = new System.Windows.Forms.Label();
            this.lbGimbal22 = new System.Windows.Forms.Label();
            this.lbGimbal12 = new System.Windows.Forms.Label();
            this.lbX2Value = new System.Windows.Forms.Label();
            this.lbY2Value = new System.Windows.Forms.Label();
            this.lbZ2Value = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbInk1 = new System.Windows.Forms.Label();
            this.lbButtons1 = new System.Windows.Forms.Label();
            this.lbGimbal31 = new System.Windows.Forms.Label();
            this.lbGimbal21 = new System.Windows.Forms.Label();
            this.lbGimbal11 = new System.Windows.Forms.Label();
            this.lbX1value = new System.Windows.Forms.Label();
            this.lbY1value = new System.Windows.Forms.Label();
            this.lbZ1value = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer();
            this.RightandLeftRecievedOmniData = new System.Windows.Forms.GroupBox();
            this.User1Label = new System.Windows.Forms.Label();
            this.User2Label = new System.Windows.Forms.Label();
            this.cb_ForceEnable = new System.Windows.Forms.CheckBox();
            this.btn_zeroForces = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.RightandLeftRecievedOmniData.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartMasterConsoleButton
            // 
            this.StartMasterConsoleButton.Location = new System.Drawing.Point(12, 12);
            this.StartMasterConsoleButton.Name = "StartMasterConsoleButton";
            this.StartMasterConsoleButton.Size = new System.Drawing.Size(471, 74);
            this.StartMasterConsoleButton.TabIndex = 20;
            this.StartMasterConsoleButton.Text = "Start Master Console";
            this.StartMasterConsoleButton.UseVisualStyleBackColor = true;
            this.StartMasterConsoleButton.Click += new System.EventHandler(this.StartMasterConsoleButtonClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbInk2);
            this.groupBox2.Controls.Add(this.lbButtons2);
            this.groupBox2.Controls.Add(this.lbGimbal32);
            this.groupBox2.Controls.Add(this.lbGimbal22);
            this.groupBox2.Controls.Add(this.lbGimbal12);
            this.groupBox2.Controls.Add(this.lbX2Value);
            this.groupBox2.Controls.Add(this.lbY2Value);
            this.groupBox2.Controls.Add(this.lbZ2Value);
            this.groupBox2.Location = new System.Drawing.Point(285, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(172, 138);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Right Omni Stats";
            // 
            // lbInk2
            // 
            this.lbInk2.AutoSize = true;
            this.lbInk2.Location = new System.Drawing.Point(7, 108);
            this.lbInk2.Name = "lbInk2";
            this.lbInk2.Size = new System.Drawing.Size(49, 13);
            this.lbInk2.TabIndex = 9;
            this.lbInk2.Text = "InkWell :";
            // 
            // lbButtons2
            // 
            this.lbButtons2.AutoSize = true;
            this.lbButtons2.Location = new System.Drawing.Point(6, 95);
            this.lbButtons2.Name = "lbButtons2";
            this.lbButtons2.Size = new System.Drawing.Size(49, 13);
            this.lbButtons2.TabIndex = 8;
            this.lbButtons2.Text = "Buttons :";
            // 
            // lbGimbal32
            // 
            this.lbGimbal32.AutoSize = true;
            this.lbGimbal32.Location = new System.Drawing.Point(6, 82);
            this.lbGimbal32.Name = "lbGimbal32";
            this.lbGimbal32.Size = new System.Drawing.Size(54, 13);
            this.lbGimbal32.TabIndex = 7;
            this.lbGimbal32.Text = "Gimbal 3 :";
            // 
            // lbGimbal22
            // 
            this.lbGimbal22.AutoSize = true;
            this.lbGimbal22.Location = new System.Drawing.Point(6, 69);
            this.lbGimbal22.Name = "lbGimbal22";
            this.lbGimbal22.Size = new System.Drawing.Size(54, 13);
            this.lbGimbal22.TabIndex = 6;
            this.lbGimbal22.Text = "Gimbal 2 :";
            // 
            // lbGimbal12
            // 
            this.lbGimbal12.AutoSize = true;
            this.lbGimbal12.Location = new System.Drawing.Point(6, 56);
            this.lbGimbal12.Name = "lbGimbal12";
            this.lbGimbal12.Size = new System.Drawing.Size(57, 13);
            this.lbGimbal12.TabIndex = 5;
            this.lbGimbal12.Text = "Gimbal 1 : ";
            // 
            // lbX2Value
            // 
            this.lbX2Value.AutoSize = true;
            this.lbX2Value.Location = new System.Drawing.Point(6, 16);
            this.lbX2Value.Name = "lbX2Value";
            this.lbX2Value.Size = new System.Drawing.Size(20, 13);
            this.lbX2Value.TabIndex = 1;
            this.lbX2Value.Text = "X :";
            // 
            // lbY2Value
            // 
            this.lbY2Value.AutoSize = true;
            this.lbY2Value.Location = new System.Drawing.Point(6, 30);
            this.lbY2Value.Name = "lbY2Value";
            this.lbY2Value.Size = new System.Drawing.Size(20, 13);
            this.lbY2Value.TabIndex = 3;
            this.lbY2Value.Text = "Y :";
            // 
            // lbZ2Value
            // 
            this.lbZ2Value.AutoSize = true;
            this.lbZ2Value.Location = new System.Drawing.Point(6, 43);
            this.lbZ2Value.Name = "lbZ2Value";
            this.lbZ2Value.Size = new System.Drawing.Size(23, 13);
            this.lbZ2Value.TabIndex = 4;
            this.lbZ2Value.Text = "Z : ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbInk1);
            this.groupBox1.Controls.Add(this.lbButtons1);
            this.groupBox1.Controls.Add(this.lbGimbal31);
            this.groupBox1.Controls.Add(this.lbGimbal21);
            this.groupBox1.Controls.Add(this.lbGimbal11);
            this.groupBox1.Controls.Add(this.lbX1value);
            this.groupBox1.Controls.Add(this.lbY1value);
            this.groupBox1.Controls.Add(this.lbZ1value);
            this.groupBox1.Location = new System.Drawing.Point(13, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 138);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Left Omni Stats";
            // 
            // lbInk1
            // 
            this.lbInk1.AutoSize = true;
            this.lbInk1.Location = new System.Drawing.Point(6, 108);
            this.lbInk1.Name = "lbInk1";
            this.lbInk1.Size = new System.Drawing.Size(52, 13);
            this.lbInk1.TabIndex = 9;
            this.lbInk1.Text = "InkWell : ";
            // 
            // lbButtons1
            // 
            this.lbButtons1.AutoSize = true;
            this.lbButtons1.Location = new System.Drawing.Point(6, 95);
            this.lbButtons1.Name = "lbButtons1";
            this.lbButtons1.Size = new System.Drawing.Size(49, 13);
            this.lbButtons1.TabIndex = 8;
            this.lbButtons1.Text = "Buttons :";
            // 
            // lbGimbal31
            // 
            this.lbGimbal31.AutoSize = true;
            this.lbGimbal31.Location = new System.Drawing.Point(6, 82);
            this.lbGimbal31.Name = "lbGimbal31";
            this.lbGimbal31.Size = new System.Drawing.Size(54, 13);
            this.lbGimbal31.TabIndex = 7;
            this.lbGimbal31.Text = "Gimbal 3 :";
            // 
            // lbGimbal21
            // 
            this.lbGimbal21.AutoSize = true;
            this.lbGimbal21.Location = new System.Drawing.Point(6, 69);
            this.lbGimbal21.Name = "lbGimbal21";
            this.lbGimbal21.Size = new System.Drawing.Size(54, 13);
            this.lbGimbal21.TabIndex = 6;
            this.lbGimbal21.Text = "Gimbal 2 :";
            // 
            // lbGimbal11
            // 
            this.lbGimbal11.AutoSize = true;
            this.lbGimbal11.Location = new System.Drawing.Point(6, 56);
            this.lbGimbal11.Name = "lbGimbal11";
            this.lbGimbal11.Size = new System.Drawing.Size(54, 13);
            this.lbGimbal11.TabIndex = 5;
            this.lbGimbal11.Text = "Gimbal 1 :";
            // 
            // lbX1value
            // 
            this.lbX1value.AutoSize = true;
            this.lbX1value.Location = new System.Drawing.Point(6, 16);
            this.lbX1value.Name = "lbX1value";
            this.lbX1value.Size = new System.Drawing.Size(23, 13);
            this.lbX1value.TabIndex = 1;
            this.lbX1value.Text = "X : ";
            // 
            // lbY1value
            // 
            this.lbY1value.AutoSize = true;
            this.lbY1value.Location = new System.Drawing.Point(6, 30);
            this.lbY1value.Name = "lbY1value";
            this.lbY1value.Size = new System.Drawing.Size(20, 13);
            this.lbY1value.TabIndex = 3;
            this.lbY1value.Text = "Y :";
            // 
            // lbZ1value
            // 
            this.lbZ1value.AutoSize = true;
            this.lbZ1value.Location = new System.Drawing.Point(6, 43);
            this.lbZ1value.Name = "lbZ1value";
            this.lbZ1value.Size = new System.Drawing.Size(20, 13);
            this.lbZ1value.TabIndex = 4;
            this.lbZ1value.Text = "Z :";
            // 
            // timer
            // 
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timerTick);
            // 
            // RightandLeftRecievedOmniData
            // 
            this.RightandLeftRecievedOmniData.Controls.Add(this.groupBox1);
            this.RightandLeftRecievedOmniData.Controls.Add(this.groupBox2);
            this.RightandLeftRecievedOmniData.Location = new System.Drawing.Point(12, 102);
            this.RightandLeftRecievedOmniData.Name = "RightandLeftRecievedOmniData";
            this.RightandLeftRecievedOmniData.Size = new System.Drawing.Size(471, 174);
            this.RightandLeftRecievedOmniData.TabIndex = 40;
            this.RightandLeftRecievedOmniData.TabStop = false;
            this.RightandLeftRecievedOmniData.Text = "RecievedOmniMessage";
            // 
            // User1Label
            // 
            this.User1Label.AutoSize = true;
            this.User1Label.Location = new System.Drawing.Point(12, 296);
            this.User1Label.Name = "User1Label";
            this.User1Label.Size = new System.Drawing.Size(35, 13);
            this.User1Label.TabIndex = 41;
            this.User1Label.Text = "User1";
            // 
            // User2Label
            // 
            this.User2Label.AutoSize = true;
            this.User2Label.Location = new System.Drawing.Point(12, 325);
            this.User2Label.Name = "User2Label";
            this.User2Label.Size = new System.Drawing.Size(34, 17);
            this.User2Label.TabIndex = 42;
            this.User2Label.Text = "User2";
            this.User2Label.UseCompatibleTextRendering = true;
            // 
            // cb_ForceEnable
            // 
            this.cb_ForceEnable.AutoSize = true;
            this.cb_ForceEnable.Location = new System.Drawing.Point(12, 346);
            this.cb_ForceEnable.Name = "cb_ForceEnable";
            this.cb_ForceEnable.Size = new System.Drawing.Size(104, 17);
            this.cb_ForceEnable.TabIndex = 43;
            this.cb_ForceEnable.Text = "Force Feedback";
            this.cb_ForceEnable.UseVisualStyleBackColor = true;
            this.cb_ForceEnable.CheckedChanged += new System.EventHandler(this.cb_ForceEnable_CheckedChanged);
            // 
            // btn_zeroForces
            // 
            this.btn_zeroForces.Location = new System.Drawing.Point(408, 346);
            this.btn_zeroForces.Name = "btn_zeroForces";
            this.btn_zeroForces.Size = new System.Drawing.Size(75, 23);
            this.btn_zeroForces.TabIndex = 51;
            this.btn_zeroForces.Text = "Zero";
            this.btn_zeroForces.UseVisualStyleBackColor = true;
            this.btn_zeroForces.Click += new System.EventHandler(this.btn_zeroForces_Click);
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 412);
            this.Controls.Add(this.btn_zeroForces);
            this.Controls.Add(this.cb_ForceEnable);
            this.Controls.Add(this.User2Label);
            this.Controls.Add(this.User1Label);
            this.Controls.Add(this.RightandLeftRecievedOmniData);
            this.Controls.Add(this.StartMasterConsoleButton);
            this.Name = "MasterForm";
            this.Text = "Master";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.RightandLeftRecievedOmniData.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartMasterConsoleButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbInk2;
        private System.Windows.Forms.Label lbButtons2;
        private System.Windows.Forms.Label lbGimbal32;
        private System.Windows.Forms.Label lbGimbal22;
        private System.Windows.Forms.Label lbGimbal12;
        private System.Windows.Forms.Label lbX2Value;
        private System.Windows.Forms.Label lbY2Value;
        private System.Windows.Forms.Label lbZ2Value;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbInk1;
        private System.Windows.Forms.Label lbButtons1;
        private System.Windows.Forms.Label lbGimbal31;
        private System.Windows.Forms.Label lbGimbal21;
        private System.Windows.Forms.Label lbGimbal11;
        private System.Windows.Forms.Label lbX1value;
        private System.Windows.Forms.Label lbY1value;
        private System.Windows.Forms.Label lbZ1value;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.GroupBox RightandLeftRecievedOmniData;
        private System.Windows.Forms.Label User1Label;
        private System.Windows.Forms.Label User2Label;
        private System.Windows.Forms.CheckBox cb_ForceEnable;
        private System.Windows.Forms.Button btn_zeroForces;
    }
}

