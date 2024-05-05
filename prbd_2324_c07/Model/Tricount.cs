using prbd_2324_c07.ViewModel;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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

        public static IQueryable<Tricount> GetAll() {
            return Context.Tricounts.OrderBy(t => t.Title);
        }

        // Utilisateur createur ou participant
        public static IQueryable<Tricount> GetAllWithUser(User user) {

            IQueryable<Tricount> createdTricounts;
            IQueryable<Tricount> participantsTricounts;

            if (user is Administrator) {
                createdTricounts = Context.Tricounts;   
                participantsTricounts = Context.Tricounts;
            } else {
                createdTricounts = Context.Tricounts.Where(t => t.Creator == user);
                participantsTricounts = Context.Subscriptions
                .Where(sub => sub.User == user)
                .Select(sub => sub.Tricount);
            }

            // Union des deux et tri
            IQueryable<Tricount> allTricounts = createdTricounts.Union(participantsTricounts);

            // TODO: tri ne fonctionne pas totalement, à modifier
            var sortedTricounts = allTricounts
            .Select(t => new {
                Tricount = t,
                MostRecentOperationDate = t.Operations.Max(o => (DateTime?)o.Operation_date)
            })
            .OrderByDescending(t => t.MostRecentOperationDate ?? t.Tricount.CreatedAt)
            .Select(t => t.Tricount);

            return sortedTricounts;
        }

        public DateTime? GetLastOperationDate() {
            var lastOp = Operations
                .Where(op => op.TricountId == this.TricountId)
                .OrderByDescending(o => o.Operation_date)
                .FirstOrDefault();

            return lastOp == null ? null : lastOp.Operation_date;

        }
        public int NumberOfOperations() {
            var operations = Operations
                .Where(op=>op.TricountId == this.TricountId)
                .Count();
            return operations;
        }
        public int NumberOfParticipants() {

            var participants = Subscriptions
                .Where(sub => sub.TricountId == this.TricountId)
                .Distinct()
                .Count();
            return participants;
        }
        // Participants ajoutés au Tricount par le créateur
        public int NumberOfFriends() {
            return NumberOfParticipants() - 1;
        }

        public double TotalExpenses() {
            var totalExpenses = Operations
                .Where(op => op.TricountId == this.TricountId)
                .Sum(op => op.Amount);

            return totalExpenses;
        }

        public double GetUserBalance(User user) {

            var balance = Balance.GetValueOrDefault(user.UserId);



            return 0;
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

            //foreach (KeyValuePair<int, float> kvp in Balance) {
            //    Console.WriteLine("Tricount ID = {0} User = {1}, Value = {2}", this.TricountId, kvp.Key, kvp.Value);
            //}

        }


        public override string ToString() {
            return $"<tricount : title  = {Title}, " +
                $"#creator = {CreatorId}, " +
                $"#subscription = {Subscriptions.Count}";
        }
    }
}
