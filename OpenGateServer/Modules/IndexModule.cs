using Nancy;
using System;
using System.Text;

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


			//for debug
			Get["/listTokens"] = parameters =>
			{
				StringBuilder sb = new StringBuilder();

				foreach (string token in Data.Tokens)
				{
					sb.AppendLine(token);
				}

				return sb.ToString();
			};


			Post["/addToken"] = parameters =>
			{
				string token = ExtractTokenFromRequest();

				bool bRes = Data.Add(token);

				if(bRes)
					return "token " + token + " was added";
				else
					return "token " + token + " is already exist";
			};

			
			Post["/openGate"] = parameters =>
			{
				Broker.SendOpenGateCommand();
				return null;
			};


			Delete["/"] = parameters =>
			{
				string token = ExtractTokenFromRequest();

				bool bRes = Data.Remove(token);

				if (bRes)
					return "token " + token + " was removed";
				else
					return "token " + token + " was not found";
			};
		}

		private string ExtractTokenFromRequest()
		{
			Console.WriteLine("trying to parse token from request");

			var body = this.Request.Body;
			int length = (int)body.Length;
			byte[] data = new byte[length];
			body.Read(data, 0, length);
			string token = Encoding.Default.GetString(data);

			Console.WriteLine("token parsed: " + token);

			return token;
		}
	}
}