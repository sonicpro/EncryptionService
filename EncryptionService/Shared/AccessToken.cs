namespace Encryption.Shared
{
	public class AccessToken
	{
		public string Token { get; set; }

		public string TokenType { get; set; }

		public long ExpiresIn { get; set; }

		public string Scope { get; set; }
	}
}
