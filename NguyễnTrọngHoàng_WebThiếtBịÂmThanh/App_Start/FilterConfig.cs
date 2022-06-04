using System.Web;
using System.Web.Mvc;

namespace NguyễnTrọngHoàng_WebThiếtBịÂmThanh
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
