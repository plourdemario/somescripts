using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.IO.FileSystem;
//or using System.IO.ZipFile for .NET Core net46

namespace cobol2cs
{
    class Program
    {
        private static ArrayList jobvariables1 = new ArrayList();
        private static ArrayList functionsoutput = new ArrayList();
        private static int datasetlines = 0;
        private static int functionscount = 0;
		

        static void Main(string[] args)
        {
            Console.Write("Please enter the COBOL file path:");
            try
            {
                //open file to get contents
                string cobolpath = Console.ReadLine();
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                string str = "";
				ArrayList list1 = new ArrayList();
                using (StreamReader sr = new StreamReader(cobolpath))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        list1.Add(line);
                    
                    }
                }

                //read all
                /*
                using (FileStream fs = File.Open(cobolpath, FileMode.Open))
                {
                    byte[] b = new byte[1024];
                    UTF8Encoding temp = new UTF8Encoding(true);

                    while (fs.Read(b, 0, b.Length) > 0)
                    {
                        str = str + temp.GetString(b).ToUpper();
                    }
                }*/
                str = str.Replace("\r", "\n");
                string[] lines1 = new string[list1.Count];
				int count3 = 0;
				foreach(string str2 in list1)
				{
					lines1[count3] = (string)list1[count3];
					count3++;
				}
                Console.WriteLine("File read complete.");
				Console.WriteLine(lines1.Length);
                for (long i = 0; i < lines1.Length; i++)
                {
					//Comment statemeent //*
                    if (lines1[i].ToUpper().IndexOf("//*") > -1)
                    {
                        jobvariables1 = ProcessCommnent(lines1, i);
                        Console.WriteLine("");
                    }else
					{
						//Main statement //						
						if (lines1[i].ToUpper().IndexOf("//") > -1)
						{
							comments1 = ProcessLine(lines1, i);
							Console.WriteLine("");
						}
						else
						{
							//delimiter statement /*
							if (lines1[i].ToUpper().IndexOf("/*") > -1)
							{
								delimiters1 = ProcessDelimiter(lines1, i);
								Console.WriteLine("");
							}
						}
					}
					
                }
				
				StreamWriter sr2 = new StreamWriter("C:\\temp\\test.cs");
				
				
				for(int i3 = 0; i3 < outputdata1.Count; i3++)
				{
					string stringdata1 = outputdata1[i3].ToString();
					sr2.Write(stringdata1);
				}
				sr2.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
		private static ArrayList ProcessLine(ArrayList lines1, long count1)
		{
			//JOB(XXXX),CLASS=A,MSGLASS=H,NOTIFY=&SYSUID COMMENT
			ArrayList variables1 = new ArrayList();
			if (lines1[count1].ToUpper().IndexOf("JOBNAME") > -1)
			{
				if (lines1[count1].ToUpper().Replace(" ", "").IndexOf("JOB(") > -1)
				{
					string templine1[] = lines1[count1].Split(",");
					ArrayList variables1 = new ArrayList();
					for(int i = 0; i < templine1.Count; i++)
					{
						if (templine1[i].ToUpper().Replace(" ", "").IndexOf("CLASS=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("CLASS=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("CLASS=") + 6)));
						
						}
						if (templine1[i].ToUpper().Replace(" ", "").IndexOf("MSGLEVEL=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("MSGLEVEL=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("MSGLEVEL=") + 6)));
						
						}
						if (templine1[i].ToUpper().Replace(" ", "").IndexOf("MSGCLASS=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("MSGCLASS=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("MSGCLASS=") + 6)));
						
						}
						if (templine1[i].ToUpper().Replace(" ", "").IndexOf("NOTIFY=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("NOTIFY=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("NOTIFY=") + 6)));
						
						}
						if (templine1[i].ToUpper().Replace(" ", "").IndexOf("COND=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("COND=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("COND=") + 6)));
						
						}if (templine1[i].ToUpper().Replace(" ", "").IndexOf("REGION=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("REGION=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("REGION=") + 6)));
						
						}if (templine1[i].ToUpper().Replace(" ", "").IndexOf("PRTY=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("PRTY=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("PRTY=") + 6)));
						
						}if (templine1[i].ToUpper().Replace(" ", "").IndexOf("RESTART=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("RESTART=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("RESTART=") + 6)));
						
						}if (templine1[i].ToUpper().Replace(" ", "").IndexOf("TYPRUN=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("TYPRUN=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("TYPRUN=") + 6)));
						
						}if (templine1[i].ToUpper().Replace(" ", "").IndexOf("TIME=") > -1)
						{
							variables1.Add(templine1[i].Substring(templine1[i].ToUpper().IndexOf("TIME=") + 6, templine1[i].Length - (templine1[i].ToUpper().IndexOf("TIME=") + 6)));
						
						}
				
					}
				}
				
				
			}
			
			if (lines1[count1].ToUpper().IndexOf("EXECNAME") > -1)
			{
				//EXEC PGM=HELLO
			}

			if (lines1[count1].ToUpper().IndexOf("DDNAME") > -1)
			{
				//DSNAME=SUBDATASET.DS.DS,DISP=SHR
			}


			return variables1;
			
		}
		
		private static ArrayList ProcessCommnent(ArrayList lines1, long count1)
		{
		}
		
		private static ArrayList ProcessLink(ArrayList lines1, long count1)
		{
			return count1;
		}
				
		private static string CreateTerraform(string gitrepo1, string compilelocation1, int priority1, string class1, string region1)
		{
			//string ram1, string cpu1, string harddrivetype1, string harddrivesize1 derived from priority1 and class1 parameters
			//git commands to run on VM
			//Run git init;
			//
			
			variables-template
			
			str = "";
			(StreamReader sr = new StreamReader("C:\\temp\\COBOL\\Windows-new\\variables-template.tf"))
			{
				string line;
				// Read and display linses from the file until the end of
				// the file is reached.
				while ((line = sr.ReadLine()) != null)
				{
					str = str + line + "\r\n";
				
				}
			}

			
			
			"commandToExecute": "powershell -command \"[System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String('${base64encode(data.template_file.tf.rendered)}')) | Out-File -filepath dcpromo.ps1\" && powershell -ExecutionPolicy Unrestricted -File dcpromo.ps1 ${azurerm_windows_virtual_machine.vm.admin_password} domain.com" 
			sr.Close();
			
			
			StreamWriter sr2 = new StreamWriter("C:\\temp\\COBOL\\Windows-pooled\\variables.tf");
			
			
			for(int i3 = 0; i3 < outputdata1.Count; i3++)
			{
				string stringdata1 = outputdata1[i3].ToString();
				sr2.Write(stringdata1);
			}
			sr2.Close();
		
			DirectoryInfo dir1 = new DirectoryInfo(@"\c:\temp\COBOL\Windows-new\");
			
			FileInfo files1 = info1.GetFiles("*.tf");
			
			using (ZipArchive zip = ZipFile.Open("output.zip", ZipArchiveMode.Create))
			{
				foreach(FileInfo file1 in files1)
				{
					zip.CreateEntryFromFile(@"C:\temp\COBOL\Windows-new\" + file1.Name, file1.Name);
				}
			}
			return count1;
		}
	}
	}
}