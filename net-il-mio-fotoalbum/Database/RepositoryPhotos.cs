using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Database
{
    public class RepositoryPhotos : IRepositoryPhotos
    {

        // uso la  Dependency Injection

        private FotoalbumContext _db;

        public RepositoryPhotos(FotoalbumContext db)
        {
            _db = db;
        }

        

        // implemento i Metodi creati nell' Interfaccia
        public List<Photo> GetPhotos()
        {
            List<Photo> photos = _db.Photos.Include(photo => photo.Categories).ToList();
            return photos;
        }

        public List<Photo> GetPhotosByTitle(string title)
        {
            List<Photo> foundedPhotos = _db.Photos.Where(photo => photo.Title.ToLower().Contains(title.ToLower())).Include(photo => photo.Categories).ToList();

            return foundedPhotos;
        }
    }
}
