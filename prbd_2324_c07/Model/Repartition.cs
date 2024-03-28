using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prbd_2324_c07.Model;

public class Repartition {

    [Required]
    public int Weight { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [Required]
    public virtual User User { get; set; }

    [Required, ForeignKey(nameof(Operation))]
    public int OperationId { get; set; }
    [Required]
    public virtual Operation Operation { get; set; }

    public Repartition() { 
        
    }

    public Repartition(int weight, User user, Operation operation) {
        Weight = weight;
        User = user;
        Operation = operation;
    }
}
