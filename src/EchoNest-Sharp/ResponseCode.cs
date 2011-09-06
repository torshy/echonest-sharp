namespace EchoNest
{
    public enum ResponseCode
    {
        UnknownError = -1,
        Success = 0,
        MissingOrInvalidApiKey = 1,
        ApiKeyCannotCallThisMethod = 2,
        RateLimitExceeded = 3,
        MissingParameter = 4,
        InvalidParameter = 5
    }
}