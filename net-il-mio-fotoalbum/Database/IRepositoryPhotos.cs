using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Database
{
    public interface IRepositoryPhotos
    {
        public List<Photo> GetPhotos();

        public List<Photo> GetPhotosByTitle(string title);

        public Photo GetPhotoById(int id);
    }
}
