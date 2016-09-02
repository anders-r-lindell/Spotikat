using System.Web.Mvc;

namespace SpotiKat.Web.Controllers {
    public class DefaultController : Controller {
        //
        // GET: /Default/

        public virtual ActionResult Index() {
            return View();
        }
    }
}