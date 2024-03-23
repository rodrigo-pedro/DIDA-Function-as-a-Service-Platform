
namespace PuppetMasterGUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.SystemStatus = new System.Windows.Forms.TabPage();
            this.clearButton = new System.Windows.Forms.Button();
            this.statusButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.StatusInputTextBox = new System.Windows.Forms.TextBox();
            this.ListGlobalButton = new System.Windows.Forms.Button();
            this.ListServerButton = new System.Windows.Forms.Button();
            this.StatusTextBox = new System.Windows.Forms.TextBox();
            this.CreateProcess = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.KillProcessTextBox = new System.Windows.Forms.TextBox();
            this.KillProcessButton = new System.Windows.Forms.Button();
            this.CreateButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.GossipDelayBox = new System.Windows.Forms.TextBox();
            this.UrlBox = new System.Windows.Forms.TextBox();
            this.serverIdBox = new System.Windows.Forms.TextBox();
            this.CreateWorkerButton = new System.Windows.Forms.RadioButton();
            this.CreateStorageButton = new System.Windows.Forms.RadioButton();
            this.CreateSchedulerButton = new System.Windows.Forms.RadioButton();
            this.LoadFiles = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.StepByStepButton = new System.Windows.Forms.Button();
            this.ScriptRunButton = new System.Windows.Forms.Button();
            this.LoadScriptBrowseButton = new System.Windows.Forms.Button();
            this.LoadScriptTextBox = new System.Windows.Forms.TextBox();
            this.PopulateGroupBox = new System.Windows.Forms.GroupBox();
            this.LoadPopulateDatabaseButton = new System.Windows.Forms.Button();
            this.PopulateDatabaseBrowseButton = new System.Windows.Forms.Button();
            this.PopulateDatabaseTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.LoadApplicationTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.SystemStatus.SuspendLayout();
            this.CreateProcess.SuspendLayout();
            this.LoadFiles.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.PopulateGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.SystemStatus);
            this.tabControl1.Controls.Add(this.CreateProcess);
            this.tabControl1.Controls.Add(this.LoadFiles);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(500, 553);
            this.tabControl1.TabIndex = 4;
            // 
            // SystemStatus
            // 
            this.SystemStatus.Controls.Add(this.clearButton);
            this.SystemStatus.Controls.Add(this.statusButton);
            this.SystemStatus.Controls.Add(this.label5);
            this.SystemStatus.Controls.Add(this.StatusInputTextBox);
            this.SystemStatus.Controls.Add(this.ListGlobalButton);
            this.SystemStatus.Controls.Add(this.ListServerButton);
            this.SystemStatus.Controls.Add(this.StatusTextBox);
            this.SystemStatus.Location = new System.Drawing.Point(4, 24);
            this.SystemStatus.Name = "SystemStatus";
            this.SystemStatus.Padding = new System.Windows.Forms.Padding(3);
            this.SystemStatus.Size = new System.Drawing.Size(492, 525);
            this.SystemStatus.TabIndex = 1;
            this.SystemStatus.Text = "System Status";
            this.SystemStatus.UseVisualStyleBackColor = true;
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(411, 496);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(75, 23);
            this.clearButton.TabIndex = 6;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // statusButton
            // 
            this.statusButton.Location = new System.Drawing.Point(370, 39);
            this.statusButton.Name = "statusButton";
            this.statusButton.Size = new System.Drawing.Size(75, 23);
            this.statusButton.TabIndex = 5;
            this.statusButton.Text = "Status";
            this.statusButton.UseVisualStyleBackColor = true;
            this.statusButton.Click += new System.EventHandler(this.statusButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(59, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "Input";
            // 
            // StatusInputTextBox
            // 
            this.StatusInputTextBox.Location = new System.Drawing.Point(100, 80);
            this.StatusInputTextBox.Name = "StatusInputTextBox";
            this.StatusInputTextBox.Size = new System.Drawing.Size(310, 23);
            this.StatusInputTextBox.TabIndex = 3;
            // 
            // ListGlobalButton
            // 
            this.ListGlobalButton.Location = new System.Drawing.Point(207, 39);
            this.ListGlobalButton.Name = "ListGlobalButton";
            this.ListGlobalButton.Size = new System.Drawing.Size(75, 23);
            this.ListGlobalButton.TabIndex = 2;
            this.ListGlobalButton.Text = "List Global";
            this.ListGlobalButton.UseVisualStyleBackColor = true;
            this.ListGlobalButton.Click += new System.EventHandler(this.ListGlobalButton_Click);
            // 
            // ListServerButton
            // 
            this.ListServerButton.Location = new System.Drawing.Point(44, 39);
            this.ListServerButton.Name = "ListServerButton";
            this.ListServerButton.Size = new System.Drawing.Size(75, 23);
            this.ListServerButton.TabIndex = 1;
            this.ListServerButton.Text = "List Server";
            this.ListServerButton.UseVisualStyleBackColor = true;
            this.ListServerButton.Click += new System.EventHandler(this.ListServerButton_Click);
            // 
            // StatusTextBox
            // 
            this.StatusTextBox.Location = new System.Drawing.Point(6, 109);
            this.StatusTextBox.Multiline = true;
            this.StatusTextBox.Name = "StatusTextBox";
            this.StatusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.StatusTextBox.Size = new System.Drawing.Size(480, 385);
            this.StatusTextBox.TabIndex = 0;
            // 
            // CreateProcess
            // 
            this.CreateProcess.Controls.Add(this.label6);
            this.CreateProcess.Controls.Add(this.KillProcessTextBox);
            this.CreateProcess.Controls.Add(this.KillProcessButton);
            this.CreateProcess.Controls.Add(this.CreateButton);
            this.CreateProcess.Controls.Add(this.label3);
            this.CreateProcess.Controls.Add(this.label2);
            this.CreateProcess.Controls.Add(this.label1);
            this.CreateProcess.Controls.Add(this.GossipDelayBox);
            this.CreateProcess.Controls.Add(this.UrlBox);
            this.CreateProcess.Controls.Add(this.serverIdBox);
            this.CreateProcess.Controls.Add(this.CreateWorkerButton);
            this.CreateProcess.Controls.Add(this.CreateStorageButton);
            this.CreateProcess.Controls.Add(this.CreateSchedulerButton);
            this.CreateProcess.Location = new System.Drawing.Point(4, 24);
            this.CreateProcess.Name = "CreateProcess";
            this.CreateProcess.Padding = new System.Windows.Forms.Padding(3);
            this.CreateProcess.Size = new System.Drawing.Size(492, 525);
            this.CreateProcess.TabIndex = 0;
            this.CreateProcess.Text = "Create/Kill Process";
            this.CreateProcess.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(58, 355);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Server ID";
            // 
            // KillProcessTextBox
            // 
            this.KillProcessTextBox.Location = new System.Drawing.Point(151, 352);
            this.KillProcessTextBox.Name = "KillProcessTextBox";
            this.KillProcessTextBox.Size = new System.Drawing.Size(140, 23);
            this.KillProcessTextBox.TabIndex = 11;
            // 
            // KillProcessButton
            // 
            this.KillProcessButton.Location = new System.Drawing.Point(302, 396);
            this.KillProcessButton.Name = "KillProcessButton";
            this.KillProcessButton.Size = new System.Drawing.Size(75, 23);
            this.KillProcessButton.TabIndex = 10;
            this.KillProcessButton.Text = "Kill";
            this.KillProcessButton.UseVisualStyleBackColor = true;
            this.KillProcessButton.Click += new System.EventHandler(this.KillProcessButton_Click);
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(302, 235);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 9;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Gossip Delay";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "URL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Server ID";
            // 
            // GossipDelayBox
            // 
            this.GossipDelayBox.Location = new System.Drawing.Point(151, 180);
            this.GossipDelayBox.Name = "GossipDelayBox";
            this.GossipDelayBox.Size = new System.Drawing.Size(140, 23);
            this.GossipDelayBox.TabIndex = 5;
            // 
            // UrlBox
            // 
            this.UrlBox.Location = new System.Drawing.Point(151, 135);
            this.UrlBox.Name = "UrlBox";
            this.UrlBox.Size = new System.Drawing.Size(140, 23);
            this.UrlBox.TabIndex = 4;
            // 
            // serverIdBox
            // 
            this.serverIdBox.Location = new System.Drawing.Point(151, 83);
            this.serverIdBox.Name = "serverIdBox";
            this.serverIdBox.Size = new System.Drawing.Size(140, 23);
            this.serverIdBox.TabIndex = 3;
            // 
            // CreateWorkerButton
            // 
            this.CreateWorkerButton.AutoSize = true;
            this.CreateWorkerButton.Location = new System.Drawing.Point(181, 33);
            this.CreateWorkerButton.Name = "CreateWorkerButton";
            this.CreateWorkerButton.Size = new System.Drawing.Size(63, 19);
            this.CreateWorkerButton.TabIndex = 2;
            this.CreateWorkerButton.TabStop = true;
            this.CreateWorkerButton.Text = "Worker";
            this.CreateWorkerButton.UseVisualStyleBackColor = true;
            this.CreateWorkerButton.CheckedChanged += new System.EventHandler(this.CreateWorkerButton_CheckedChanged);
            // 
            // CreateStorageButton
            // 
            this.CreateStorageButton.AutoSize = true;
            this.CreateStorageButton.Location = new System.Drawing.Point(302, 33);
            this.CreateStorageButton.Name = "CreateStorageButton";
            this.CreateStorageButton.Size = new System.Drawing.Size(65, 19);
            this.CreateStorageButton.TabIndex = 1;
            this.CreateStorageButton.TabStop = true;
            this.CreateStorageButton.Text = "Storage";
            this.CreateStorageButton.UseVisualStyleBackColor = true;
            this.CreateStorageButton.CheckedChanged += new System.EventHandler(this.CreateStorageButton_CheckedChanged);
            // 
            // CreateSchedulerButton
            // 
            this.CreateSchedulerButton.AutoSize = true;
            this.CreateSchedulerButton.Location = new System.Drawing.Point(51, 33);
            this.CreateSchedulerButton.Name = "CreateSchedulerButton";
            this.CreateSchedulerButton.Size = new System.Drawing.Size(77, 19);
            this.CreateSchedulerButton.TabIndex = 0;
            this.CreateSchedulerButton.TabStop = true;
            this.CreateSchedulerButton.Text = "Scheduler";
            this.CreateSchedulerButton.UseVisualStyleBackColor = true;
            this.CreateSchedulerButton.CheckedChanged += new System.EventHandler(this.CreateSchedulerButton_CheckedChanged);
            // 
            // LoadFiles
            // 
            this.LoadFiles.Controls.Add(this.groupBox2);
            this.LoadFiles.Controls.Add(this.PopulateGroupBox);
            this.LoadFiles.Controls.Add(this.groupBox1);
            this.LoadFiles.Location = new System.Drawing.Point(4, 24);
            this.LoadFiles.Name = "LoadFiles";
            this.LoadFiles.Size = new System.Drawing.Size(492, 525);
            this.LoadFiles.TabIndex = 2;
            this.LoadFiles.Text = "Load Files";
            this.LoadFiles.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.StepByStepButton);
            this.groupBox2.Controls.Add(this.ScriptRunButton);
            this.groupBox2.Controls.Add(this.LoadScriptBrowseButton);
            this.groupBox2.Controls.Add(this.LoadScriptTextBox);
            this.groupBox2.Location = new System.Drawing.Point(28, 358);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(428, 139);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Load Script";
            // 
            // StepByStepButton
            // 
            this.StepByStepButton.Location = new System.Drawing.Point(159, 103);
            this.StepByStepButton.Name = "StepByStepButton";
            this.StepByStepButton.Size = new System.Drawing.Size(112, 23);
            this.StepByStepButton.TabIndex = 8;
            this.StepByStepButton.Text = "Run Step By Step";
            this.StepByStepButton.UseVisualStyleBackColor = true;
            this.StepByStepButton.Click += new System.EventHandler(this.StepByStepButton_Click);
            // 
            // ScriptRunButton
            // 
            this.ScriptRunButton.Location = new System.Drawing.Point(326, 104);
            this.ScriptRunButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ScriptRunButton.Name = "ScriptRunButton";
            this.ScriptRunButton.Size = new System.Drawing.Size(82, 22);
            this.ScriptRunButton.TabIndex = 7;
            this.ScriptRunButton.Text = "Run";
            this.ScriptRunButton.UseVisualStyleBackColor = true;
            this.ScriptRunButton.Click += new System.EventHandler(this.ScriptRunButton_Click);
            // 
            // LoadScriptBrowseButton
            // 
            this.LoadScriptBrowseButton.Location = new System.Drawing.Point(270, 45);
            this.LoadScriptBrowseButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LoadScriptBrowseButton.Name = "LoadScriptBrowseButton";
            this.LoadScriptBrowseButton.Size = new System.Drawing.Size(82, 22);
            this.LoadScriptBrowseButton.TabIndex = 6;
            this.LoadScriptBrowseButton.Text = "Browse";
            this.LoadScriptBrowseButton.UseVisualStyleBackColor = true;
            this.LoadScriptBrowseButton.Click += new System.EventHandler(this.LoadScriptBrowseButton_Click);
            // 
            // LoadScriptTextBox
            // 
            this.LoadScriptTextBox.Location = new System.Drawing.Point(23, 45);
            this.LoadScriptTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LoadScriptTextBox.Name = "LoadScriptTextBox";
            this.LoadScriptTextBox.Size = new System.Drawing.Size(232, 23);
            this.LoadScriptTextBox.TabIndex = 5;
            // 
            // PopulateGroupBox
            // 
            this.PopulateGroupBox.Controls.Add(this.LoadPopulateDatabaseButton);
            this.PopulateGroupBox.Controls.Add(this.PopulateDatabaseBrowseButton);
            this.PopulateGroupBox.Controls.Add(this.PopulateDatabaseTextBox);
            this.PopulateGroupBox.Location = new System.Drawing.Point(28, 190);
            this.PopulateGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PopulateGroupBox.Name = "PopulateGroupBox";
            this.PopulateGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PopulateGroupBox.Size = new System.Drawing.Size(428, 139);
            this.PopulateGroupBox.TabIndex = 4;
            this.PopulateGroupBox.TabStop = false;
            this.PopulateGroupBox.Text = "Populate Database";
            // 
            // LoadPopulateDatabaseButton
            // 
            this.LoadPopulateDatabaseButton.Location = new System.Drawing.Point(326, 104);
            this.LoadPopulateDatabaseButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LoadPopulateDatabaseButton.Name = "LoadPopulateDatabaseButton";
            this.LoadPopulateDatabaseButton.Size = new System.Drawing.Size(82, 22);
            this.LoadPopulateDatabaseButton.TabIndex = 7;
            this.LoadPopulateDatabaseButton.Text = "Load";
            this.LoadPopulateDatabaseButton.UseVisualStyleBackColor = true;
            this.LoadPopulateDatabaseButton.Click += new System.EventHandler(this.LoadPopulateDatabaseButton_Click);
            // 
            // PopulateDatabaseBrowseButton
            // 
            this.PopulateDatabaseBrowseButton.Location = new System.Drawing.Point(270, 45);
            this.PopulateDatabaseBrowseButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PopulateDatabaseBrowseButton.Name = "PopulateDatabaseBrowseButton";
            this.PopulateDatabaseBrowseButton.Size = new System.Drawing.Size(82, 22);
            this.PopulateDatabaseBrowseButton.TabIndex = 6;
            this.PopulateDatabaseBrowseButton.Text = "Browse";
            this.PopulateDatabaseBrowseButton.UseVisualStyleBackColor = true;
            this.PopulateDatabaseBrowseButton.Click += new System.EventHandler(this.PopulateDatabaseBrowseButton_Click);
            // 
            // PopulateDatabaseTextBox
            // 
            this.PopulateDatabaseTextBox.Location = new System.Drawing.Point(23, 45);
            this.PopulateDatabaseTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PopulateDatabaseTextBox.Name = "PopulateDatabaseTextBox";
            this.PopulateDatabaseTextBox.Size = new System.Drawing.Size(232, 23);
            this.PopulateDatabaseTextBox.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.InputTextBox);
            this.groupBox1.Controls.Add(this.RunButton);
            this.groupBox1.Controls.Add(this.LoadApplicationTextBox);
            this.groupBox1.Controls.Add(this.BrowseButton);
            this.groupBox1.Location = new System.Drawing.Point(28, 22);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(428, 142);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Load Application";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Input";
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(66, 100);
            this.InputTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(126, 23);
            this.InputTextBox.TabIndex = 3;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(326, 104);
            this.RunButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(82, 22);
            this.RunButton.TabIndex = 2;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // LoadApplicationTextBox
            // 
            this.LoadApplicationTextBox.Location = new System.Drawing.Point(23, 47);
            this.LoadApplicationTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LoadApplicationTextBox.Name = "LoadApplicationTextBox";
            this.LoadApplicationTextBox.Size = new System.Drawing.Size(232, 23);
            this.LoadApplicationTextBox.TabIndex = 1;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(270, 47);
            this.BrowseButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(82, 22);
            this.BrowseButton.TabIndex = 0;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 577);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.SystemStatus.ResumeLayout(false);
            this.SystemStatus.PerformLayout();
            this.CreateProcess.ResumeLayout(false);
            this.CreateProcess.PerformLayout();
            this.LoadFiles.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.PopulateGroupBox.ResumeLayout(false);
            this.PopulateGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage SystemStatus;
        private System.Windows.Forms.TabPage CreateProcess;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox GossipDelayBox;
        private System.Windows.Forms.TextBox UrlBox;
        private System.Windows.Forms.TextBox serverIdBox;
        private System.Windows.Forms.RadioButton CreateWorkerButton;
        private System.Windows.Forms.RadioButton CreateStorageButton;
        private System.Windows.Forms.RadioButton CreateSchedulerButton;
        private System.Windows.Forms.TabPage LoadFiles;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.TextBox LoadApplicationTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox PopulateGroupBox;
        private System.Windows.Forms.TextBox PopulateDatabaseTextBox;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button LoadPopulateDatabaseButton;
        private System.Windows.Forms.Button PopulateDatabaseBrowseButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button ScriptRunButton;
        private System.Windows.Forms.Button LoadScriptBrowseButton;
        private System.Windows.Forms.TextBox LoadScriptTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox StatusInputTextBox;
        private System.Windows.Forms.Button ListGlobalButton;
        private System.Windows.Forms.Button ListServerButton;
        private System.Windows.Forms.TextBox StatusTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox KillProcessTextBox;
        private System.Windows.Forms.Button KillProcessButton;
        private System.Windows.Forms.Button StepByStepButton;
        private System.Windows.Forms.Button statusButton;
        private System.Windows.Forms.Button clearButton;
    }
}

