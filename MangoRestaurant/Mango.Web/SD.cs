using System.Security.Principal;

namespace MangoWeb
{
    public class SD
    {
        static public string ProductAPIBase { get; set; }

        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
