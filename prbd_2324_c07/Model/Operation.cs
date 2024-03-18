using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2324_c07.Model;

public  class Operation {
    [Key]
    public int OperationId { get; set; }

    [Required, MinLength(3)]
    public String Title { get; set; }
    [Required]
    public decimal Amount { get; set; }
    [Required, DataType(DataType.Date)]
    public DateTime Operation_date { get; set; }

    [Required, ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    public virtual Tricount Tricount { get; set; }

    [Required, ForeignKey(nameof(Initiator))]
    public int InitiatorId { get; set; }
    public virtual User Initiator { get; set; }

    public virtual ICollection<Repartition> Repartitions { get; set; } = new HashSet<Repartition>();



    public Operation() {

    }

    public Operation (string title, Tricount tricount, decimal amount, DateTime operation_date, User initiator) {
        Title = title;
        Amount = amount;
        Tricount = tricount;
        Initiator = initiator;
        Operation_date = operation_date;
    }


}
