
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2324_c07.Model;

public class TemplateItem {

    [Required]
    public int Weight { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int UserId {  get; set; }
    [Required]
    public virtual User User { get; set; }

    [Required, ForeignKey(nameof(Template))]
    public int TemplateId { get; set; }
    [Required]
    public virtual Template Template { get; set; }

    public TemplateItem() { 
        
    }

    public TemplateItem(int weight, User user, Template template) {
        Weight = weight;
        User = user;
        Template = template;
    } 
}
