using Nancy;
using System;

namespace OpenGateServer.Modules
{
	public class IndexModule : NancyModule
	{
		public IndexModule()
		{
			Get["/"] = parameters =>
			{
				return View["index"];
			};

			Post["/addToken"] = parameters =>
			{
				//extract token from request
				Console.WriteLine("trying to parse token from request");

				var body = this.Request.Body;
				int length = (int)body.Length;
				byte[] data = new byte[length];
				body.Read(data, 0, length);
				string token = System.Text.Encoding.Default.GetString(data);

				Console.WriteLine("token parsed: " + token);

				Data.Add(token);

				return "token " + token + " was added";
			};

			
			Post["/openGate"] = parameters =>
			{
				Broker.SendOpenGateCommand();
				return null;
			};
		}
	}
}