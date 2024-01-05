using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace cobol2cs
{
    class Program
    {
        public static ArrayList outputdata1 = new ArrayList();
        private static ArrayList functionsoutput = new ArrayList();
        private static int functionscount = 0;
		private static ArrayList variables1 = new ArrayList();

        static void Main(string[] args)
        {
            Console.Write("Please enter the PL/I file path:");
            try
            {
                //open file to get contents
                string cobolpath = Console.ReadLine();
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                
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
				int count3 = 0;
				string[] lines1 = new string[list1.Count];
				foreach(string str2 in list1)
				{
					lines1[count3] = (string)list1[count3];
					count3++;
				}
                Console.WriteLine("File read complete.");
				Console.WriteLine(lines1.Length);
                ProcessCode(lines1, 0);
                
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

        private static string GetExpression(string string1)
        {
            string1 = string1.Replace("=", "==");
			Console.WriteLine(string1);
            return string1;
        }

        public static bool isNumeric(string str1)
        {
            for (int i = 0; i < str1.Length; i++)
            {
                if (str1.Substring(i, 1) == "0" || str1.Substring(i, 1) == "1" || str1.Substring(i, 1) == "2" || str1.Substring(i, 1) == "3" || str1.Substring(i, 1) == "4" || str1.Substring(i, 1) == "5" || str1.Substring(i, 1) == "6" || str1.Substring(i, 1) == "7" || str1.Substring(i, 1) == "8" || str1.Substring(i, 1) == "9")
                {
                    //no action needed
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        private static string GetStringVariable(string string1)
        {
            string addvalue1 = "";
			string addvalue2 = "";
            string vartype1 = "";
			
			
            if (string1.ToUpper().IndexOf("DCL ") > -1 && string1.ToUpper().IndexOf("CHAR(") > -1)
            {
				//FIXED INIT;
                addvalue1 =  string1.Substring(string1.IndexOf("DCL ") + 5,  string1.ToUpper().IndexOf("CHAR(") - string1.IndexOf("DCL ") + 5);
				if(string1.ToUpper().IndexOf("INIT") > -1)
				{
					addvalue2 = string1.Substring(string1.IndexOf("DCL ") + 5,  string1.ToUpper().Length - string1.ToUpper().IndexOf("DCL ") + 5);
					addvalue2 = addvalue2.Substring(addvalue2.IndexOf("(") + 1, (addvalue2.IndexOf(")") - addvalue2.IndexOf("(") + 1));
					while(addvalue2.Contains("'"))
					{
						addvalue2 = addvalue2.Replace("'", "");
					}
				}
				//for(int i=0; i < value2; i++)
            }
			
			if (string1.ToUpper().IndexOf("DCL ") > -1 && string1.ToUpper().IndexOf("DEC(") > -1)
            {
				addvalue1 =  string1.Substring(string1.IndexOf("DCL ") + 5,  string1.ToUpper().IndexOf("DEC(") - string1.IndexOf("DCL ") + 5);
				if(string1.ToUpper().IndexOf("INIT") > -1)
				{
					//FIXED INIT;
					addvalue2 = string1.Substring(string1.IndexOf("DCL ") + 4,  string1.ToUpper().Length - string1.ToUpper().IndexOf("DCL ") + 4);
					addvalue2 = addvalue2.Substring(addvalue2.IndexOf("(") + 1, (addvalue2.IndexOf(")") - addvalue2.IndexOf("(") + 1));
				}
            }
			
			return vartype1;

        }

		private static string addtabs(int tablevel1)
		{
			string temptabs1 = "";
			for(int i = 0; i < tablevel1; i++)
			{
				temptabs1 = temptabs1 + "\t";
			}
			return temptabs1;
		}
		
		private static string removequotes(string noquotes1)
		{
			if(noquotes1.IndexOf("'") > -1)
			{
				int firstquoteindex = noquotes1.IndexOf("'");
				int nextquoteindex = noquotes1.Substring(firstquoteindex + 1).IndexOf("'");
				noquotes1 = noquotes1.Replace(noquotes1.Substring(firstquoteindex, nextquoteindex - firstquoteindex), "");
			}
			if(noquotes1.IndexOf("\"") > -1)
			{
				int firstquoteindex = noquotes1.IndexOf("\"");	
				int nextquoteindex = noquotes1.Substring(firstquoteindex + 1).IndexOf("\"");
				noquotes1 = noquotes1.Replace(noquotes1.Substring(firstquoteindex, nextquoteindex - firstquoteindex), "");
			}
			noquotes1 = modifyfunctions(noquotes1); 
			return noquotes1;
		}
		
		private static string modifyfunctions(string modfunc1)
		{
			if(modfunc1.ToUpper().IndexOf("ABS") > -1)
			{
			}
			return modfunc1;
		}
		
		private static bool CheckString(string string1)
		{
			for(int i = 0; i < string1.Length; i++)
			{
				for(int i2 = 65; i2 <= (65 + 26); i2++)
				{
					if(string1.ToUpper().Substring(i, 1) == Convert.ToString(i2))
					{
						return true;
					}
				}
			}
			return false;
		}

        private static void ProcessCode(string[] lines1, long count1)
        {
			int tablevel = 0;
			string stringnoquotes1 = removequotes(lines1[count1].ToUpper());
			string stringwithquotes1 = lines1[count1].ToUpper();
			
			
            while (count1 < lines1.Length)
            {
				
				if (lines1[count1].IndexOf("/*") > -1 && lines1[count1].IndexOf("*/") > -1)
				{
					string temp1 = stringnoquotes1.Replace(" ", "");
					while(temp1.IndexOf(" ") > -1)
					{
						temp1 = temp1.Replace(" ", "");
					}
					
					if(stringnoquotes1.StartsWith(temp1) > -1)
					{
						outputdata1.Add(addtabs(tablevel) + temp1);
					}
				}
				
				if(stringnoquotes1.IndexOf("=") > -1)
				{
					foreach(string variable1 in variables1)
					{
						string temp1 = stringnoquotes1.Replace(" ", "");
						while(temp1.IndexOf(" ") > -1)
						{
							temp1 = temp1.Replace(" ", "");
						}
						
						if(stringnoquotes1.StartsWith(variable1) > -1)
						{
							outputdata1.Add(addtabs(tablevel) + temp1);
						}
					}
				}
				
				
				if (stringnoquotes1.IndexOf("DCL ") > -1)
                {	
					Console.WriteLine("variable being added.");
					outputdata1.Add(addtabs(tablevel) + GetStringVariable(stringnoquotes1));
					Console.WriteLine("variable added.");
                }
                if (stringnoquotes1.IndexOf("DO ") > -1 && stringnoquotes1.IndexOf("=") > -1 && stringnoquotes1.IndexOf(" TO ") > -1)
                {
					Console.WriteLine("loop being added.");
                    string string1 = GetExpression(lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("UNTIL ") + 7, lines1[count1].Length - (lines1[count1].ToUpper().IndexOf("UNTIL ") + 7)));
                    outputdata1.Add(addtabs(tablevel) + "while()\n" + addtabs(tablevel) + "{");
					tablevel++;
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("loop added.");
                }
				
				if (stringnoquotes1.IndexOf("SELECT") > -1 && stringnoquotes1.IndexOf("(") > -1 && stringnoquotes1.IndexOf(")") > -1)
                {
					Console.WriteLine("loop being added.");
                    string string1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("(") + 1, ((lines1[count1].ToUpper().IndexOf(")") -1) - lines1[count1].IndexOf(")") + 1));
                    outputdata1.Add(addtabs(tablevel) + "switch(" + string1 + ")\n" + addtabs(tablevel) + "{");
					tablevel++;
					while(lines1[count1 + 1].IndexOf("WHEN") > -1)
					{
						string string2 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("(") + 1, ((lines1[count1].ToUpper().IndexOf(")") -1) - lines1[count1].IndexOf(")") + 1));
						outputdata1.Add(addtabs(tablevel) + "case " + string2 + ":");
						outputdata1.Add(addtabs(tablevel) + "break;");
					}
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("loop added.");
                }
				
				if (stringnoquotes1.IndexOf("PUT SKIP LIST ") > -1)
                {
					Console.WriteLine("text output being added.");
					string addvalue2 = "";
					
					//FIXED INIT;
					addvalue2 = stringnoquotes1.Substring(stringnoquotes1.IndexOf("LIST ") + 5,  stringnoquotes1.ToUpper().Length - stringnoquotes1.ToUpper().IndexOf("INIT " + 5));
					addvalue2 = addvalue2.Substring(addvalue2.IndexOf("(") + 1, (addvalue2.IndexOf(")") - addvalue2.IndexOf("(") + 1));
					
					outputdata1.Add(addtabs(tablevel) + "Console.WriteLine(" + addvalue2 + ");");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("text output added.");
                }
				
				if (stringnoquotes1.IndexOf("GET EDIT ") > -1)
                {
					string addvalue2 = "";
					Console.WriteLine("text output being added.");
					
					addvalue2 = stringnoquotes1.Substring(stringnoquotes1.IndexOf("LIST ") + 5,  stringnoquotes1.ToUpper().Length - stringnoquotes1.ToUpper().IndexOf("INIT " + 5));
					addvalue2 = addvalue2.Substring(addvalue2.IndexOf("(") + 1, (addvalue2.IndexOf(")") - addvalue2.IndexOf("(") + 1));
					
					outputdata1.Add(addtabs(tablevel) + "string " + addvalue2 + " = Console.ReadLine();");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("text output added.");
                }
				
				
				if(stringnoquotes1.IndexOf("IF ") > -1 && stringnoquotes1.IndexOf("THEN ") > -1 && stringnoquotes1.IndexOf("(") > -1 && stringnoquotes1.IndexOf(")") > -1)
				{
					string addvalue2 = "";
					Console.WriteLine("text output being added.");
					addvalue2 = stringnoquotes1.Substring(stringnoquotes1.IndexOf("LIST ") + 5,  stringnoquotes1.ToUpper().Length - stringnoquotes1.ToUpper().IndexOf("INIT " + 5));
					addvalue2 = addvalue2.Substring(addvalue2.IndexOf("(") + 1, (addvalue2.IndexOf(")") - addvalue2.IndexOf("(") + 1));
					addvalue2 = addvalue2.Replace("=", "==");
					outputdata1.Add(addtabs(tablevel) + "if(" + addvalue2 + ")\n{");
					tablevel++;
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					if(lines1[count1 + 1].IndexOf("DO") > -1)
					{
						count1++;
					}
					
					Console.WriteLine("text output added.");
                }
				
				if(stringnoquotes1.IndexOf("ELSE"))
				{
					Console.WriteLine("else being added.");
					string addvalue2 = "";
					Console.WriteLine("text output being added.");
					outputdata1.Add(addtabs(tablevel) + "}else" + addvalue2 + "\n{");
					tablevel++;
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("else added.");
                }
				
				if (stringnoquotes1.IndexOf("END;") > -1 || (stringnoquotes1.IndexOf("END ") > -1 && stringnoquotes1.IndexOf(";") > -1))
                {
					Console.WriteLine("END being added");
					tablevel++;
					outputdata1.Add(addtabs(tablevel) + "}");
					Console.WriteLine("END added.");
                }
                count1++;
            }
        }
    }
}
