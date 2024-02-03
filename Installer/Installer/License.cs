using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Installer
{
    public partial class License : Form
    {
        private Dataclass dataclass1;
        private bool backbutton1 = false;

        public License(Dataclass dataclasstemp1)
        {
            InitializeComponent();
            System.Reflection.Assembly assembly1 = System.Reflection.Assembly.GetExecutingAssembly();
            Stream stream1 = assembly1.GetManifestResourceStream("Installer.EULAtermify.rtf");

            LicenseText.LoadFile(stream1, RichTextBoxStreamType.RichText);
            LicenseText.Refresh();
            dataclass1 = dataclasstemp1;
       }



        private void AcceptCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (AcceptCheckbox.Checked == true)
            {
                NextButton.Enabled = true;
            }
            else
            {
                NextButton.Enabled = false;
            }
        }

        private void License_Load(object sender, EventArgs e)
        {

        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            dataclass1.NextStepComponents();
           
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            backbutton1 = true;
            dataclass1.BackStepLicense();
        }

        private void License_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backbutton1 == false)
            {
                dataclass1.LicenseClosing();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this installation?", "Cancellation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dataclass1.LicenseClosing();
            }
        }
    }
}
