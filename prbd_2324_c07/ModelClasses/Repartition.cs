//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//public class Repartition {
//    [Required]
//    public int OperationId { get; set; }
//    public virtual Operation Operation { get; set; }
//    [Required, ForeignKey(nameof(User))]
//    public int UserId { get; set; }
//    [Required]
//    public virtual User User { get; set; }
//    [Required]
//    public int Weight { get; set; }

//    public Repartition() { }
//    public Repartition(Operation operation, User user, int weight) {
//        this.Operation = operation;
//        this.OperationId= operation.Id;
//        this.User = user;
//        this.UserId=user.Id;
//        this.Weight= weight;
//    }


//}
