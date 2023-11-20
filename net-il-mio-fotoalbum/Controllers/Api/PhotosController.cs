using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models.Database_Models;

namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {

        // uso la  Dependency Injection

        private IRepositoryPhotos _repoPhotos;

        public PhotosController(IRepositoryPhotos repoPhotos)
        {
            _repoPhotos = repoPhotos;
        }


        [HttpGet]
        public IActionResult GetPhotos()
        {

            List<Photo> photos = _repoPhotos.GetPhotos();

            return Ok(photos);

        }


        [HttpGet]
        public IActionResult SearchPhoto(string? search)
        {
            List<Photo> foundedPhotos = new List<Photo>();

            if (search == null)
            {
                foundedPhotos = _repoPhotos.GetPhotos();
            }
            else
            {
                foundedPhotos = _repoPhotos.GetPhotosByTitle(search);
            }

            return Ok(foundedPhotos);

        }


        [HttpGet("{id}")]
        public IActionResult PhotoById(int id)
        {
            Photo photo = _repoPhotos.GetPhotoById(id);

            if (photo != null)
            {
                return Ok(photo);
            }
            else
            {
                return NotFound();
            }

        }

    }
}
