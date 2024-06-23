namespace Server.Models
{
	public class SettingsOption
	{
		public const string Settings = "Settings";
		public string BearerKey { get; set; } = String.Empty;
		public string AppId {  get; set; } = String.Empty;
		public string SecretKey {  get; set; } = String.Empty;
		public string ServiceToken { get; set; } = String.Empty;
		public string RedirectUri {  get; set; } = String.Empty;
	}
}
