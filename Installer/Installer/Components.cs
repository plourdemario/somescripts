using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Installer
{
    public partial class Components : Form
    {
        private Dataclass dataclass1;
        private bool backbutton1 = false;

        public Components(Dataclass dataclasstemp1)
        {
            InitializeComponent();
            dataclass1 = dataclasstemp1;
        }

        private void AcceptCheckbox_CheckedChanged(object sender, EventArgs e)
        {
        }
        

        private void NextButton_Click(object sender, EventArgs e)
        {
            dataclass1.NextStepEnd();
        }
        private void BackButton_Click(object sender, EventArgs e)
        {
            backbutton1 = true;
            dataclass1.BackStepComponents();
        }

        private void Components_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backbutton1 == false)
            {
                dataclass1.ComponentsClosing();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this installation?", "Cancellation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dataclass1.ComponentsClosing();
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog1 = new FolderBrowserDialog();
            dialog1.SelectedPath = dataclass1.GetInstallPath();
            dialog1.ShowDialog();
            dataclass1.SetInstallPath(dialog1.SelectedPath);
            //PathLabel.Text = dialog1.SelectedPath;
        }
    }
}
