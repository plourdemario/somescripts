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
                    if (lines1[i].ToUpper().IndexOf("IDENTIFICATION DIVISION.") > -1)
                    {
                        i = i + ProcessIdentificationCode(lines1, i);
                        Console.WriteLine("Identification Division code processed.");
                    }
                    if (lines1[i].ToUpper().IndexOf("ENVIRONMENT DIVISION.") > -1)
                    {
                        i = i + ProcessEnvironmentDivisionCode(lines1, i);
                        Console.WriteLine("Environment Division code processed.");
                    }
                    if (lines1[i].ToUpper().IndexOf("DATA DIVISION.") > -1)
                    {
                        i = i + ProcessDataDivisionCode(lines1, i);
                        Console.WriteLine("Data Division code processed.");
                    }
					
					//why this???
					i = i -3;
					Console.WriteLine("got here");
					Console.WriteLine(i.ToString());
                    if (lines1[i].ToUpper().IndexOf("PROCEDURE DIVISION.") > -1)
                    {
                        i = i + ProcessProcedureCode(lines1, i);
                        Console.WriteLine("Procedure Division code processed.");
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

        private static string GetExpression(string string1)
        {
            string1 = string1.Replace("EQUAL TO", "==");
            string1 = string1.Replace("LESS THAN", "<");
            string1 = string1.Replace("GREATER THAN", ">");
            string1 = string1.Replace("IS POSITIVE", "> 0");
            string1 = string1.Replace("IS NEGATIVE ", "< 0");
            string1 = string1.Replace("IS ZERO ", "== 0");
            string1 = string1.Replace("IS ALPHABETIC ", "!= int.TryParse(string1, out n)");
            string1 = string1.Replace("IS NUMERIC ", "== int.TryParse(string1, out n)");
			string1 = string1.Replace("NOT ", "!");
			string1 = string1.Replace("AND ", " && ");
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

        public static long ProcessIdentificationCode(string[] lines1, long count1)
        {
			Console.WriteLine("Process identification started.");
			//Console.Write(lines1[count1]);
            while (lines1[count1].ToUpper().IndexOf("DATA DIVISION.") < 0 && lines1[count1].ToUpper().IndexOf("PROCEDURE DIVISION.") < 0)
            {
                count1++;
				
                if (lines1[count1].IndexOf("PROGRAM-ID.") > -1)
                {
                    string right1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("PROGRAM-ID.") + 11, lines1[count1].Length - lines1[count1].IndexOf("PROGRAM-ID.") - 11);
                    outputdata1.Add("// PROGRAM-ID. " + right1);
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
                }

                if (lines1[count1].IndexOf("AUTHOR.") > -1)
                {
                    string right1 = lines1[count1].Substring(lines1[count1].IndexOf("AUTHOR.") + 8, lines1[count1].Length - lines1[count1].IndexOf("PROGRAM-ID.") - 8);
                    outputdata1.Add("// AUTHOR. " + right1);
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
                }
            }
            return count1;
        }

        private static long ProcessEnvironmentDivisionCode(string[] lines1, long count1)
        {
			Console.WriteLine("Process environment division started.");
            while (lines1[count1].ToUpper().IndexOf("DATA DIVISION.") < 0 && lines1[count1].ToUpper().IndexOf("PROCEDURE DIVISION.") < 0)
            {
                count1++;
                if (lines1[count1].IndexOf("CONFIGURATION SECTION.") > -1)
                {
                    count1++;
                    while (lines1[count1].IndexOf("INPUT-OUTPUT SECTION.") < 0 && lines1[count1].IndexOf("PROCEDURE DIVISION.") < 0)
                    {
                        count1++;
                    }
                }
                if (lines1[count1].IndexOf("INPUT-OUTPUT SECTION.") == -1)
                {
                    count1++;
                    if (lines1[count1].IndexOf("FILE-CONTROL.") == -1)
                    {
                        count1++;
                        while (lines1[count1].IndexOf("PROCEDURE DIVISION.") == -1)
                        {
                            count1++;
                        }
                    }
                }
            }
            return count1;
        }

        private static string GetStringVariable(string string1)
        {
            string addvalue1 = "";
			string addvalue2 = "";
            string vartype1 = "";
			string lastvariable = "";
			
			int length1 = 0;
			
            if (string1.ToUpper().IndexOf("VALUES ARE ") > -1)
            {
				//Console.WriteLine("VALUES ARE entry");
				//Console.WriteLine((string1.IndexOf("THRU ") - (string1.IndexOf("VALUES ARE ") + 11)).ToString());
				//Console.WriteLine((string1.IndexOf("VALUES ARE ") + 11).ToString());
                addvalue1 = string1.Substring(string1.IndexOf("VALUES ARE ") + 11,  string1.IndexOf("THRU ") - (string1.IndexOf("VALUES ARE ") + 11));
				addvalue2 =  string1.Substring(string1.IndexOf("THRU ") + 5, string1.Length - (string1.IndexOf("THRU ") + 5));
				addvalue1 = lastvariable + " > " + addvalue1 + " && " + lastvariable + " < " + addvalue2;
				//for(int i=0; i < value2; i++)
            }else
			{
				
				if (string1.ToUpper().IndexOf("VALUE ") > -1)
				{
					//Console.WriteLine("VALUE entry");
					addvalue1 = string1.Substring(string1.IndexOf("VALUE ") + 6, string1.Length - (string1.IndexOf("VALUE ") + 6));
					//Console.WriteLine("VALUE DONE");
				}
			}
			
			
			
            if (string1.ToUpper().IndexOf("PIC") > -1 && string1.ToUpper().IndexOf("X(") > -1)
            {
				Console.WriteLine("string entry");
                string str1 = string1.Substring(string1.IndexOf("(") + 1, string1.IndexOf(")") - (string1.IndexOf("(") + 1));
                length1 = str1.Length * 10;
                vartype1 = "string = " + length1.ToString();
				Console.WriteLine("string exit");
            }
			
			
			
            if (string1.ToUpper().IndexOf("PIC") > -1 && string1.ToUpper().IndexOf("A(") > -1)
            {
				Console.WriteLine("char entry");
                vartype1 = "char ";
            }

			
			
			Console.WriteLine("entry point 1");

			
            if (string1.ToUpper().IndexOf("PIC") > -1 && string1.ToUpper().IndexOf("99V99") > -1)
            {
				Console.WriteLine("decimal entry");
            }else
            {
				if (string1.ToUpper().IndexOf("PIC") > -1 && string1.ToUpper().IndexOf("S9") > -1)
				{
					//Console.WriteLine("string entry 2");
					string str1 = string1.Substring(string1.IndexOf("(") + 1, (string1.IndexOf(")")  - string1.IndexOf("(")));
					length1 = str1.Length * 10;
					vartype1 = "int ";
				}else
				{
					if (string1.ToUpper().IndexOf("PIC") > -1 && string1.ToUpper().IndexOf("9") > -1)
					{
						//Console.WriteLine("int entry");
						if (string1.Substring(string1.IndexOf("(") + 1, string1.Length - string1.IndexOf(")")) == "9")
						{
							string str1 = string1.Substring(string1.IndexOf("(") + 1, string1.Length - string1.IndexOf(")"));
							vartype1 = "int ";
						}
					}
				}
            }

            int count1 = 0;
            string varname1 = "";
            while (varname1 == "")
            {
				//Console.WriteLine("variable name entry");
                while (string1.Substring(count1, 1).IndexOf("0") > -1 || string1.Substring(count1, 1).IndexOf("1") > -1 || string1.Substring(count1, 1).IndexOf("2") > -1 || string1.Substring(count1, 1).IndexOf("3") > -1 || string1.Substring(count1, 1).IndexOf("4") > -1 || string1.Substring(count1, 1).IndexOf("5") > -1 || string1.Substring(count1, 1).IndexOf("6") > -1 || string1.Substring(count1, 1).IndexOf("7") > -1 || string1.Substring(count1, 1).IndexOf("8") > -1 || string1.Substring(count1, 1).IndexOf("9") > -1)
                {
                    count1++;
                    varname1 = "found";
					Console.WriteLine("variable number passed");
                }
                if (varname1 == "found")
                {
                    while (string1.Substring(count1, 1).IndexOf(" ") > -1 || string1.Substring(count1, 1).IndexOf("\t") > -1)
                    {
                        count1++;
                    }
					Console.WriteLine("variable spaces removed");
                    varname1 = string1.Substring(count1 - 1, string1.Substring(count1).IndexOf(" "));
                }
                count1++;
            }
			addvalue1 = addvalue1.Trim('.');
            string1 = vartype1 + " " + varname1 + " = " + addvalue1 + ";\n";
            return string1;
        }


        private static long ProcessFileName(string[] lines1, long count1)
        {
            while (lines1[count1] != "")
            {
                int endstringstart1 = lines1[count1].Length - 12;
                if (lines1[count1].Substring(endstringstart1, lines1[count1].Length - endstringstart1) == "RECORD-NAME.")
                {
                    count1++;
                    outputdata1.Add(GetStringVariable(lines1[count1]));
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
                }
                count1++;
            }
            return count1;
        }


        private static long ProcessDataDivisionCode(string[] lines1, long count1)
        {
			Console.WriteLine("Processing Data Division section.");
            while (lines1[count1].ToUpper().IndexOf("PROCEDURE DIVISION.") < 0)
            {
				Console.WriteLine("Checking variables section.");
                if (lines1[count1].IndexOf("FILE SECTION.") > -1)
                {
					Console.WriteLine("Processing file section.");
                    string right1 = lines1[count1].Substring(7, lines1[count1].Length - 7);
                    outputdata1.Add("// Temporary variables to be used by the program" + right1 + "\t;");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
                    count1++;
                    ProcessFileName(lines1, count1);
					
                }

                if(lines1[count1].IndexOf("WORKING-STORAGE SECTION.") > -1)
                {
					count1++;
					while(lines1[count1].ToUpper().IndexOf("PIC") > -1 || lines1[count1].ToUpper().IndexOf("EXEC SQL BEGIN DECLARE SECTION") > -1 || lines1[count1].IndexOf("INCLUDE ") > -1 || lines1[count1].IndexOf("EXEC SQL BEGIN DECLARE SECTION") > -1 || lines1[count1].ToUpper().IndexOf("VALUES ") > -1 || lines1[count1].ToUpper().IndexOf("VALUES ARE") > -1)
					{
						
						Console.WriteLine("Processing working-storage section.");
						string string2 = "";
						if(lines1[count1].ToUpper().IndexOf("PIC") > -1)
						{
							Console.WriteLine("Adding a variable.");
							outputdata1.Add(GetStringVariable(lines1[count1]));
							Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
						}
						if(lines1[count1].ToUpper().IndexOf("VALUES ARE") > -1)
						{
							Console.WriteLine("Adding a variable.");
							outputdata1.Add(GetStringVariable(lines1[count1]));
							Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
						}
						

						if (lines1[count1].IndexOf("EXEC SQL BEGIN DECLARE SECTION") > -1)
						{
							string2 = "SELECT * FROM ";
						}

						if (lines1[count1].IndexOf("INCLUDE ") > -1)
						{
							if (lines1[count1].IndexOf("SQLCA") > -1)
							{
								
							}
							else
							{
								string2 = string2 + lines1[count1].Substring(lines1[count1].IndexOf("INCLUDE "), lines1[count1].Length - lines1[count1].IndexOf("INCLUDE "));
								outputdata1.Add(string2);
								Console.WriteLine(outputdata1.Count - 1);
							}
							//INCLUDE table-name
						}
						if (lines1[count1].IndexOf("EXEC SQL BEGIN DECLARE SECTION") > -1)
						{
							outputdata1.Add("OleDbConnection connection1 = new OleDbConnection();");
							outputdata1.Add("connection1.ConnectionString = connectstring1;");
							outputdata1.Add("connection1.Open();");
							outputdata1.Add("OdbcCommand command1 = new OdbcCommand(string2);");
							outputdata1.Add("data1 = \"\";");
							outputdata1.Add("OdbcDataReader reader1 = command1.ExecuteReader();");

							outputdata1.Add("while(reader1.Read())");
							outputdata1.Add("{");
							outputdata1.Add("\tdata1 = data1 + reader.GetString(0);");
							outputdata1.Add("}");
							Console.WriteLine("processed odbc");
						}

						if (lines1[count1].IndexOf("END-EXEC.") > -1)
						{
							outputdata1.Add("\n");
						}
						count1++;
					}
                }

                if (lines1[count1].IndexOf("LOCAL-STORAGE SECTION.") > -1)
                {
					Console.WriteLine("Processing local-storage section.");
                    string right1 = lines1[count1].Substring(7, lines1[count1].Length - 7);
                    outputdata1.Add("// Temporary variables to be used by the program and reallocated and initiated on every run." + right1 + "\t;");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
				
                }

                if (lines1[count1].IndexOf("LINKAGE SECTION.") > -1)
                {
                    string right1 = lines1[count1].Substring(7, lines1[count1].Length - 7);
                    outputdata1.Add("// Data names from other programs" + right1 + "\t;");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
                }


                count1++;
				Console.WriteLine(count1.ToString());
            }
			
            return count1;
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
				noquotes1 = noquotes1.Replace(noquotes1.Substring(firstquoteindex, nextquoteindex - firstquoteindex));
			}
			if(noquotes1.IndexOf("\"") > -1)
			{
				int firstquoteindex = noquotes1.IndexOf("\"");	
				int nextquoteindex = noquotes1.Substring(firstquoteindex + 1).IndexOf("\"");
				noquotes1 = noquotes1.Replace(noquotes1.Substring(firstquoteindex, nextquoteindex - firstquoteindex));
			}
			return noquotes1;
		}

        private static long ProcessProcedureCode(string[] lines1, long count1)
        {
			int tablevel = 0;
			Console.WriteLine("Processing Procedure division section.");
			string stringnoquotes1 = removequotes(lines1[count1].ToUpper());
			string stringwithquotes1 = lines1[count1].ToUpper();
            while (count1 < lines1.Length)
            {
				
				Console.WriteLine(count1.ToString());
				
                if (stringnoquotes1.IndexOf("PERFORM ") > -1 && stringnoquotes1.IndexOf("UNTIL") > -1)
                {
					Console.WriteLine("loop being added.");
                    string string1 = GetExpression(lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("UNTIL ") + 7, lines1[count1].Length - (lines1[count1].ToUpper().IndexOf("UNTIL ") + 7)));
                    outputdata1.Add(addtabs(tablevel) + "while()\n" + addtabs(tablevel) + "{");
					tablevel++;
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("loop added.");
                }else
				{
					if (stringnoquotes1.IndexOf("PERFORM ") > -1 && stringnoquotes1.IndexOf("TIMES ") > -1)
					{
						Console.WriteLine("loop 2 being added.");
						string multiplier1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("TIMES ") + 7, lines1[count1].Length - (lines1[count1].ToUpper().IndexOf("TIMES ") + 7));
						outputdata1.Add(addtabs(tablevel) + "for(int i = 0; i < " + multiplier1 + ";i++)\n" + addtabs(tablevel) + "{\n");
						tablevel++;
						Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
						Console.WriteLine("loop 2 added.");
					}else
					{
						if (stringnoquotes1.IndexOf("PERFORM ") > -1)
						{
							Console.WriteLine("new function being added.");
							string right1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("PERFORM ") + 9, lines1[count1].ToUpper().IndexOf(".") - (lines1[count1].ToUpper().IndexOf("PERFORM ") + 9));
							outputdata1.Add(addtabs(tablevel) + right1 + "();\n" + addtabs(tablevel) + "{");
							tablevel++;
							Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
							Console.WriteLine("new function added.");
						}else
						{
							if (stringnoquotes1.IndexOf("PERFORM ") < 0 && (stringnoquotes1.IndexOf("PARA.") > -1 || stringnoquotes1.IndexOf("PARA-") > -1 || stringnoquotes1.IndexOf("PARAGRAPH.") > -1))
							{
								Console.WriteLine("paragraph being added.");
								//add replace 
								string str1 = addtabs(tablevel) + "void " + lines1[count1].ToUpper().Substring(0, lines1[count1].Length - 1) + "()\n" + addtabs(tablevel) + "{\n";
								tablevel++;
								functionsoutput.Add("private static void " + lines1[count1].ToUpper().Substring(0, lines1[count1].Length - 1) + "();\n");
								functionscount++;
								outputdata1.Add(str1);
								Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
								Console.WriteLine("paragraph added.");
							}
						}
					}
				}
                if (stringnoquotes1.IndexOf("DISPLAY ") > -1)
                {
					Console.WriteLine("display being added.");
                    //add code for displaying a variable
                    string data1 = lines1[count1].ToUpper().Substring(lines1[count1].ToUpper().IndexOf("DISPLAY") + 6, lines1[count1].Length - (lines1[count1].ToUpper().IndexOf("DISPLAY") + 6));
                    data1 = data1.Trim('.');
					data1 = data1.TrimEnd();
					//my_str.replace(index, sub_str.Length, sub_str); 
					if (data1.ToUpper().IndexOf("\"") > -1)
					{
						data1 = data1.Substring(data1.IndexOf("\""), (data1.Length - data1.IndexOf("\"")));
					}else
					{
					
						if(data1.ToUpper().IndexOf("'") > -1)
						{
							
							data1 = data1.Substring(data1.IndexOf("'") + 1, (data1.Length - data1.IndexOf("'") - 2));
							data1 = "\"" + data1 +  "\"";
						}
						else
						{
							//get variable
							data1 = data1.Substring(data1.IndexOf("DISPLAY ") + 7, (data1.Length - (data1.IndexOf("DISPLAY ") + 7)));
							
						}
					}

                    
                    outputdata1.Add(addtabs(tablevel) + "Console.WriteLine(" + data1 + ");\n");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("display added.");
                }

                if (stringnoquotes1.IndexOf("INITIALIZE ") > -1)
                {
					Console.WriteLine("iniatilize variable being added.");
                    string right1 = lines1[count1].ToUpper().Substring(lines1[count1].ToUpper().IndexOf("INITIALIZE "), lines1[count1].IndexOf(".") - lines1[count1].ToUpper().IndexOf("INITIALIZE "));
                    outputdata1.Add(addtabs(tablevel) + right1);
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("iniatilize variable added.");
                }
                if (stringnoquotes1.IndexOf("ACCEPT ") > -1)
                {
					//Console.WriteLine("accept prompt being added.");
                    string right1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("ACCEPT ") + 7, lines1[count1].Length - (lines1[count1].ToUpper().IndexOf("ACCEPT ") + 7));
                    outputdata1.Add(addtabs(tablevel) + right1 + " = Console.ReadLine();\n;");
                    //Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					//Console.WriteLine("accept prompt added.");
                }

                if (stringnoquotes1.IndexOf("MOVE ") > -1 && stringnoquotes1.IndexOf("TO ") > -1)
                {
					Console.WriteLine("move value to another variable being added.");
                    string value1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("MOVE ") + 5, lines1[count1].ToUpper().IndexOf("TO") - (lines1[count1].ToUpper().IndexOf("MOVE ") + 5));
                    string right1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("TO ") + 3, lines1[count1].Length - (lines1[count1].ToUpper().IndexOf("TO ") + 3));
                    outputdata1.Add(addtabs(tablevel) + right1 + " = " + value1);
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("move value to another variable added.");
                }

                if (stringnoquotes1.IndexOf("ADD ") > -1 && stringnoquotes1.IndexOf("TO ") > -1)
                {
					Console.WriteLine("add to variable being added.");
                    string string1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("ADD ") + 5, lines1[count1].ToUpper().IndexOf("FROM ") - (lines1[count1].ToUpper().IndexOf("ADD ") + 5));
                    string string2 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("TO "), lines1[count1].Length - lines1[count1].ToUpper().IndexOf("TO "));
                    outputdata1.Add(addtabs(tablevel) + string1 + " = " + string1 + " + " + string2 + ";\n");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("add to variable added.");
                }

                if (stringnoquotes1.IndexOf("SUBTRACT ") > -1 && stringnoquotes1.IndexOf("FROM ") > -1)
                {
					Console.WriteLine("substract from variable being added.");
                    string string1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("SUBTRACT ") + 10, lines1[count1].ToUpper().IndexOf("FROM ") - (lines1[count1].ToUpper().IndexOf("SUBTRACT ") + 10));
                    string string2 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("FROM ") + 5, lines1[count1].Length - (lines1[count1].ToUpper().IndexOf("FROM ") + 5));
                    outputdata1.Add(addtabs(tablevel) + string1 + " = " + string1 + " - " + string2 + ";\n");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("substract from variable added.");

                }
                if (stringnoquotes1.IndexOf("MULTIPLY ") > -1 && stringnoquotes1.IndexOf("BY ") > -1)
                {
					Console.WriteLine("multiply variable being added.");
                    string string1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("MULTIPLY ") + 11, lines1[count1].ToUpper().IndexOf("BY ") - (lines1[count1].ToUpper().IndexOf("MULTIPLY ") + 11));
                    string string2 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("BY "), lines1[count1].Length - lines1[count1].ToUpper().IndexOf("BY "));
                    outputdata1.Add(addtabs(tablevel) + string1 + " = " + string1 + " - " + string2 + ";\n");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("multiply variable added.");
                }
                if (stringnoquotes1.IndexOf("DIVIDE ") > -1 && stringnoquotes1.IndexOf("INTO ") > -1)
                {
					Console.WriteLine("divide from variable being added.");
                    string string1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("DIVIDE ") + 11, lines1[count1].ToUpper().IndexOf("INTO ") - (lines1[count1].ToUpper().IndexOf("DIVIDE ") + 11));
                    string string2 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("INTO "), lines1[count1].Length - lines1[count1].ToUpper().IndexOf("BY "));
                    outputdata1.Add(addtabs(tablevel) + string1 + " = " + string1 + " % " + string2 + ";\n");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("divide from variable added.");
                }

                if (stringnoquotes1.IndexOf("DIVIDE ") > -1 && stringnoquotes1.IndexOf("BY ") > -1)
                {
					Console.WriteLine("divide from variable being added.");
                    string string1 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("DIVIDE ") + 11, lines1[count1].ToUpper().IndexOf("BY ") - (lines1[count1].ToUpper().IndexOf("DIVIDE ") + 11));
                    string string2 = lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("BY ") + 3, lines1[count1].Length - (lines1[count1].ToUpper().IndexOf("BY ") + 3));
                    outputdata1.Add(addtabs(tablevel) + string1 + " = " + string1 + " / " + string2 + ";\n");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("divide from variable added.");
                }

                if (stringnoquotes1.IndexOf("IF ") > -1 && stringnoquotes1.IndexOf("THEN") > -1)
                {
					Console.WriteLine("if then being added.");
					
                    string string1 = GetExpression(lines1[count1].Substring(lines1[count1].ToUpper().IndexOf("IF ") + 3, lines1[count1].ToUpper().IndexOf("THEN") - (lines1[count1].ToUpper().IndexOf("IF ") + 3)));
                    outputdata1.Add(addtabs(tablevel) + "if(" + string1 + ")\n" + addtabs(tablevel) + "{\n");
					tablevel++;
					Console.WriteLine(string1);
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("if then added.");

                }

                if (stringnoquotes1.IndexOf("END-IF.") > -1)
                {
					//Console.WriteLine("end of if then being added.");
					tablevel--;
                    outputdata1.Add(addtabs(tablevel) + "}\n");
                    //Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("end of if then added.");
                }






                //more loops https://www.tutorialspoint.com/cobol/cobol_loop_statements.htm

                if (stringnoquotes1.IndexOf("END-PERFORM") > -1)
                {
					Console.WriteLine("end loop being added.");
                    tablevel--;
					outputdata1.Add(addtabs(tablevel) + "}\n");
					
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					Console.WriteLine("end loop added.");
                }

                if (stringnoquotes1.IndexOf("SORT ") > -1 && stringnoquotes1.IndexOf("ON ASCENDING KEY ") > -1)
                {
					Console.WriteLine("sort being added.");
                    //DIVIDE WS-NUMA BY WS-NUMB GIVING WS-NUMC REMAINDER WS-REM.
					Console.WriteLine("sort added.");
                }

                if (stringnoquotes1.IndexOf("STOP RUN.") > -1)
                {
					//Console.WriteLine("system exit being added.");
					
                    outputdata1.Add(addtabs(tablevel) + "System.Environment.Exit(1);\n" + addtabs(tablevel - 1) + "}\n");
                    Console.WriteLine(outputdata1[outputdata1.Count - 1].ToString());
					tablevel--;
					//Console.WriteLine("system exit added.");
                }

				if (lines1[count1].IndexOf("EXEC SQL") > -1)
				{
					string sqlstring = lines1[count1].Replace("EXEC SQL", "").Replace("END-EXEC", "");
					if(lines1[count1].IndexOf("END-EXEC") == -1)
					{
						count++;
						while(lines1[count1].IndexOf("END-EXEC") == -1)
						{
							sqlstring = sqlstring + lines1[count1].Replace("EXEC SQL", "").Replace("END-EXEC", "");
							count++;
						}
					}
					
				
					// EXEC SQL SELECT col INTO :hostvar FROM table END-EXEC.
					sqlstring = sqlstring.Replace("\n", "");
					sqlstring = sqlstring.Replace("\r", "");
				}


                count1++;
            }
            return count1;
        }
    }
}
