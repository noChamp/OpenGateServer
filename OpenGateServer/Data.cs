using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGateServer
{
	public static class Data
	{
		public static List<string> Tokens = new List<string>();
		private static string fileName;

		public static void Init()
		{
			//todo: read the file and populate the list

			//todo: delete this since its hard coded
			Tokens.Add("95265f74 fa5f37bf efa2661d 369ae92e 1657f57f 21ffce11 0b789fde 96ead87a");
		}

		public static void Add(string sToken)
		{
			Tokens.Add(sToken);

			//todo: write all to file
		}
	}
}
