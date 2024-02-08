namespace AWS.Core.Common
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
          => Id = Guid.NewGuid().ToString();

        public string Id { get; protected init; }

        public bool Equals(string id)
            => Id == id;

        public override int GetHashCode()
            => base.GetHashCode();

        private DateTime _createdAt;

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => _createdAt = DateTime.UtcNow;
        }

        private DateTime _updatedAt;

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => _updatedAt = value;
        }
    }
}
