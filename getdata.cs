using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace GetData
{
	
	//class of main program
    class Program
    {

		//function to get last position of text in a text
		public static int NumbersCount(string fulltext1, string previoustext1)
		{
			int count1 = 0;
			while(fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "0" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "1" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "2" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "3" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "4" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "5" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "6" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "7" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "8" || fulltext1.Substring(fulltext1.IndexOf(previoustext1) - count1, 1) == "9")
			{
				count1 = count1 + 1;
			}
			return count1 - 1;
		}

		
		//starts here
        static void Main(string[] args)
        {

			//create a users' list to send emails
            ArrayList userlists1 = new ArrayList();
			
			//Set SSL type for getting web data
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

			
            
			//Set data on the location of words in word list
            char[] index1 = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            int[] startindex1 = { 0, 5582, 9472, 16366, 20291, 23046, 25713, 27942, 30539, 33390, 33965, 34573, 37012, 40929, 42576, 44327, 50066, 50436, 53385, 60649, 64230, 66759, 68005, 69414, 69495, 69688 };
            int[] endindex1 = { 5581, 9471, 16365, 20290, 23045, 25712, 27941, 30538, 33389, 33964, 34572, 37011, 40928, 42575, 44326, 50065, 50435, 53384, 60648, 64229, 66758, 68004, 69413, 69494, 69687, 69882 };
			string[] words1 = File.ReadAllLines("C:\\project\\wordslist.txt");
			
			//Display amount of words
			Console.WriteLine(words1.Length);

			//Set variables to hold company data
			ArrayList companycodes = new ArrayList();
			ArrayList companynames = new ArrayList();
			ArrayList companywebsites = new ArrayList();

			//get password to connect to database
			var password1 = System.Environment.GetEnvironmentVariable("databasepassword");

			//Connect to database to get list of companies
			string sqlstring1 = "SELECT companies.companies, companies.NewsLink, companies.NewsLink2, companies.Name, companies.Bad_word_length from companies where slowwebsite = 0";
            string connstring1 = "server=localhost;uid=root;pwd=" + password1 + ";database=Investment_Leads;CharSet=utf8;";
            MySql.Data.MySqlClient.MySqlConnection mysqlconnection1 = new MySql.Data.MySqlClient.MySqlConnection(connstring1);
            MySql.Data.MySqlClient.MySqlCommand mysqlcmd1;
            MySql.Data.MySqlClient.MySqlDataReader readersql1;
			
			//Create logfile for results of data comparison
			FileStream logfile1 = new FileStream("c:\\Project\\logfile.txt", FileMode.Create, FileAccess.ReadWrite);
			byte[] info3 = new UTF8Encoding(true).GetBytes("start of file\n");
			logfile1.Write(info3, 0, info3.Length);
			

			//Read all readable companies
			try
			{
				mysqlconnection1.Open();
				mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
                readersql1 = mysqlcmd1.ExecuteReader();

				while(readersql1.Read())
				{
					companycodes.Add(readersql1["Companies"].ToString());
					companynames.Add(readersql1["Name"].ToString());
					companywebsites.Add(readersql1["NewsLink"].ToString());
				}
			}
			catch (System.Exception e)
			{
				//write errors to logfile
				info3 = new UTF8Encoding(true).GetBytes("Failed to get company data.\n");
				logfile1.Write(info3, 0, info3.Length);
				Console.WriteLine(e.Message);
				mysqlconnection1.Close();
			}

			//write to logfile
			info3 = new UTF8Encoding(true).GetBytes("Got companies data.\n");
			logfile1.Write(info3, 0, info3.Length);
		
			mysqlconnection1.Close();
			int count1 = 0;
			
            try
            {
                
				mysqlconnection1.Open();

				//loop through all companies retreived
                while (count1 < companycodes.Count)
                {
					try
                    {
						
						string newfile1;
						string oldfile1;
						
						Console.WriteLine("Start Download " + companywebsites[count1].ToString());
						
						//Download of html file
						HttpWebRequest download1 = (HttpWebRequest)WebRequest.Create(companywebsites[count1].ToString());
						download1.Timeout = 7000;
						download1.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0";
						download1.Method = "GET";
						StreamReader reader = new StreamReader(download1.GetResponse().GetResponseStream());
						newfile1 = reader.ReadToEnd();
						info3 = new UTF8Encoding(true).GetBytes(companynames[count1].ToString() + " Downloaded\n");
						logfile1.Write(info3, 0, info3.Length);

						//clean the old file data
                        newfile1 = newfile1.Replace("\r", "\n"); 
						string newfilenohtml1 = replacehtml(newfile1);
                        string[] newfilelines1 = newfilenohtml1.Split('\n');

						//Read last downloaded file
                        TextReader readeroldfile11 = new StreamReader("c:\\Downloads\\download" + companycodes[count1].ToString() + ".txt");
						oldfile1 = readeroldfile11.ReadToEnd();
						readeroldfile11.Close();
						
						//clean new file
						oldfile1 = oldfile1.Replace("\r", "\n");
						oldfile1 = replacehtml(oldfile1);
						string[] oldfilelines1 = oldfile1.Split('\n');
						
						//compare both files
						string output1 = CompareDataNew(oldfilelines1, newfilelines1);
						info3 = new UTF8Encoding(true).GetBytes(companynames[count1].ToString() + " Compared\n");
						logfile1.Write(info3, 0, info3.Length);

						string[] allwords = output1.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
						
						int index2 = 0;
						
						
						
						//Write status to log file
						info3 = new UTF8Encoding(true).GetBytes(allwords.Length.ToString() + " Filtering\n");
						logfile1.Write(info3, 0, info3.Length);
						
						//check if new data has dictionary words
						for(int i2 = 0; i2 < allwords.Length; i2++)
						{
							
							if(isDictWord(allwords[i2], index1, startindex1, endindex1, words1))
							{
								index2 = index2 + 1;
							}
							if((index2 > 3 && allwords.Length >= 3 && allwords.Length < 20) || (index2 > 10 && allwords.Length < 60) || (index2 > 15 && allwords.Length < 80) || (index2 > 20))
							{
								break;
							}
						}

						//write status to word file
						info3 = new UTF8Encoding(true).GetBytes(index2.ToString() + " Dict words\n");
						logfile1.Write(info3, 0, info3.Length);
						
						
						//proceed with next steps if criterias met
						if((index2 > 3 && allwords.Length >= 3 && allwords.Length < 20) || (index2 > 10 && allwords.Length < 60) || (index2 > 15 && allwords.Length < 80) || (index2 > 20))
						{
							Console.WriteLine("all words: " + allwords.Length.ToString());
							Console.WriteLine("dict words: " + index2.ToString());
							
							//Save new download to the downloads folder
							string date1 = DateTime.Now.ToString();
							string truncdate1 = date1.Replace(":", "").Replace("/","").Replace(" ", "").Replace("-", "");
							string filename1 = "company" + companycodes[count1].ToString() + truncdate1 + ".txt";
							FileStream writerdownloadfile1 = new FileStream("C:\\Apache24\\htdocs\\Exports\\" + filename1, FileMode.Create, FileAccess.ReadWrite);
							byte[] info = new UTF8Encoding(true).GetBytes(newfilenohtml1);
							writerdownloadfile1.Write(info, 0, info.Length);
							writerdownloadfile1.Close();
							
							//record new file in database
							string sqlstring2 = "INSERT INTO InvestorRelations (companyID, Datetime1, filename1, Url) VALUES (" + companycodes[count1].ToString() + ", CURRENT_TIMESTAMP,'" + filename1 + "','" + companywebsites[count1].ToString()  + "');";
							MySql.Data.MySqlClient.MySqlCommand mysqlcmd2;
							mysqlcmd2 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring2, mysqlconnection1);
							mysqlcmd2.ExecuteNonQuery();
							FileStream writerdownloadfile2 = new FileStream("c:\\Downloads\\download" + companycodes[count1].ToString() + ".txt", FileMode.Create, FileAccess.ReadWrite);
							byte[] info2 = new UTF8Encoding(true).GetBytes(newfile1);
							writerdownloadfile2.Write(info2, 0, info2.Length);
							writerdownloadfile2.Close();

							//update log file
							info3 = new UTF8Encoding(true).GetBytes(companynames[count1].ToString() + " Company processed\n");
							logfile1.Write(info3, 0, info3.Length);

							//send emails to all recipients subscribed
							sqlstring1 = "select * from notifications a, usermonitorsignup b, userinfo c where a.UserID = b.UserID AND b.UserID = c.UserID AND a.enabled1 = 1 and b.companies = " + companycodes[count1].ToString() + ";";							
					        mysqlcmd1 = new MySql.Data.MySqlClient.MySqlCommand(sqlstring1, mysqlconnection1);
						    readersql1 = mysqlcmd1.ExecuteReader();
							while (readersql1.Read())
							{
								int index3 = finduser(readersql1["UserID"].ToString(), userlists1);
								if(index3 > -1)
								{
									((UserListing)userlists1[index3]).AddList(companynames[count1].ToString(), date1);
								}else
								{
									UserListing newlist1 = new UserListing(readersql1["UserID"].ToString(), readersql1["Email"].ToString());
									userlists1.Add(newlist1);
									((UserListing)userlists1[userlists1.Count - 1]).AddList(companynames[count1].ToString(), date1);
								}
							}

							info3 = new UTF8Encoding(true).GetBytes(companynames[count1].ToString() + " Company emails added\n");
							logfile1.Write(info3, 0, info3.Length);
							Console.WriteLine("Proccessed: " + companywebsites[count1].ToString());
							readersql1.Close();
						}
						count1 = count1 + 1;
					}catch(System.Exception e)
					{
						count1 = count1 + 1;
						info3 = new UTF8Encoding(true).GetBytes("data problem:." + "problem found: " + e.Message + "\n");
						logfile1.Write(info3, 0, info3.Length);
	
					}
				}
				for(int i2 = 0; i2 < userlists1.Count; i2++)
				{
					((UserListing)userlists1[i2]).SendEmail();
				}
			}catch(System.Exception e)
			{
				count1 = count1 + 1;
				info3 = new UTF8Encoding(true).GetBytes("other problem: " + e.Message + "\n");
			}
			logfile1.Close();
        }

		//function to find user to send email
		private static int finduser(string userid, ArrayList userlists1)
		{
			for(int i = 0; i < userlists1.Count; i++)
			{
				if(userid == ((UserListing)userlists1[i]).GetUserID())
				{
					return i;
				}
			}
			return -1;
		}

		//function to remove a HTML tag
		private static string htmltag(string text1)
		{
			if(text1.IndexOf('<') > -1)
			{
				if(text1.IndexOf('>') > text1.IndexOf('<'))
				{
					return text1.Substring(text1.IndexOf('<'), text1.IndexOf('>') - text1.IndexOf('<') + 1);
				}else
				{
					return text1.Substring(text1.IndexOf('<'), 2);
				}
			}
			return "none";
		}
		
		

		
		//Function to replace all HTML tags in an email
		private static string replacehtml(string text1)
		{
			//remove style scripts that are not valid changes
			while(text1.ToLower().IndexOf("<style") > -1)
			{
				string scriptline1 = text1.Substring(text1.ToLower().IndexOf("<style"), text1.ToLower().IndexOf("/style>") - text1.ToLower().IndexOf("<style") + 7);
				text1 = text1.Replace(scriptline1, "");
			}

			//remove scripts that are not valid changes
			while(text1.ToLower().IndexOf("<script") > -1)
			{
				if(text1.ToLower().IndexOf("/script>") > text1.ToLower().IndexOf("<script"))
				{
					string scriptline1 = text1.Substring(text1.ToLower().IndexOf("<script"), text1.ToLower().IndexOf("/script>") - text1.ToLower().IndexOf("<script") + 8);
					text1 = text1.Replace(scriptline1, "");
				}else
				{
					if(text1.ToLower().IndexOf("script/>") > text1.ToLower().IndexOf("<script"))
					{
						string scriptline1 = text1.Substring(text1.ToLower().IndexOf("<script"), text1.ToLower().IndexOf("script/>") - text1.ToLower().IndexOf("<script") + 8);
						text1 = text1.Replace(scriptline1, "");
					}else
					{
						text1 = text1.Replace(text1.Substring(text1.ToLower().IndexOf("<script"), 7), "");
					}
				}
			}
		
			//remove multiple spaces
			while(text1.IndexOf("  ") > -1)
			{
				text1 = text1.Replace("  ", " ");
			}
			
			//remove all other html tages
			string htmltag1= htmltag(text1);
			while(htmltag1 != "none")
			{
				text1 = text1.Replace(htmltag(text1), "");
				htmltag1 = htmltag(text1);
			}
			return text1;
		}
		
		//function to find if a word is a dictionary word
        private static bool isDictWord(string word1, char[] index1, int[] startindex1, int[] endindex1, string[] words1)
        {
            word1 = word1.ToLower();
            int currentindex1 = 0 ;
			bool found1 = false;
            for(int letterindex1 = 0; letterindex1 < 25; letterindex1++)
            {
                if(word1.ToCharArray()[0] == index1[letterindex1])
                {
                    currentindex1 = letterindex1;
					found1 = true;
                    break;
                }
            }
			
			if(found1 == true)
			{
				for(int i = startindex1[currentindex1]; i < endindex1[currentindex1]; i++)
				{
					if(words1[i].ToString().Contains(word1))
					{
						return true;
					}
				}
			}

            return false;
        }
	
		//functino to compare data from previous and new download
		private static string CompareDataNew(string[] oldfile1, string[] newfile1)
        {
            int currentold1 = 0;
            int currentnew1 = 0;
			
            string newdata1 = "";
            while(oldfile1.Length >  currentold1 && currentnew1 < newfile1.Length)
            {
				bool found1 = false;
                if (oldfile1[currentold1] == newfile1[currentnew1])
                {
                    currentnew1 = currentnew1 + 1;
					found1 = true;
                }
                else
                {
					int index2 = 0;
                    for (int i = 1; i < 50; i++)
                    {
						if((currentnew1 + i) < newfile1.Length)
						{
							if (oldfile1[currentold1] == newfile1[currentnew1 + i])
							{
								found1 = true;
								index2 = i;
								break;
							}
						}else
						{
							index2 = index2 + 1;
							break;
						}
                    }
					if(currentnew1 + index2 >= newfile1.Length)
					{
						break;
					}
					currentnew1 = currentnew1 + index2;
					if(found1 == false)
					{
						newdata1 = newdata1 + newfile1[currentnew1];
					}
					
					
                }   
                currentold1 = currentold1 + 1;
            }
            return newdata1;
        }
    }
	class UserListing
	{
		//variables to store company details
		public static ArrayList companies1 = new ArrayList();
		public static ArrayList timestamps1 = new ArrayList();
		public static string UserID = "";
		public static string email1 = "";
		
		//set user's email and userid 
		public UserListing(string UserID1, string emailtemp1)
		{
			email1 = emailtemp1;
			UserID = UserID1;
		}
		
		//add a company item to send to user
		public void AddList(string company1, string timestamp1)
		{
			companies1.Add(company1);
			timestamps1.Add(timestamp1);
		}
        public string GetUserID(){
			return UserID;
         }

		//function to send an email to a user
		public void SendEmail()
		{
			var password2 = System.Environment.GetEnvironmentVariable("emailpassword");
			var smtpClient = new SmtpClient("smtp.ionos.com")
			{
				Port = 587,
				
				Credentials = new NetworkCredential("noreply@aqesda.com", password2),
				EnableSsl = true,
			};

			
				string body1 = "<P><img src=\"https://daytradingsignaler.com/investmentnews.jpg\"></P><h1>Hello, </h1><P>The companies below have been flagged with news at the respectives date/times listed below. Please be advised that not all company news is investable even in this system. You will need to use good judgement to determine if it will change the value of the stocks. You should not invest without good day trading knowledge. News is scanned Monday to Friday between 6:00 AM CST to 6:00 PM CST so news in the late evening will only show at 6:00 AM. News is scanned every 10 minutes and this system only scans some companies. News might appear on another website before it appears on the company's website.<BR>";
				for(int i = 0; i < companies1.Count; i++)
				{
					body1 = body1 + "<BR>" + companies1[i].ToString() + " news found at " + timestamps1[i].ToString(); 
				}
				body1 = body1 + "<CENTER><HR></CENTER><BR><CENTER><H4>Powered by Aqesda Software Solutions Inc. All rights reserved.</H4></CENTER>";
	
			var mailmessage1 = new MailMessage
			{
				From = new MailAddress("noreply@aqesda.com"),
				Subject = companies1.Count.ToString() + " notifications from daytradingsignaler.com",
				Body = body1,
				IsBodyHtml = true,
			};
			mailmessage1.To.Add(email1);

			smtpClient.Send(mailmessage1);
		}
	}

}


