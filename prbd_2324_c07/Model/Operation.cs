using PRBD_Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2324_c07.Model;

public  class Operation : EntityBase<PridContext>
{
    [Key]
    public int OperationId { get; set; }

    [Required, MinLength(3)]
    public String Title { get; set; }
    [Required]
    public Double Amount { get; set; }
    [Required, DataType(DataType.Date)]
    public DateTime Operation_date { get; set; }

    [Required, ForeignKey(nameof(Tricount))]
    public int TricountId { get; set; }
    public virtual Tricount Tricount { get; set; }

    [Required, ForeignKey(nameof(Initiator))]
    public int InitiatorId { get; set; }
    public virtual User Initiator { get; set; }

    public virtual ICollection<Repartition> Repartitions { get; set; } = new HashSet<Repartition>();

    [NotMapped]
    public Dictionary<int, float> Balance { get; set; } = new();


    public Operation() {

    }

    public Operation (string title, Tricount tricount, Double amount, DateTime operation_date, User initiator) {
        Title = title;
        Amount = amount;
        Tricount = tricount;
        Initiator = initiator;
        Operation_date = operation_date;
        RefreshBalance();
    }

    public void RefreshBalance() {
        Balance.Clear();

        var repartitions = Repartitions
            .ToList();

        int totalWeight = Repartitions
            .Sum(rep => rep.Weight);

        float temp = 0;
        repartitions.ForEach(rep => {

            if (rep.UserId == InitiatorId) {
                temp = (float) Amount / totalWeight;
                temp = temp * (totalWeight - rep.Weight);

                Balance.Add(rep.UserId, temp);
                temp = 0;
            } else {
                temp = (float) Amount / totalWeight;
                temp = (rep.Weight * temp) * -1;
                Balance.Add(rep.UserId, temp);
                temp = 0;
            }
        });
    }


}
