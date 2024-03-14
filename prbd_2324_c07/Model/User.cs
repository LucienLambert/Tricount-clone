using PRBD_Framework;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_c07.Model;
public enum Role
{
    User = 0,
    Administrator = 1
}
public class User : EntityBase<PridContext>
{

    [Key]
    [Required]
    public int UserId { get; set; }
    [Required, EmailAddress]
    public string Mail { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public Role Role { get; protected set; } = Role.User;

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
    public virtual ICollection<Tricount> Tricounts { get; set; } = new HashSet<Tricount>();
    //public virtual ICollection<Operation> Operations { get; set; }
    //public virtual ICollection<Repartition> Repartitions { get; set; }
    //public virtual ICollection<TemplateItem> TemplateItems { get; set; }

}