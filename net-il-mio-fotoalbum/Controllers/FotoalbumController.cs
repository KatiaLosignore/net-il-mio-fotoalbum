using Microsoft.AspNetCore.Mvc;

namespace net_il_mio_fotoalbum.Controllers
{
    public class FotoalbumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
