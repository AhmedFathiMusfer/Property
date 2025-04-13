using System.Xml;

namespace Property_Utility
{
    public static class SD
    {
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE,
            PATCH

        }
        public static string AccessToken = "JWTToken";
        public static string RefreshToken = "RefreshToken";
        public static string CurrentVersion = "v2";
        public const string Admin = "admin";
        public const string Customer = "customer";
        public enum ContentType
        {
            Jsone,
            MultipartFormData
        }
    }
}