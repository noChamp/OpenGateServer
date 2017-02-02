﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGateServer
{
	public static class Data
	{
		public static List<string> Tokens = new List<string>();
		private static string m_sFileName = "tokens.txt";

		public static void Init()
		{
			//read the tokens file and populate the list
			if (!File.Exists(m_sFileName))
				return;

			string[] tokens = File.ReadAllLines(m_sFileName);

			Tokens = new List<string>(tokens);
		}

		public static void Add(string sToken)
		{
			Tokens.Add(sToken);

			//write all to file
			File.WriteAllLines(m_sFileName, Tokens);
		}
	}
}
