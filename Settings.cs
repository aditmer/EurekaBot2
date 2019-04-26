namespace Microsoft.EurekaBot
{
	public class Settings
	{
		public string NoAnswerMessage { get; set; }
		public WelcomeCardSettings WelcomeCard { get; set; }
	}

	public class WelcomeCardSettings
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string VideoUrl { get; set; }
		public string LearnMoreUrl { get; set; }
	}
}