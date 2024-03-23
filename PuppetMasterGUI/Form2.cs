using System;
using System.Drawing;
using System.Windows.Forms;

namespace PuppetMasterGUI
{
    public partial class Form2 : Form
    {
        private int currentLine = 0;
        private PuppetMaster puppet;

        public Form2(ref PuppetMaster puppet)
        {
            InitializeComponent();
            this.puppet = puppet;
        }

        private void StepButton_Click(object sender, EventArgs e)
        {
            if (currentLine >= ScriptTextBox.Lines.Length)
                return;

            puppet.RunLine(ScriptTextBox.Lines[currentLine]);

            if (currentLine <= ScriptTextBox.Lines.Length)
            {
                var line = ScriptTextBox.Lines[currentLine];
                ScriptTextBox.Select(ScriptTextBox.GetFirstCharIndexFromLine(currentLine), line.Length);
                ScriptTextBox.SelectionBackColor = Color.White;
            }

            currentLine++;

            if (currentLine < ScriptTextBox.Lines.Length)
            {
                var line = ScriptTextBox.Lines[currentLine];
                ScriptTextBox.Select(ScriptTextBox.GetFirstCharIndexFromLine(currentLine), line.Length);
                ScriptTextBox.SelectionBackColor = Color.Yellow;
            }
        }

    }
}
