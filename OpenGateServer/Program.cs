using System;
using Nancy.Hosting.Self;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;
using System.IO;


namespace OpenGateServer
{
	class Program
	{
		static void Main(string[] args)
		{
			Data.Init();
			Broker.Start();

			//luanch Nancy web server
			var uri = new Uri("http://localhost:3579");

			using (var host = new NancyHost(uri))
			{
				host.Start();

				Console.WriteLine("Your application is running on " + uri);
				Console.WriteLine("Press any [Enter] to close the host.");
				Console.ReadLine();
			}

			Broker.Stop();
		}
	}
}
