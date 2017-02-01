using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGateServer
{
	public class Broker
	{
		static ApnsConfiguration config;
		static ApnsServiceBroker apnsBroker;

		public static void Start()
		{
			// Configuration (NOTE: .pfx can also be used here)
			// use the certificate from Resources folder

			//build absolute path, otherwise on Heroku it builds a wrong relative path
			var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Certificates.p12");

			config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, path, "server");

			// Create a new broker
			apnsBroker = new ApnsServiceBroker(config);

			// Wire up events
			apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
			{
				aggregateEx.Handle(ex =>
				{
					// See what kind of exception it was to further diagnose
					if (ex is ApnsNotificationException)
					{
						var notificationException = (ApnsNotificationException)ex;

						// Deal with the failed notification
						var apnsNotification = notificationException.Notification;
						var statusCode = notificationException.ErrorStatusCode;

						Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");
					}
					else
					{
						// Inner exception might hold more useful information like an ApnsConnectionException           
						Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
					}

					// Mark it as handled
					return true;
				});
			};

			apnsBroker.OnNotificationSucceeded += (notification) =>
			{
				Console.WriteLine("Apple Notification Sent!");
			};

			// Start the broker
			apnsBroker.Start();

			HouseKeeping();
		}

		public static void SendOpenGateCommand()
		{
			//todo: imp a mechanism that first try one agent. if could not - try the next one and so on
			foreach (var deviceToken in Data.Tokens)
			{
				//build absolute path, otherwise on Heroku it builds a wrong relative path
				var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "payload.json");

				// Queue a notification to send
				apnsBroker.QueueNotification(new ApnsNotification
				{
					DeviceToken = deviceToken,
					Payload = JObject.Parse(File.ReadAllText(path))//send the payload as the body of http/2 using ssl to gateway.sandbox.push.apple.com on TCP port 2195 or gateway.push.apple.com on TCP port 2195:
				});
			}
		}

		private static void HouseKeeping()
		{
			//do this in order to know what tokens have been expired:
			var fbs = new FeedbackService(config);

			// called for each expired device token. timestamp is the time the token was reported as expired
			fbs.FeedbackReceived += (string deviceToken, DateTime timestamp) =>
			{
				// Remove the deviceToken from your database
				Data.Tokens.Remove(deviceToken);
			};

			//TODO it should happen periodically in another thread
			//fbs.Check();
		}

		public static void Stop()
		{
			// Stop the broker, wait for it to finish   
			// This isn't done after every message, but after you're
			// done with the broker
			apnsBroker.Stop();
		}
	}
}
