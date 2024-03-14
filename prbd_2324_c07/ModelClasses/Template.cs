//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//public class Template {
//    [Key, Required]
//    public int Id { get; set; }
//    [Required]
//    public string Title { get; set; }
//    [Required, ForeignKey(nameof(Tricount))]
//    public int TricountId { get; set; }
//    public virtual Tricount Tricount { get; set; }

//    public ICollection<TemplateItem> TemplateItems { get; set; }
//}
