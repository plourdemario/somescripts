using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace GetData
{
	
	//class of main program
    class Program
    {
		//starts here
        static void Main(string[] args)
        {
			var value1 = System.Environment.GetEnvironmentVariable("PATH");
			Console.WriteLine(value1);
		}
	}
}