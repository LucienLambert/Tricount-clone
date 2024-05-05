using PRBD_Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace prbd_2324_c07.Model
{
    public class Tricount : EntityBase<PridContext>
    {

        [Key]
        public int TricountId { get; set; }

        [Required, MinLength(3)]
        public string Title { get; set; }

        [MinLength(3)]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required, ForeignKey(nameof(Creator))]
        public int CreatorId { get; set; }
        [Required]
        public virtual User Creator { get; set; }

        //public virtual ICollection<Subscription> Subscriptions { get; set; }
        //public virtual ICollection<Operation> Operations { get; set; }
        public virtual ICollection<Template> Templates { get; set; } = new HashSet<Template>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new HashSet<Subscription>();
        public virtual ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();

        [NotMapped]
        public Dictionary<int, float> Balance { get; set; } = new();

        public Tricount() {
        }

        public Tricount(string title, string description, User creator) {
            Title = title;
            Description = description;
            Creator = creator;
            CreatedAt = DateTime.Now;
        }

        public override bool Validate() {
            ClearErrors();

            if(string.IsNullOrEmpty(Title)) {
                AddError(nameof(Title), "required");
            } else if(Title.Length < 3) {
                AddError(nameof(Title), " length must be >= 3");
            } else if((IsDetached || IsAdded) && Context.Tricounts.Any(t => t.Title == Title)) {
                    AddError(nameof(Title), "title arleady exists");
            }


            if (!string.IsNullOrEmpty(Description) && Description.Length < 3) {
                AddError(nameof(Description), "the length must be > 3 or empty");
            }

            if (DateTime.Now.CompareTo(CreatedAt) < 0) {
                AddError(nameof(CreatedAt), "the date cannot be greater than the current date");
            }
            return !HasErrors;
        }


        public void RefreshBalance() {

            Balance.Clear();

            var userIds = Context.Subscriptions
                .Where(s => s.TricountId == this.TricountId)
                .Select(s => s.UserId)
                .ToList();

            var operationsList = Context.Operations
                .Where(op => op.TricountId == this.TricountId)
                .ToList();

            var totalAmount = Context.Operations
                .Where(op => op.TricountId == this.TricountId)
                .Sum(op => op.Amount);

            float temp = 0;
            foreach (var userId in userIds) {
                operationsList.ForEach(op => {
                    op.RefreshBalance();
                    foreach (var kvp in op.Balance) {
                        if (kvp.Key == userId) {
                            temp += kvp.Value;
                        }
                    }
                });
                Balance.Add(userId, temp);
                temp = 0;
            }

            foreach (KeyValuePair<int, float> kvp in Balance) {
                Console.WriteLine("Tricount ID = {0} User = {1}, Value = {2}", this.TricountId, kvp.Key, kvp.Value);
            }

        }


        public override string ToString() {
            return $"<tricount : title  = {Title}, " +
                $"#creator = {CreatorId}, " +
                $"#subscription = {Subscriptions.Count}";
        }
    }
}
