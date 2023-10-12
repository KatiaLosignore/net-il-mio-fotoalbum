using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models.Database_Models;
using System.Linq;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class FotoalbumController : Controller
    {


        private ICustomLogger _myLogger;
        private FotoalbumContext _myDatabase;

        public FotoalbumController(FotoalbumContext db, ICustomLogger logger)
        {
            _myDatabase = db;
            _myLogger = logger;
        }


        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Fotoalbum > Index");

            List<Photo> photos = _myDatabase.Photos.Include(photo => photo.Categories).ToList<Photo>();

            return View("Index", photos);
        }


        public IActionResult Details(int id)
        {
            Photo? foundedElement = _myDatabase.Photos.Where(element => element.Id == id).Include(element => element.Categories).FirstOrDefault();

            if (foundedElement == null)
            {
                return NotFound($"La foto con {id} non è stata trovata!");
            }
            else
            {
                return View("Details", foundedElement);
            }

        }





    }
}
