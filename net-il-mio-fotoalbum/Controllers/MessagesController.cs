using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class MessagesController : Controller
    {

        private ICustomLogger _myLogger;

        private FotoalbumContext _myDatabase;

        public MessagesController(FotoalbumContext db, ICustomLogger logger)
        {
            _myDatabase = db;
            _myLogger = logger;
        }

        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Messages > Index");

            List<Message> messages = _myDatabase.Messages.ToList<Message>();

            return View("Index",messages);
        }

        public IActionResult Details(int id)
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Messages > Details");

            Message? foundedMessage = _myDatabase.Messages.Where(message => message.Id == id).FirstOrDefault();

            if (foundedMessage == null)
            {
                return NotFound($"Il messaggio {id} non è stato trovato!");
            }
            else
            {
                return View("Details", foundedMessage);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            Message? messageToDelete = _myDatabase.Messages.Where(message => message.Id == id).FirstOrDefault();

            if (messageToDelete != null)
            {
                _myDatabase.Messages.Remove(messageToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Il messaggio da eliminare non è stato trovato!");
            }
        }
    }
    
}
