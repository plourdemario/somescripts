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
using Microsoft.Win32;
using System.Globalization;

namespace Installer
{
    public partial class End : Form
    {
        private Dataclass dataclass1;
        private bool backbutton1 = false;
        public End(Dataclass dataclasstemp1)
        {
            InitializeComponent();
            dataclass1 = dataclasstemp1;
            dataclass1.SetInstallPath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
        }

        public void SetActiveNow(Dataclass dataclasstemp1)
        {
            dataclass1 = dataclasstemp1;
            this.Visible = true;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly1 = System.Reflection.Assembly.GetExecutingAssembly();
            Stream stream1 = assembly1.GetManifestResourceStream("Installer.CodeGenerator.exe");
            
            if (Directory.Exists (dataclass1.GetInstallPath() + "\\PWAW") != true)
            {
                Directory.CreateDirectory(dataclass1.GetInstallPath() + "\\PWAW");
            }
            if(Directory.Exists(dataclass1.GetUninstallPath()) != true)
            {
                Directory.CreateDirectory(dataclass1.GetUninstallPath());
            }
            FileStream stream2 = new FileStream(dataclass1.GetInstallPath() + "\\PWAW\\CodeGenerator.exe", FileMode.Create);
            stream1.CopyTo(stream2);
            stream1.Close();
            stream2.Close();

            stream1 = assembly1.GetManifestResourceStream("Installer.pwaw.ico");
            stream2 = new FileStream(dataclass1.GetInstallPath() + "\\PWAW\\PWAW.ico", FileMode.Create);
            stream1.CopyTo(stream2);
            stream1.Close();
            stream2.Close();

            stream1 = assembly1.GetManifestResourceStream("Installer.MySql.Data.dll");
            stream2 = new FileStream(dataclass1.GetInstallPath() + "\\PWAW\\MySql.Data.dll", FileMode.Create);
            stream1.CopyTo(stream2);
            stream1.Close();
            stream2.Close();

            stream1 = assembly1.GetManifestResourceStream("Installer.System.Data.SQLite.dll");
            stream2 = new FileStream(dataclass1.GetInstallPath() + "\\PWAW\\System.Data.SQLite.dll", FileMode.Create);
            stream1.CopyTo(stream2);
            stream1.Close();
            stream2.Close();

            if (Directory.Exists(dataclass1.GetUninstallPath()) != true)
            {
                Directory.CreateDirectory(dataclass1.GetUninstallPath());
            }

            Stream stream3 = assembly1.GetManifestResourceStream("Installer.UninstallPyWebWizard.exe");
            FileStream stream4 = new FileStream(dataclass1.GetUninstallPath() + "\\uninstall.exe", FileMode.Create);
            stream3.CopyTo(stream4);
            stream3.Close();
            stream4.Close();
            RegistryKey key1 = Registry.LocalMachine.OpenSubKey("Software");
            RegistryKey key2 = key1.OpenSubKey("Microsoft");
            RegistryKey key3 = key2.OpenSubKey("Windows");
            RegistryKey key4 = key3.OpenSubKey("CurrentVersion");
            RegistryKey key5 = key4.OpenSubKey("Uninstall", true);
            key5.CreateSubKey("{078AF4DE-FA37-4D78-AF4A-0D1E327451AB}");
            RegistryKey key6 = key5.OpenSubKey("{078AF4DE-FA37-4D78-AF4A-0D1E327451AB}", true);
            key6.SetValue("Comments", "", RegistryValueKind.String);
            key6.SetValue("Contact", "", RegistryValueKind.String);
            key6.SetValue("DisplayName", "Python 3 Web Application Wizard", RegistryValueKind.String);
            key6.SetValue("DisplayVersion", "1.0", RegistryValueKind.String);
            key6.SetValue("EstimateSize", 532000, RegistryValueKind.DWord);
            key6.SetValue("HelpLink", "", RegistryValueKind.ExpandString);
            key6.SetValue("HelpTelephone", "", RegistryValueKind.String);
          
            key6.SetValue("InstallDate", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString(), RegistryValueKind.String);
            key6.SetValue("InstallLocation", dataclass1.GetInstallPath(), RegistryValueKind.String);
            key6.SetValue("InstallSource", "", RegistryValueKind.String);
            key6.SetValue("Language", 1033, RegistryValueKind.DWord);
            key6.SetValue("ModifyPath", dataclass1.GetUninstallPath() + "\\uninstall.exe", RegistryValueKind.String);
            key6.SetValue("NoRepair", 1, RegistryValueKind.DWord);
            key6.SetValue("Publisher", "Aqesda Software", RegistryValueKind.String);
            key6.SetValue("Readme", "", RegistryValueKind.ExpandString);
            key6.SetValue("Size", "", RegistryValueKind.String);
            key6.SetValue("SystemComponent", 0, RegistryValueKind.DWord);
            key6.SetValue("UninstallString", dataclass1.GetUninstallPath() + "\\uninstall.exe", RegistryValueKind.String);
            key6.SetValue("URLInfoAbout", "", RegistryValueKind.String);
            key6.SetValue("URLUpdateInfo", "", RegistryValueKind.String);
            key6.SetValue("Version", 1, RegistryValueKind.DWord);
            key6.SetValue("VersionMajor", 2, RegistryValueKind.DWord);
            key6.SetValue("VersionMinor", 1, RegistryValueKind.DWord);
            key6.SetValue("WindowsInstaller", 0, RegistryValueKind.DWord);
            dataclass1.CreateShortcutURL("Python 3 Web Application Wizard", dataclass1.GetInstallPath() + "\\PWAW\\CodeGenerator.exe");
            this.Close();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            backbutton1 = true;
            dataclass1.BackStepEnd();
        }

        private void End_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backbutton1 == false)
            {
                dataclass1.EndClosing();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this installation?", "Cancellation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                dataclass1.EndClosing();
            }
        }
    }
}
