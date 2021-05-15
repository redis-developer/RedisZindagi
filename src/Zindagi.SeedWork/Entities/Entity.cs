using System;
using System.Globalization;

namespace Zindagi.SeedWork
{
    public interface IEntity { }

    public abstract class Entity : IEntity
    {
        public virtual Guid Id { get; set; }

        public override string ToString() => $"[ENTITY: {GetType().Name}] Id = {Id}";

        public virtual string GetPersistenceKey() => $"{GetType().Name}:{Id}".ToUpper(CultureInfo.InvariantCulture);
    }

    public interface IAggregateRoot { }

    public abstract class AggregateBase : Entity, IAggregateRoot { }
}
