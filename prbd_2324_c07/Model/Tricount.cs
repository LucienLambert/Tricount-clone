using prbd_2324_c07.ViewModel;
using PRBD_Framework;
using System.Collections.ObjectModel;
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
            //CreatedAt = DateTime.Now;
            RefreshBalance();
        }

        public static IQueryable<Tricount> GetAll() {
            return Context.Tricounts.OrderBy(t => t.Title);
        }

        public void Delete() {
            Context.Tricounts.Remove(this);
            Context.SaveChanges();
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
                    MostRecentDate = t.Operations.Any() ? t.Operations.Max(o => o.Operation_date) : t.CreatedAt
                })
                .OrderByDescending(t => t.MostRecentDate)
                .Select(t => t.Tricount);

            return sortedTricounts;
        }


        public static IQueryable<Tricount> GetFiltered(string Filter, User user) {

            var allTricounts = GetAllWithUser(user);

            var filtered = from t in allTricounts
                           where t.Title.Contains(Filter)
                           || t.Description.Contains(Filter)
                           || t.Creator.FullName.Contains(Filter)
                           || t.Operations.Any(o => o.Title.Contains(Filter))
                           || t.Subscriptions.Any(s => s.User.FullName.Contains(Filter))
                           select t;

            // Peut-être redondant
            var sortedTricounts = filtered
                .Select(t => new {
                    Tricount = t,
                    MostRecentDate = t.Operations.Any() ? t.Operations.Max(o => o.Operation_date) : t.CreatedAt
                })
                .OrderByDescending(t => t.MostRecentDate)
                .Select(t => t.Tricount);

            return filtered;
        }

        public DateTime? GetLastOperationDate() {
            var lastOp = Operations
                .Where(op => op.TricountId == this.TricountId)
                .OrderByDescending(o => o.Operation_date)
                .FirstOrDefault();

            return lastOp == null ? null : lastOp.Operation_date;

        }

        public static IQueryable<Operation> GetOperationsForTricount(Tricount tricount) {
            var operations = Context.Operations
                .Where(op => op.TricountId == tricount.TricountId)
                .OrderBy(op => op.Operation_date);
            return operations;
        }


        public int NumberOfOperations() {
            var operations = Operations
                .Where(op => op.TricountId == this.TricountId)
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
                .Sum(op => op.Amount);

            return Math.Round(totalExpenses, 2);
        }

        // récupère les expesense du user (paramètre) du tricount actuel
        public double GetUserExpenses(User user) {
            int totalWeight = 0;
            int userWeight = 0;
            double expenses = 0;

            foreach (var op in Operations) {
                userWeight = op.Repartitions.FirstOrDefault(rep => rep.UserId == user.UserId)?.Weight ?? 0;
                if (userWeight != 0) {
                    totalWeight = op.Repartitions.Sum(rep => rep.Weight);
                    expenses += (op.Amount / totalWeight) * userWeight;
                }
            }
            return Math.Round(expenses, 2);
        }



        public double GetUserBalance(User user) {
            RefreshBalance();
            var balance = Balance.GetValueOrDefault(user.UserId);

            return Math.Round(balance, 2);
        }

        public bool Validate(User user) {
            ClearErrors();

            if (string.IsNullOrEmpty(Title)) {
                AddError(nameof(Title), "required");
            } else if (Title.Length < 3) {
                AddError(nameof(Title), " length must be >= 3");
            } else if ((IsDetached || IsAdded) && Context.Tricounts.Any((t => t.Title == Title && t.CreatorId == user.UserId))) {
                AddError(nameof(Title), "title arleady exists");
            }
            //Context.Tricounts.Any(t => t.Title == Title)


            if (!string.IsNullOrEmpty(Description) && Description.Length < 3) {
                AddError(nameof(Description), "the length must be > 3 or empty");
            }

            if (DateTime.Now.CompareTo(CreatedAt) < 0) {
                AddError(nameof(CreatedAt), "the date cannot be greater than the current date");
            }
            return !HasErrors;
        }

        // Peut se faire différement
        public void RefreshBalance() {

            Balance.Clear();

            var participantsIds = Subscriptions
                .Select(s => s.UserId)
                .ToList();

            var operationsList = Operations
                .ToList();

            var totalAmount = operationsList
                .Sum(op => op.Amount);

            float temp = 0;
            foreach (var userId in participantsIds) {
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
        }

        // crée une subscription qui lie un user à un tricount.
        public void AddUserSubTricount(User user) {
            Subscriptions.Add(new Subscription(user, this));
        }

        public void RemoveUserSubTricount(User user) {
            foreach (var s in Subscriptions) {
                if (s.User.Equals(user)) {
                    Subscriptions.Remove(s);
                }
            }
        }

        // return la liste des subscriptants du tricount
        public ICollection<Subscription> ParticipantTricount() {
            return Subscriptions;
        }

        public override string ToString() {
            return $"<tricount : title  = {Title}\n" +
                $"#description = {Description}\n" +
                $"#creatorId = {CreatorId}\n" +
                $"#subscription = {Subscriptions.Count}";
        }
    }
}
