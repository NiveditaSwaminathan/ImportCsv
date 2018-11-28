using System;
using System.Diagnostics;
using System.IO;

using ImportCSV;

namespace CSVDataReceiver
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			if (args == null)
			{
				Console.WriteLine("args is null");
			}

			// args : logfile path; server; database; 
			if (args.Length < 3)
			{
				Console.WriteLine("Provide enough arguments");
			}

			var file = args[0];
			var datasource = args[1];
			var database = args[2];

			Stopwatch sw = new Stopwatch();
			try
			{
				sw.Start();
				Importer importer = new Importer(file, datasource, database);
				importer.Import();
				sw.Stop();
				Console.WriteLine("The records are inserted");
				Console.WriteLine("Stopwatch elapsed: {0}", sw.ElapsedMilliseconds);
			}
			catch (FileNotFoundException ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}
			Console.Read();
		}
	}
}
