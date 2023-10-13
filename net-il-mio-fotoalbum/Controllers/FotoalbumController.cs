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
            
            if (newPhoto.Photo.ImageUrl == null && newPhoto.ImageFormFile == null)
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

            //Richiamo il metodo scritto in basso
            this.SetImageFileFromFormFile(newPhoto);

            _myDatabase.Photos.Add(newPhoto.Photo);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");

        }


        [HttpGet]
        public IActionResult Update(int id)
        {

            Photo? photoToEdit = _myDatabase.Photos.Where(photo => photo.Id == id).Include(element => element.Categories).FirstOrDefault();

            if (photoToEdit == null)
            {
                return NotFound("La foto che vuoi modificare non è stata trovata");
            }
            else
            {

                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<Category> dbcategoryList = _myDatabase.Categories.ToList();

                foreach (Category category in dbcategoryList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.Name,
                        Selected = photoToEdit.Categories.Any(categoryAssociated => categoryAssociated.Id == category.Id)
                    });

                }

                PhotoFormModel model
                  = new PhotoFormModel { Photo = photoToEdit, Categories = selectListItem };

                return View("Update", model);

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {

                List<SelectListItem> selectListItem = new List<SelectListItem>();
                List<Category> dbcategoryList = _myDatabase.Categories.ToList();

                foreach (Category category in dbcategoryList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.Name
                    });
                }

                data.Categories = selectListItem;


                return View("Update", data);
            }

            Photo? photoToUpdate = _myDatabase.Photos.Where(photo => photo.Id == id).Include(element => element.Categories).FirstOrDefault();

            if (photoToUpdate != null)
            {
                photoToUpdate.Categories.Clear();

                photoToUpdate.Title = data.Photo.Title;
                photoToUpdate.Description = data.Photo.Description;
                photoToUpdate.ImageUrl = data.Photo.ImageUrl;
               

                if (data.SelectedCategoriesId != null)
                {
                    foreach (string categorySelectedId in data.SelectedCategoriesId)
                    {
                        int intCategorySelectedId = int.Parse(categorySelectedId);

                        Category? categoryInDb = _myDatabase.Categories.Where(category => category.Id == intCategorySelectedId).FirstOrDefault();

                        if (categoryInDb != null)
                        {
                            photoToUpdate.Categories.Add(categoryInDb);
                        }
                    }
                }

                if (data.ImageFormFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    data.ImageFormFile.CopyTo(stream);
                    photoToUpdate.ImageFile = stream.ToArray();
                }

                _myDatabase.SaveChanges();

                return RedirectToAction("Details", "Fotoalbum", new { id = photoToUpdate.Id });
            }
            else
            {
                return NotFound("Mi dispiace non è stata trovata la foto da aggiornare");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            Photo? photoToDelete = _myDatabase.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (photoToDelete != null)
            {
                _myDatabase.Photos.Remove(photoToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La foto da eliminare non è stata trovata!");
            }

        }


        // Metodo per il File Image
        private void SetImageFileFromFormFile(PhotoFormModel formData)
        {
            if (formData.ImageFormFile == null)
            {
                return;
            }

            MemoryStream stream = new MemoryStream();
            formData.ImageFormFile.CopyTo(stream);
            formData.Photo.ImageFile = stream.ToArray();

        }


    }

}
