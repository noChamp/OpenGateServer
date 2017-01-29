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
				Data.Add("momo");
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