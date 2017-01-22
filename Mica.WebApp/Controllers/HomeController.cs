using System.Web.Mvc;
using Mica.Core.Builders;

namespace Mica.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var test = new SteamAchievementBuilder();
            var model = test.Build("211420", "flave_229");
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}