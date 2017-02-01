using System;
using Nancy.Hosting.Self;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading;

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

				//prevent app from getting closed since on Heroku Console.ReadLine() doesn't wait for user action.
				//instead it prints to Heroku's console
				while (true)
				{
					Console.WriteLine("{0}\tWaiting for request", DateTime.UtcNow.ToString("u"));
					Thread.Sleep(TimeSpan.FromSeconds(1));
				}
			}

			Broker.Stop();
		}
	}
}
