using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class MessageController : Controller
    {

        private ICustomLogger _myLogger;

        private FotoalbumContext _myDatabase;

        public MessageController(FotoalbumContext db, ICustomLogger logger)
        {
            _myDatabase = db;
            _myLogger = logger;
        }

        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Message > Index");

            List<Message> messages = _myDatabase.Messages.ToList();

            return View(messages);
        }
    }
    
}
