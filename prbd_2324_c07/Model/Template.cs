using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2324_c07.Model;

public class Template {

    [Key]
    public int TemplateId { get; set; }
    [Required]
    public String Title { get; set; }

    [Required, ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    [Required]
    public virtual Tricount Tricount { get; set; }

    //public virtual ICollection<TemplateItem> Templates_Items { get; set; }
    public virtual ICollection<TemplateItem> TemplatesItems { get; set; } = new HashSet<TemplateItem>();

    public Template() {

    }

    public Template(String title, Tricount tricount) {
        Title = title;
        Tricount = tricount;
    }
}
