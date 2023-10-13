using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using net_il_mio_fotoalbum.Database;

namespace net_il_mio_fotoalbum.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private FotoalbumContext _db;

        public MessageController(FotoalbumContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult CreateMessage([FromBody] Message message)
        {
            try
            {
                _db.Messages.Add(message);
                _db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
