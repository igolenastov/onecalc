namespace OneCalc.Domain.Security
{
    public static class AuthorizationConstant
    {
        public const string AuthorizedUser = "AuthorizedUser";

        public const string AuthorizedAdmin = "AuthorizedAdmin";

        public static class ApplicationClaimTypes
        {
            public const string Operation = "AllowMathOperations";

            public const string Admin = "Admin";

            public const string True = "true";
        }
    }
}
