using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PuppetMasterGUI
{
    public partial class Form1 : Form
    {
        PuppetMaster puppet;

        public Form1()
        {
            puppet = new PuppetMaster();
            InitializeComponent();
        }

        private void CreateSchedulerButton_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateSchedulerButton.Checked)
            {
                CreateStorageButton.Checked = false;
                CreateWorkerButton.Checked = false;
                GossipDelayBox.Enabled = false;
            }
        }

        private void CreateWorkerButton_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateWorkerButton.Checked)
            {
                CreateSchedulerButton.Checked = false;
                CreateStorageButton.Checked = false;
                GossipDelayBox.Enabled = true;
            }
        }

        private void CreateStorageButton_CheckedChanged(object sender, EventArgs e)
        {
            if (CreateStorageButton.Checked)
            {
                CreateSchedulerButton.Checked = false;
                CreateWorkerButton.Checked = false;
                GossipDelayBox.Enabled = true;
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (CreateSchedulerButton.Checked)
            {
                puppet.CreateScheduler(serverIdBox.Text, UrlBox.Text);

            }
            else if (CreateWorkerButton.Checked)
            {
                puppet.CreateWorker(serverIdBox.Text, UrlBox.Text, GossipDelayBox.Text);
            }
            else if (CreateStorageButton.Checked)
            {
                puppet.CreateStorage(serverIdBox.Text, UrlBox.Text, GossipDelayBox.Text);
            }

            serverIdBox.Clear();
            UrlBox.Clear();
            GossipDelayBox.Clear();
            CreateStorageButton.Checked = false;
            CreateSchedulerButton.Checked = false;
            CreateSchedulerButton.Checked = false;
        }



        private void BrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                LoadApplicationTextBox.Text = file;
            }

        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            puppet.RunApplication(LoadApplicationTextBox.Text, InputTextBox.Text);
        }

        private void LoadScriptBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                LoadScriptTextBox.Text = file;
            }
        }

        private void ScriptRunButton_Click(object sender, EventArgs e)
        {
            puppet.RunScript(LoadScriptTextBox.Text);
        }

        private void PopulateDatabaseBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                PopulateDatabaseTextBox.Text = file;
            }
        }

        private void LoadPopulateDatabaseButton_Click(object sender, EventArgs e)
        {
            puppet.PopulateDatabase(PopulateDatabaseTextBox.Text);
        }

        private void ListServerButton_Click(object sender, EventArgs e)
        {
            StatusTextBox.Text = puppet.ListServer(StatusInputTextBox.Text);
            StatusInputTextBox.Clear();
        }

        private void ListGlobalButton_Click(object sender, EventArgs e)
        {
            StatusTextBox.Text = puppet.ListGlobal();
        }

        private void KillProcessButton_Click(object sender, EventArgs e)
        {
            puppet.KillNode(KillProcessTextBox.Text);
        }

        private void StepByStepButton_Click(object sender, EventArgs e)
        {
            var stepByStepPopup = new Form2(ref puppet);
            stepByStepPopup.Show(this);

            stepByStepPopup.ScriptTextBox.Text = File.ReadAllText(LoadScriptTextBox.Text);

            if (stepByStepPopup.ScriptTextBox.Lines.Length > 0)
            {
                var line = stepByStepPopup.ScriptTextBox.Lines[0];
                stepByStepPopup.ScriptTextBox.Select(stepByStepPopup.ScriptTextBox.GetFirstCharIndexFromLine(0), line.Length);
                stepByStepPopup.ScriptTextBox.SelectionBackColor = Color.Yellow;
            }
        }

        private void statusButton_Click(object sender, EventArgs e)
        {
            StatusTextBox.Text = puppet.Status();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            StatusTextBox.Clear();
        }
    }
}
