namespace Mango.Web
{
    public class SD
    {
        static public string ProductAPIBase { get; set; }
        static public string ShoppingCartAPIBase { get; set; }
        static public string CouponAPIBase { get; set; }

        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
