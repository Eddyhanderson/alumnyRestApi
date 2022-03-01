using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using alumni.Domain;

public class Organ
{
    [Key]
    public string Id { get; set; }    
    public int Code { get; set; }
    public string Name { get; set; }
    public float Badget { get; set; }

    public string UserId {get;set;}

    [ForeignKey(nameof(UserId))]
    public User Responsable {get;set;}
}