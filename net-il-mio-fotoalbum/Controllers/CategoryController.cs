using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class CategoryController : Controller
    {

        private ICustomLogger _myLogger;
        private FotoalbumContext _myDatabase;

        public CategoryController(FotoalbumContext db, ICustomLogger logger)
        {
            _myDatabase = db;
            _myLogger = logger;
        }

        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Category > Index");

            List<Category>? categories = _myDatabase.Categories.ToList<Category>();

            return View("Index", categories);
        }

        public IActionResult Details(int id)
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Category > Details");

            Category? foundedCategory = _myDatabase.Categories.Where(category => category.Id == id).FirstOrDefault();

            if (foundedCategory == null)
            {
                return NotFound($"La categoria {id} non è stata trovata!");
            }
            else
            {
                return View("Details", foundedCategory);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Category > Create");

            Category newCategory = new Category();

            return View("Create", newCategory);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            _myDatabase.Categories.Add(category);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");
        }

        
        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            Category? categoryToEdit = _myDatabase.Categories.Where(category => category.Id == id).FirstOrDefault();

            if (categoryToEdit == null)
            {
                return NotFound("La categoria che vuoi modificare non è stata trovata");
            }
            else
            {
                _myLogger.WriteLog("L'utente è arrivato sulla pagina Category > Update");

                Category category = categoryToEdit;

                return View("Update", category);
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Category category)
        {

            Category? categoryToUpdate = _myDatabase.Categories.Where(category => category.Id == id).FirstOrDefault();

            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Attenzione non è stata trovata la categoria da aggiornare");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            Category? categoryToDelete = _myDatabase.Categories.Where(category => category.Id == id).FirstOrDefault();

            if (categoryToDelete != null)
            {
                _myDatabase.Categories.Remove(categoryToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La categoria da eliminare non è stata trovata!");
            }
        }

    }

}    
