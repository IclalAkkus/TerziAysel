namespace TerzimAysel.Models;
using System.ComponentModel.DataAnnotations;


public class YorumViewModel
{
    [Required]
    [StringLength(500, ErrorMessage = "Yorum metni 500 karakteri geçemez.")]
    public string YorumMetni { get; set; }= string.Empty;
    
}
