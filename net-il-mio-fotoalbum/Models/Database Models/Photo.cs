
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using net_il_mio_fotoalbum.Validation_Attributes;

namespace net_il_mio_fotoalbum.Models.Database_Models
{
    public class Photo
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il titolo della photo è obbligatorio!")]
        [StringLength(100, ErrorMessage = "Il titolo della  photo non può superare i 100 caratteri")]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        [Required(ErrorMessage = "La descrizione della photo è obbligatoria!")]
        [MoreThanFiveWords]
        public string Description { get; set; }


        [MaxLength(500, ErrorMessage = "Il link non può essere lungo più di 500 caratteri")]
        public string? ImageUrl { get; set; }

        public byte[]? ImageFile { get; set; }

        public string ImageSrc =>
            ImageFile is null ? (ImageUrl is null ? "" : ImageUrl) : $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}";

        public bool? Visible { get; set; }



        // Creo la relazione N:N con la classe Category
        public List<Category>? Categories { get; set; }

        
        public Photo()
        {

        }

        public Photo(string title, string description, string image)
        {
            this.Title = title;
            this.Description = description;
            this.ImageUrl = image;
        }

        internal Photo? Include(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}

