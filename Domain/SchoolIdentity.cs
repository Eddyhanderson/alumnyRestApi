using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using alumni.Domain;

public class SchoolIdentity
{
    [Key]
    public string Id { get; set; }

    [Required]
    public string SchoolCode { get; set; }

    [Required]
    public string UserId { get; set; }
    [Required]
    public string Name { get; set; }

    [Required]
    public string Acronym { get; set; }    

    [Required]
    public string Nif { get; set; }

    public string Adress { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}