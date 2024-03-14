//using ProjectModel;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//public class Operation {
//    [Key, Required]
//    public int Id { get; set; }
//    [Required]
//    public string Title { get; set; }
//    [Required]
//    public float Amount { get; set; }
//    [Required]
//    public DateTime Date { get; set; }
//    [Required, ForeignKey(nameof(Initiator))]
//    public int InitiatorId { get; set; }
//    [Required]

//    public virtual User Initiator { get; set; }

//    [Required, ForeignKey(nameof(Tricount))]
//    public int TricountId { get; set; }
//    [Required]
//    public virtual Tricount Tricount { get; set; }

//    public ICollection<Repartition> Repartitions { get; set; }
//    [NotMapped]
//    public Dictionary<int, float> Balance { get; set; } = new();

//    public Operation() { }
//    public Operation(string title, float amount, DateTime date, User initiator, Tricount tricount) {
//        Title = title;
//        Amount = amount;
//        Date = date;
//        Initiator = initiator;
//        InitiatorId = initiator.Id;
//        Tricount = tricount;
//        TricountId = tricount.Id;
//    }

//    public void RefreshBalance() {
//        Balance.Clear();
//        var repartitions = Program.Context.Repartitions
//            .Where(rep => rep.Operation == this)
//            .ToList();

//        int initiator = InitiatorId;
//        int totalWeight = repartitions
//            .Sum(rep => rep.Weight);

//        float temp = 0;
//        repartitions.ForEach(rep => {

//            if (rep.UserId == InitiatorId) {
//                temp = Amount / totalWeight;
//                temp = temp * (totalWeight - rep.Weight);

//                Balance.Add(rep.UserId, temp);
//                temp = 0;
//            } else {
//                temp = Amount / totalWeight;
//                temp = (rep.Weight * temp) * -1;
//                Balance.Add(rep.UserId, temp);
//                temp = 0;
//            }
//        });
//        //foreach (KeyValuePair<int, float> kvp in Balance) {
//        //    Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
//        //}


//    }


//}
