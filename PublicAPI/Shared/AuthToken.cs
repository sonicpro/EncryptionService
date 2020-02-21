namespace PublicAPI.Shared
{

    // In real application AccessToken provision
    // will be the responsibility of the authorization endpoint party.
    public class AccessToken
    {
        public string Token
        {
            get => "4119fa8b6bc53e486b3a318da1eae613d4791158";
            set { }
        }

        public string TokenType
        {
            get => "Bearer";
            set { }
        }

        public long ExpiresIn
        {
            get => 86400;
            set { }
        }

        public string Scope
        {
            get => "any";
            set { }
        }
    }
}
