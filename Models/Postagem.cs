using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace bog_include.Models
{
    [Table("Postagens")]
    public class Postagem
    {
        [Key]
        [JsonIgnore]
        public int PostagemId { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(50)]
        public string Mensagem { get; set; }

        [JsonIgnore]
        public byte[]? Imagem { get; set; } // Campo para armazenar a imagem binária
    }
}
