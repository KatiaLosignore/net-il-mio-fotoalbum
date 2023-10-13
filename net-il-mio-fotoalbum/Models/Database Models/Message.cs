using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models.Database_Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "L'email è obbligatoria")]
        [EmailAddress(ErrorMessage = "Formato dell'email non valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Il messaggio da inserire è obbligatorio")]
        public string MessageText { get; set; }

        // COSTRUTTORE
        public Message()
        {

        }
    }
}
