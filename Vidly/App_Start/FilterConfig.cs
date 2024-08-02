using System.Web;
using System.Web.Mvc;

namespace Vidly2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());      // adds [Authorize] to every controller
            filters.Add(new RequireHttpsAttribute());   // for external login
        }
    }
}
