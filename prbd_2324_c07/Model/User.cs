using PRBD_Framework;
using System.ComponentModel.DataAnnotations;

namespace prbd_2324_c07.Model;
public enum Role {
    User = 0,
    Administrator = 1,
}
public class User : EntityBase<PridContext> {

    [Key]
    public int UserId { get; set; }
    [Required, EmailAddress]
    public string Mail { get; set; }
    [Required]
    public string Password { get; set; }
    [Required, MinLength(3)]
    public string FullName { get; set; }
    [Required]
    public Role Role { get; protected set; } = Role.User;

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
    public virtual ICollection<Tricount> Tricounts { get; set; } = new HashSet<Tricount>();
    public virtual ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();
    public virtual ICollection<Repartition> Repartitions { get; set; } = new HashSet<Repartition>();
    public virtual ICollection<TemplateItem> TemplatesItems { get; set; } = new HashSet<TemplateItem>();

    public User() {

    }

    public User(string mail, string password, string fullname, Role role = 0) {
        Mail = mail;
        Password = password;
        FullName = fullname;
    }

     public override string ToString() {
        return $"<User : fullname ={FullName}, " +
            $"#subscription = {Subscriptions.Count}";
    }
}