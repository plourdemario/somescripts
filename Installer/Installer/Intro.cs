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
using System.Globalization;

namespace Installer
{
    public partial class Intro : Form
    {
        private Dataclass dataclass1;
        private License license1 = null;
        public Intro()
        {
            //Guid obj = Guid.NewGuid();
            //MessageBox.Show(obj.ToString());
            
            try
            {
                RegistryKey key1 = Registry.LocalMachine.OpenSubKey("Software");
                RegistryKey key2 = key1.OpenSubKey("Microsoft");
                RegistryKey key3 = key2.OpenSubKey("Windows");
                RegistryKey key4 = key3.OpenSubKey("CurrentVersion");
                dataclass1 = new Dataclass(key4.GetValue("ProgramFilesDir").ToString(), this);
                RegistryKey key5 = key4.OpenSubKey("Uninstall", true);
                RegistryKey key6 = key5.OpenSubKey("{078AF4DE-FA37-4D78-AF4A-0D1E327451AB}", true);
                if (key6 != null)
                {
                    dataclass1.SetPWAWInstalled();   
                }else
                {
                    
                }
                InitializeComponent();
            }
            catch(Exception e1)
            {
                //MessageBox.Show(e1.Message);
                MessageBox.Show("This installer needs to be run as administrator.");
            }

        }
        
        private void label1_Click(object sender, EventArgs e)
        {

        }
        
        private void NextButton_Click(object sender, EventArgs e)
        {
            dataclass1.NextStepLicense();
            
        }

        private void Intro_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to cancel this installation?", "Cancellation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dataclass1.LicenseClosing();
            }
        }
    }
}
