using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
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


        [HttpGet]
        public IActionResult Create()
        {

            List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
            List<Category> databaseAllCategories = _myDatabase.Categories.ToList();

            foreach (Category category in databaseAllCategories)
            {
                allCategoriesSelectList.Add(
                    new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    });
            }

            PhotoFormModel model =
                new PhotoFormModel { Photo = new Photo(), Categories = allCategoriesSelectList };

            return View("Create", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhotoFormModel newPhoto)
        {
            if (newPhoto.Photo.ImageUrl == null)
            {
                newPhoto.Photo.ImageUrl = "/img/default.jpg";
            }
            if (!ModelState.IsValid)
            {

                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
                List<Category> databaseAllCategories = _myDatabase.Categories.ToList();

                foreach (Category category in databaseAllCategories)
                {
                    allCategoriesSelectList.Add(
                        new SelectListItem
                        {
                            Text = category.Name,
                            Value = category.Id.ToString()
                        });
                }

                newPhoto.Categories = allCategoriesSelectList;

                return View("Create", newPhoto);
            }

            newPhoto.Photo.Categories = new List<Category>();

            if (newPhoto.SelectedCategoriesId != null)
            {
                foreach (string categorySelectedId in newPhoto.SelectedCategoriesId)
                {
                    int intCategorySelectedId = int.Parse(categorySelectedId);

                    Category? categoryInDb = _myDatabase.Categories.Where(category => category.Id == intCategorySelectedId).FirstOrDefault();

                    if (categoryInDb != null)
                    {
                        newPhoto.Photo.Categories.Add(categoryInDb);
                    }
                }
            }


            _myDatabase.Photos.Add(newPhoto.Photo);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");

        }




    }
}
