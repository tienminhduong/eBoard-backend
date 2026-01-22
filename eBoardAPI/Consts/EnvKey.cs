namespace eBoardAPI.Consts;

public static class EnvKey
{
    public const string DATABASE_CONNECTION_STRING = "DATABASE_CONNECTION_STRING";
    public const string AUTOMAPPER_LICENSE_KEY = "AUTOMAPPER_LICENSE_KEY";
    public const string VIETNAM_PROVINCE_API_URL = "VIETNAM_PROVINCE_API_URL";

    // Email configuration keys
    public const string EMAIL_HOST = "EMAIL_HOST";
    public const string EMAIL_PORT = "EMAIL_PORT";
    public const string EMAIL_USERNAME = "EMAIL_USERNAME";
    public const string EMAIL_PASSWORD = "EMAIL_PASSWORD";
    public const string EMAIL_FROM = "EMAIL_FROM";
    public const string EMAIL_ENABLE_SSL = "EMAIL_ENABLE_SSL";

    // REDIRECT URLS
    public const string FRONTEND_RESET_PASSWORD_URL = "FRONTEND_RESET_PASSWORD_URL";

    // jwt
    public const string JWT_ISSUER = "JWT_ISSUER";
    public const string JWT_AUDIENCE = "JWT_AUDIENCE";
    public const string JWT_KEY = "JWT_KEY";
}