using System.ComponentModel.DataAnnotations;

public class CreateSchoolRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Acronym { get; set; }

    [Required]
    public string Nif { get; set; }

    [Required]
    public string Email { get; set; }

    public string Adress { get; set; }

    public int PhoneNumber { get; set; }
    public string Picture { get; set; }
}