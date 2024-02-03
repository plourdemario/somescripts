using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Installer
{
    public class Dataclass
    {
        private bool PWAW = true;
        private bool python = false;
        private bool apache = false;
        private bool upgrade = false;
        private bool license = true;
        private string installlocation = "";
        private License license1;
        private Components components1;
        private End end1;
        private Intro intro1;

        public Dataclass (string installlocation1, Intro introtemp1)
        {
            installlocation = installlocation1;
            intro1 = introtemp1;
        }

        public void CreateShortcutURL(string name, string url)
        {
            string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string deskDir2 = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);

            using (StreamWriter writer = new StreamWriter(deskDir + "\\" + name + ".url"))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + url);
                writer.WriteLine("IconIndex = 0");
                writer.WriteLine("IconFile = " + GetInstallPath() + "\\PWAW\\PWAW.ico");
                writer.WriteLine("HotKey = 0");
                writer.WriteLine("IDList =");
                writer.Flush();
            }
            using (StreamWriter writer2 = new StreamWriter(deskDir2 + "\\" + name + ".url"))
            {
                writer2.WriteLine("[InternetShortcut]");
                writer2.WriteLine("URL=" + url);
                writer2.WriteLine("IconIndex = 0");
                writer2.WriteLine("IconFile = " + GetInstallPath() + "\\PWAW\\PWAW.ico");
                writer2.WriteLine("HotKey = 0");
                writer2.WriteLine("IDList =");
                writer2.Flush();
            }
        }

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            /*string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
             WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "My shortcut description";   // The description of the shortcut
            shortcut.IconLocation = @"c:\myicon.ico";           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();*/                                    // Save the shortcut
        }

        public void SetPWAWInstalled()
        {
            python = true;
        }

        public string GetUninstallPath()
        {
            string string1 = "{078AF4DE-FA37-4D78-AF4A-0D1E327451AB}";
            string1 = "C:\\ProgramData\\Package Cache\\" + string1;
            return string1;
        }

        public string GetInstallPath()
        {
            return installlocation;
        }

        public void SetInstallPath(string installpathtemp1)
        {
            installlocation = installpathtemp1;
        }

        public void BackStepLicense()
        {
            license1.Close();
            intro1.Visible = true;
        }

        public void BackStepComponents()
        {
            components1.Close();
            license1.Visible = true;
        }

        public void BackStepEnd()
        {
            end1.Close();
            components1.Visible = true;
        }

        public void NextStepLicense()
        {
            intro1.Visible = false;
            license1 = new License(this);
            license1.Show();
        }

        public void NextStepComponents()
        {
            license1.Visible = false;
            components1 = new Components(this);
            components1.Show();
        }

        public void NextStepEnd()
        {
            components1.Visible = false;
            end1 = new End(this);
            end1.Show();
        }

        public void EndClosing()
        {
            components1.Close();
            license1.Close();
            intro1.Close();
        }

        public void ComponentsClosing()
        {
            license1.Close();
            intro1.Close();
        }

        public void LicenseClosing()
        {
            intro1.Close();
        }
    }
}
