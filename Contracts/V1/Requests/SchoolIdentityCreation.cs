using System.ComponentModel.DataAnnotations;

public class SchoolIdentityCreation
{
    public string Name { get; set; }
    public string Country { get; set; }

    [Required]
    public string Nif { get; set; }

    [Required]
    public string Email { get; set; }

    public string Adress { get; set; }
    public string PictureProfilePath { get; set; }
}