using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2324_c07.Model;

public  class Operation {
    [Key]
    public int OperationId { get; set; }
    [Required]
    public String Title { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public DateTime Operation_date { get; set; }

    [Required, ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    public virtual Tricount Tricount { get; set; }

    [Required, ForeignKey(nameof(Initiator))]
    public int InitiatorId { get; set; }
    public virtual User Initiator { get; set; }


    public Operation() {

    }

    public Operation (string title, double amount, Tricount tricount, User initiator) {
        Title = title;
        Amount = amount;
        Tricount = tricount;
        Initiator = initiator;
    }


}
