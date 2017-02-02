using Nancy;

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

				//todo: test multiple time to see iof i need to seek the stream to the begining or not

				var body = this.Request.Body;
				int length = (int)body.Length;
				byte[] data = new byte[length];
				body.Read(data, 0, length);
				string token = System.Text.Encoding.Default.GetString(data);
				System.Console.WriteLine("token added: " + token);

				Data.Add(token);

				return null;
			};

			
			Post["/openGate"] = parameters =>
			{
				Broker.SendOpenGateCommand();
				return null;
			};
		}
	}
}