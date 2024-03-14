using PRBD_Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace prbd_2324_c07.Model
{
    public class Tricount : EntityBase<PridContext>
    {

        [Key, Required]
        public int TricountId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required, ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }
        [Required]
        public virtual User Creator { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        //public virtual ICollection<Operation> Operations { get; set; }
        //public virtual ICollection<Template> Templates { get; set; }

        //public Dictionary<int, float> Balance = new();


    }
}
