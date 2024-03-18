using PRBD_Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace prbd_2324_c07.Model
{
    public class Subscription : EntityBase<PridContext>{

        [Required, ForeignKey(nameof(User))]
        public int UserId { get; set; }
        [Required]
        public virtual User User { get; set; }

        [Required, ForeignKey(nameof(Tricount))]
        public int TricountId { get; set; }
        [Required]
        public virtual Tricount Tricount { get; set; }

        public Subscription() { 

        }

        public Subscription(User user, Tricount tricount) {
           User = user;
           Tricount = tricount;
        }

    }
}
