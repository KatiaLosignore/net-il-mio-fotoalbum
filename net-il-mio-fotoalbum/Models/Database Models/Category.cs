

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace net_il_mio_fotoalbum.Models.Database_Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome della categoria è obbligatorio!")]
        [StringLength(100, ErrorMessage = "Il nome della categoria non può superare i 100 caratteri")]
        public string Name { get; set; }


        // Creo la relazione N:N con la classe Photo

        [JsonIgnore]
        public List<Photo>? Photos { get; set; }



        public Category()
        {

        }

    }

}
