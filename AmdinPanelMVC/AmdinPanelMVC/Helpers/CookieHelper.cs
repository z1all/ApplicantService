namespace AmdinPanelMVC.Helpers
{
    public static class CookieHelper
    {
        private const string _jwtToken = "jwtToken";
        private const string _refreshToken = "refreshToken";

        public static void SetTokens(this IResponseCookies cookies, string jwtToken, string refreshToken)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.MaxValue;
            cookies.Append(_jwtToken, jwtToken, options);
            cookies.Append(_refreshToken, refreshToken, options);
        }

        public static bool TryGetTokens(this IRequestCookieCollection cookies, out string? jwtToken, out string? refreshToken)
        {
            refreshToken = null;
            return cookies.TryGetValue(_jwtToken, out jwtToken) && cookies.TryGetValue(_refreshToken, out refreshToken);
        }

        public static void RemoveTokens(this IResponseCookies cookies)
        {
            cookies.Delete(_jwtToken);
            cookies.Delete(_refreshToken);
        }
    }
}
