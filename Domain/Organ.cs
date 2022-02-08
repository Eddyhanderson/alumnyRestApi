using System.ComponentModel.DataAnnotations;

public class Organ
{
    [Key]
    public string Id { get; set; }
    public decimal Code { get; set; }
    public string Name { get; set; }
    public float Badget { get; set; }
}