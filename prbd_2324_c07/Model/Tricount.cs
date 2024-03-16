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

        public Tricount() {

        }

        public Tricount(string title, string description, User creator) {
            Title = title;
            Description = description;
            Creator = creator;
            CreatedAt = DateTime.Now;
        }

        public override string ToString() {
            return $"<tricount : title  = {Title}, " +
                $"#creator = {CreatorId}, " +
                $"#subscription = {Subscriptions.Count}";
        }
    }
}
